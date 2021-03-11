Feature: Service

@ignore
Scenario: POST
	Given я сохраняю текст в переменную "input":
"""
{
    "title": "foo",
    "body": "bar",
    "userId": 1
}
"""
	Given я создаю json документ "output":
"""
{
  "title": "foo",
  "body": "bar",
  "userId": 1,
  "id": 101
}
"""
	When я вызываю веб-сервис "Post" по адресу "https://jsonplaceholder.typicode.com/posts" с методом "Post", используя параметры:
		| Name         | Value            | Style  |
		| Content-Type | application/json | HEADER |
		| Body         | input          | BODY   |
	Then веб-сервис "Post" выполнился со статусом "Created"
		And я сохраняю результат вызова веб-сервиса "Post" как json в переменную "result"
		And я убеждаюсь, что значение переменной "result.//" равно "{{output.//}}"

@ignore
Scenario: GET
	Given я создаю json документ "output":
"""
{
	"userId": 1,
	"id": 1,
	"title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
	"body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
}
"""
	When я вызываю веб-сервис "Get" по адресу "https://jsonplaceholder.typicode.com/posts/1" с методом "Get", используя параметры:
		| Name         | Value            | Style  |
		| Content-Type | application/json | HEADER |
		| ABS          | {output.//id}    | HEADER |
	Then веб-сервис "Get" выполнился со статусом "200"
		And я сохраняю результат вызова веб-сервиса "Get" как json в переменную "result"
		And я убеждаюсь, что значение переменной "result.//" равно "{{output.//}}"

Scenario: DELETE

@ignore
Scenario: PUT
	Given я сохраняю текст в переменную "input":
"""
{
    "id": 1,
    "title": "foo",
    "body": "bar",
    "userId": 1
}
"""
	Given я создаю json документ "output":
"""
{
    "id": 1,
    "title": "foo",
    "body": "bar",
    "userId": 1
}
"""
	When я вызываю веб-сервис "Put" по адресу "https://jsonplaceholder.typicode.com/posts/1" с методом "Put", используя параметры:
		| Name         | Value            | Style  |
		| Content-Type | application/json | HEADER |
		| Body         | input            | BODY   |
	Then веб-сервис "Put" выполнился со статусом "OK"
		And я сохраняю результат вызова веб-сервиса "Put" как json в переменную "result"
		And я убеждаюсь, что значение переменной "result.//" равно "{{output.//}}"

@ignore
Scenario: PATCH
	Given я сохраняю текст в переменную "input":
"""
{
    "title": "foo"
}
"""
	Given я создаю json документ "output":
"""
{
	"userId": 1,
    "id": 1,
    "title": "foo",
	"body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
}
"""
	When я вызываю веб-сервис "Patch" по адресу "https://jsonplaceholder.typicode.com/posts/1" с методом "Patch", используя параметры:
		| Name         | Value            | Style  |
		| Content-Type | application/json | HEADER |
		| Body         | input            | BODY   |
	Then веб-сервис "Patch" выполнился со статусом "OK"
		And я сохраняю результат вызова веб-сервиса "Patch" как json в переменную "result"
		And я убеждаюсь, что значение переменной "result.//" равно "{{output.//}}"