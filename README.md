
[license]: https://github.com/alfa-laboratory/Molder/blob/master/LICENSE "MIT License 2021"

# Molder [![build](https://ci.appveyor.com/api/projects/status/j33s710ss3f0nf4u?svg=true)](https://ci.appveyor.com/project/egorsh0/Molder) 

Набор библиотек с шагами для **BDD** тестирования баз данных, веб-сервисов и WebUI

## Как запустить?

В проект необходимо добавить файл **specflow.json** в котором описывается используемый язык ключевых слов и те *assembly*, в которых находятся шаги

``` json
{
  "language": {
    "feature": "en-EN"
  },
  "stepAssemblies": [
    {
      "assembly": "Molder.Generator"
    }
  ]
}
```
и добавить ему свойство **Copy if newer**

## Конфигурационный файл

В качестве конфигурационного файла используется **appsetting.json**, в который добавляются переменные в формате ключ-значение с привязкой к тегам. Теги, в свою очередь, могут использоваться в сценариях для того, чтобы подключить конкретные переменные к коду
Ключевое слово **Molder** обязательно, так как оно является основным, по которому ищутся переменные в конфигурационном файле

``` json
{
  "Molder": {
    "WebService": {
      "Key1": 1,
      "Key2": 2
    },
    "DataBase": {
      "Key12": "Value1",
      "Key21": "Value2"
    }
  }
}
```
и добавить ему свойство **Copy if newer**

## Dependencies 
1. Net Core 2.2 or Net Standart
1. SpecFlow 3.X
2. SpecFlow.Tools.MsBuild.Generation 3.X
3. **TestFramework** (xUnit, nUnit and etc) (xUnit 2.4.1)
4. SpecFlow.**TestFramework** 3.X
5. Molder.**Type**

## Contacts

[Telegram](https://t.me/AlfaBankAFTCore)

## License

The **Molder** is released of the [MIT License 2021][license].
