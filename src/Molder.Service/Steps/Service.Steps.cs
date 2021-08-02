using System.Linq;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Molder.Controllers;
using Molder.Service.Controllers;
using Molder.Service.Helpers;
using Molder.Service.Models;
using Molder.Service.Infrastructures;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Collections.Generic;
using System;
using Molder.Helpers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Molder.Extensions;

namespace Molder.Service.Steps
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
        /// <param name="serviceController">Контекст для работы с web сервисами.</param>
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
                { HTTPMethodType.HEAD, HttpMethod.Head},
                { HTTPMethodType.PATCH, HttpMethod.Patch},
            };
        }

        /// <summary>
        /// Очистка подключений к сервисам в конце сценария.
        /// </summary>
        [AfterScenario]
        public void AfterScenario()
        {
            serviceController.Services.Clear();
        }

        [StepArgumentTransformation]
        public RequestDto TableToRequestDTO(Table table)
        {
            var headers = table.CreateSet<Header>();
            return new RequestDto(headers, variableController);
        }

        [StepArgumentTransformation]
        public JToken StringToJToken(string str)
        {
            str = variableController.ReplaceVariables(str);
            return str.ToJson();
        }

        [StepDefinition(@"я создаю json документ ""(.+)"":")]
        public void CreateJson(string varName, JToken token)
        {
            variableController.SetVariable(varName, token.GetType(), token);
        }

        /// <summary>
        /// Вызов Rest сервиса с телом
        /// </summary>
        /// <param name="url">Ссылка на сервис.</param>
        /// <param name="method">Метод сервиса.</param>
        /// <param name="name">Название сервиса.</param>
        /// <param name="requestDto">Параметры вызова.</param>
        [When(@"я вызываю веб-сервис ""(.+)"" по адресу ""(.+)"" с методом ""(.+)"", используя параметры:")]
        public void SendToRestServiceWithBody(string name, string url, HTTPMethodType method, RequestDto requestDto) 
        {
            variableController.Variables.Should().NotContainKey($"Данные по сервису с именем \"{name}\" уже существуют");
            serviceController.Services.Should().NotContainKey($"Данные по сервису с именем \"{name}\" уже существуют");

            url = variableController.ReplaceVariables(url);

            if (requestDto.Query.Any())
            {
                url = url.AddQueryInURL(requestDto.Query);
            }

            if(!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                Log.Logger().LogWarning($"Url {url} is not valid.");
                throw new ArgumentException($"Url {url} is not valid.");
            }            

            var request = new RequestInfo
            {
                Content = requestDto.Content,
                Headers = requestDto.Header,
                Credential = requestDto.Credentials,
                Timeout = requestDto.Timeout,
                Method = webMethods[method],
                Url = url 
            };

            using var service = new WebService();
            
            var responce =  service.SendMessage(request).Result;

            if (responce != null)
            {
                serviceController.Services.TryAdd(name, responce);
            }
            else
            {
                Log.Logger().LogInformation($"Сервис с названием \"{name}\" не добавлен. Подробности в логах");
            }
        }

        /// <summary>
        /// Шаг проверки статуса выполнения web сервиса.
        /// </summary>
        /// <param name="name">Название сервиса.</param>
        /// <param name="status">Статус.</param>
        [Then(@"веб-сервис \""(.+)\"" выполнился со статусом \""(.+)\""")]
        public void Then_ReceivedService_Status(string name, HttpStatusCode status)
        {
            serviceController.Services.Should().ContainKey(name, $"Сервис с названием \"{name}\" не существует");

            serviceController.Services.TryGetValue(name, out var service);
            status.Should().Be(service.StatusCode, $"Статус сервиса \"{name}\":\"{service.StatusCode}\" не равен \"{status}\"");
        }

        /// <summary>
        /// Шаг сохранения результата вызова web сервиса как строка в переменную.
        /// </summary>
        /// <param name="name">Название сервиса.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [Then(@"я сохраняю результат вызова веб-сервиса \""(.+)\"" как текст в переменную \""(.+)\""")]
        public void StoreReceivedResultInVariable_String(string name, string varName)
        {
            serviceController.Services.Should().ContainKey(name, $"Сервис с названием \"{name}\" не существует");
            variableController.Variables.Should().NotContainKey(name, $"Сервис с названием \"{name}\" существует");

            serviceController.Services.TryGetValue(name, out var service);
            service.Content.Should()
                .NotBeNull($"Результат вызова сервиса с названием \"{name}\" не существует");

            Log.Logger().LogInformation($"Результат сервиса \"{name}\" (сериализован): {Environment.NewLine}{service.Content}");
            variableController.SetVariable(varName, service.Content.GetType(), service.Content);
        }

        /// <summary>
        /// Шаг сохранения результата вызова web сервиса как json в переменную.
        /// </summary>
        /// <param name="name">Название сервиса.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [Then(@"я сохраняю результат вызова веб-сервиса \""(.+)\"" как json в переменную \""(.+)\""")]
        public void StoreReceivedResultInVariable_Json(string name, string varName)
        {
            serviceController.Services.Should().ContainKey(name, $"Сервис с названием \"{name}\" не существует");
            variableController.Variables.Should().NotContainKey(name, $"Сервис с названием \"{name}\" существует");

            serviceController.Services.TryGetValue(name, out var service);
            service.Content.Should()
                .NotBeNull($"Результат вызова сервиса с названием \"{name}\" не существует");

            var json = service.Content.ToJson();

            Log.Logger().LogInformation($"Результат сервиса \"{name}\" (сериализован): {Environment.NewLine}{json}");
            variableController.SetVariable(varName, json.GetType(), service.Content);
        }

        /// <summary>
        /// Шаг сохранения результата вызова web сервиса как xml в переменную.
        /// </summary>
        /// <param name="name">Название сервиса.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [Then(@"я сохраняю результат вызова веб-сервиса \""(.+)\"" как xml в переменную \""(.+)\""")]
        public void StoreReceivedResultInVariable_Xml(string name, string varName)
        {
            serviceController.Services.Should().ContainKey(name, $"Сервис с названием \"{name}\" не существует");
            variableController.Variables.Should().NotContainKey(name, $"Сервис с названием \"{name}\" существует");

            serviceController.Services.TryGetValue(name, out var service);
            service.Content.Should()
                .NotBeNull($"Результат вызова сервиса с названием \"{name}\" не существует");

            var xml = service.Content.ToXml();

            Log.Logger().LogInformation($"Результат сервиса \"{name}\" (сериализован): {Environment.NewLine}{xml}");
            variableController.SetVariable(varName, xml.GetType(), xml);
        }
    }
}