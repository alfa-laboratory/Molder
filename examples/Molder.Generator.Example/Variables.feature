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
	Given я сохраняю Enumerable массив с типом "string" в переменную "Test":
	| 9999999999999 | 888888888888888 | 12345678912345 |
	When я выбираю произвольное значение из коллекции "Test" и записываю его в переменную "tmp"
	Then write variable "tmp"