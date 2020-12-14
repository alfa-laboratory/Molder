using System.Linq;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using EvidentInstruction.Controllers;
using EvidentInstruction.Service.Controllers;
using EvidentInstruction.Service.Helpers;
using EvidentInstruction.Service.Models;
using EvidentInstruction.Service.Infrastructures;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Collections.Generic;

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
        private readonly Dictionary<HTTPMethodType, HttpMethod> webMethods;       

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
            webMethods = new Dictionary<HTTPMethodType, HttpMethod>()
            {
                { HTTPMethodType.GET, HttpMethod.Get},
                { HTTPMethodType.PUT, HttpMethod.Put},
                { HTTPMethodType.POST, HttpMethod.Post},
                { HTTPMethodType.DELETE, HttpMethod.Delete},
                { HTTPMethodType.HEAD, HttpMethod.Head}                
            };
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
        
        /// <summary>
        /// Вызов Rest сервиса с телом
        /// </summary>
        /// <param name="url">Ссылка на сервис.</param>
        /// <param name="method">Метод сервиса.</param>
        /// <param name="service">Название сервиса.</param>
        /// <param name="parameters">Параметры вызова.</param>
        [Scope(Tag = "WebService")]
        [When(@"я вызываю веб-сервис ""([A-z]+)"" по адресу ""(.+)"" с методом ""(.+)"", используя параметры :")]
        public void SendToRestServiceWithBody(string name, string url, HTTPMethodType method, Table table) 
        {
            StringContent content = null;

            this.variableController.Variables.ContainsKey(name).Should().BeFalse($"Данные по сервису с именем \"{name}\" уже существуют");
            this.serviceController.Services.ContainsKey(name).Should().BeFalse($"Данные по сервису с именем \"{name}\" уже существуют");

            var headers = table.CreateSet<Header>().ToList();

            var requestDto = new RequestDto(headers, variableController);

            if (requestDto.Body.Any())
            {
                //получаем тип контента
                var doc = ServiceHelpers.GetObjectFromString(requestDto.Body.Values.First());
                //получаем StringContent для формирования RequestInfo
                content = ServiceHelpers.GetStringContent(doc, requestDto.Body.Values.First());
            }

            //Добавляем к Url query
            if(requestDto.Query.Any())
            {
                url = ServiceHelpers.AddQueryInURL(url, requestDto.Query.Values.First()); 
            }

            var request = new RequestInfo
            {
                Content = content,
                Headers = requestDto.Header,
                Method = webMethods[method],
                Url = url 
            };

            using (var service = new WebService(request))
            {
                var responce =  service.SendMessage(request, webMethods);
                this.serviceController.Services.TryAdd(name, responce);
                this.variableController.SetVariable(name, responce.Content.GetType(), responce.Content);
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
