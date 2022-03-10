@ignore
Feature: Kafka

Scenario: Kafka Example
    Given запустить обработчик очереди kafka c именем "test"
    Given сохранить сообщения для kafka очереди "test" в переменную "mes"
    Given write list from variable "mes"
    