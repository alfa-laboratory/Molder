# Tips
В данном разделе собраны интересные приемы, которые помогут вам в написании тестовых сценариев, позволят уменьшить объем сценариев или просто немного упростить работу

------

## Проверка healthcheck для сервисов

В большинстве случаев при тестировании REST Api самым первым шагом должен быть HealthCheck. Описывать отдельный шаг, который будет вызываться в блоке *background* не всегда целесообразно (когда один сервис вызывается в Background, а несколько других (разных) в соответствующих сценариях. Дополнительно - это увеличивает сам сценарии.
В таком случае самым оптимальным подходом будет описание **Hook** для *BefareFeature* и *BeforeScenarion*. Как это будет выглядеть

Обязательные условия:
* Подключенная библиотека Molder.Configuration
* Описание url в *appsettings.json*, который будет отвечать за расположение сервисов (dev, int, pre)

1. Создаем регулярное выражение, которое определяет - что есть *endpoint* для сервиса
> ```c#
> 
> public static class Patterns
> {
>     public static string HEALTH_CHECK = "^(.*-service|.*-api)";
> }
> ```

2. Добавляем класс Hooks со связью к SpecFlow и описанием BeforeFeature и BeforeScenario
> ```c#
> [Binding]
> public sealed class Hooks
> {
>     // Очередь выполнения BeforeFeature
>     [BeforeFeature(Order = -10000)]
>     // Передача FeatureContext для работы с содержимым Feature файла и VariableController для работы с переменными.
>     public static void HealthCheckFeature(FeatureContext featureContext, VariableController variableController)
>     {
>         // Получение всех тегов, указанных перед Feature и выделение только тех, которые подходят под регулярное выражение
>         var serviceTags = featureContext.GetTags()
>             .Where(t => Regex.IsMatch(t, Patterns.HEALTH_CHECK, RegexOptions.IgnoreCase));
>         // Вызов HealthCheck
>         HealthCheckHelper.HealthCheck(variableController, serviceTags);
>     }
>     
>     // Очередь выполнения BeforeScenario
>     [BeforeScenario(Order = -30000)]
>     public void HealthCheckScenario(ScenarioContext scenarioContext, VariableController variableController)
>     {
>         var serviceTags = scenarioContext.GetTags()
>             .Where(t => Regex.IsMatch(t, Patterns.HEALTH_CHECK, RegexOptions.IgnoreCase));
>         HealthCheckHelper.HealthCheck(variableController, serviceTags);
>     }
> }
> ```

3. Добавляем функционал вызова HealthCheck
> ```c#
> public static void HealthCheck(VariableController variableController, IEnumerable<string> tags)
> {
>     if (tags.Any())
>     {
>         // Получение url стенда, на котором развернуты сервисы
>         var endpoint = variableController.GetVariableValueText("base_url") ??
>                        throw new ArgumentNullException($"Стенд по url с названием \"base_url\" не существует");
>         // Для каждого тега ...
>         foreach (var tag in tags)
>         {
>             // формируем url
>             var url = $"http://{endpoint}/{tag}";
>             // создаем instance сервиса
>             using (var service = new WebService())
>             {
>                 // формируем параметры вызова
>                 var request = new RequestInfo
>                 {
>                     Url = url,
>                     Method = HttpMethod.Get,
>                     Content = null,
>                     Headers = null
>                 };
>                 // вызываем
>                 var responce = service.SendMessage(request).Result;
>                 // проверяем статус на 200
>                 responce.StatusCode.Should().Be(HttpStatusCode.OK,
>                     $"HealthCheck для сервиса \"{tag}\" имеет статус {responce.StatusCode}");
>             }
>         }
>     }
> }
> ```

### Использование в сценарии
>> ```gherkin
>> @urls @auth @environment
>> @client-service/ping
>> @ad-api/healthcheck
>> Feature: client-service
>> Background: 
>> ...
>> ```

------
## Функции генерации данных в запросах
При создании тестовых данных мы часто прибегаем к псевдорандомным данным и любым другим генераторам. Ранее, чтобы создать уникальный вызов к сервису по созданию какого-нибудь объекта, приходилось делать следующие шаги

> ```gherkin
> Given я сохраняю рандомную дату в формате "dd-MM-yyyy" в переменную "randomDate"
>   And я сохраняю случайный набор букв и цифр длиной 10 знаков в переменную "randomString"
>   And я сохраняю текст в переменную "jsonBody":
>   """
>   {
>     "date": "{{randomDate}}",
>     "name": "{{randomString}}"
>   }
>   """
Как результат, в переменной *jsonBody* у нас будет следующий текст
```json
{
  "date": "25-01-2065",
  "name": "ASDAD24234SDA123"
}
```
В качестве упрощения и уменьшения размера сценариев была добавлениа функциональность, позволяющая перенести функции генерации данных в двойные фигурные скобки `{{_}}`

Чтобы добавить свои функции, надо описать статический класс, в котором функции будут принимать **только строковые параметры**, например
```c#
public static string randomInt(string len = "15")
{
    return new FakerGenerator().Numbers(int.Parse(len));
}
```
Далее, в своих *custom* шагах, необходимо описать *Hook*, который добавит созданные вами функции в общий пул используемых для *replace*
```c#
[BeforeTestRun()]
public static void Initialize()
{
    var obj = ReplaceMethods.Get() as List<Type>;
    // где YourStaticClass это имя статического класса, содержащего ваши функции
    if (!obj.Contains(typeof(YourStaticClass)))
    {
        (ReplaceMethods.Get() as List<Type>).Add(typeof(YourStaticClass));
    }
}
```
Как теперь можно описывать тестовые сценарии, используя [функции](/src/Molder.Generator/Extensions/GenerationFunctions.cs)

> ```gherkin
>   Given я сохраняю текст в переменную "jsonBody":
>   """
>   {
>     "date": "{{randomDateTime(dd-MM-yyyy)}}",
>     "name": "{{randomString(10)}}"
>   }
>   """
  
**Важно!**
Если функция содержит параметры, то они обязательны в `{{_()}}`, иначе функция не распознается системой. *Default* параметры не работают

Если в функцию вы хотите добавить значение из другой переменной, то ее можно передавать напрямую в `{{_(variable)}}`
> ```gherkin
>   Given я сохраняю текст в переменную "jsonBody":
>   """
>   {
>     "custom1": "{{custom1(another_variable)}}",
>     "custom2": "{{custom2(json.//first)}}",
>     "custom3": "{{custom3(xml.//first)}}"
>   }
>   """

  
  
  
  
  
  
  
  
------
## Работа с шифрованными значениями
Работая с базой данных или другими структурами, где требуется ввод логина и пароля, хранить пароли в открытом виде не является безопасным. В таком случае можно указать системе - что есть шифрованное значение.
Есть две функциональности: создать шифрованное значение и его использовать. Рассмотрим каждый из них
1. Чтобы из строки получить шифрованное значение, нужно в своих custom шагах описать функцию, которая по входной строке выведет в консоль шифрованную строку. В качестве примера рассмотрим подобную функцию для *xunit*.
> ```c#
> 
>    [ExcludeFromCodeCoverage]
>    [Binding]
>    public class Steps
>    {
>        private ITestOutputHelper output;
>
>        public Steps(ITestOutputHelper output)
>        {
>            this.output = output;
>        }
>
>        [StepDefinition(@"DEBUG: encrypt value ""(.+)""")]
>        public void encrypt(string value)
>        {
>            var text = Encryptor.Encrypt(value);
>            output.WriteLine($"Encrypt string is {text}");
>        }
>    }
> ```
Применив в сценарии данный шаг
> ```gherkin
>   Given DEBUG: encrypt value "test"
> ```
В *Test Output* мы получим шифрованное значение той строки, которую передали. И данное значение мы можем использовать в конфигурационном файле и при создании строковых переменнных, указывая, что входное значение *зашифрованное**.

  2. Чтобы использовать шифрованные значения, нужно ключи, в которые будет помещен шифр помечать спец.симфолами. В систему пойдет пара ключ-значение уже преобразованные, но как входные данные - шифрованные. 
  Ключ для такого значения должен выглядеть **$key#**.
В качестве примера рассмотрим конфигурационный файл *appsetting.json* с использованием шифрованного значения.
```json
{
  "Molder": {
    "Tips": {
      "Key1": "Value1",
      "$Password#": "W9qNIafQbJCZzEafUaYmQw"
    }
  }
}
```
После создания необходимых переменных из конфигурационного файла, в систему попадет переменная *Password* и дешифрованное значение строки *W9qNIafQbJCZzEafUaYmQw*.
  Аналогично можно сохранять шифрованные строки с помощью шагов, например
> ```gherkin
>   Given я сохраняю текст "W9qNIafQbJCZzEafUaYmQw" в переменную "$Password#"
> ```
При использовании шага создания конкретных полномочий, есть два варианта
* передавать явные значения (они будут в открытом виде);
* использовать переменные.
  Например:
> ```gherkin
>   Given я создаю полномочия для хоста "test.com" c типом "NTLM" для пользователя с доменом "mail", логином "test", паролем "test" и сохраняю в переменную "auth"
>   # или
>   Given я создаю полномочия для хоста "test.com" c типом "NTLM" для пользователя с доменом "mail", логином "test", паролем "{{Password}}" и сохраняю в переменную "auth"
> ```
