using AlfaBank.AFT.Core.Helpers;
using AlfaBank.AFT.Core.Model.Context;
using AlfaBank.AFT.Core.Models.Context;
using AlfaBank.AFT.Core.Models.Web.Elements;
using AlfaBank.AFT.Core.Supports;
using FluentAssertions;
using System;
using System.Data;
using System.Linq;
using TechTalk.SpecFlow;

namespace AlfaBank.AFT.Core.Library.Web
{
    [Binding]
    [Scope(Tag = "web")]
    [Scope(Tag = "Web")]
    public class WebSteps_Then
    {
        private readonly WebContext webContext;
        private readonly VariableContext variableContext;
        private readonly CommandSupport commandSupport;
        private readonly DriverSupport driverSupport;

        /// <summary>
        /// Initializes a new instance of the <see cref="webContext"/> class.
        /// Привязка шагов работы с web driver через контекст.
        /// </summary>
        /// <param name="webContext">Контекст для работы с web driver.</param>
        /// <param name="variableContext">Контекст для работы с переменными.</param>
        /// <param name="commandSupport">Контекст для обработки команд.</param>
        /// <param name="driverSupport"></param>
        public WebSteps_Then(WebContext webContext, VariableContext variableContext, CommandSupport commandSupport, DriverSupport driverSupport)
        {
            this.webContext = webContext;
            this.commandSupport = commandSupport;
            this.driverSupport = driverSupport;
            this.variableContext = variableContext;
        }

        [StepArgumentTransformation]
        public DataTable ToDatatable(Table table)
        {
            var dataTable = new DataTable();

            if (table.Rows.Any())
            {
                for (var index = 0; index < table.Rows[0].Keys.Count; index++)
                {
                    dataTable.Columns.Add(table.Rows[0].Keys.ToArray()[index]);
                }
            }

            foreach (var row in table.Rows)
            {
                DataRow dataRow = dataTable.NewRow();

                foreach (var value in row)
                {
                    
                    dataRow[value.Key] = this.variableContext.ReplaceVariablesInXmlBody(value.Value);
                }

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" пусто")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" пустая строка")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" равно пустой строке")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" равно null")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" заполнено пробелами")]
        public void WebElementValueIsEmpty(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var value = this.commandSupport.SendCommand(() => element.GetValue());
            value.ToString().Should().BeNullOrWhiteSpace($"Значение элемента \"{name}\" не пусто ");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" пустой")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" пустая строка")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" равен пустой строке")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" равен null")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" заполнен пробелами")]
        public void WebElementTextIsEmpty(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var text = this.commandSupport.SendCommand(() => element.GetText());
            text.ToString().Should().BeNullOrWhiteSpace($"Текст элемента \"{name}\" не пустой ");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" заполнено")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" не равно null")]
        [Then(@"на веб-странице значение элемента \""(.+)\"" содержит символы, отличные от пробелов")]
        public void WebElementValueIsNotEmpty(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var value = this.commandSupport.SendCommand(() => element.GetValue());
            value.ToString().Should().NotBeNullOrWhiteSpace($"Значение элемента \"{name}\" пусто или не существует");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" заполнен")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" не равно null")]
        [Then(@"на веб-странице текст элемента \""(.+)\"" содержит символы, отличные от пробелов")]
        public void WebElementTextIsNotEmpty(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var text = this.commandSupport.SendCommand(() => element.GetText());
            text.ToString().Should().NotBeNullOrWhiteSpace($"Текст элемента \"{name}\" пустой или не существует");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" содержит значение \""(.+)\""")]
        public void WebElementValueContainsValue(string name, string expected)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var value = this.commandSupport.SendCommand(() => element.GetValue());
            value.ToString().Should().NotBeNullOrWhiteSpace($"Значение элемента \"{name}\" пусто или не существует");

            value.ToString().Contains(expected).Should()
                .BeTrue($"Значение элемента \"{name}\":\"{value}\" не содержит \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" содержит значение из переменной \""(.+)\""")]
        public void WebElementValueContainsVariableValue(string name, string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var value = this.commandSupport.SendCommand(() => element.GetValue());
            value.ToString().Should().NotBeNullOrWhiteSpace($"Значение элемента \"{name}\" пусто или не существует");

            value.ToString().Contains(varValue).Should()
                .BeTrue($"Значение элемента \"{name}\":\"{value}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" содержит значение \""(.+)\""")]
        public void WebElementTextContainsValue(string name, string expected)
        {
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var isText = this.commandSupport.SendCommand(() => element.IsTextContains(expected));

            ((bool)isText).Should()
                .BeTrue($"Текст у элемента \"{name}\":\"{element.GetText()}\" не содержит \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" содержит значение из переменной \""(.+)\""")]
        public void WebElementTextContainsVariableValue(string name, string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var isText = this.commandSupport.SendCommand(() => element.IsTextContains(varValue));

            ((bool)isText).Should()
              .BeTrue($"Текст у элемента \"{name}\":\"{element.GetText()}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не содержит значение \""(.+)\""")]
        public void WebElementValueNotContainsValue(string name, string expected)
        {
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var value = this.commandSupport.SendCommand(() => element.GetValue());
            value.ToString().Should().NotBeNullOrWhiteSpace($"Значение элемента \"{name}\" пусто или не существует");

            value.ToString().Contains(expected).Should()
                .BeFalse($"Значение элемента \"{name}\":\"{value}\" содержит \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не содержит значение из переменной \""(.+)\""")]
        public void WebElementValueNotContainsVariableValue(string name, string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var value = this.commandSupport.SendCommand(() => element.GetValue());
            value.ToString().Should().NotBeNullOrWhiteSpace($"Значение элемента \"{element}\" пусто или не существует");

            value.ToString().Contains(varValue).Should()
                .BeFalse($"Значение элемента \"{element}\":\"{value}\" содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не содержит значение \""(.+)\""")]
        public void WebElementTextNotContainsValue(string name, string expected)
        {
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var isText = this.commandSupport.SendCommand(() => element.IsTextContains(expected));

            ((bool)isText).Should()
                .BeFalse($"Текст у элемента \"{name}\":\"{element.GetText()}\" не содержит \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не содержит значение из переменной \""(.+)\""")]
        public void WebElementTextNotContainsVariableValue(string name, string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var isText = this.commandSupport.SendCommand(() => element.IsTextContains(varValue));

            ((bool)isText).Should()
                .BeFalse($"Текст у элемента \"{name}\":\"{element.GetText()}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" равно значению \""(.+)\""")]
        public void WebElementValueEqualValue(string name, string expected)
        {
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var value = this.commandSupport.SendCommand(() => element.GetValue());
            value.ToString().Should().NotBeNullOrWhiteSpace($"Значение элемента \"{name}\" пусто или не существует");

            value.ToString().Should().Be(expected, $"Значение элемента \"{name}\":\"{value}\" не равно \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" равно значению из переменной \""(.+)\""")]
        public void WebElementValueEqualVariableValue(string name, string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var value = this.commandSupport.SendCommand(() => element.GetValue());
            value.ToString().Should().NotBeNullOrWhiteSpace($"Значение элемента \"{name}\" пусто или не существует");

            value.ToString().Should().Be(varValue, $"Значение элемента \"{name}\":\"{value}\" не равно значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" равен значению \""(.+)\""")]
        public void WebElementTextEqualValue(string name, string expected)
        {
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var text = this.commandSupport.SendCommand(() => element.GetText());
            text.ToString().Should().NotBeNullOrWhiteSpace($"Текст у элемента \"{name}\" пустой или не существует");

            text.ToString().Should()
                .Be(expected,$"Текст у элемента \"{name}\":\"{text}\" не равен \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" равен значению из переменной \""(.+)\""")]
        public void WebElementTextEqualVariableValue(string name, string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var text = this.commandSupport.SendCommand(() => element.GetText());
            text.ToString().Should().NotBeNullOrWhiteSpace($"Текст у элемента \"{name}\" пустой или не существует");

            text.ToString().Should()
                .Be(varValue,$"Текст у элемента \"{name}\":\"{text}\" не равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не равно значению \""(.+)\""")]
        public void WebElementValueNotEqualValue(string name, string expected)
        {
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var value = this.commandSupport.SendCommand(() => element.GetValue());
            value.ToString().Should().NotBeNullOrWhiteSpace($"Значение элемента \"{name}\" пусто или не существует");

            value.ToString().Should()
                .NotBe(expected, $"Значение элемента \"{name}\":\"{value}\" равно \"{expected}\"");
        }

        [Then(@"на веб-странице значение элемента \""(.+)\"" не равно значению из переменной \""(.+)\""")]
        public void WebElementValueNotEqualVariableValue(string name, string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var value = this.commandSupport.SendCommand(() => element.GetValue());
            value.ToString().Should().NotBeNullOrWhiteSpace($"Значение элемента \"{name}\" пусто или не существует");

            value.ToString().Should()
                .NotBe(varValue,$"Значение элемента \"{name}\":\"{value}\" равно значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не равен значению \""(.+)\""")]
        public void WebElementTextNotEqualValue(string name, string expected)
        {
            expected.Should().NotBeNullOrWhiteSpace("Значение \"expected\" не задано");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var text = this.commandSupport.SendCommand(() => element.GetText());
            text.ToString().Should().NotBeNullOrWhiteSpace($"Текст у элемента \"{name}\" пустой или не существует");

            text.ToString().Should()
                .NotBe(expected,$"Текст у элемента \"{name}\":\"{text}\" равен \"{expected}\"");
        }

        [Then(@"на веб-странице текст элемента \""(.+)\"" не равен значению из переменной \""(.+)\""")]
        public void WebElementTextNotEqualVariableValue(string name, string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var text = this.commandSupport.SendCommand(() => element.GetText());
            text.ToString().Should().NotBeNullOrWhiteSpace($"Текст у элемента \"{name}\" пустой или не существует");

            text.ToString().Should()
                .NotBe(varValue,$"Текст у элемента \"{name}\":\"{text}\" равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"элемент \""(.+)\"" отображается на веб-странице")]
        public void WebElementIsDisplayed(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var isVisible = this.commandSupport.SendCommand(() => element.IsVisible());

            ((bool)isVisible).Should().BeTrue($"Элемент \"{name}\" не отображается");
        }

        [Then(@"элемент \""(.+)\"" не отображается на веб-странице")]
        public void WebElementIsNotDisplayed(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var isInvisible = this.commandSupport.SendCommand(() => element.IsInvisible());

            ((bool)isInvisible).Should().BeFalse($"Элемент \"{name}\" отображается");
        }

        [Then(@"элемент \""(.+)\"" существует на веб-странице")]
        public void WebElementIsNotNull(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var isNotNull = this.commandSupport.SendCommand(() => element.IsLoad());

            ((bool)isNotNull).Should().BeTrue($"Элемент \"{name}\" не существует");
        }

        [Then(@"элемент \""(.+)\"" отсутствует на веб-странице")]
        public void WebElementIsNull(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var isNull = this.commandSupport.SendCommand(() => element.IsLoad());

            ((bool)isNull).Should().BeFalse($"Элемент \"{name}\" существует");
        }

        [Then(@"на веб-странице элемент \""(.+)\"" активен")]
        public void WebElementIsEnabled(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var isEnable = this.commandSupport.SendCommand(() => element.IsEnabled());

            ((bool)isEnable).Should().BeTrue($"Элемент \"{name}\" не активен");
        }

        [Then(@"на веб-странице элемент \""(.+)\"" неактивен")]
        public void WebElementIsDisabled(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var isDisabled = this.commandSupport.SendCommand(() => element.IsDisabled());

            ((bool)isDisabled).Should().BeFalse($"Элемент \"{name}\" активен");
        }

        [Then(@"на веб-странице элемент \""(.+)\"" нельзя редактировать")]
        public void WebElementIsNotEditable(string name)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            var notEditable = this.commandSupport.SendCommand(() => element.IsEditable());

            ((bool)notEditable).Should().BeTrue($"Элемент \"{name}\" доступен для редактирования");
        }

        [Then(@"адрес активной веб-страницы содержит значение \""(.+)\""")]
        public void WebPageUrlContainsExpected(string url)
        {
            this.webContext.GetCurrentPage().GetUrl().Contains(url).Should()
                .BeTrue($"адрес активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetUrl()}\" не содержит \"{url}\"");
        }

        [Then(@"адрес активной веб-страницы содержит значение переменной \""(.+)\""")]
        public void WebPageUrlContainsVarValue(string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.GetCurrentPage().GetUrl().Contains(varValue).Should()
                .BeTrue($"адрес активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetUrl()}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"адрес активной веб-страницы не содержит значение \""(.+)\""")]
        public void WebPageUrlNotContainsExpected(string url)
        {
            this.webContext.GetCurrentPage().GetUrl().Contains(url).Should()
                .BeFalse($"адрес активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetUrl()}\" содержит \"{url}\"");
        }

        [Then(@"адрес активной веб-страницы не содержит значение переменной \""(.+)\""")]
        public void WebPageUrlNotContainsvarValue(string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.GetCurrentPage().GetUrl().Contains(varValue).Should()
                .BeFalse($"адрес активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetUrl()}\" содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"адрес активной веб-страницы равен значению \""(.+)\""")]
        public void WebPageUrlEqualExpected(string expected)
        {
            this.webContext.GetCurrentPage().GetUrl().Should()
                .Be(expected, $"адрес активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetUrl()}\" не равен \"{expected}\"");
        }

        [Then(@"адрес активной веб-страницы равен значению переменной \""(.+)\""")]
        public void WebPageUrlEqualVarValue(string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.GetCurrentPage().GetUrl().Should()
                .Be(varValue, $"адрес активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetUrl()}\" не равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"адрес активной веб-страницы не равен значению \""(.+)\""")]
        public void WebPageUrlNotEqualExpected(string expected)
        {
            this.webContext.GetCurrentPage().GetUrl().Should()
                .NotBe(expected, $"адрес активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetUrl()}\" равен \"{expected}\"");
        }

        [Then(@"адрес активной веб-страницы не равен значению переменной \""(.+)\""")]
        public void WebPageUrlNotEqualVarValue(string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.GetCurrentPage().GetUrl().Should()
                .NotBe(varValue, $"адрес активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetUrl()}\" равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"заголовок веб-страницы равен значению \""(.+)\""")]
        public void WebPageTitleIsEqual(string expected)
        {
            expected.Should().NotBeEmpty("Параметр \"title\" не задан");

            this.webContext.GetCurrentPage().GetTitle().Should()
                .Be(expected, $"Заголовок активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetTitle()}\" не равен \"{expected}\"");
        }

        [Then(@"заголовок веб-страницы равен значению переменной \""(.+)\""")]
        public void WebPageTitleIsEqualVarValue(string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.GetCurrentPage().GetTitle().Should()
                .Be(varValue, $"Заголовок активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetTitle()}\" не равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"заголовок веб-страницы не равен значению \""(.+)\""")]
        public void WebPageTitleIsNotEqual(string expected)
        {
            expected.Should().NotBeEmpty("Параметр \"title\" не задан");

            this.webContext.GetCurrentPage().GetTitle().Should()
                .NotBe(expected, $"Заголовок активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetTitle()}\" равен \"{expected}\"");
        }

        [Then(@"заголовок веб-страницы не равен значению переменной \""(.+)\""")]
        public void WebPageTitleIsNotEqualVarValue(string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.GetCurrentPage().GetTitle().Should()
                .NotBe(varValue, $"Заголовок активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetTitle()}\" равен значению переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"заголовок веб-страницы содержит значение \""(.+)\""")]
        public void WebPageTitleIsContains(string title)
        {
            title.Should().NotBeEmpty("Параметр \"title\" не задан");

            this.webContext.GetCurrentPage().GetTitle().Contains(title).Should()
                .BeTrue($"Заголовок активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetTitle()}\" не содержит \"{title}\"");
        }

        [Then(@"заголовок веб-страницы содержит значение переменной \""(.+)\""")]
        public void WebPageTitleIsContainsVarValue(string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.GetCurrentPage().GetTitle().Contains(varValue).Should()
                .BeTrue($"Заголовок активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetTitle()}\" не содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"заголовок веб-страницы не содержит значение \""(.+)\""")]
        public void WebPageTitleIsNotContains(string title)
        {
            title.Should().NotBeEmpty("Параметр \"title\" не задан");

            this.webContext.GetCurrentPage().GetTitle().Contains(title).Should()
                .BeFalse($"Заголовок активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetTitle()}\" содержит \"{title}\"");
        }

        [Then(@"заголовок веб-страницы не содержит значение переменной \""(.+)\""")]
        public void WebPageTitleIsNotContainsVarValue(string varName)
        {
            var varValue = this.variableContext.GetVariableValueText(varName);
            varValue.Should().NotBeNull($"Значения в переменной {varName} нет");

            this.webContext.GetCurrentPage().GetTitle().Contains(varValue).Should()
                .BeFalse($"Заголовок активной веб страницы \"{this.webContext.GetCurrentPage().GetName()}\":\"{this.webContext.GetCurrentPage().GetTitle()}\" содержит значение переменной \"{varName}\":\"{varValue}\"");
        }

        [Then(@"я убеждаюсь, что на веб-странице появилось диалоговое окно")]
        public void CheckAlert()
        {
            var alert = this.commandSupport.SendCommand(() => driverSupport.GetAlert());
            alert.Should().NotBeNull($"Диалоговое окно не найдено");
        }

        [Then(@"таблица \""(.+)\"" на веб-странице равна:")]
        public void CompareWebTableWith(string name, DataTable table)
        {
            var element = this.webContext.GetCurrentPage().GetElementByName(name);

            if (element is TableElement)
            {
                var webTable = this.commandSupport.SendCommand(() => ((TableElement)element).GetTable());

                try
                {
                    DataTable newTable = new DataTable();

                    foreach (DataRow row in ((DataTable)webTable).Rows)
                    {
                        DataRow dataRow = null;
                        dataRow = table.NewRow();
                        foreach (DataColumn column in table.Columns)
                        {
                            if (!newTable.Columns.Contains(column.ColumnName))
                            {
                                newTable.Columns.Add(column.ColumnName);
                            }
                            dataRow[column.ColumnName] = row[column.ColumnName];
                        }

                        newTable.Rows.Add(dataRow.ItemArray);
                    }

                    DataCompare.AreTablesTheSame(newTable, table).Should()
                        .BeTrue($"таблица в элементе \"{name}\" не совпадает с предложенной таблицей");
                }
                catch (Exception)
                {
                    throw new ArgumentException($"таблица в элементе \"{name}\" не совпадает с предложенной таблицей");
                }
            }
            else
            {
                throw new ArgumentException($"Элемент '{name}' имеет отличный тип от table");
            }
        }
    }
}