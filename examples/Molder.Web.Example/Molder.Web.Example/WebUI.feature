@ignore
@WebUI
Feature: WebUI

Scenario: OpenWebUI
	Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"
		And я закрываю веб-страницу
		And я закрываю браузер
@ignore
# в процессе тестирования
Scenario: OpenWebUI with auth
	Given я инициализирую аутентификацию для прокси сервера:
		| Proxy          | Port | Username | Password |
		| 192.168.99.100 | 9080 | admin    | admin    |
	Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"
	Given выполнено нажатие на элемент "Basic Auth" на веб-странице
		And я обновляю текущую страницу на "Basic Auth"
	Then на веб-странице текст элемента "Text" содержит значение "Congratulations! You must have the proper credentials."
		And я закрываю веб-страницу
		And я закрываю браузер

Scenario: Add/Remove Elements With Block
	Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"

	Given выполнено нажатие на элемент "Add/Remove Elements" на веб-странице
		And я обновляю текущую страницу на "Add/Remove Elements With Block"
		And я перехожу на блок "Add/Remove Elements" на веб-странице
		And выполнено нажатие на элемент "Add Element" на веб-странице
		And выполнено нажатие на элемент "Delete" на веб-странице
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
		
	When я перехожу на блок "Content..row 1" на веб-странице
	Then на веб-странице текст элемента "Text" заполнен

		And я закрываю веб-страницу
		And я закрываю браузер

Scenario: Dynamic Content v.2
		Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"

		Given выполнено нажатие на элемент "Dynamic Content" на веб-странице
		And я обновляю текущую страницу на "Dynamic Content"
		
		When я перехожу на блок "Content..row 1" на веб-странице
		Then на веб-странице текст элемента "Text" заполнен

		Given я перехожу на страницу "Dynamic Content"
		When я перехожу на блок "Content..row 2" на веб-странице
		Then на веб-странице текст элемента "Text" заполнен

		And я закрываю веб-страницу
		And я закрываю браузер

Scenario: Dynamic Content v.3
		Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"

		Given выполнено нажатие на элемент "Dynamic Content" на веб-странице
		And я обновляю текущую страницу на "Dynamic Content"
		
		When я перехожу на блок "Content..row 1" на веб-странице
		Then на веб-странице текст элемента "Text" заполнен

		Given я возвращаюсь к основной веб-странице
		When я перехожу на блок "Content..row 2" на веб-странице
		Then на веб-странице текст элемента "Text" заполнен

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
		
	When я перехожу на фрейм "Bottom" на веб-странице
	Then на веб-странице текст элемента "Text" заполнен
		And я закрываю веб-страницу
		And я закрываю браузер
		
Scenario: Frames v.2
		Given я инициализирую браузер
		And я развернул веб-страницу на весь экран
		And я перехожу на страницу "InternetHerokuapp"

		Given выполнено нажатие на элемент "Frames" на веб-странице
		And я обновляю текущую страницу на "Frames"
		Given выполнено нажатие на элемент "Nested Frames" на веб-странице
		And я обновляю текущую страницу на "Nested Frames"
		
		When я перехожу на фрейм "Bottom" на веб-странице
		Then на веб-странице текст элемента "Text" заполнен
			#переход по Frame.Default() к основной странице
			And я перехожу на стандартный фрейм на веб-странице
		When я перехожу на фрейм "Top" на веб-странице
		When я перехожу на фрейм "Left" на веб-странице
		Then на веб-странице текст элемента "Text" заполнен
			And я перехожу на стандартный фрейм на веб-странице
		And я закрываю веб-страницу
		And я закрываю браузер
