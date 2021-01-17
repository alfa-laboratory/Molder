@WebUI
Feature: WebUI

Scenario: OpenWebUI
	Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"
		And я закрываю веб-страницу
		And я закрываю браузер

Scenario: Add/Remove Elements With Block
	Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"

	Given выполнено нажатие на элемент "Add/Remove Elements" на веб-странице
		And я обновляю текущую страницу на "Add/Remove Elements With Block"
		And выполнено нажатие в блоке "Add/Remove Elements" на элемент "Add Element" на веб-странице
		And выполнено нажатие в блоке "Add/Remove Elements" на элемент "Delete" на веб-странице
		And я закрываю веб-страницу
		And я закрываю браузер

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

Scenario: Checkboxes
	Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"

	Given выполнено нажатие на элемент "Checkboxes" на веб-странице
		And я обновляю текущую страницу на "Checkboxes"

	Then на веб-странице элемент "checkbox 1" не выбран
		And на веб-странице элемент "checkbox 2" выбран
	When выполнено нажатие на элемент "checkbox 1" на веб-странице
		And выполнено нажатие на элемент "checkbox 2" на веб-странице

	Then на веб-странице элемент "checkbox 1" выбран
		And на веб-странице элемент "checkbox 2" не выбран

		And я закрываю веб-страницу
		And я закрываю браузер

Scenario: Dropdown
	Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"

	Given выполнено нажатие на элемент "Dropdown" на веб-странице
		And я обновляю текущую страницу на "Dropdown"

	Then на веб-странице текст элемента "Dropdown List" содержит значение "Please select an option"

	When я выбираю в поле "Dropdown List" веб-страницы значение "1"

	When я выбираю в поле "Dropdown List" веб-страницы текст "Option 2"

	When я выбираю в поле "Dropdown List" веб-страницы номер значения "1"

		And я закрываю веб-страницу
		And я закрываю браузер

Scenario: Dynamic Content
	Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"

	Given выполнено нажатие на элемент "Dynamic Content" на веб-странице
		And я обновляю текущую страницу на "Dynamic Content"
		
	Then на веб-странице в блоке "Content\\row 1" текст элемента "Text" заполнен

		And я закрываю веб-страницу
		And я закрываю браузер

Scenario: Frames
	Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"

	Given выполнено нажатие на элемент "Frames" на веб-странице
		And я обновляю текущую страницу на "Frames"
	Given выполнено нажатие на элемент "Nested Frames" на веб-странице
		And я обновляю текущую страницу на "Nested Frames"
		
	Then на веб-странице во фрейме "Bottom" текст элемента "Text" заполнен
		And я закрываю веб-страницу
		And я закрываю браузер