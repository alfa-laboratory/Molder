Feature: Test1

@WebService
Scenario: Add two numbers

#When я сохраняю текст "Bla Bla" в переменную "Body"
Given я сохраняю текст в переменную "Body":
"""
bla bla bla
"""
When я вызываю веб-сервис "Test" по адресу "https://www.consul.io/api-docs/agent/service" с методом "Get", используя тело "Body" и заголовки  :
	| Name          | Value                                  | Style  |
	| Content-type  | application/json                       | HEADER |
	| Authorization | Basic bG9hbm1hbmFnZXJfdXNlcjpsbV91c2Vy | HEADER |
	| name           |    {Body}                             |  Body  |

	# можно убрать используя тело ("Body") .. как вар класс параметры, в ктором будут параметры тело, 
	# можно 3 экстеншена с телом, заголовком, квери
	#1. чек тело => header => мб нейм можно перезатереть, на тот, что нужно
	

		
	
		
Then веб-сервис "Test" выполнился со статусом "200"

