using EvidentInstruction.Controllers;
using EvidentInstruction.Web.Controllers;
using EvidentInstruction.Web.Models.PageObject.Models.Abstracts.Elements;
using EvidentInstruction.Web.Models.PageObject.Models.Blocks;
using EvidentInstruction.Web.Models.PageObject.Models.Elements;
using EvidentInstruction.Web.Models.Settings;
using FluentAssertions;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace EvidentInstruction.Web.Steps
{
    [Binding]
    public class Steps : TechTalk.SpecFlow.Steps
    {
        private VariableController variableController;

        public Steps(VariableController controller)
        {
            variableController = controller;
            BrowserController.SetVariables(variableController);
        }

        [StepArgumentTransformation]
        public BrowserSetting CreateBrowserSettings(Table tableSettings)
        {
            return tableSettings.CreateInstance<BrowserSetting>();
        }

        [Given(@"я инициализирую браузер")]
        public void StartBrowser()
        {
            BrowserController.GetBrowser();
        }

        [Given(@"я инициализирую браузер с настройками:")]
        public void StartBrowser(BrowserSetting settings)
        {
            BrowserController.Create(settings);
        }

        [StepDefinition(@"установлено разрешение окна браузера ([0-9]+) X ([0-9]+)")]
        public void SetSizeBrowserWindow(int width, int height)
        {
            BrowserController.GetBrowser().WindowSize(width, height);
        }

        [StepDefinition(@"я развернул веб-страницу на весь экран")]
        public void MaximizeWindow()
        {
            BrowserController.GetBrowser().Maximize();
        }

        [Given(@"я перехожу на страницу \""(.+)\""")]
        public void SetCurrentPage(string name)
        {
            BrowserController.GetBrowser().SetCurrentPage(name);
        }

        [StepDefinition(@"я обновляю текущую страницу на \""(.+)\""")]
        public void UpdateCurrentPage(string name)
        {
            BrowserController.GetBrowser().UpdateCurrentPage(name);
        }

        [StepDefinition(@"я обновляю веб-страницу")]
        public void Refresh()
        {
            BrowserController.GetBrowser().Refresh();
        }

        [StepDefinition(@"я сохраняю адрес активной веб-страницы в переменную \""(.+)\""")]
        public void SaveUrlActivePage(string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var url = BrowserController.GetBrowser().Url;
            this.variableController.SetVariable(varName, url.GetType(), url);
        }

        [StepDefinition(@"я сохраняю заголовок активной веб-страницы в переменную \""(.+)\""")]
        public void SaveTitleActiveWebPage(string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var title = BrowserController.GetBrowser().Title;
            this.variableController.SetVariable(varName, title.GetType(), title);
        }

        [StepDefinition(@"я закрываю веб-страницу")]
        public void CloseWebPage()
        {
            BrowserController.GetBrowser().Close();
        }

        [StepDefinition(@"я закрываю браузер")]
        public void CloseBrowser()
        {
            BrowserController.Quit();
        }

        [StepDefinition(@"я перемещаюсь к элементу \""(.+)\"" на веб-странице")]
        public void ScrollToElement(string name)
        {
            BrowserController.GetBrowser().GetCurrentPage().GetElement(name).Move();
        }

        [StepDefinition(@"совершен переход в начало веб-страницы")]
        public void GoPageTop()
        {
            BrowserController.GetBrowser().GetCurrentPage().PageTop();
        }

        [StepDefinition(@"совершен переход в конец веб-страницы")]
        public void GoPageDown()
        {
            BrowserController.GetBrowser().GetCurrentPage().PageDown();
        }

        [StepDefinition(@"выполнено нажатие на элемент \""(.+)\"" на веб-странице")]
        public void ClickToWebElement(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is BaseClick).Should().BeTrue($"элемент \"{name}\" имеет отличный от Click профиль");
            (element as BaseClick).Click();
        }

        [StepDefinition(@"выполнено двойное нажатие на элемент \""(.+)\"" на веб-странице")]
        public void DoubleClickToWebElement(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is BaseClick).Should().BeTrue($"элемент \"{name}\" имеет отличный от Click профиль");
            (element as BaseClick).DoubleClick();
        }

        [StepDefinition(@"выполнено нажатие с удержанием на элементе \""(.+)\"" на веб-странице")]
        public void ClickAndHoldToWebElement(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is BaseClick).Should().BeTrue($"элемент \"{name}\" имеет отличный от Click профиль");
            (element as BaseClick).ClickAndHold();
        }

        [StepDefinition(@"я ввожу в поле \""(.+)\"" веб-страницы значение \""(.+)\""")]
        public void InputValueIntoField(string name, string text)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Input).Should().BeTrue($"элемент \"{name}\" имеет отличный от Input профиль");
            (element as Input).SetText(text);
        }

        [StepDefinition(@"я ввожу в поле \""(.+)\"" веб-страницы значение из переменной \""(.+)\""")]
        public void InputVarNameValueIntoField(string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var text = this.variableController.GetVariableValueText(varName);

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Input).Should().BeTrue($"элемент \"{name}\" имеет отличный от Input профиль");
            (element as Input).SetText(text);
        }

        #region Работа с Dropdown 

        [StepDefinition(@"я выбираю в поле \""(.+)\"" веб-страницы значение \""(.+)\""")]
        public void DropdownIntoValue(string name, string value)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Dropdown).Should().BeTrue($"элемент \"{name}\" имеет отличный от Dropdown профиль");
            (element as Dropdown).SelectByValue(value);
        }

        [StepDefinition(@"я выбираю в поле \""(.+)\"" веб-страницы значение из переменной \""(.+)\""")]
        public void DropdownIntoVariableValue(string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var value = this.variableController.GetVariableValueText(varName);

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Dropdown).Should().BeTrue($"элемент \"{name}\" имеет отличный от Dropdown профиль");
            (element as Dropdown).SelectByValue(value);
        }

        [StepDefinition(@"я выбираю в поле \""(.+)\"" веб-страницы текст \""(.+)\""")]
        public void DropdownIntoText(string name, string text)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Dropdown).Should().BeTrue($"элемент \"{name}\" имеет отличный от Dropdown профиль");
            (element as Dropdown).SelectByText(text);
        }

        [StepDefinition(@"я выбираю в поле \""(.+)\"" веб-страницы значение из переменной \""(.+)\""")]
        public void DropdownIntoVariableText(string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var text = this.variableController.GetVariableValueText(varName);

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Dropdown).Should().BeTrue($"элемент \"{name}\" имеет отличный от Dropdown профиль");
            (element as Dropdown).SelectByText(text);
        }

        [StepDefinition(@"я выбираю в поле \""(.+)\"" веб-страницы номер значения \""(.+)\""")]
        public void DropdownIntoIndex(string name, int index)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Dropdown).Should().BeTrue($"элемент \"{name}\" имеет отличный от Dropdown профиль");
            (element as Dropdown).SelectByIndex(index);
        }

        #endregion

        #region Проверка работы с блоками

        [StepDefinition(@"в блоке \""(.+)\"" я ввожу в поле \""(.+)\"" веб-страницы значение \""(.+)\""")]
        public void InputValueIntoFieldInBlock(string block, string name, string text)
        {
            var blockElement = BrowserController.GetBrowser().GetCurrentPage().GetBlock(block);
            var element = blockElement.GetElement(name);
            (element is Input).Should().BeTrue($"элемент \"{name}\" имеет отличный от Input профиль");
            (element as Input).SetText(text);
        }

        [StepDefinition(@"в блоке \""(.+)\"" я ввожу в поле \""(.+)\"" веб-страницы значение из переменной \""(.+)\""")]
        public void InputVarNameValueIntoFieldInBlock(string block, string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var text = this.variableController.GetVariableValueText(varName);

            var blockElement = BrowserController.GetBrowser().GetCurrentPage().GetBlock(block);
            var element = blockElement.GetElement(name);

            (element is Input).Should().BeTrue($"элемент \"{name}\" имеет отличный от Input профиль");
            (element as Input).SetText(text);
        }

        [StepDefinition(@"в блоке \""(.+)\"" выполнено нажатие на элемент \""(.+)\"" на веб-странице")]
        public void ClickToWebElementInBlock(string block, string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetBlock(block).GetElement(name);
            (element is BaseClick).Should().BeTrue($"элемент \"{name}\" имеет отличный от Click профиль");
            (element as BaseClick).Click();
        }


        [Then(@"в блоке \""(.+)\"" текст элемента \""(.+)\"" пустой")]
        [Then(@"в блоке \""(.+)\"" текст элемента \""(.+)\"" пустая строка")]
        [Then(@"в блоке \""(.+)\"" текст элемента \""(.+)\"" равен пустой строке")]
        [Then(@"в блоке \""(.+)\"" текст элемента \""(.+)\"" равен null")]
        [Then(@"в блоке \""(.+)\"" текст элемента \""(.+)\"" заполнен пробелами")]
        public void WebElementInBlockTextIsEmpty(string block, string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetBlock(block).GetElement(name);
            element.Text.Should().BeNullOrWhiteSpace($"текст элемента \"{name}\" не пустой");
        }

        [Then(@"в блоке \""(.+)\"" текст элемента \""(.+)\"" заполнен")]
        [Then(@"в блоке \""(.+)\"" текст элемента \""(.+)\"" не равно null")]
        [Then(@"в блоке \""(.+)\"" текст элемента \""(.+)\"" содержит символы, отличные от пробелов")]
        public void WebElementInBlockTextIsNotEmpty(string block, string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetBlock(block).GetElement(name);
            element.Text.Should().NotBeNullOrWhiteSpace($"текст элемента \"{name}\" пустой или не существует");
        }

        #endregion

        [StepDefinition(@"я очищаю поле \""(.+)\"" веб-страницы")]
        public void ClearField(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Input).Should().BeTrue($"элемент \"{name}\" имеет отличный от Input профиль");
            (element as Input).Clear();
        }

        [StepDefinition(@"выполнен переход на вкладку номер ([1-9]+)")]
        public void GoToTabByNumber(int number)
        {
            (number--).Should().BePositive("неверно задан номер вкладки");
            number.Should().BeLessOrEqualTo(BrowserController.GetBrowser().Tabs,
                "выбранной вкладки не существует");

            BrowserController.GetBrowser().SwitchTo(number);
        }

        [StepDefinition(@"я создаю переменную \""(.+)\"" с текстом из элемента \""(.+)\"" на веб-странице")]
        public void SetVariableValueOfElementText(string varName, string name)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            this.variableController.SetVariable(varName, element.Text.GetType(), element.Text);
        }

        [StepDefinition(@"я создаю переменную \""(.+)\"" со значением из элемента \""(.+)\"" на веб-странице")]
        public void SetVariableValueOfElementValue(string varName, string name)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            this.variableController.SetVariable(varName, element.Value.GetType(), element.Value);
        }


        #region Проверка работы с Alert
        [StepDefinition(@"я создаю переменную \""(.+)\"" с текстом из диалогового окна на веб-странице")]
        public void SetVariableValueOfAlertText(string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");

            var alert = BrowserController.GetBrowser().Alert();
            this.variableController.SetVariable(varName, alert.Text.GetType(), alert.Text);
        }

        [StepDefinition(@"выполнено нажатие на \""(Accept|Dismiss)\"" в диалоговом окне на веб-странице")]
        public void AlertClick(string key)
        {
            var alert = BrowserController.GetBrowser().Alert();

            switch (key)
            {
                case "Accept":
                    alert.Accept();
                    break;
                case "Dismiss":
                    alert.Dismiss();
                    break;
                default:
                    /// добавить текст ошибки
                    throw new ArgumentOutOfRangeException("Key for alert is not valid");
            }
        }

        [Then(@"я убеждаюсь, что на веб-странице появилось диалоговое окно")]
        public void CheckAlert()
        {
            var alert = BrowserController.GetBrowser().Alert();
            alert.Should().NotBeNull("диалоговое окно не найдено");
        }
        #endregion

        [StepDefinition(@"я сохраняю значение атрибута \""(.+)\"" элемента \""(.+)\"" веб-страницы в переменную \""(.+)\""")]
        public void StoreWebElementValueOfAttributeInVariable(string attribute, string name, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            var attributeValue = element.GetAttribute(attribute);
            this.variableController.SetVariable(varName, attributeValue.GetType(), attributeValue);
        }

        #region Работа с фреймом
        [StepDefinition(@"выполнен переход на фрейм \""(.+)\""")]
        public void GoToFrameBy(string name)
        {
            BrowserController.GetBrowser().GetCurrentPage().GetFrame(name);
        }

        [StepDefinition(@"выполнен переход на основной контент")]
        public void GoToFrameDefaultContent()
        {
            BrowserController.GetBrowser().GetCurrentPage().GetDefaultFrame();
        }

        [StepDefinition(@"выполнен переход на родительский фрейм")]
        public void GoToparentFrame()
        {
            BrowserController.GetBrowser().GetCurrentPage().GetParentFrame();
        }

        [StepDefinition(@"в фрейме \""(.+)\"" я ввожу в поле \""(.+)\"" веб-страницы значение \""(.+)\""")]
        public void InputValueIntoFieldInFrame(string frame, string name, string text)
        {
            var frameElement = BrowserController.GetBrowser().GetCurrentPage().GetFrame(frame);
            var element = frameElement.GetElement(name);
            (element is Input).Should().BeTrue($"элемент \"{name}\" имеет отличный от Input профиль");
            (element as Input).SetText(text);
        }

        [StepDefinition(@"в фрейме \""(.+)\"" я ввожу в поле \""(.+)\"" веб-страницы значение из переменной \""(.+)\""")]
        public void InputVarNameValueIntoFieldInFrame(string frame, string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var text = this.variableController.GetVariableValueText(varName);

            var frameElement = BrowserController.GetBrowser().GetCurrentPage().GetFrame(frame);
            var element = frameElement.GetElement(name);

            (element is Input).Should().BeTrue($"элемент \"{name}\" имеет отличный от Input профиль");
            (element as Input).SetText(text);
        }

        [StepDefinition(@"в фрейме \""(.+)\"" выполнено нажатие на элемент \""(.+)\"" на веб-странице")]
        public void ClickToWebElementInFrame(string frame, string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetFrame(frame).GetElement(name);
            (element is BaseClick).Should().BeTrue($"элемент \"{name}\" имеет отличный от Click профиль");
            (element as BaseClick).Click();
        }

        [Then(@"в фрейме \""(.+)\"" текст элемента \""(.+)\"" пустой")]
        [Then(@"в фрейме \""(.+)\"" текст элемента \""(.+)\"" пустая строка")]
        [Then(@"в фрейме \""(.+)\"" текст элемента \""(.+)\"" равен пустой строке")]
        [Then(@"в фрейме \""(.+)\"" текст элемента \""(.+)\"" равен null")]
        [Then(@"в фрейме \""(.+)\"" текст элемента \""(.+)\"" заполнен пробелами")]
        public void WebElementInFrameTextIsEmpty(string frame, string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetFrame(frame).GetElement(name);
            element.Text.Should().BeNullOrWhiteSpace($"текст элемента \"{name}\" не пустой");
        }

        [Then(@"в фрейме \""(.+)\"" текст элемента \""(.+)\"" заполнен")]
        [Then(@"в фрейме \""(.+)\"" текст элемента \""(.+)\"" не равно null")]
        [Then(@"в фрейме \""(.+)\"" текст элемента \""(.+)\"" содержит символы, отличные от пробелов")]
        public void WebElementInFrameTextIsNotEmpty(string frame, string name)
        {
            var tst1 = BrowserController.GetBrowser().GetCurrentPage().GetFrame(frame);
            var element = BrowserController.GetBrowser().GetCurrentPage().GetFrame(frame).GetElement(name);
            var tst = element.Text;

            element.Text.Should().NotBeNullOrWhiteSpace($"текст элемента \"{name}\" пустой или не существует");
        }

        #endregion

        [StepDefinition(@"загружен файл из переменной \""(.+)\"" в элемент \""(.+)\"" на веб-странице")]
        public void LoadFileToElement(string varName, string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);

            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var path = this.variableController.GetVariableValueText(varName);
            path.Should().NotBeNull($"путь к файлу \"{varName}\" пустой");
            (element as File).SetText(path);
        }

        [StepDefinition(@"нажата клавиша \""(.+)\"" на элементе \""(.+)\"" на веб-странице")]
        public void PressKeyToWebElement(string key, string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.PressKey(key);
        }

        [Then(@"текущая страница загрузилась")]
        public void PageIsLoaded()
        {
            bool loaded = BrowserController.GetBrowser().GetCurrentPage().IsLoadElements();
            loaded.Should().BeTrue($"страница \"{BrowserController.GetBrowser().GetCurrentPage().Name}\" не загрузилась");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" пусто")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" пустая строка")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" равно пустой строке")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" равно null")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" заполнено пробелами")]
        public void WebElementValueIsEmpty(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().BeNullOrWhiteSpace($"значение элемента \"{name}\" не пусто");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" пустой")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" пустая строка")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" равен пустой строке")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" равен null")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" заполнен пробелами")]
        public void WebElementTextIsEmpty(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.Should().BeNullOrWhiteSpace($"текст элемента \"{name}\" не пустой");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" заполнено")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" не равно null")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" содержит символы, отличные от пробелов")]
        public void WebElementValueIsNotEmpty(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"значение элемента \"{name}\" пусто или не существует");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" заполнен")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" не равно null")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" содержит символы, отличные от пробелов")]
        public void WebElementTextIsNotEmpty(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.Should().NotBeNullOrWhiteSpace($"текст элемента \"{name}\" пустой или не существует");
        }

        #region Проверка на Contains и Equal со значением и переменной для текста и значения элемента
        [Then(@"на веб-странице значение элемента \""(.+)\"" содержит значение \""(.+)\""")]
        public void WebElementValueContainsValue(string name, string expected)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");

            element.Value.ToString().Should().Contain(expected, $"значение элемента \"{name}\":\"{element.Value}\" не содержит \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" содержит значение из переменной \""(.+)\""")]
        public void WebElementValueContainsVariableValue(string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");

            element.Value.ToString().Should().Contain(varValue, $"значение элемента \"{name}\":\"{element.Value}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" содержит значение \""(.+)\""")]
        public void WebElementTextContainsValue(string name, string expected)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.Should().NotBeNullOrWhiteSpace($"текст у элемента \"{name}\" пустой или не существует");

            element.Text.Should().Contain(expected, $"текст элемента \"{name}\":\"{element.Text}\" не содержит \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" содержит значение из переменной \""(.+)\""")]
        public void WebElementTextContainsVariableValue(string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.Should().NotBeNullOrWhiteSpace($"текст у элемента \"{name}\" пустой или не существует");

            element.Text.Should().Contain(varValue, $"текст элемента \"{name}\":\"{element.Text}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не содержит значение \""(.+)\""")]
        public void WebElementValueNotContainsValue(string name, string expected)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");

            element.Value.ToString().Should().NotContain(expected, $"значение элемента \"{name}\":\"{element.Value}\" содержит \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не содержит значение из переменной \""(.+)\""")]
        public void WebElementValueNotContainsVariableValue(string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");

            element.Value.ToString().Should().NotContain(varValue, $"значение элемента \"{element}\":\"{element.Value}\" содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не содержит значение \""(.+)\""")]
        public void WebElementTextNotContainsValue(string name, string expected)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");

            element.Text.Should().NotContain(expected, $"текст у элемента \"{name}\":\"{element.Text}\" содержит \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не содержит значение из переменной \""(.+)\""")]
        public void WebElementTextNotContainsVariableValue(string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.Should().NotBeNullOrWhiteSpace($"текст у элемента \"{name}\" пустой или не существует");

            element.Text.Should().NotContain(varValue, $"текст у элемента \"{element}\":\"{element.Value}\" содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" равно значению \""(.+)\""")]
        public void WebElementValueEqualValue(string name, string expected)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");

            element.Value.ToString().Should().Be(expected, $"значение элемента \"{name}\":\"{element.Value}\" не равно \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" равно значению из переменной \""(.+)\""")]
        public void WebElementValueEqualVariableValue(string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");

            element.Value.ToString().Should().Be(varValue, $"значение элемента \"{name}\":\"{element.Value}\" не равно значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" равен значению \""(.+)\""")]
        public void WebElementTextEqualValue(string name, string expected)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");

            element.Text.Should()
                .Be(expected, $"текст у элемента \"{name}\":\"{element.Text}\" не равен \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" равен значению из переменной \""(.+)\""")]
        public void WebElementTextEqualVariableValue(string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.Should().NotBeNullOrWhiteSpace($"текст у элемента \"{name}\" пустой или не существует");

            element.Text.Should()
                .Be(varValue, $"текст у элемента \"{name}\":\"{element.Text}\" не равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не равно значению \""(.+)\""")]
        public void WebElementValueNotEqualValue(string name, string expected)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");

            element.Value.ToString().Should()
                .NotBe(expected, $"значение элемента \"{name}\":\"{element.Value}\" равно \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не равно значению из переменной \""(.+)\""")]
        public void WebElementValueNotEqualVariableValue(string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");

            element.Value.ToString().Should()
                .NotBe(varValue, $"значение элемента \"{name}\":\"{element.Value}\" равно значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не равен значению \""(.+)\""")]
        public void WebElementTextNotEqualValue(string name, string expected)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");

            element.Text.Should()
                .NotBe(expected, $"текст у элемента \"{name}\":\"{element.Text}\" равен \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не равен значению из переменной \""(.+)\""")]
        public void WebElementTextNotEqualVariableValue(string name, string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.Should().NotBeNullOrWhiteSpace($"текст у элемента \"{name}\" пустой или не существует");

            element.Text.Should()
                .NotBe(varValue, $"текст у элемента \"{name}\":\"{element.Text}\" равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        #endregion
        #region Проверка свойств элемента на отображение, активность и редактируемость
        [Then(@"элемент \""(.+)\"" отображается на веб-странице")]
        public void WebElementIsDisplayed(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Displayed.Should().BeTrue($"элемент \"{name}\" не отображается");
        }

        [Then(@"элемент \""(.+)\"" не отображается на веб-странице")]
        public void WebElementIsNotDisplayed(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);

            element.Displayed.Should().BeFalse($"элемент \"{name}\" отображается");
        }

        [Then(@"на веб-странице элемент \""(.+)\"" активен")]
        public void WebElementIsEnabled(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);

            element.Enabled.Should().BeTrue($"элемент \"{name}\" не активен");
        }

        [Then(@"на веб-странице элемент \""(.+)\"" неактивен")]
        public void WebElementIsDisabled(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);

           element.Enabled.Should().BeFalse($"элемент \"{name}\" активен");
        }

        [Then(@"на веб-странице элемент \""(.+)\"" выбран")]
        public void WebElementIsSelected(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);

            element.Selected.Should().BeTrue($"элемент \"{name}\" не выбран");
        }

        [Then(@"на веб-странице элемент \""(.+)\"" не выбран")]
        public void WebElementIsNotSelected(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Selected.Should().BeFalse($"элемент \"{name}\" выбран");
        }

        [Then(@"на веб-странице элемент \""(.+)\"" нельзя редактировать")]
        public void WebElementIsNotEditable(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);

            element.Editabled.Should().BeTrue($"элемент \"{name}\" доступен для редактирования");
        }

        [Then(@"на веб-странице элемент \""(.+)\"" можно редактировать")]
        public void WebElementIsEditable(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);

            element.Editabled.Should().BeFalse($"элемент \"{name}\" не доступен для редактирования");
        }

        #endregion
        #region Проверка адреса активной веб страницы
        [Then(@"адрес активной веб-страницы содержит значение \""(.+)\""")]
        public void WebPageUrlContainsExpected(string url)
        {
           BrowserController.GetBrowser().Url.Should().Contain(url, $"адрес активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Url}\" не содержит \"{url}\"");
        }

        [Then(@"адрес активной веб-страницы содержит значение переменной \""(.+)\""")]
        public void WebPageUrlContainsVarValue(string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            BrowserController.GetBrowser().Url.Should().Contain(varValue, $"адрес активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Url}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"адрес активной веб-страницы не содержит значение \""(.+)\""")]
        public void WebPageUrlNotContainsExpected(string url)
        {
            BrowserController.GetBrowser().Url.Should().NotContain(url, $"адрес активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Url}\" содержит \"{url}\"");
        }

        [Then(@"адрес активной веб-страницы не содержит значение переменной \""(.+)\""")]
        public void WebPageUrlNotContainsvarValue(string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            BrowserController.GetBrowser().Url.Should().Contain(varValue, $"адрес активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Url}\" содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"адрес активной веб-страницы равен значению \""(.+)\""")]
        public void WebPageUrlEqualExpected(string url)
        {
            BrowserController.GetBrowser().Url.Should().Be(url, $"адрес активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Url}\" не равен \"{url}\"");
        }

        [Then(@"адрес активной веб-страницы равен значению переменной \""(.+)\""")]
        public void WebPageUrlEqualVarValue(string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            BrowserController.GetBrowser().Url.Should().Be(varValue, $"адрес активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Url}\" не равен значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"адрес активной веб-страницы не равен значению \""(.+)\""")]
        public void WebPageUrlNotEqualExpected(string url)
        {
            BrowserController.GetBrowser().Url.Should().NotBe(url, $"адрес активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Url}\" равен \"{url}\"");
        }

        [Then(@"адрес активной веб-страницы не равен значению переменной \""(.+)\""")]
        public void WebPageUrlNotEqualVarValue(string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            BrowserController.GetBrowser().Url.Should().NotBe(varValue, $"адрес активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Url}\" равен значение переменной \"{varName}\":\"{varValue}\"");
        }

        #endregion
        #region Проверка заголовка активной веб страницы
        [Then(@"заголовок веб-страницы равен значению \""(.+)\""")]
        public void WebPageTitleIsEqual(string title)
        {
            BrowserController.GetBrowser().Title.Should().Be(title, $"заголовок активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Title}\" не равен \"{title}\"");
        }

        [Then(@"заголовок веб-страницы равен значению переменной \""(.+)\""")]
        public void WebPageTitleIsEqualVarValue(string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            BrowserController.GetBrowser().Title.Should().Be(varValue, $"заголовок активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Title}\" не равен значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"заголовок веб-страницы не равен значению \""(.+)\""")]
        public void WebPageTitleIsNotEqual(string title)
        {
            BrowserController.GetBrowser().Title.Should().NotBe(title, $"заголовок активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Title}\" равен \"{title}\"");
        }

        [Then(@"заголовок веб-страницы не равен значению переменной \""(.+)\""")]
        public void WebPageTitleIsNotEqualVarValue(string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            BrowserController.GetBrowser().Title.Should().NotBe(varValue, $"заголовок активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Title}\" равен значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"заголовок веб-страницы содержит значение \""(.+)\""")]
        public void WebPageTitleIsContains(string title)
        {
            BrowserController.GetBrowser().Title.Should().Contain(title, $"заголовок активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Title}\" не содержит \"{title}\"");
        }

        [Then(@"заголовок веб-страницы содержит значение переменной \""(.+)\""")]
        public void WebPageTitleIsContainsVarValue(string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            BrowserController.GetBrowser().Title.Should().Contain(varValue, $"заголовок активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Title}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"заголовок веб-страницы не содержит значение \""(.+)\""")]
        public void WebPageTitleIsNotContains(string title)
        {
            BrowserController.GetBrowser().Title.Should().NotContain(title, $"заголовок активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Title}\" содержит \"{title}\"");
        }

        [Then(@"заголовок веб-страницы не содержит значение переменной \""(.+)\""")]
        public void WebPageTitleIsNotContainsVarValue(string varName)
        {
            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var varValue = this.variableController.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"значения в переменной \"{varName}\" нет");

            BrowserController.GetBrowser().Title.Should().NotContain(varValue, $"заголовок активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Title}\" содержит значение переменной \"{varName}\":\"{varValue}\"");
        }
#endregion
    }
}
