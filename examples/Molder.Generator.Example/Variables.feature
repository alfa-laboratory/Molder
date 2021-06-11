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