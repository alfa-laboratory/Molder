using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AlfaBank.AFT.Core.Helpers;
using AlfaBank.AFT.Core.Infrastructure.Web;
using AlfaBank.AFT.Core.Model.Common.Support;
using AlfaBank.AFT.Core.Model.Context;
using AlfaBank.AFT.Core.Model.Web;
using AlfaBank.AFT.Core.Model.Web.Support;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit.Abstractions;

namespace AlfaBank.AFT.Core.Library.Web
{
    [Binding]
    [Scope(Tag = "web")]
    [Scope(Tag = "Web")]
    public class WebSteps
    {
        private readonly WebContext webContext;
        private readonly VariableContext variableContext;
        private readonly NavigationSupport navigationSupport;
        private readonly PageObjectSupport pageObjectSupport;
        private readonly ClickSupport clickSupport;
        private readonly TextBoxSupport textBoxSupport;
        private readonly MoveSupport moveSupport;
        private readonly FrameSupport frameSupport;
        private readonly FileSupport fileSupport;   
        private readonly KeySupport keySupport;
        private readonly CommandSupport commandSupport;
        private readonly DragAndDropSupport dragAndDropSupport;
        private readonly ElementSupport elementSupport;

        private readonly ITestOutputHelper consoleOutputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="webContext"/> class.
        /// Привязка шагов работы с web driver через контекст.
        /// </summary>
        /// <param name="webContext">Контекст для работы с web driver.</param>
        /// <param name="variableContext">Контекст для работы с переменными.</param>
        /// <param name="navigationSupport">Контекст для работы с навигацией.</param>
        /// <param name="pageObjectSupport">Контекст для работы с page object.</param>
        /// <param name="clickSupport">Контекст для работы с нажатиями.</param>
        /// <param name="textBoxSupport">Контекст для работы с textBox.</param>
        /// <param name="moveSupport">Контекст для работы с перемещениями.</param>
        /// <param name="dragAndDropSupport">Контекст для работы с перетаскиваниями.</param>
        /// <param name="frameSupport">Контекст для работы с фреймами.</param>
        /// <param name="fileSupport">Контекст для работы с файлами.</param>
        /// <param name="keySupport">Контекст для работы с клавиатурой.</param>
        /// <param name="commandSupport">Контекст для дополнительной проверки команд.</param>
        /// <param name="consoleOutputHelper">Capturing Output.</param>
        /// <param name="elementSupport">Контекст для работы с перетаскиваниями.</param>
        public WebSteps(WebContext webContext, VariableContext variableContext, 
            NavigationSupport navigationSupport, PageObjectSupport pageObjectSupport,
            ClickSupport clickSupport, TextBoxSupport textBoxSupport,
            MoveSupport moveSupport, DragAndDropSupport dragAndDropSupport,
            FrameSupport frameSupport, FileSupport fileSupport,
            KeySupport keySupport, CommandSupport commandSupport,
            ElementSupport elementSupport, ITestOutputHelper consoleOutputHelper)
        {
            this.webContext = webContext;
            this.navigationSupport = navigationSupport;
            this.pageObjectSupport = pageObjectSupport;
            this.clickSupport = clickSupport;
            this.textBoxSupport = textBoxSupport;
            this.moveSupport = moveSupport;
            this.dragAndDropSupport = dragAndDropSupport;
            this.frameSupport = frameSupport;
            this.fileSupport = fileSupport;
            this.keySupport = keySupport;
            this.commandSupport = commandSupport;
            this.variableContext = variableContext;
            this.consoleOutputHelper = consoleOutputHelper;
            this.elementSupport = elementSupport;
        }

        [StepArgumentTransformation]
        public Dictionary<string, Element> TableToDictionary(Table table)
        {
            var elements = table.CreateSet<Element>();

            return elements.ToDictionary(element => element.Name, element => new Element() {Name = element.Name, Classname = element.Classname, Id = element.Id, Xpath = element.Xpath});
        }

        [AfterScenario]
        public void AfterWebScenario()
        {
            this.webContext.Stop();
        }

        [StepDefinition("у меня есть элементы, присутствующие на веб-странице:")]
        public void HaveWebElements(Dictionary<string, Element> pageObject)
        {
            if (this.webContext.PageObject is null)
            {
                this.webContext.PageObject = pageObject;
            }
            else
            {
                foreach (var element in pageObject)
                {
                    this.webContext.PageObject.ContainsKey(element.Key).Should()
                        .BeFalse($"Элемент с именем {element.Key} уже есть в PageObject");
                    this.webContext.PageObject.Add(element.Key, element.Value);
                }
            }
        }

        [StepDefinition(@"я устанавливаю ожидание для веб-драйвера в ([0-9]+) сек")]
        public void SetTimeout(int sec)
        {
            sec.Should().NotBe(0, "Время не должно быть равно нулю.");
            sec.Should().BePositive("Указанное время отрицательно.");

            this.webContext.Timeout = sec * 1000;
        }

        [Given(@"я инициализирую браузер \""(.+)\"" версии \""(.+)\"" на удаленной машине \""(.+)\""")]
        public void InitRemoteWebDriver(BrowserType browser, string version, string url)
        {
            version.Should().NotBeEmpty("Версия не указана");
            url.Should().NotBeEmpty("Ссылка на удаленный хаб не указана");
            this.webContext.WebDriver.Should()
                .BeNull($"Браузер \"{browser}\" версии \"{version}\" на \"{url}\" уже инициализирован");

            this.webContext.Start(browser, remote: true, version: version, url: url);
        }

        [Given(@"я инициализирую браузер \""(.+)\""")]
        public void InitWebDriver(BrowserType browser)
        {
            this.webContext.WebDriver.Should()
                .BeNull($"Браузер \"{browser}\" уже инициализирован");
            this.webContext.Start(browser, null);
        }

        [Given(@"я инициализирую браузер \""(.+)\"" в headless режиме")]
        public void InitHeadlessWebDriver(BrowserType browser)
        {
            this.webContext.WebDriver.Should()
                .BeNull($"Браузер \"{browser}\" уже инициализирован");
            DriverOptions options;
            switch (browser)
            {
                case BrowserType.Chrome:
                {
                    options = new ChromeOptions();
                    ((ChromeOptions)options).AddArgument("--headless");
                    ((ChromeOptions)options).AddArgument("--disable-gpu");
                    break;
                }
                case BrowserType.Mozila:
                {
                    options = new FirefoxOptions();
                    ((FirefoxOptions)options).AddArgument("--headless");
                    ((FirefoxOptions)options).AddArgument("--disable-gpu");
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(browser), browser, null);
            }

            this.webContext.Start(browser, options);
        }

        [StepDefinition(@"установлено разрешение окна браузера ([0-9]+) X ([0-9]+)")]
        public void SetSizeBrowserWindow(int width, int height)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            this.webContext.WebDriver.Manage().Window.Size = new Size(width, height);
        }

        [StepDefinition(@"ОТЛАДКА: показать размер окна браузера")]
        public void GetSizeBrowserWindow()
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var browserSize = this.webContext.WebDriver.Manage().Window.Size;
            this.consoleOutputHelper.WriteLine($"[DEBUG] Browser Size: {browserSize.Width} X {browserSize.Height}");
        }

        [StepDefinition(@"я развернул веб-страницу на весь экран")]
        public void MaximizeWindow()
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            this.webContext.WebDriver.Manage().Window.Maximize();
        }

        [StepDefinition(@"я открываю веб-страницу \""(.+)\""")]
        public void OpenWebPage(string url)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            url.Should().NotBeEmpty("Url не задан").And.NotBeNullOrWhiteSpace("Url не задан");

            url = this.variableContext.ReplaceVariablesInXmlBody(url, (val) => System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)));
            this.commandSupport.SendCommand(() => this.navigationSupport.NavigateTo(url));
        }

        [StepDefinition(@"я обновляю веб-страницу")]
        public void RefreshWebPage()
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            this.commandSupport.SendCommand(() => this.navigationSupport.Refresh());
        }

        [StepDefinition(@"я сохраняю адрес активной веб-страницы в переменную \""(.+)\""")]
        public void SaveUrlActiveWebPage(string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var url = this.webContext.WebDriver.Url;
            this.variableContext.SetVariable(varName, url.GetType(), url);
        }

        [StepDefinition(@"я сохраняю заголовок активной веб-страницы в переменную \""(.+)\""")]
        public void SaveTitleActiveWebPage(string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var title = this.webContext.WebDriver.Title;
            this.variableContext.SetVariable(varName, title.GetType(), title);
        }

        [StepDefinition(@"я закрываю веб-страницу")]
        public void CloseWebPage()
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            this.commandSupport.SendCommand(() => this.webContext.WebDriver.Close());
        }

        [StepDefinition(@"веб-страница проскроллена до элемента \""(.+)\""")]
        public void ScrollToElement(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            this.commandSupport.SendCommand(() => moveSupport.MoveToElement(parameter));
        }

        [StepDefinition(@"совершен переход в начало веб-страницы")]
        public void GoPageTop()
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            this.commandSupport.SendCommand(() => this.moveSupport.PageTop());
        }

        [StepDefinition(@"совершен переход в конец веб-страницы")]
        public void GoPageDown()
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            this.commandSupport.SendCommand(() => this.moveSupport.PageDown());
        }

        [StepDefinition(@"выполнено нажатие на элемент \""(.+)\"" на веб-странице")]
        public void ClickToWebElement(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            this.commandSupport.SendCommand(() => this.clickSupport.Click(parameter));
        }

        [StepDefinition(@"выполнено двойное нажатие на элемент \""(.+)\"" на веб-странице")]
        public void DoubleClickToWebElement(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            this.commandSupport.SendCommand(() => this.clickSupport.DoubleClick(parameter));
        }

        [StepDefinition(@"выполнено нажатие с удержанием на элементе \""(.+)\"" на веб-странице")]
        public void ClickAndHoldToWebElement(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            this.commandSupport.SendCommand(() => this.clickSupport.ClickAndHold(parameter));
        }

        [StepDefinition(@"я ввожу в поле \""(.+)\"" веб-страницы значение \""(.+)\""")]
        public void InputValueIntoField(string element, string value)
        {
            value.Should().NotBeEmpty("Версия не указана");
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            this.commandSupport.SendCommand(() => this.textBoxSupport.SetTextWithoutClear(parameter, value));
        }

        [StepDefinition(@"я ввожу в поле \""(.+)\"" веб-страницы зашифрованное значение \""(.+)\""")]
        public void InputEncriptedValueIntoField(string element, string value)
        {
            value.Should().NotBeEmpty("Версия не указана");
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            this.commandSupport.SendCommand(() => this.textBoxSupport.SetTextWithoutClear(parameter, new Encryptor().Decrypt(value)));
        }

        [StepDefinition(@"я ввожу в поле \""(.+)\"" веб-страницы значение из переменной \""(.+)\""")]
        public void InputVarNameValueIntoField(string element, string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var value = this.variableContext.GetVariableValueText(varName);
            this.commandSupport.SendCommand(() => this.textBoxSupport.SetTextWithoutClear(parameter, value));
        }

        [StepDefinition(@"я очищаю поле \""(.+)\"" веб-страницы")]
        public void ClearField(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            this.commandSupport.SendCommand(() => this.textBoxSupport.Clear(parameter));
        }

        [StepDefinition(@"выполнен переход на вкладку номер ([1-9]+)")]
        public void GoToTabByNumber(int number)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            number.Should().BePositive("Неверно задан номер вкладки.");
            var actualNumber = number - 1;
            actualNumber.Should().BeLessThan(this.webContext.WebDriver.WindowHandles.Count(),
                "Выбранной вкладки не существует.");

            this.commandSupport.SendCommand(() => 
                this.webContext.WebDriver.SwitchTo().Window(this.webContext.WebDriver.WindowHandles[actualNumber]));
        }

        [StepDefinition(@"я перемещаюсь к элементу \""(.+)\"" на веб-странице")]
        public void MoveToElement(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            this.commandSupport.SendCommand(() => this.moveSupport.MoveToElement(parameter));
        }

        [StepDefinition(@"я создаю переменную \""(.+)\"" с текстом из элемента \""(.+)\"" на веб-странице")]
        public void SetVariableValueOfElementText(string varName, string element)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var text = this.commandSupport.SendCommand(() => textBoxSupport.GetText(parameter));

            this.variableContext.SetVariable(varName, typeof(string), text.ToString());
        }

        [StepDefinition(@"я создаю переменную \""(.+)\"" со значением из элемента \""(.+)\"" на веб-странице")]
        public void SetVariableValueOfElementValue(string varName, string element)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var value = this.commandSupport.SendCommand(() => this.textBoxSupport.GetValue(parameter));

            this.variableContext.SetVariable(varName, typeof(string), value.ToString()
            );
        }

        [StepDefinition(@"я создаю переменную \""(.+)\"" с текстом из диалогового окна на веб-странице")]
        public void SetVariableValueOfAlertText(string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            Action act = () => this.webContext.WebDriver.SwitchTo().Alert();
            act.Should().NotThrow<NoAlertPresentException>();

            var alert = this.commandSupport.SendCommand(() => this.webContext.WebDriver.SwitchTo().Alert());
            alert.Should().NotBeNull($"Диалоговое окно не найдено");
            this.variableContext.SetVariable(varName, typeof(string), ((IAlert)alert).Text);
        }

        [StepDefinition(@"выполнено нажатие на \""(Accept|Dismiss)\"" в диалоговом окне на веб-странице")]
        public void AlertClick(AlertKeys key)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            Action act = () => this.webContext.WebDriver.SwitchTo().Alert();
            act.Should().NotThrow<NoAlertPresentException>();

            var alert = this.commandSupport.SendCommand(() => this.webContext.WebDriver.SwitchTo().Alert());
            alert.Should().NotBeNull($"Диалоговое окно не найдено");

            switch (key)
            {
                case AlertKeys.Accept:
                    ((IAlert)alert).Accept();
                    break;
                case AlertKeys.Dismiss:
                    ((IAlert)alert).Dismiss();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(key), key, null);
            }
        }

        [StepDefinition(@"я сохраняю текст элемента \""(.+)\"" веб-страницы как JSON в переменную \""(.+)\""")]
        public void StoreWebElementTextInVariable(string element, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var text = this.commandSupport.SendCommand(() => this.textBoxSupport.GetText(parameter));
            var json = JObject.Parse(text.ToString());

            this.variableContext.SetVariable(varName, typeof(JObject), json);
        }

        [StepDefinition(@"я сохраняю значение элемента \""(.+)\"" веб-страницы как JSON в переменную \""(.+)\""")]
        public void StoreWebElementValueInVariable(string element, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var value = this.commandSupport.SendCommand(() => this.textBoxSupport.GetValue(parameter));
            var json = JObject.Parse(value.ToString());

            this.variableContext.SetVariable(varName, typeof(JObject), json);
        }

        [StepDefinition(@"я сохраняю значение атрибута \""(.+)\"" элемента \""(.+)\"" веб-страницы в переменную \""(.+)\""")]
        public void StoreWebElementValueOfAttributeInVariable(string attributeName, string element, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            this.webContext.WebDriver.Should()
            .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var value = this.commandSupport.SendCommand(() => this.elementSupport.GetAttribute(parameter, attributeName));

            this.variableContext.SetVariable(varName, typeof(string), value);
        }

        [StepDefinition(@"выполнен переход на фрейм номер ([0-9]+)")]
        public void GoToFrameByNumber(int number)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            number.Should().BePositive("Неверно задан номер вкладки.");

            this.commandSupport.SendCommand(() => this.frameSupport.SwitchFrameBy(number));
        }

        [StepDefinition(@"выполнен переход на основной контент")]
        public void GoToFrameDefaultContent()
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            this.commandSupport.SendCommand(() => this.frameSupport.SwitchToDefaultContent());
        }
        
        [StepDefinition(@"выполнен переход на родительский фрейм")]
        public void GoToparentFrame()
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            this.commandSupport.SendCommand(() => this.frameSupport.SwitchToParentFrame());
        }

        [StepDefinition(@"выполнен переход на фрейм \""(.+)\""")]
        public void GoToFrameBy(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            this.commandSupport.SendCommand(() => this.frameSupport.SwitchFrameBy(parameter));
        }

        [StepDefinition(@"загружен файл с именем \""(.+)\"" в элемент \""(.+)\"" на веб-странице")]
        public void LoadFileToElement(string filename, string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var path = fileSupport.GetValidFilepath(filename);
            path.Should().NotBeNull($"Файла \"{filename}\" не существует");

            this.commandSupport.SendCommand(() => textBoxSupport.SetTextWithoutClear(parameter, path));
        }

        [StepDefinition(@"нажата клавиша \""(.+)\"" на элементе \""(.+)\"" на веб-странице")]
        public void PressKeyToWebElement(string key, string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            this.commandSupport.SendCommand(() => this.keySupport.PressKey(parameter, key));
        }

        [StepDefinition(@"я нажимаю сочетание клавиш \""(.+)\"" на элементе \""(.+)\"" веб-страницы")]
        public void PressKeysToWebElement(string key, string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            this.commandSupport.SendCommand(() => this.keySupport.PressKeysBy(parameter, key));
        }

        [StepDefinition(@"я нажимаю сочетание клавиш \""(.+)\"" на веб-странице")]
        public void PressKeysToWebPage(string key)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            this.commandSupport.SendCommand(() => this.keySupport.PressKeys(key));
        }
    }
}