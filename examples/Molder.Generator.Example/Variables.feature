@WebService
Feature: Variables

Background: 
	Given write variable "Key1"
	Given я изменяю значение переменной "Key1" на "11"
	Given write variable "Key1"

Scenario: Create RandomDate
	Given я сохраняю рандомную дату в переменную "randomDate"

Scenario: Create Random Text
	Given я сохраняю случайный набор букв длиной 10 знаков в переменную "text"
	
Scenario: Create Random UUID
	Given я сохраняю новый UUID в переменную "randomUUID"

Scenario: Create Enumerable list
	Given я сохраняю коллекцию с типом "int" в переменную "Test":
	|8|133|64|
	When я выбираю значение из коллекции "Test[1]" и записываю его в переменную "tmp"
	Then write variable "tmp"

Scenario: Create Dictionary
	Given я сохраняю словарь в переменную "Test":
	| Test | Qwerty   | asdf  |
	| 56   | "qweasd" | "zxc" |
	When я выбираю значение из коллекции "Test[Qwerty]" и записываю его в переменную "tmp"
	Then write variable "tmp"