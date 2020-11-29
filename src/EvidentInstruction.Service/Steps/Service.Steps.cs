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
using EvidentInstruction.Service.Infrastructures;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System;
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
       // private readonly Dictionary<object, Func<string, Encoding, string, StringContent>> content;

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
                { HTTPMethodType.POST, HttpMethod.Post}
            };

            /*content = new Dictionary<object, Func<string, Encoding, string, StringContent>>
            {
                {new XDocument(), (replaceContent, defEncod, defType) => new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.TEXT)},
                {new XmlDocument(), (replaceContent, defEncod, defType) => new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.TEXT)},
                {new JObject(), (replaceContent, defEncod, defType) => new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.TEXT)},
                { (object)string , (replaceContent, defEncod, defType) => new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.TEXT)}



            };*/
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
        /// Определяем какой тип у Body
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [StepArgumentTransformation]
        public StringContent StringToContent(string content)
        {
            
            var str = variableController.GetVariableValue(content);

            //var type = ServiceHelpers.GetObjectFromString(str.ToString()/*.GetType().ToString()*/); //зачем toString& //убрать!
            //если есть
            var replaceContent = this.variableController.ReplaceVariables(str.ToString());


            var doc = ServiceHelpers.GetObjectFromString(replaceContent);
            StringContent stringContent = null;
            switch (doc)
            {
                case XDocument xDoc:
                case XmlDocument xmlDocument:
                    {
                        stringContent = new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.XML);
                        break;
                    }
                case JObject jObject:
                    {
                        stringContent = new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.JSON);
                        break;
                    }
                default:
                    {
                        stringContent = new StringContent(replaceContent, Encoding.UTF8, DefaultContentType.TEXT);
                        break;
                    }
            }

            //logger&
            return stringContent;
        }

        /// <summary>
        /// Вызов Rest сервиса с телом
        /// </summary>
        /// <param name="url">Ссылка на сервис.</param>
        /// <param name="method">Метод сервиса.</param>
        /// <param name="service">Название сервиса.</param>
        /// <param name="parameters">Параметры вызова.</param>
        [Scope(Tag = "WebService")]
        [When(@"я вызываю веб-сервис ""([A-z]+)"" по адресу ""(.+)"" с методом ""(.+)"", используя тело ""(.+)"" и заголовки  :")]
        public void SendToRestServiceWithBody(string name, string url, HTTPMethodType method, StringContent content, Table table/* можно класс, чтобы сразу заполнить мождельку*/) //вместо table класс?
        {
           
            this.variableController.Variables.ContainsKey(name).Should().BeFalse($"Данные по сервису с именем \"{name}\" уже существуют");
            this.serviceController.Services.ContainsKey(name).Should().BeFalse($"Данные по сервису с именем \"{name}\" уже существуют");

           
            var headers = table.CreateSet<Header>().ToDictionary(header => header.Name, header => this.variableController.ReplaceVariables(header.Value));
            //будет возвращать через transformation с 3 значениями
            var request = new RequestInfo
            {
                Content = content,
                Headers = headers,
                Method = webMethods[method],
                Url = url
            };

            /*using (var service = new FlurlProvider(request))
            {
               var responce =  service.SendAsync(request, method);
                *//*this.serviceController.Services.TryAdd(name, responce);
                this.variableController.SetVariable(name, responce.Content.GetType(), responce.Content);*//*
            }*/
             
           using (var service = new WebService())
            {
                var responceInfo = service.SendMessage(request, webMethods);
                this.serviceController.Services.TryAdd(name, responceInfo);
                this.variableController.SetVariable(name, responceInfo.Content.GetType(), responceInfo.Content);
            }
        }

        /// <summary>
        /// Вызов Rest сервиса.
        /// </summary>
        /// <param name="url">Ссылка на сервис.</param>
        /// <param name="method">Метод сервиса.</param>
        /// <param name="service">Название сервиса.</param>
        /// <param name="parameters">Параметры вызова.</param>
        [Scope(Tag = "WebService")]
        [When(@"я вызываю веб-сервис ""([A-z]+)"" по адресу ""(.+)"" с методом ""(.+)"", используя только заголовки ""(.+)"" :")]
        public void SendToRestService(string name, string url, HTTPMethodType method, Table table) 
        {
            this.variableController.Variables.ContainsKey(name).Should().BeFalse($"Данные по сервису с именем \"{name}\" уже существуют");
            this.serviceController.Services.ContainsKey(name).Should().BeFalse($"Данные по сервису с именем \"{name}\" уже существуют");

          
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
