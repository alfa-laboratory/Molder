using Molder.Controllers;
using Molder.Web.Controllers;
using FluentAssertions;
using System;
using Molder.Extensions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Molder.Web.Models.PageObjects.Elements;
using Molder.Web.Models.Proxy;

namespace Molder.Web.Steps
{
    [Binding]
    public class Steps
    {
        protected VariableController variableController;

        public Steps(VariableController controller)
        {
            variableController = controller;
            BrowserController.SetVariables(variableController);
        }

        [StepArgumentTransformation]
        public Authentication Authentication(Table table)
        {
            return table.CreateInstance<Authentication>();
        }

        [Given(@"я инициализирую браузер")]
        public void StartBrowser()
        {
            BrowserController.GetBrowser();
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

        [StepDefinition(@"выполнен переход на вкладку номер ([1-9]+)")]
        public void GoToTabByNumber(int number)
        {
            (number--).Should().BePositive("неверно задан номер вкладки");
            number.Should().BeLessOrEqualTo(BrowserController.GetBrowser().Tabs,
                "выбранной вкладки не существует");

            BrowserController.GetBrowser().SwitchTo(number);
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
                    /// TODO добавить текст ошибки
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

        [Then(@"текущая страница загрузилась")]
        public void PageIsLoaded()
        {
            var loaded = BrowserController.GetBrowser().GetCurrentPage().IsLoadElements();
            loaded.Should().BeTrue($"страница \"{BrowserController.GetBrowser().GetCurrentPage().Name}\" не загрузилась");
        }

        #region Проверка адреса активной веб страницы
        [Then(@"адрес активной веб-страницы содержит значение \""(.+)\""")]
        public void WebPageUrlContainsExpected(string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;
            BrowserController.GetBrowser().Url.Should().Contain(expected, $"адрес активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Url}\" не содержит \"{expected}\"");
        }
        

        [Then(@"адрес активной веб-страницы не содержит значение \""(.+)\""")]
        public void WebPageUrlNotContainsExpected(string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;
            BrowserController.GetBrowser().Url.Should().NotContain(expected, $"адрес активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Url}\" содержит \"{expected}\"");
        }

        [Then(@"адрес активной веб-страницы равен значению \""(.+)\""")]
        public void WebPageUrlEqualExpected(string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;
            BrowserController.GetBrowser().Url.Should().Be(expected, $"адрес активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Url}\" не равен \"{expected}\"");
        }

        [Then(@"адрес активной веб-страницы не равен значению \""(.+)\""")]
        public void WebPageUrlNotEqualExpected(string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;
            BrowserController.GetBrowser().Url.Should().NotBe(expected, $"адрес активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Url}\" равен \"{expected}\"");
        }

        #endregion
        #region Проверка заголовка активной веб страницы
        [Then(@"заголовок веб-страницы равен значению \""(.+)\""")]
        public void WebPageTitleIsEqual(string title)
        {
            title.Should().NotBeNull($"значение \"expected\" не задано");
            title = this.variableController.ReplaceVariables(title) ?? title;
            BrowserController.GetBrowser().Title.Should().Be(title, $"заголовок активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Title}\" не равен \"{title}\"");
        }

        [Then(@"заголовок веб-страницы не равен значению \""(.+)\""")]
        public void WebPageTitleIsNotEqual(string title)
        {
            title.Should().NotBeNull($"значение \"expected\" не задано");
            title = this.variableController.ReplaceVariables(title) ?? title;
            BrowserController.GetBrowser().Title.Should().NotBe(title, $"заголовок активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Title}\" равен \"{title}\"");
        }

        [Then(@"заголовок веб-страницы содержит значение \""(.+)\""")]
        public void WebPageTitleIsContains(string title)
        {
            title.Should().NotBeNull($"значение \"expected\" не задано");
            title = this.variableController.ReplaceVariables(title) ?? title;
            BrowserController.GetBrowser().Title.Should().Contain(title, $"заголовок активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Title}\" не содержит \"{title}\"");
        }

        [Then(@"заголовок веб-страницы не содержит значение \""(.+)\""")]
        public void WebPageTitleIsNotContains(string title)
        {
            title.Should().NotBeNull($"значение \"expected\" не задано");
            title = this.variableController.ReplaceVariables(title) ?? title;
            BrowserController.GetBrowser().Title.Should().NotContain(title, $"заголовок активной веб страницы \"{BrowserController.GetBrowser().GetCurrentPage().Name}\":\"{BrowserController.GetBrowser().Title}\" содержит \"{title}\"");
        }
        
        #endregion
        
        #region Elements

        [StepDefinition(@"я перемещаюсь к элементу \""(.+)\"" на веб-странице")]
        public void ScrollToElement(string name)
        {
            BrowserController.GetBrowser().GetCurrentPage().GetElement(name).Move();
        }

        [StepDefinition(@"выполнено нажатие на элемент \""(.+)\"" на веб-странице")]
        public void ClickToWebElement(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is BaseClick).Should().BeTrue($"элемент \"{name}\" имеет отличный от Click профиль");
            (element as BaseClick)?.Click();
        }

        [StepDefinition(@"выполнено двойное нажатие на элемент \""(.+)\"" на веб-странице")]
        public void DoubleClickToWebElement(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is BaseClick).Should().BeTrue($"элемент \"{name}\" имеет отличный от Click профиль");
            (element as BaseClick)?.DoubleClick();
        }

        [StepDefinition(@"выполнено нажатие с удержанием на элементе \""(.+)\"" на веб-странице")]
        public void ClickAndHoldToWebElement(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is BaseClick).Should().BeTrue($"элемент \"{name}\" имеет отличный от Click профиль");
            (element as BaseClick)?.ClickAndHold();
        }

        [StepDefinition(@"я ввожу в поле \""(.+)\"" веб-страницы значение \""(.+)\""")]
        public void InputValueIntoField(string name, string text)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Input).Should().BeTrue($"элемент \"{name}\" имеет отличный от Input профиль");
            
            text.Should().NotBeNull($"значение \"text\" не задано");
            text = this.variableController.ReplaceVariables(text) ?? text;
            (element as Input)?.SetText(text);
        }

        [StepDefinition(@"я очищаю поле \""(.+)\"" веб-страницы")]
        public void ClearField(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Input).Should().BeTrue($"элемент \"{name}\" имеет отличный от Input профиль");
            (element as Input)?.Clear();
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

        [StepDefinition(@"я сохраняю значение атрибута \""(.+)\"" элемента \""(.+)\"" веб-страницы в переменную \""(.+)\""")]
        public void StoreWebElementValueOfAttributeInVariable(string attribute, string name, string varName)
        {
            this.variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            var attributeValue = element.GetAttribute(attribute);
            this.variableController.SetVariable(varName, attributeValue.GetType(), attributeValue);
        }

        [StepDefinition(@"загружен файл из переменной \""(.+)\"" в элемент \""(.+)\"" на веб-странице")]
        public void LoadFileToElement(string varName, string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);

            this.variableController.Variables.Should().ContainKey(varName, $"переменная \"{varName}\" не существует");
            var path = this.variableController.GetVariableValueText(varName);
            path.Should().NotBeNull($"путь к файлу \"{varName}\" пустой");
            (element as File)?.SetText(path);
        }

        [StepDefinition(@"нажата клавиша \""(.+)\"" на элементе \""(.+)\"" на веб-странице")]
        public void PressKeyToWebElement(string key, string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.PressKey(key);
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

        #endregion
        
        #region Проверка на Contains и Equal со значением и переменной для текста и значения элемента
        [Then(@"на веб-странице значение элемента \""(.+)\"" содержит значение \""(.+)\""")]
        public void WebElementValueContainsValue(string name, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;
            
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");
        
            element.Value.ToString().Should().Contain(expected, $"значение элемента \"{name}\":\"{element.Value}\" не содержит \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" содержит значение \""(.+)\""")]
        public void WebElementTextContainsValue(string name, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;
            
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.Should().NotBeNullOrWhiteSpace($"текст у элемента \"{name}\" пустой или не существует");
        
            element.Text.Should().Contain(expected, $"текст элемента \"{name}\":\"{element.Text}\" не содержит \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не содержит значение \""(.+)\""")]
        public void WebElementValueNotContainsValue(string name, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;
            
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");
        
            element.Value.ToString().Should().NotContain(expected, $"значение элемента \"{name}\":\"{element.Value}\" содержит \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не содержит значение \""(.+)\""")]
        public void WebElementTextNotContainsValue(string name, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;
            
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");
        
            element.Text.Should().NotContain(expected, $"текст у элемента \"{name}\":\"{element.Text}\" содержит \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" равно значению \""(.+)\""")]
        public void WebElementValueEqualValue(string name, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;
            
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");
        
            element.Value.ToString().Should().Be(expected, $"значение элемента \"{name}\":\"{element.Value}\" не равно \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" равен значению \""(.+)\""")]
        public void WebElementTextEqualValue(string name, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;
            
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");
        
            element.Text.Should()
                .Be(expected, $"текст у элемента \"{name}\":\"{element.Text}\" не равен \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не равно значению \""(.+)\""")]
        public void WebElementValueNotEqualValue(string name, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;
            
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Value.ToString().Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");
        
            element.Value.ToString().Should()
                .NotBe(expected, $"значение элемента \"{name}\":\"{element.Value}\" равно \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не равен значению \""(.+)\""")]
        public void WebElementTextNotEqualValue(string name, string expected)
        {
            expected.Should().NotBeNull($"значение \"expected\" не задано");
            expected = this.variableController.ReplaceVariables(expected) ?? expected;
            
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Text.Should().NotBeNullOrWhiteSpace($"эначение элемента \"{name}\" пусто или не существует");
        
            element.Text.Should()
                .NotBe(expected, $"текст у элемента \"{name}\":\"{element.Text}\" равен \"{expected}\"");
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
        
            element.NotDisplayed.Should().BeTrue($"элемент \"{name}\" отображается");
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
        
            element.Disabled.Should().BeTrue($"элемент \"{name}\" активен");
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
            element.NotSelected.Should().BeTrue($"элемент \"{name}\" выбран");
        }
        
        [Then(@"на веб-странице элемент \""(.+)\"" нельзя редактировать")]
        public void WebElementIsNotEditable(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.NotEditable.Should().BeTrue($"элемент \"{name}\" доступен для редактирования");
        }
        
        [Then(@"на веб-странице элемент \""(.+)\"" можно редактировать")]
        public void WebElementIsEditable(string name)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            element.Editabled.Should().BeTrue($"элемент \"{name}\" не доступен для редактирования");
        }
        
        #endregion
        
        #region Работа с Dropdown 
        
        [StepDefinition(@"я выбираю в поле \""(.+)\"" веб-страницы значение \""(.+)\""")]
        public void DropdownIntoValue(string name, string value)
        {
            value.Should().NotBeNull($"значение \"expected\" не задано");
            value = this.variableController.ReplaceVariables(value) ?? value;
            
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Dropdown).Should().BeTrue($"элемент \"{name}\" имеет отличный от Dropdown профиль");
            (element as Dropdown)?.SelectByValue(value);
        }

        [StepDefinition(@"я выбираю в поле \""(.+)\"" веб-страницы текст \""(.+)\""")]
        public void DropdownIntoText(string name, string text)
        {
            text.Should().NotBeNull($"значение \"expected\" не задано");
            text = this.variableController.ReplaceVariables(text) ?? text;
            
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Dropdown).Should().BeTrue($"элемент \"{name}\" имеет отличный от Dropdown профиль");
            (element as Dropdown)?.SelectByText(text);
        }

        [StepDefinition(@"я выбираю в поле \""(.+)\"" веб-страницы номер значения \""(.+)\""")]
        public void DropdownIntoIndex(string name, int index)
        {
            var element = BrowserController.GetBrowser().GetCurrentPage().GetElement(name);
            (element is Dropdown).Should().BeTrue($"элемент \"{name}\" имеет отличный от Dropdown профиль");
            (element as Dropdown)?.SelectByIndex(index);
        }
        
        #endregion
        
        #region Blocks
        [StepDefinition(@"я перехожу на блок \""(.+)\"" на веб-странице")]
        public void GoToBlock(string block)
        {
            BrowserController.GetBrowser().GetCurrentPage().GetBlock(block);
        }
        
        [StepDefinition(@"я возвращаюсь к основной веб-странице")]
        public void BackToPage()
        {
            BrowserController.GetBrowser().GetCurrentPage().BackToPage();
        }
        #endregion
        
        #region Frames
        [StepDefinition(@"я перехожу на фрейм \""(.+)\"" на веб-странице")]
        public void GoToFrame(string frame)
        {
            BrowserController.GetBrowser().GetCurrentPage().GetFrame(frame);
        }
        
        [StepDefinition(@"я перехожу на стандартный фрейм на веб-странице")]
        public void GetDefaultFrame()
        {
            BrowserController.GetBrowser().GetCurrentPage().GetDefaultFrame();
        }
        #endregion
    }
}