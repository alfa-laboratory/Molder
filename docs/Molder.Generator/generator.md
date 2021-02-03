# Molder.Generation
------
## Шаги для генерации тестовых данных

------
>  Сохранение даты в переменную.
```c#
[StepDefinition(@"я сохраняю дату ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) в переменную ""(.+)""")]
```
>> Example
```gherkin
Given я сохраняю дату 1.5.2000 в переменную "test"
```

------
>  Сохранение точного времени в переменную (с миллисекундами).
```c#
[StepDefinition(@"я сохраняю время ([0-9]{1,2}):([0-9]{2}):([0-9]{2})\.([0-9]+) в переменную ""(.+)""")]
```
>> Example
```gherkin
Given я сохраняю время 23:59:59.100 в переменную "test"
```

------
>  Сохранение точного времени (с миллисекундами) в переменную, используя конкретный формат.
```c#
[StepDefinition(@"я сохраняю время ([0-9]{1,2}):([0-9]{2}):([0-9]{2})\.([0-9]+) в формате ""(.+)"" в переменную ""(.+)""")]
```
>> Example
```gherkin
Given я сохраняю время 23:59:59.100 в формате "HH:mm:ss" в переменную "test"
```

------
>  Сохранение даты и времени в переменную.
```c#
[StepDefinition(@"я сохраняю дату и время ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) ([0-9]{1,2}):([0-9]{2}):([0-9]{2}) в переменную ""(.+)""")]
```
>> Example
```gherkin
Given я сохраняю дату и время 31.01.2000 23:59:59 в переменную "test"
```

------
>  Сохранение даты и времены в переменную, используя конкретный формат.
```c#
[StepDefinition(@"я сохраняю дату и время ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) ([0-9]{1,2}):([0-9]{2}):([0-9]{2}) в формате ""(.+)"" в переменную ""(.+)""")]
```
>> Example
```gherkin
Given я сохраняю дату и время 31.01.2000 23:59:59 в формате "dd.MM.yyyy hh:mm:ss" в переменную "test"
```

------
>  Сохранение текущей даты в переменную.
```c#
[StepDefinition(@"я сохраняю текущую дату в переменную ""(.+)""")]
```
>> Example
```gherkin
Given я сохраняю текущую дату в переменную "test"
```

------
>  Сохранение  текущей даты в переменную, используя конкретный формат.
```c#
[StepDefinition(@"я сохраняю текущую дату в формате ""(.+)"" в переменную ""(.+)""")]
```
>> Example
```gherkin
Given я сохраняю текущую дату в формате "dd.MM.yyyy" в переменную "test"
```

------
>  Сохранение  рандомной даты.
```c#
[StepDefinition(@"я сохраняю рандомную дату в переменную ""(.+)""")]
```
>> Example
```gherkin
Given я сохраняю рандомную дату в переменную "test"
```

------
>  Сохранение  рандомной даты, используя конкретный формат.
```c#
[StepDefinition(@"я сохраняю рандомную дату в формате ""(.+)"" в переменную ""(.+)""")]
```
>> Example
```gherkin
Given я сохраняю рандомную дату в формате "dd.MM.yyyy" в переменную "test"
```

------

#region Past DateTime

/// <summary>
/// Шаг для сохранения прошедшей даты, которая отличается от текущей на определенный срок в переменную.
/// </summary>
/// <param name="year">Количество лет от текущей даты.</param>
/// <param name="month">Количество месяцев от текущей даты.</param>
/// <param name="day">Количество дней от текущей даты.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю прошедшую дату, которая отличается от текущей на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в переменную ""(.+)""")]
public void StoreAsVariablePastDateTimeWithDifference(int year, int month, int day, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

    var dt = fakerGenerator.GetDate(day, month, year, false);
    dt.Should().NotBeNull($"проверьте корректность создания даты day:{day},month:{month},year:{year}");

    this.variableController.SetVariable(varName, dt.GetType(), dt);
}

/// <summary>
/// Шаг для сохранения прошедшей даты, которая отличается от текущей на определенный срок в переменную, используя конкретный формат.
/// </summary>
/// <param name="year">Количество лет от текущей даты.</param>
/// <param name="month">Количество месяцев от текущей даты.</param>
/// <param name="day">Количество дней от текущей даты.</param>
/// <param name="format">Формат представления даты.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю прошедшую дату, которая отличается от текущей на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в формате ""(.+)"" в переменную ""(.+)""")]
public void StoreAsVariablePastDateTimeWithDifference(int year, int month, int day, string format, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

    var dt = fakerGenerator.GetDate(day, month, year, false);
    dt.Should().NotBeNull($"проверьте корректность создания даты day:{day},month:{month},year:{year}");
    var pastDateTime = dt?.ToString(format);

    this.variableController.SetVariable(varName, pastDateTime.GetType(), pastDateTime);
}

/// <summary>
/// Шаг для сохранения прошедшей даты, которая отличается от текущей на определенный срок в переменную, используя конкретный формат.
/// </summary>
/// <param name="year">Количество лет от текущей даты.</param>
/// <param name="month">Количество месяцев от текущей даты.</param>
/// <param name="day">Количество дней от текущей даты.</param>
/// <param name="format">Формат представления даты.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю прошедшую дату, которая отличается от ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в переменную ""(.+)""")]
public void StoreAsVariablePastDateTime(int fYear, int fMonth, int fDay, int year, int month, int day, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

    var dt = fakerGenerator.GetDate(fDay, fMonth, fYear);
    dt.Should().NotBeNull($"проверьте корректность создания даты day:{fDay},month:{fMonth},year:{fYear}");

    var pdt = fakerGenerator.GetDate(day, month, year, false, dt);
    pdt.Should().NotBeNull($"проверьте корректность создания даты day:{day},month:{month},year:{year}");

    this.variableController.SetVariable(varName, pdt.GetType(), pdt);
}

/// <summary>
/// Шаг для сохранения прошедшей даты, которая отличается от текущей на определенный срок в переменную, используя конкретный формат.
/// </summary>
/// <param name="year">Количество лет от текущей даты.</param>
/// <param name="month">Количество месяцев от текущей даты.</param>
/// <param name="day">Количество дней от текущей даты.</param>
/// <param name="format">Формат представления даты.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю прошедшую дату, которая отличается от ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в формате ""(.+)"" в переменную ""(.+)""")]
public void StoreAsVariablePastDateTime(int fYear, int fMonth, int fDay, int year, int month, int day, string format, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

    var dt = fakerGenerator.GetDate(fDay, fMonth, fYear);
    dt.Should().NotBeNull($"проверьте корректность создания даты day:{fDay},month:{fMonth},year:{fYear}");

    var pdt = fakerGenerator.GetDate(day, month, year, false, dt);
    pdt.Should().NotBeNull($"проверьте корректность создания даты day:{day},month:{month},year:{year}");
    var pastDateTime = pdt?.ToString(format);

    this.variableController.SetVariable(varName, pastDateTime.GetType(), pastDateTime);
}
#endregion
#region Future DateTime
/// <summary>
/// Шаг для сохранения будущей даты, которая отличается от текущей на определенный срок в переменную.
/// </summary>
/// <param name="year">Количество лет от текущей даты.</param>
/// <param name="month">Количество месяцев от текущей даты.</param>
/// <param name="day">Количество дней от текущей даты.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю будущую дату, которая отличается от текущей на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в переменную ""(.+)""")]
public void StoreAsVariableFutureDateTimeWithDifference(int year, int month, int day, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

    var dt = fakerGenerator.GetDate(day, month, year, true);
    dt.Should().NotBeNull($"Проверьте корректность создания даты day:{day},month:{month},year:{year}.");

    this.variableController.SetVariable(varName, dt.GetType(), dt);
}


/// <summary>
/// Шаг для сохранения будущей даты, которая отличается от текущей на определенный срок в переменную, используя конкретный формат.
/// </summary>
/// <param name="year">Количество лет от текущей даты.</param>
/// <param name="month">Количество месяцев от текущей даты.</param>
/// <param name="day">Количество дней от текущей даты.</param>
/// <param name="format">Формат представления даты.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю будущую дату, которая отличается от текущей на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в формате ""(.+)"" в переменную ""(.+)""")]
public void StoreAsVariableFutureDateTimeWithDifference(int year, int month, int day, string format, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

    var dt = fakerGenerator.GetDate(day, month, year, true);
    dt.Should().NotBeNull($"Проверьте корректность создания даты day:{day},month:{month},year:{year}.");
    var futureDateTime = dt?.ToString(format);

    this.variableController.SetVariable(varName, futureDateTime.GetType(), futureDateTime);
}

/// <summary>
/// Шаг для сохранения будущей даты, которая отличается от даты на определенный срок в переменную.
/// </summary>
/// <param name="year">Количество лет от текущей даты.</param>
/// <param name="month">Количество месяцев от текущей даты.</param>
/// <param name="day">Количество дней от текущей даты.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю будущую дату, которая отличается от ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в переменную ""(.+)""")]
public void StoreAsVariableFutureDateTime(int fYear, int fMonth, int fDay, int year, int month, int day, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

    var dt = fakerGenerator.GetDate(fDay, fMonth, fYear);
    dt.Should().NotBeNull($"Проверьте корректность создания даты day:{fDay},month:{fMonth},year:{fYear}.");

    var fdt = fakerGenerator.GetDate(day, month, year, true, dt);
    fdt.Should().NotBeNull($"Проверьте корректность создания даты day:{day},month:{month},year:{year}.");

    this.variableController.SetVariable(varName, fdt.GetType(), fdt);
}

/// <summary>
/// Шаг для сохранения будущей даты, которая отличается от даты на определенный срок в переменную, используя конкретный формат.
/// </summary>
/// <param name="year">Количество лет от текущей даты.</param>
/// <param name="month">Количество месяцев от текущей даты.</param>
/// <param name="day">Количество дней от текущей даты.</param>
/// <param name="format">Формат представления даты.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю будущую дату, которая отличается от ([0-9]{1,2})\.([0-9]{2})\.([0-9]+) на ""([0-9]+)"" (?:лет|год[а]?) ""([0-9]+)"" (?:месяц|месяц(?:а|ев)) ""([0-9]+)"" (?:день|дн(?:я|ей)) в формате ""(.+)"" в переменную ""(.+)""")]
public void StoreAsVariableFutureDateTime(int fYear, int fMonth, int fDay, int year, int month, int day, string format, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

    var dt = fakerGenerator.GetDate(fDay, fMonth, fYear);
    dt.Should().NotBeNull($"Проверьте корректность создания даты day:{fDay},month:{fMonth},year:{fYear}.");

    var fdt = fakerGenerator.GetDate(day, month, year, true, dt);
    fdt.Should().NotBeNull($"Проверьте корректность создания даты day:{day},month:{month},year:{year}.");
    var futureDateTime = fdt?.ToString(format);

    this.variableController.SetVariable(varName, futureDateTime.GetType(), futureDateTime);
}
#endregion
#region Random string with prefix
/// <summary>
/// Шаг для сохранения случанойго набора букв и цифр в переменную, используя конкретный префикс.
/// </summary>
/// <param name="len">Длина строки.</param>
/// <param name="prefix">Префикс.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю случайный набор букв и цифр длиной ([0-9]+) знаков с префиксом ""(.+)"" в переменную ""(.+)""")]
public void StoreAsVariableRandomStringWithPrefix(int len, string prefix, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    len.Check(prefix, string.Empty);

    var str = prefix + fakerGenerator.String(len - prefix.Length);
    this.variableController.SetVariable(varName, str.GetType(), str);
}

/// <summary>
/// Шаг для добавления случайного набора букв в переменную, используя конкретный префикс.
/// </summary>
/// <param name="len">Длина строки.</param>
/// <param name="prefix">Префикс.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю случайный набор букв длиной ([0-9]+) знаков с префиксом ""(.+)"" в переменную ""(.+)""")]
public void StoreAsVariableRandomCharWithPrefix(int len, string prefix, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    len.Check(prefix, string.Empty);

    var str = prefix + fakerGenerator.Chars(len - prefix.Length);
    this.variableController.SetVariable(varName, str.GetType(), str);
}

/// <summary>
/// Шаг для добавления случайного набора цифр в переменную, ипользуя конкретный префикс.
/// </summary>
/// <param name="len">Длина строки.</param>
/// <param name="prefix">Префикс.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю случайный набор цифр длиной ([0-9]+) знаков с префиксом ""(.+)"" в переменную ""(.+)""")]
public void StoreAsVariableRandomNumberWithPrefix(int len, string prefix, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    len.Check(prefix, string.Empty);

    var str = prefix + fakerGenerator.Numbers(len - prefix.Length);
    this.variableController.SetVariable(varName, str.GetType(), str);
}
#endregion
#region Random string with postfix
/// <summary>
/// Шаг для сохранения случанойго набора букв и цифр в переменную, используя конкретный постфикс.
/// </summary>
/// <param name="len">Длина строки.</param>
/// <param name="postfix">Постфикс.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю случайный набор букв и цифр длиной ([0-9]+) знаков с постфиксом ""(.+)"" в переменную ""(.+)""")]
public void StoreAsVariableRandomStringWithPostFix(int len, string postfix, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    len.Check(string.Empty, postfix);

    var str = fakerGenerator.String(len - postfix.Length) + postfix;
    this.variableController.SetVariable(varName, str.GetType(), str);
}

/// <summary>
/// Шаг для добавления случайного набора букв в переменную, используя конкретный префикс.
/// </summary>
/// <param name="len">Длина строки.</param>
/// <param name="postfix">Постфикс.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю случайный набор букв длиной ([0-9]+) знаков с постфиксом ""(.+)"" в переменную ""(.+)""")]
public void StoreAsVariableRandomCharWithPostfix(int len, string postfix, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    len.Check(string.Empty, postfix);

    var str = fakerGenerator.Chars(len - postfix.Length) + postfix;
    this.variableController.SetVariable(varName, str.GetType(), str);
}

/// <summary>
/// Шаг для добавления случайного набора цифр в переменную, ипользуя конкретный префикс.
/// </summary>
/// <param name="len">Длина строки.</param>
/// <param name="postfix">Постфикс.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю случайный набор цифр длиной ([0-9]+) знаков с постфиксом ""(.+)"" в переменную ""(.+)""")]
public void StoreAsVariableRandomNumberWithPostfix(int len, string postfix, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    len.Check(string.Empty, postfix);

    var str = fakerGenerator.Numbers(len - postfix.Length) + postfix;
    this.variableController.SetVariable(varName, str.GetType(), str);
}
#endregion
#region Random string
/// <summary>
/// Шаг для добавления случайного набора букв и цифр в переменную.
/// </summary>
/// <param name="len">Длина строки.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю случайный набор букв и цифр длиной ([1-9]+) знаков в переменную ""(.+)""")]
public void StoreAsVariableRandomString(int len, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    len.Check();
    var str = fakerGenerator.String(len);

    this.variableController.SetVariable(varName, str.GetType(), str);
}

/// <summary>
/// Шаг для добавления случайного набора букв в переменную.
/// </summary>
/// <param name="len">Длина строки.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю случайный набор букв длиной ([0-9]+) знаков в переменную ""(.+)""")]
public void StoreAsVariableRandomChar(int len, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    len.Check();
    var str = fakerGenerator.Chars(len);

    this.variableController.SetVariable(varName, str.GetType(), str);
}

/// <summary>
/// Шаг для добавления случайного набора цифр в переменную.
/// </summary>
/// <param name="len">Длина строки.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю случайный набор цифр длиной ([0-9]+) знаков в переменную ""(.+)""")]
public void StoreAsVariableRandomNumber(int len, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    len.Check();
    var str = fakerGenerator.Numbers(len);

    this.variableController.SetVariable(varName, str.GetType(), str);
}
#endregion

/// <summary>
/// Шаг для сохранения случайного номера телефона в переменную, используя конкретный формат.
/// Пример формата: 7##########.
/// </summary>
/// <param name="mask">Маска для телефона.</param>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю случайный номер телефона в формате ""(.+)"" в переменную ""(.+)""")]
public void StoreAsVariableRandomPhone(string mask, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    var str = fakerGenerator.Phone(mask);
    this.variableController.SetVariable(varName, str.GetType(), str);
}

/// <summary>
/// Шаг для сохранения UUID в переменную.
/// </summary>
/// <param name="varName">Идентификатор переменной.</param>
[StepDefinition(@"я сохраняю новый (универсальный уникальный идентификатор|UUID) в переменную ""(.+)""")]
public void StoreAsVariableUuid(string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    var str = fakerGenerator.Guid();
    this.variableController.SetVariable(varName, str.GetType(), str);
}

[StepDefinition(@"я сохраняю случайный месяц в переменную ""(.+)""")]
public void StoreAsVariableMonth(string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    var str = fakerGenerator.Month();
    this.variableController.SetVariable(varName, str.GetType(), str);
}

[StepDefinition(@"я сохраняю случайный день недели в переменную ""(.+)""")]
public void StoreAsVariableWeekday(string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    var str = fakerGenerator.Weekday();
    this.variableController.SetVariable(varName, str.GetType(), str);
}

[StepDefinition(@"я сохраняю случайный email с провайдером ""(.+)"" в переменную ""(.+)""")]
public void StoreAsVariableEmail(string provider, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    var str = fakerGenerator.Email(provider);
    this.variableController.SetVariable(varName, str.GetType(), str);
}

[StepDefinition(@"я сохраняю случайный Ip адрес в переменную ""(.+)""")]
public void StoreAsVariableIp(string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    var str = fakerGenerator.Ip();
    this.variableController.SetVariable(varName, str.GetType(), str);
}

[StepDefinition(@"я сохраняю случайный Url в переменную ""(.+)""")]
public void StoreAsVariableUrl(string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    var str = fakerGenerator.Url();
    this.variableController.SetVariable(varName, str.GetType(), str);
}

/// <summary>
/// Шаг для сохранения Credentials в переменную.
/// </summary>
/// <param name="host">Хост.</param>
/// <param name="authType">Тип авторизации.</param>
/// <param name="domain">Домен.</param>
/// <param name="username">Логин.</param>
/// <param name="password">Зашифрованный пароль.</param>
/// <param name="varName">Идентификатор переменной.</param>
[ExcludeFromCodeCoverage]
[StepDefinition(@"я создаю полномочия для хоста ""(.+)"" c типом ""(.+)"" для пользователя с доменом ""(.+)"", логином ""(.+)"", паролем ""(.+)"" и сохраняю в переменную ""(.+)""")]
public void StoreCredentialsForHostToVariable(string host, AuthType authType, string domain, string username, string password, string varName)
{
    this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
    var credentialCache = new CredentialCache();
    var networkCredential = new NetworkCredential(username, Encryptor.Decrypt(password), domain);
    credentialCache.Add(new Uri(host), authType.ToString(), networkCredential);
    this.variableController.SetVariable(varName, credentialCache.GetType(), credentialCache);
}

/// <summary>
/// Шаг для преобразования значения одной переменной в массив.
/// </summary>
/// <param name="varName">Исходная переменная.</param>
/// <param name="chars">Массив символов-разделителей.</param>
/// <param name="newVarName">Переменная-результат.</param>
[StepDefinition(@"я преобразую значение переменной ""(.+)"" в массив, используя символы ""(.+)"" и сохраняю в переменную ""(.+)""")]
public void StoreVariableValueToArrayVariable(string varName, string chars, string newVarName)
{
    this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
    this.variableController.Variables.Should().NotContainKey(newVarName, $"переменная \"{newVarName}\" уже существует");

    var str = this.variableController.GetVariableValueText(varName);
    str.Should().NotBeNull($"Значения в переменной \"{varName}\" нет");

    var enumerable = Converter.CreateEnumerable(str, chars);

    this.variableController.SetVariable(newVarName, enumerable.GetType(), enumerable);
}