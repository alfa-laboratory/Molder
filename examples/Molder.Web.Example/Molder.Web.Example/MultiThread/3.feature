@WebUI
Feature: Task #3

Scenario: Add/Remove Elements
	Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"

	Given выполнено нажатие на элемент "Add/Remove Elements" на веб-странице
		And я обновляю текущую страницу на "Add/Remove Elements"
		And выполнено нажатие на элемент "Add Element" на веб-странице
		And выполнено нажатие на элемент "Delete" на веб-странице
		And я закрываю веб-страницу
		And я закрываю браузер