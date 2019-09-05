Feature: CommonTest
Scenario: Test #1
	Given я сохраняю текст "texxt" в переменную "var"
	Given ОТЛАДКА: показать значение переменной "var"
	Then я убеждаюсь, что значение переменной "var" равно "texxt"