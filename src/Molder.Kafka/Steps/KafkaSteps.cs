using Confluent.Kafka;
using FluentAssertions;
using Molder.Controllers;
using Molder.Extensions;
using Molder.Kafka.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Molder.Kafka.Steps
{
    [Binding]
    public class KafkaSteps
    {
        private readonly object kafkaLock = new();
        
        private readonly VariableController variableController;

        public KafkaSteps(VariableController variableController)
        {
            this.variableController = variableController;
        }
        
        #region Transformations
        /// <summary>
        /// Трансформация параметров подключения к Kafka.
        /// </summary>
        /// <param name="table">Параметры подключения.</param>
        /// <returns>Параметры подключения к Kafka.</returns>
        [StepArgumentTransformation]
        public ConsumerConfig GetKafkaParametersFromTable(Table table)
        {
            return table.ReplaceWith(variableController).CreateInstance<ConsumerConfig>();
        }
        
        #endregion
        #region Connections
        /// <summary>
        /// Подключение к Kafka.
        /// </summary>
        /// <param name="name">Название подключения.</param>
        /// <param name="topic">Название топика.</param>
        /// <param name="consumerConfig">Параметры подключения.</param>
        [Given(@"я создаю параметры для подключения к kafka c именем ""(.+)"" и топиком ""(.+)"":")]
        public void ConnectToKafka_Kafka(string name, string topic, ConsumerConfig consumerConfig)
        {
            KafkaSettings.Settings.Should().NotContainKey(name,
                $"параметров подлючения к kafka с именем \"{name}\" не существует");
            
            KafkaSettings.Settings.Add(name, new Settings()
            {
                Config = consumerConfig,
                Name = name,
                Topic = topic
            });
        }
        #endregion
        
        [StepDefinition(@"запустить обработчик очереди kafka c именем \""(.+)\""")]
        public void Run(string name)
        {
            lock (kafkaLock)
            {
                KafkaSettings.Settings.Should().ContainKey(name,
                    $"параметров подлючения к kafka с именем \"{name}\" не существует");
                var setting = KafkaSettings.Settings[name];
                var kafka = new Models.Kafka(name, setting.Topic, setting.Config);
                kafka.CreateConsumer();
                KafkaQuery.KafkaList.Add(name, kafka);
            }
        }

        [StepDefinition(@"сохранить сообщения для kafka очереди \""(.+)\"" в переменную \""(.+)\""")]
        public void SaveMessage(string name, string varName)
        {
            variableController.Variables.Should().NotContainKey(varName, $"переменная \"{varName}\" уже существует");
            KafkaQuery.KafkaList.Should().ContainKey(name, $"подлючения к kafka с именем \"{name}\" не существует");
            var kafka = KafkaQuery.KafkaList[name];
            var messages = kafka.Messages;
            variableController.SetVariable(varName, messages.GetType(), messages);
        }
    }
}