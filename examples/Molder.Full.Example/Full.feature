@ignore
@WebService
Feature: Full
Background: 
	Given я сохраняю текст в переменную "input":
"""
{
    "title": "foo",
    "body": "bar",
    "userId": {{Key1}}
}
"""
	When я вызываю веб-сервис "Post" по адресу "https://jsonplaceholder.typicode.com/posts" с методом "Post", используя параметры:
		| Name         | Value            | Style  |
		| Content-Type | application/json | HEADER |
		| Body         | input            | BODY   |
	Then веб-сервис "Post" выполнился со статусом "Created"

Scenario: GET 1
	Given я создаю json документ "output":
"""
{
  "userId": 1,
  "id": 2,
  "title": "qui est esse",
  "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
}
"""
	When я вызываю веб-сервис "Get" по адресу "https://jsonplaceholder.typicode.com/posts/2" с методом "Get", используя параметры:
		| Name         | Value            | Style  |
		| Content-Type | application/json | HEADER |
	Then веб-сервис "Get" выполнился со статусом "200"
		And я сохраняю результат вызова веб-сервиса "Get" как json в переменную "result"
		And я убеждаюсь, что значение переменной "result.//" равно "{{output.//}}"

Scenario: GET 2
	Given я создаю json документ "output":
"""
{
	"userId": {{Key1}},
	"id": {{Key1}},
	"title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
	"body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
}
"""
	When я вызываю веб-сервис "Get" по адресу "https://jsonplaceholder.typicode.com/posts/1" с методом "Get", используя параметры:
		| Name         | Value            | Style  |
		| Content-Type | application/json | HEADER |
	Then веб-сервис "Get" выполнился со статусом "200"
		And я сохраняю результат вызова веб-сервиса "Get" как json в переменную "result"
		And я убеждаюсь, что значение переменной "result.//" равно "{{output.//}}"
