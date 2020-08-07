using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using EvidentInstruction.Controllers;
using EvidentInstruction.Service.Controllers;
using EvidentInstruction.Service.Helpers;
using EvidentInstruction.Service.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace EvidentInstruction.Service.Steps
{
    /// <summary>
    /// Шаги для работы с web сервисами.
    /// </summary>
    [Binding]
    public class ServiceSteps
    {
        private readonly VariableController variableController;
        private readonly ServiceController serviceController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSteps"/> class.
        /// Привязка шагов работы с с web сервисами к работе с переменным через контекст.
        /// </summary>
        /// <param name="variableController">Контекст для работы с переменными.</param>
        /// <param name="serviceContext">Контекст для работы с web сервисами.</param>
        public ServiceSteps(VariableController variableController, ServiceController serviceController)
        {
            this.variableController = variableController;
            this.serviceController = serviceController;
        }

        /// <summary>
        /// Очистка подключений к сервисам в конце сценария.
        /// </summary>
        [AfterScenario]
        [Scope(Tag = "WebService")]
        [Scope(Tag = "WebServiceAuth")]
        public void AfterScenario()
        {
            this.serviceController.Services.Clear();
        }

        [StepArgumentTransformation]
        public StringContent StringToContent(string content)
        {
            //если есть
            var replaceContent = this.variableController.ReplaceVariables(content);

            var doc = ServiceHelpers.GetObjectFromString(replaceContent);
            StringContent stringContent = null;
            switch (doc)
            {
                case XDocument xDoc:
                case XmlDocument xmlDocument:
                    {
                        stringContent = new StringContent(replaceContent, Encoding.UTF8, "text/xml");
                        break;
                    }
                case JObject jObject:
                    {
                        stringContent = new StringContent(replaceContent, Encoding.UTF8, "application/json");
                        break;
                    }
                default:
                    {
                        stringContent = new StringContent(replaceContent, Encoding.UTF8, "text/plain");
                        break;
                    }
            }
            return stringContent;
        }


        [StepArgumentTransformation]
        public ServiceAttribute TableTrans(Table table)
        {
            ServiceAttribute result = new ServiceAttribute();
            var paramsObject = table.CreateSet<Parameters>();
            foreach (var i in paramsObject)
            {
                variableController.ReplaceVariables(i.Value);
            }

            result.Headers = paramsObject.Where(c => c.Type == ParameterType.Header)
                .ToDictionary(c => c.Name, c => c.Value);
            result.Parameters = paramsObject.Where(c => c.Type == ParameterType.Query)
                .ToDictionary(c => c.Name, c => c.Value);
            result.Timeout = Convert.ToInt32(paramsObject.Where(c => c.Type == ParameterType.Timeout).Select(c => c.Value).Last());
            return result;

        }


        /// <summary>
        /// Вызов Rest сервиса.
        /// </summary>
        /// <param name="url">Ссылка на сервис.</param>
        /// <param name="method">Метод сервиса.</param>
        /// <param name="service">Название сервиса.</param>
        /// <param name="parameters">Параметры вызова.</param>
        [Scope(Tag = "WebService")]
        [When(@"я вызываю веб-сервис \""([A-z]+)\"" по адресу \""(.+)\"" с методом \""(POST|GET|PUT|DELETE)\"", используя заголовки и тело")]
        public void SendToRestService(string name, string url, HttpMethod method, HttpContent content, ServiceAttribute attributes)
        {
            this.variableController.Variables.ContainsKey(name).Should().BeFalse($"Данные по сервису с именем \"{name}\" уже существуют");
            this.serviceController.Services.ContainsKey(name).Should().BeFalse($"Данные по сервису с именем \"{name}\" уже существуют");


            var request = new RequestInfo
            {
                Name = name,
                Content = content,
                Method = method,
                Url = url,
                ServiceAttribute = attributes
            };

            using (var service = new FlurlService(attributes))
            {
                var responseInfo = service.SendMessage(request);
                this.serviceController.Services.TryAdd(name, responseInfo);
                // тип ответа и записать его в нужном типе (выходной контент в свой тип)
                this.variableController.SetVariable(name, responseInfo.Content.GetType(), responseInfo.Content);
            }
        }

        /// <summary>
        /// Шаг проверки статуса выполнения web сервиса.
        /// </summary>
        /// <param name="service">Название сервиса.</param>
        /// <param name="status">Статус.</param>
        [Scope(Tag = "WebService")]
        [Scope(Tag = "WebServiceAuth")]
        [Then(@"веб-сервис \""(.+)\"" выполнился со статусом \""(.+)\""")]
        public void Then_ReceivedService_Status(string name, HttpStatusCode status)
        {
            this.serviceController.Services.SingleOrDefault(_ => _.Key == name).Value.Should()
                .NotBeNull($"Сервис с названием \"{name}\" не существует");

            this.serviceController.Services.TryGetValue(name, out var service);
            status.Should().Be(service.StatusCode, $"Статус сервиса \"{name}\":\"{service.StatusCode}\" не равен \"{status}\"");
        }
    }
}
