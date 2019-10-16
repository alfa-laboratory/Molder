using System;
using AlfaBank.AFT.Core.Model.Context;
using AlfaBank.AFT.Core.Model.Web.Support;
using FluentAssertions;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace AlfaBank.AFT.Core.Library.Web
{
    [Binding]
    [Scope(Tag = "web")]
    [Scope(Tag = "Web")]
    public class WebSteps_Then
    {
        private readonly WebContext webContext;
        private readonly VariableContext variableContext;
        private readonly PageObjectSupport pageObjectSupport;
        private readonly TextBoxSupport textBoxSupport;
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
        /// <param name="elementSupport">Контекст для работы с перетаскиваниями.</param>
        /// <param name="consoleOutputHelper">Capturing Output.</param>
        public WebSteps_Then(WebContext webContext, VariableContext variableContext, 
            NavigationSupport navigationSupport, PageObjectSupport pageObjectSupport,
            ClickSupport clickSupport, TextBoxSupport textBoxSupport,
            MoveSupport moveSupport, DragAndDropSupport dragAndDropSupport,
            ElementSupport elementSupport, 
            ITestOutputHelper consoleOutputHelper)
        {
            this.webContext = webContext;
            this.pageObjectSupport = pageObjectSupport;
            this.textBoxSupport = textBoxSupport;
            this.elementSupport = elementSupport;
            this.variableContext = variableContext;
            this.consoleOutputHelper = consoleOutputHelper;
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" пусто")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" пустая строка")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" равно пустой строке")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" равно null")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" заполнено пробелами")]
        public void WebElementValueIsEmpty(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var value = this.textBoxSupport.GetValue(parameter);
            value.Should().BeNullOrWhiteSpace($"Значение элемента \"{element}\" не пусто ");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" пустой")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" пустая строка")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" равен пустой строке")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" равен null")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" заполнен пробелами")]
        public void WebElementTextIsEmpty(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var value = this.textBoxSupport.GetText(parameter);
            value.Should().BeNullOrWhiteSpace($"Значение элемента \"{element}\" не пусто ");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" заполнено")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" не равно null")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" содержит символы, отличные от пробелов")]
        public void WebElementValueIsNotEmpty(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var value = this.textBoxSupport.GetValue(parameter);
            value.Should().NotBeNullOrWhiteSpace($"Значение элемента \"{element}\" пусто или не существует");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" заполнен")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" не равно null")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" содержит символы, отличные от пробелов")]
        public void WebElementTextIsNotEmpty(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var text = this.textBoxSupport.GetText(parameter);
            text.Should().NotBeNullOrWhiteSpace($"Текст элемента \"{element}\" пустой или не существует");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" содержит значение \""(.+)\""")]
        public void WebElementValueContainsValue(string element, string expected)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var value = this.textBoxSupport.GetValue(parameter);
            value.Should().NotBeNullOrWhiteSpace($"Значение элемента \"{element}\" пусто или не существует");

            value.Contains(expected).Should()
                .BeTrue($"Значение элемента \"{element}\":\"{value}\" не содержит \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" содержит значение из переменной \""(.+)\""")]
        public void WebElementValueContainsVariableValue(string element, string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var value = this.textBoxSupport.GetValue(parameter);
            value.Should().NotBeNullOrWhiteSpace($"Значение элемента \"{element}\" пусто или не существует");

            value.Contains(varValue).Should()
                .BeTrue($"Значение элемента \"{element}\":\"{value}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" содержит значение \""(.+)\""")]
        public void WebElementTextContainsValue(string element, string expected)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var text = this.textBoxSupport.GetText(parameter);
            text.Should().NotBeNullOrWhiteSpace($"Текст у элемента \"{element}\" пустой или не существует");

            text.Contains(expected).Should()
                .BeTrue($"Текст у элемента \"{element}\":\"{text}\" не содержит \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" содержит значение из переменной \""(.+)\""")]
        public void WebElementTextContainsVariableValue(string element, string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var text = this.textBoxSupport.GetText(parameter);
            text.Should().NotBeNullOrWhiteSpace($"Текст у элемента \"{element}\" пустой или не существует");

            text.Contains(varValue).Should()
                .BeTrue($"Текст у элемента \"{element}\":\"{text}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не содержит значение \""(.+)\""")]
        public void WebElementValueNotContainsValue(string element, string expected)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var value = this.textBoxSupport.GetValue(parameter);
            var text = this.textBoxSupport.GetText(parameter);
            value.Should().NotBeNullOrWhiteSpace($"Значение элемента \"{element}\" пусто или не существует");

            value.Contains(expected).Should()
                .BeFalse($"Значение элемента \"{element}\":\"{value}\" содержит \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не содержит значение из переменной \""(.+)\""")]
        public void WebElementValueNotContainsVariableValue(string element, string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var value = this.textBoxSupport.GetValue(parameter);
            value.Should().NotBeNullOrWhiteSpace($"Значение элемента \"{element}\" пусто или не существует");

            value.Contains(varValue).Should()
                .BeFalse($"Значение элемента \"{element}\":\"{value}\" содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не содержит значение \""(.+)\""")]
        public void WebElementTextNotContainsValue(string element, string expected)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var text = this.textBoxSupport.GetText(parameter);
            text.Should().NotBeNullOrWhiteSpace($"Текст у элемента \"{element}\" пустой или не существует");

            text.Contains(expected).Should()
                .BeFalse($"Текст у элемента \"{element}\":\"{text}\" не содержит \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не содержит значение из переменной \""(.+)\""")]
        public void WebElementTextNotContainsVariableValue(string element, string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var text = this.textBoxSupport.GetText(parameter);
            text.Should().NotBeNullOrWhiteSpace($"Текст у элемента \"{element}\" пустой или не существует");

            text.Contains(varValue).Should()
                .BeFalse($"Текст у элемента \"{element}\":\"{text}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" равно значению \""(.+)\""")]
        public void WebElementValueEqualValue(string element, string expected)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var value = this.textBoxSupport.GetValue(parameter);
            value.Should().NotBeNullOrWhiteSpace($"Значение элемента \"{element}\" пусто или не существует");

            value.Should().Be(expected, $"Значение элемента \"{element}\":\"{value}\" не равно \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" равно значению из переменной \""(.+)\""")]
        public void WebElementValueEqualVariableValue(string element, string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var value = this.textBoxSupport.GetValue(parameter);
            value.Should().NotBeNullOrWhiteSpace($"Значение элемента \"{element}\" пусто или не существует");

            value.Should().Be(varValue, $"Значение элемента \"{element}\":\"{value}\" не равно значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" равен значению \""(.+)\""")]
        public void WebElementTextEqualValue(string element, string expected)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var text = this.textBoxSupport.GetText(parameter);
            text.Should().NotBeNullOrWhiteSpace($"Текст у элемента \"{element}\" пустой или не существует");

            text.Should()
                .Be(expected,$"Текст у элемента \"{element}\":\"{text}\" не равен \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" равен значению из переменной \""(.+)\""")]
        public void WebElementTextEqualVariableValue(string element, string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var text = this.textBoxSupport.GetText(parameter);
            text.Should().NotBeNullOrWhiteSpace($"Текст у элемента \"{element}\" пустой или не существует");

            text.Should()
                .Be(varValue,$"Текст у элемента \"{element}\":\"{text}\" не равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не равен значению \""(.+)\""")]
        public void WebElementValueNotEqualValue(string element, string expected)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var value = this.textBoxSupport.GetValue(parameter);
            value.Should().NotBeNullOrWhiteSpace($"Значение элемента \"{element}\" пусто или не существует");

            value.Should()
                .NotBe(expected, $"Значение элемента \"{element}\":\"{value}\" равно \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не равно значению из переменной \""(.+)\""")]
        public void WebElementValueNotEqualVariableValue(string element, string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var value = this.textBoxSupport.GetValue(parameter);
            value.Should().NotBeNullOrWhiteSpace($"Значение элемента \"{element}\" пусто или не существует");

            value.Should()
                .NotBe(varValue,$"Значение элемента \"{element}\":\"{value}\" равно значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не равен значению \""(.+)\""")]
        public void WebElementTextNotEqualValue(string element, string expected)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            var text = this.textBoxSupport.GetText(parameter);
            text.Should().NotBeNullOrWhiteSpace($"Текст у элемента \"{element}\" пустой или не существует");

            text.Should()
                .NotBe(expected,$"Текст у элемента \"{element}\":\"{text}\" равен \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не равен значению из переменной \""(.+)\""")]
        public void WebElementTextNotEqualVariableValue(string element, string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            var text = this.textBoxSupport.GetText(parameter);
            text.Should().NotBeNullOrWhiteSpace($"Текст у элемента \"{element}\" пустой или не существует");

            text.Should()
                .NotBe(varValue,$"Текст у элемента \"{element}\":\"{text}\" равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"элемент \""(.+)\"" отображается на веб-странице")]
        public void WebElementIsDisplayed(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
             
            this.elementSupport.BeDisplayed(parameter);
        }

        [Then(@"элемент \""(.+)\"" не отображается на веб-странице")]
        public void WebElementIsNotDisplayed(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            this.elementSupport.NotBeDisplayed(parameter);
        }

        [Then(@"элемент \""(.+)\"" существует на веб-странице")]
        public void WebElementIsNotNull(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            this.elementSupport.NotBeNull(parameter);
        }

        [Then(@"элемент \""(.+)\"" отсутствует на веб-странице")]
        public void WebElementIsNull(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");

            this.elementSupport.BeNull(parameter);
        }

        [Then(@"на веб-странице элемент \""(.+)\"" активен")]
        public void WebElementIsEnabled(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            this.elementSupport.BeEnabled(parameter);
        }

        [Then(@"на веб-странице элемент \""(.+)\"" неактивен")]
        public void WebElementIsDisabled(string element)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var parameter = this.pageObjectSupport.GetParameterByName(element);
            parameter.Should().NotBeNull($"Элемент \"{element}\" не инициализирован в PageObject");
            this.elementSupport.BeDisabled(parameter);
        }

        [Then(@"адрес активной веб-страницы содержит значение \""(.+)\""")]
        public void WebPageUrlContainsExpected(string url)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            this.webContext.WebDriver.Url.Contains(url).Should()
                .BeTrue($"адрес активной веб страницы \"{this.webContext.WebDriver.Url}\" не содержит \"{url}\"");
        }

        [Then(@"адрес активной веб-страницы содержит значение переменной \""(.+)\""")]
        public void WebPageUrlContainsVarValue(string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.WebDriver.Url.Contains(varValue).Should()
                .BeTrue($"адрес активной веб страницы \"{this.webContext.WebDriver.Url}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"адрес активной веб-страницы не содержит значение \""(.+)\""")]
        public void WebPageUrlNotContainsExpected(string url)
        {
            this.webContext.WebDriver.Url.Contains(url).Should()
                .BeFalse($"адрес активной веб страницы \"{this.webContext.WebDriver.Url}\" содержит \"{url}\"");
        }

        [Then(@"адрес активной веб-страницы не содержит значение переменной \""(.+)\""")]
        public void WebPageUrlNotContainsvarValue(string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.WebDriver.Url.Contains(varValue).Should()
                .BeFalse($"адрес активной веб страницы \"{this.webContext.WebDriver.Url}\" содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"адрес активной веб-страницы равен значению \""(.+)\""")]
        public void WebPageUrlEqualExpected(string expected)
        {
            this.webContext.WebDriver.Url.Should()
                .Be(expected, $"адрес активной веб страницы \"{this.webContext.WebDriver.Url}\" не равен \"{expected}\"");
        }

        [Then(@"адрес активной веб-страницы равен значению переменной \""(.+)\""")]
        public void WebPageUrlEqualVarValue(string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.WebDriver.Url.Should()
                .Be(varValue, $"адрес активной веб страницы \"{this.webContext.WebDriver.Url}\" не равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"адрес активной веб-страницы не равен значению \""(.+)\""")]
        public void WebPageUrlNotEqualExpected(string expected)
        {
            this.webContext.WebDriver.Url.Should()
                .NotBe(expected, $"адрес активной веб страницы \"{this.webContext.WebDriver.Url}\" равен \"{expected}\"");
        }

        [Then(@"адрес активной веб-страницы не равен значению переменной \""(.+)\""")]
        public void WebPageUrlNotEqualVarValue(string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.WebDriver.Url.Should()
                .NotBe(varValue, $"адрес активной веб страницы \"{this.webContext.WebDriver.Url}\" равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"заголовок веб-страницы равен значению \""(.+)\""")]
        public void WebPageTitleIsEqual(string expected)
        {
            expected.Should().NotBeEmpty("Параметр \"title\" не задан");

            this.webContext.WebDriver.Title.Should()
                .Be(expected, $"Заголовок активной веб страницы \"{this.webContext.WebDriver.Url}\" не равен \"{expected}\"");
        }

        [Then(@"заголовок веб-страницы равен значению переменной \""(.+)\""")]
        public void WebPageTitleIsEqualVarValue(string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.WebDriver.Title.Should()
                .Be(varValue, $"Заголовок активной веб страницы \"{this.webContext.WebDriver.Url}\" не равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"заголовок веб-страницы не равен значению \""(.+)\""")]
        public void WebPageTitleIsNotEqual(string expected)
        {
            expected.Should().NotBeEmpty("Параметр \"title\" не задан");

            this.webContext.WebDriver.Title.Should()
                .NotBe(expected, $"Заголовок активной веб страницы \"{this.webContext.WebDriver.Url}\" равен \"{expected}\"");
        }

        [Then(@"заголовок веб-страницы не равен значению переменной \""(.+)\""")]
        public void WebPageTitleIsNotEqualVarValue(string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.WebDriver.Title.Should()
                .NotBe(varValue, $"Заголовок активной веб страницы \"{this.webContext.WebDriver.Url}\" равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"заголовок веб-страницы содержит значение \""(.+)\""")]
        public void WebPageTitleIsContains(string title)
        {
            title.Should().NotBeEmpty("Параметр \"title\" не задан");

            this.webContext.WebDriver.Title.Contains(title).Should()
                .BeTrue($"адрес активной веб страницы \"{this.webContext.WebDriver.Url}\" не содержит \"{title}\"");
        }

        [Then(@"заголовок веб-страницы содержит значение переменной \""(.+)\""")]
        public void WebPageTitleIsContainsVarValue(string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.WebDriver.Title.Contains(varValue).Should()
                .BeTrue($"адрес активной веб страницы \"{this.webContext.WebDriver.Url}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"заголовок веб-страницы не содержит значение \""(.+)\""")]
        public void WebPageTitleIsNotContains(string title)
        {
            title.Should().NotBeEmpty("Параметр \"title\" не задан");

            this.webContext.WebDriver.Title.Contains(title).Should()
                .BeFalse($"адрес активной веб страницы \"{this.webContext.WebDriver.Url}\" содержит \"{title}\"");
        }

        [Then(@"заголовок веб-страницы не содержит значение переменной \""(.+)\""")]
        public void WebPageTitleIsNotContainsVarValue(string varName)
        {
            this.webContext.WebDriver.Should()
                .NotBeNull($"Браузер не инициализирован");

            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.WebDriver.Title.Contains(varValue).Should()
                .BeFalse($"адрес активной веб страницы \"{this.webContext.WebDriver.Url}\" содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"я убеждаюсь, что на веб-странице появилось диалоговое окно")]
        public void CheckAlert()
        {
            var act = new Action(() => this.webContext.WebDriver.SwitchTo().Alert());
            act.Should().NotThrow<NoAlertPresentException>();
        }
    }
}
