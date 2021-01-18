@WebUI
Feature: Task #2
@ignore
Scenario: Add/Remove Elements With Block
	Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"

	Given выполнено нажатие на элемент "Add/Remove Elements" на веб-странице
		And я обновляю текущую страницу на "Add/Remove Elements With Block"
		And в блоке "Add/Remove Elements" выполнено нажатие на элемент "Add Element" на веб-странице
		And в блоке "Add/Remove Elements" выполнено нажатие на элемент "Delete" на веб-странице
		And я закрываю веб-страницу
		And я закрываю браузер