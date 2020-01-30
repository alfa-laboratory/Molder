using System;
using System.Diagnostics;
using AlfaBank.AFT.Core.Helpers;
using AlfaBank.AFT.Core.Infrastructure.Web;
using AlfaBank.AFT.Core.Infrastructures.Web;
using AlfaBank.AFT.Core.Model.Context;
using AlfaBank.AFT.Core.Models.Context;
using AlfaBank.AFT.Core.Models.Web.Elements;
using AlfaBank.AFT.Core.Supports;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;

namespace AlfaBank.AFT.Core.Library.Web
{
    [Binding]
    [Scope(Tag = "web")]
    [Scope(Tag = "Web")]
    public class WebSteps
    {
        private readonly VariableContext variableContext;
        private readonly WebContext webContext;
        private readonly CommandSupport commandSupport;
        private readonly DriverSupport driverSupport;
        private readonly ConfigContext config;
        private readonly FileSupport fileSupport;

        /// <summary>
        /// Initializes a new instance of the <see cref="webContext"/> class.
        /// Привязка шагов работы с web driver через контекст.
        /// </summary>
        /// <param name="webContext">Контекст для работы с web driver.</param>
        /// <param name="variableContext">Контекст для работы с переменными.</param>
        /// <param name="driverSupport"></param>
        /// <param name="fileSupport">Контекст для работы с файлами.</param>
        /// <param name="commandSupport">Контекст для дополнительной проверки команд.</param>
        public WebSteps(WebContext webContext,
                            CommandSupport commandSupport, DriverSupport driverSupport, FileSupport fileSupport,
                            VariableContext variableContext, ConfigContext config)
        {
            this.webContext = webContext;
            this.commandSupport = commandSupport;
            this.driverSupport = driverSupport;
            this.fileSupport = fileSupport;
            this.config = config;
            this.variableContext = variableContext;
        }

        [BeforeScenario]
        public void BeforeWebScenario()
        {
            DisposeDriverService.TestRunStartTime = DateTime.Now;
        }

        [AfterScenario]
        public void AfterWebScenario() => webContext.Dispose();

        [Given(@"я устанавливаю ожидание для веб-драйвера в ([0-9]+) сек")]
        public void SetTimeout(int sec)
        {
            sec.Should().NotBe(0, "Время не должно быть равно нулю.");
            sec.Should().BePositive("Указанное время отрицательно.");

            this.webContext.SetTimeout(sec * 1000);
        }

        [Given(@"я инициализирую браузер \""(.+)\"" версии \""(.+)\"" на удаленной машине \""(.+)\""")]
        public void InitRemoteDriver(BrowserType browser, string version, string url)
        {
            version.Should().NotBeEmpty("Версия не указана");
            url.Should().NotBeEmpty("Ссылка на удаленный хаб не указана");

            this.webContext.Start(browser, remote: true, version: version, url: url);
        }

        [Given(@"я инициализирую браузер \""(.+)\""")]
        public void InitWebDriver(BrowserType browser)
        {
            this.webContext.Start(browser, null);
        }

        [Given(@"я инициализирую браузер \""(.+)\"" в headless режиме")]
        public void InitHeadlessWebDriver(BrowserType browser)
        {
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
            this.webContext.SetSizeBrowser(width, height);
        }

        [StepDefinition(@"ОТЛАДКА: показать размер окна браузера")]
        public void GetSizeBrowserWindow()
        {
            var browserSize = this.webContext.GetSizeBrowser();
            Debug.WriteLine($"[DEBUG] Browser Size: {browserSize.Width} X {browserSize.Height}");
        }

        [StepDefinition(@"я развернул веб-страницу на весь экран")]
        public void MaximizeWindow()
        {
            this.webContext.Maximize();
        }

        [Given(@"я перехожу на страницу ""(.+)""")]
        public void GoToUrl(string pageName)
        {
            this.commandSupport.SendCommand(() => this.webContext.SetCurrentPageBy(pageName, true));
        }

        [StepDefinition(@"я обновляю текущую страницу на \""(.+)\""")]
        public void UpdateCurrentPage(string pageName)
        {
            this.commandSupport.SendCommand(() => this.webContext.SetCurrentPageBy(pageName));
        }

        [StepDefinition(@"я обновляю веб-страницу")]
        public void Refresh()
        {
            this.commandSupport.SendCommand(() => this.webContext.GetCurrentPage().Refresh());
        }

        [StepDefinition(@"ОТЛАДКА: я получаю имя текущей страницы")]
        public void GetNameCurrentPage()
        {
            var name = this.webContext.GetCurrentPage().Name;
            Trace.Write(name);
        }

        [StepDefinition(@"я сохраняю адрес активной веб-страницы в переменную \""(.+)\""")]
        public void SaveUrlActiveWebPage(string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var url = this.webContext.GetCurrentPage().Url;
            this.variableContext.SetVariable(varName, url.GetType(), url);
        }

        [StepDefinition(@"я сохраняю заголовок активной веб-страницы в переменную \""(.+)\""")]
        public void SaveTitleActiveWebPage(string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var title = this.webContext.GetCurrentPage().Title;
            this.variableContext.SetVariable(varName, title.GetType(), title);
        }

        [StepDefinition(@"я закрываю веб-страницу")]
        public void CloseWebPage()
        {
            this.commandSupport.SendCommand(() => this.webContext.GetCurrentPage().Close());
        }

        [StepDefinition(@"я перемещаюсь к элементу \""(.+)\"" на веб-странице")]
        public void ScrollToElement(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);
            this.commandSupport.SendCommand(() => element.MoveTo());
        }

        [StepDefinition(@"совершен переход в начало веб-страницы")]
        public void GoPageTop()
        {
            this.commandSupport.SendCommand(() => this.webContext.GetCurrentPage().PageTop());
        }

        [StepDefinition(@"совершен переход в конец веб-страницы")]
        public void GoPageDown()
        {
            this.commandSupport.SendCommand(() => this.webContext.GetCurrentPage().PageDown());
        }

        [StepDefinition(@"выполнено нажатие на элемент \""(.+)\"" на веб-странице")]
        public void ClickToWebElement(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            if (element is ClickElement)
            {
                this.commandSupport.SendCommand(() => ((ClickElement)element).Click());
            }
            else
            {
                throw new ArgumentException($"Элемент '{name}' имеет отличный тип от click");
            }
        }

        [StepDefinition(@"выполнено двойное нажатие на элемент \""(.+)\"" на веб-странице")]
        public void DoubleClickToWebElement(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            if (element is ClickElement)
            {
                this.commandSupport.SendCommand(() => ((ClickElement)element).DoubleClick());
            }
            else
            {
                throw new ArgumentException($"Элемент '{name}' имеет отличный тип от click");
            }
        }

        [StepDefinition(@"выполнено нажатие с удержанием на элементе \""(.+)\"" на веб-странице")]
        public void ClickAndHoldToWebElement(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            if (element is ClickElement)
            {
                this.commandSupport.SendCommand(() => ((ClickElement)element).ClickAndHold());
            }
            else
            {
                throw new ArgumentException($"Элемент '{name}' имеет отличный тип от click");
            }
        }

        [StepDefinition(@"я ввожу в поле \""(.+)\"" веб-страницы значение \""(.+)\""")]
        public void InputValueIntoField(string name, string value)
        {
            value.Should().NotBeEmpty("Значение не указано");
            var element = this.webContext.GetCurrentPage().GetElementByName(name);
            if (element is InputElement)
            {
                this.commandSupport.SendCommand(() => ((InputElement)element).SetText(value));
            }
            else
            {
                throw new ArgumentException($"Элемент '{name}' имеет отличный тип от input");
            }
        }

        [StepDefinition(@"я ввожу в поле \""(.+)\"" веб-страницы зашифрованное значение \""(.+)\""")]
        public void InputEncriptedValueIntoField(string name, string value)
        {
            value.Should().NotBeEmpty("Значение не указано");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            if (element is InputElement)
            {
                this.commandSupport.SendCommand(() => ((InputElement)element).SetText(new Encryptor().Decrypt(value)));
            }
            else
            {
                throw new ArgumentException($"Элемент '{name}' имеет отличный тип от input");
            }
        }

        [StepDefinition(@"я ввожу в поле \""(.+)\"" веб-страницы значение из переменной \""(.+)\""")]
        public void InputVarNameValueIntoField(string name, string varName)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);
            var value = this.variableContext.GetVariableValueText(varName);

            if (element is InputElement)
            {
                this.commandSupport.SendCommand(() => ((InputElement)element).SetText(value));
            }
            else
            {
                throw new ArgumentException($"Элемент '{name}' имеет отличный тип от input");
            }
        }

        [StepDefinition(@"я очищаю поле \""(.+)\"" веб-страницы")]
        public void ClearField(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            if (element is InputElement)
            {
                this.commandSupport.SendCommand(() => ((InputElement)element).Clear());
            }
            else
            {
                throw new ArgumentException($"Элемент '{name}' имеет отличный тип от input");
            }
        }

        [StepDefinition(@"выполнен переход на вкладку номер ([1-9]+)")]
        public void GoToTabByNumber(int number)
        {
            (number--).Should().BePositive("Неверно задан номер вкладки.");
            number.Should().BeLessOrEqualTo(this.webContext.GetCountTabs(),
                "Выбранной вкладки не существует.");

            this.commandSupport.SendCommand(() => this.webContext.GoToTab(number));
        }

        [StepDefinition(@"я создаю переменную \""(.+)\"" с текстом из элемента \""(.+)\"" на веб-странице")]
        public void SetVariableValueOfElementText(string varName, string name)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var text = this.commandSupport.SendCommand(() => element.GetText());

            this.variableContext.SetVariable(varName, typeof(string), text.ToString());
        }

        [StepDefinition(@"я создаю переменную \""(.+)\"" со значением из элемента \""(.+)\"" на веб-странице")]
        public void SetVariableValueOfElementValue(string varName, string name)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var value = this.commandSupport.SendCommand(() => element.GetValue());

            this.variableContext.SetVariable(varName, typeof(string), value);
        }

        [StepDefinition(@"я создаю переменную \""(.+)\"" с текстом из диалогового окна на веб-странице")]
        public void SetVariableValueOfAlertText(string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");

            var alert = driverSupport.GetAlert();
            alert.Should().NotBeNull($"Диалоговое окно не найдено");

            this.variableContext.SetVariable(varName, typeof(string), alert.Text);
        }

        [StepDefinition(@"выполнено нажатие на \""(Accept|Dismiss)\"" в диалоговом окне на веб-странице")]
        public void AlertClick(AlertKeys key)
        {
            var alert = driverSupport.GetAlert();
            alert.Should().NotBeNull($"Диалоговое окно не найдено");

            switch (key)
            {
                case AlertKeys.Accept:
                    alert.Accept();
                    break;
                case AlertKeys.Dismiss:
                    alert.Dismiss();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(key), key, null);
            }
        }

        [StepDefinition(@"я сохраняю значение атрибута \""(.+)\"" элемента \""(.+)\"" веб-страницы в переменную \""(.+)\""")]
        public void StoreWebElementValueOfAttributeInVariable(string attribute, string name, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeFalse($"Переменная '{varName}' уже существует");
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var value = this.commandSupport.SendCommand(() => element.GetAttribute(attribute));

            this.variableContext.SetVariable(varName, typeof(string), value);
        }

        [StepDefinition(@"выполнен переход на фрейм номер ([0-9]+)")]
        public void GoToFrameByNumber(int number)
        {
            number.Should().BePositive("Неверно задан номер вкладки.");

            this.commandSupport.SendCommand(() => this.driverSupport.SwitchFrameBy(number));
        }

        [StepDefinition(@"выполнен переход на фрейм \""(.+)\""")]
        public void GoToFrameBy(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            if (element is FrameElement)
            {
                this.commandSupport.SendCommand(() => ((FrameElement)element).Switch());
            }
            else
            {
                throw new ArgumentException($"Элемент '{name}' имеет отличный тип от frame");
            }
        }

        [StepDefinition(@"выполнен переход на основной контент")]
        public void GoToFrameDefaultContent()
        {
            this.commandSupport.SendCommand(() => this.driverSupport.SwitchToDefaultContent());
        }

        [StepDefinition(@"выполнен переход на родительский фрейм")]
        public void GoToparentFrame()
        {
            this.commandSupport.SendCommand(() => this.driverSupport.SwitchToParentFrame());
        }

        [StepDefinition(@"загружен файл с именем \""(.+)\"" в элемент \""(.+)\"" на веб-странице")]
        public void LoadFileToElement(string filename, string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var path = fileSupport.GetValidFilepath(filename);
            path.Should().NotBeNull($"Файла \"{filename}\" не существует");

            if (element is FileElement)
            {
                this.commandSupport.SendCommand(() => ((FileElement)element).SetText(path));
            }
            else
            {
                throw new ArgumentException($"Элемент '{name}' имеет отличный тип от input с типом file");
            }
        }

        [StepDefinition(@"нажата клавиша \""(.+)\"" на элементе \""(.+)\"" на веб-странице")]
        public void PressKeyToWebElement(string key, string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            this.commandSupport.SendCommand(() => element.PressKey(key));
        }
    }
}