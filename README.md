[license]: https://github.com/alfa-laboratory/Molder/blob/master/LICENSE "MIT License 2021"

# Molder 
[![build](https://ci.appveyor.com/api/projects/status/j33s710ss3f0nf4u?svg=true)](https://ci.appveyor.com/project/egorsh0/Molder)
[![GitHub license](https://img.shields.io/github/license/alfa-laboratory/Molder?style=flat-square)](https://github.com/alfa-laboratory/Molder/blob/master/LICENSE)
[![GitHub stars](https://img.shields.io/github/stars/alfa-laboratory/Molder)](https://github.com/alfa-laboratory/Molder/stargazers)
<br/><br/>[Join us!](https://t.me/AlfaBankAFTCore)<br/>

Набор библиотек с шагами для **BDD** тестирования баз данных, сервисов и WebUI 

------
Documentation
=======================
<br/> [Обшие рекомендации](/docs/index.md), [интересные фишки](/docs/tips.md)

<br/> **1.** *Molder* является основной библиотекой, содержащей контроллер для инициализации работы с переменными, логирования и дополнительные провайдеры, которые могут использоваться в дочерних библиотеках.
<br/> **2.** *Molder.Generation* содержит функциональность для генерации тестовых данных, которые могут пригодиться в тестировании. За основу была взята библиотека [*Bogus*](https://github.com/bchavez/Bogus) и дополнительно, чтобы не потерять обширный её функционал, была оставлена возможность использовать *Faker*. 
<br/> **3.** Многие тестовые сценарии содержат работу с базой данных. Чтобы использовать данную возможность в сценариях, можно подключить библиотеку *Molder.Database*. Вы можете совершать любой сложности запросы в рамках одной транзакции к SQLServer с помощью нескольких простых шагов. 
<br/> **4.** Когда вам необходимо обратиться к сервису в рамках сценария, вы можете подключить библиотеку *Molder.Service* и с помощью нескольких шагов дополнить ваши сценарии новыми данными.
<br/> **5.** Большинство интеграционных тестов связаны с веб страницами. Чтобы проверить ваш сайт на корректность с помощью шагов, можно воспользоваться библиотекой *Molder.Web*. С ее помощью можно создать PageObject, который будет олицетворять те страницы, которые учавствуют в тестировании и с помощью шагов проверить их корректность.
<br/> **6.** Когда требуется запустить тесты на различных стендах, то необходимо иметь файлы конфигурации, с параметрами, отличающие один стенд от другого (или просто часто используемые данные). Для этого можно подключить библиотеку *Molder.Configuration* и добавить в свой проект *appsettings.json* в качестве конфигуратора.

------
How to start?
=======================

The **specflow.json** file must be added to the project, which describes the keyword language used and the *assembly* where the steps are located 
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
and add the **Copy if newer** property to it 


------
License
=======================
The **Molder** is released of the [MIT License 2021][license].
