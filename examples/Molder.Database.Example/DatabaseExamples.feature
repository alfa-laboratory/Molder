﻿@ignore
@SqlServer
Feature: DatabaseExamples
	Background: 
		Given я подключаюсь к БД MS SQL Server с названием "QA":
		| Source     | Database     | Login     | Password     |
		| {{SOURCE}} | {{DATABASE}} | {{LOGIN}} | {{PASSWORD}} |

Scenario: SELECT
	Given я выполняю "SELECT" запрос в БД "QA" и сохраняю результат в переменную "result":
"""
SELECT * FROM ftTest
"""
Scenario: INSERT
	Given я сохраняю случайный набор цифр длиной 5 знаков в переменную "id"
	Given я сохраняю случайный набор букв длиной 5 знаков в переменную "name"
	Given я выполняю "INSERT" запрос в БД "QA" и сохраняю результат в переменную "result":
"""
INSERT ftTest (id, name) VALUES ({{id}}, '{{name}}');
"""