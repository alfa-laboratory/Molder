@DBAccess
Feature: DatabaseTests
Background: 
	Given я подключаюсь к БД MS SQL Server с названием "test":
		| Source               | Database | Login | Password                 |
		| FOSSEGRIM\SQLEXPRESS | AFT      | QA    | rUC/FX3wBiHBJs/m5IKxiw== |

Scenario: Select Test
	When я выбираю единственную запись из БД "test" и сохраняю её в переменную "result":
"""
	SELECT TOP 1 * FROM ftTest;
"""
	Then я убеждаюсь, что значение переменной "result" существует

Scenario: Insert Test
	Given я сохраняю случайный набор цифр длиной 5 знаков в переменную "num"
		And я сохраняю случайный набор букв и цифр длиной 10 знаков в переменную "name"

	When я заношу записи в БД "test" в таблицу "ftTest" и сохраняю результат в переменную "result":
		| id    | name   |
		| {num} | {name} |
	Then я убеждаюсь, что значение переменной "result[0][0]" существует
Scenario: _Update Test
Scenario: _Delete Test
Scenario: _Execute Test



