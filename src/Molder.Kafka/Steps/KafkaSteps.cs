using FluentAssertions;
using Molder.Controllers;
using Molder.Kafka.Models;
using TechTalk.SpecFlow;

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