// <copyright file="ServiceSteps.Steps.cs" company="AlfaBank">
// Copyright (c) AlfaBank. All rights reserved.
// </copyright>

namespace AlfaBank.AFT.Core.Library.Service
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Xml.Linq;
    using AlfaBank.AFT.Core.Data.Services;
    using AlfaBank.AFT.Core.Helpers;
    using AlfaBank.AFT.Core.Infrastructure.Service;
    using AlfaBank.AFT.Core.Model.Context;
    using AlfaBank.AFT.Core.Model.Service;
    using FluentAssertions;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;
    using Xunit.Abstractions;

    /// <summary>
    /// Шаги для работы с web сервисами.
    /// </summary>
    [Binding]
    public class ServiceSteps
    {
        private readonly VariableContext variableContext;
        private readonly ServiceContext serviceContext;
        private readonly ITestOutputHelper consoleOutputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSteps"/> class.
        /// Привязка шагов работы с с web сервисами к работе с переменным через контекст.
        /// </summary>
        /// <param name="variableContext">Контекст для работы с переменными.</param>
        /// <param name="serviceContext">Контекст для работы с web сервисами.</param>
        /// <param name="consoleOutputHelper">Capturing Output.</param>
        public ServiceSteps(VariableContext variableContext, ServiceContext serviceContext, ITestOutputHelper consoleOutputHelper)
        {
            this.variableContext = variableContext;
            this.serviceContext = serviceContext;
            this.consoleOutputHelper = consoleOutputHelper;
        }

        /// <summary>
        /// Инициализация подключений к сервисам.
        /// </summary>
        [BeforeScenario]
        [Scope(Tag = "WebService")]
        [Scope(Tag = "WebServiceAuth")]
        public void BeforeScenario()
        {
            this.serviceContext.Services = new Dictionary<string, WebService>();
        }

        /// <summary>
        /// Очистка подключений к сервисам в конце сценария.
        /// </summary>
        [AfterScenario]
        [Scope(Tag = "WebService")]
        [Scope(Tag = "WebServiceAuth")]
        public void AfterScenario()
        {
            this.serviceContext.Services.Clear();
        }

        /// <summary>
        /// Вызов Rest сервиса.
        /// </summary>
        /// <param name="url">Ссылка на сервис.</param>
        /// <param name="method">Метод сервиса.</param>
        /// <param name="service">Название сервиса.</param>
        /// <param name="parameters">Параметры вызова.</param>
        [Scope(Tag = "WebService")]
        [When(@"я вызываю веб-сервис на REST по адресу \""(.+)\"" с методом \""(POST|GET|PUT|DELETE)\"", названием \""([A-z]+)\"" и параметрами:")]
        public void SendToRestService(string url, WebServiceMethod method, string service, Table parameters)
        {
            this.serviceContext.Services.SingleOrDefault(key => key.Key == service).Value.Should()
                .BeNull($"Сервис с названием '{service}' уже существует");
            url.Should().NotBeEmpty("Ссылка на сервис не задана");
            parameters.Should().NotBeNull("Параметры не заданы");

            NameValueCollection headersCollection;
            Dictionary<string, string> queryCollection;
            (url, headersCollection, queryCollection) = this.InitializationRestService(url, parameters);

            this.consoleOutputHelper.WriteLine($"url (сериализован): {url}");
            using (var rest = new Rest(url, webServiceMethod: method))
            {
                rest.Headers = headersCollection;
                var (statusCode, errors) = rest.CallWebService();

                this.serviceContext.Services.Add(service, new WebService()
                {
                    Url = url,
                    Service = (Core.Data.Services.Service)rest.Clone(),
                    Body = string.Empty,
                    HeadersCollection = headersCollection,
                    ParametersCollection = queryCollection,
                    StatusCode = statusCode,
                    Errors = errors,
                });
            }
        }

        /// <summary>
        /// Вызов Rest сервиса с полномочиями.
        /// </summary>
        /// <param name="url">Ссылка на сервис.</param>
        /// <param name="method">Метод сервиса.</param>
        /// <param name="service">Название сервиса.</param>
        /// <param name="varCredentials">Полномочия.</param>
        /// <param name="parameters">Параметры вызова.</param>
        [Scope(Tag = "WebServiceAuth")]
        [When(@"я вызываю веб-сервис на REST по адресу \""(.+)\"" с методом \""(POST|GET|PUT|DELETE)\"", названием \""([A-z]+)\"" и параметрами, используя полномочия из переменной \""(.+)\"":")]
        public void SendToRestServiceWithAuth(string url, WebServiceMethod method, string service, string varCredentials, Table parameters)
        {
            this.serviceContext.Services.SingleOrDefault(key => key.Key == service).Value.Should()
                .BeNull($"Сервис с названием '{service}' уже существует");
            url.Should().NotBeEmpty("Ссылка на сервис не задана");
            parameters.Should().NotBeNull("Параметры не заданы");

            var credentials = this.variableContext.GetVariableValue(varCredentials);
            credentials.Should().NotBeNull("Полномочия для входа были не созданы");

            NameValueCollection headersCollection;
            Dictionary<string, string> queryCollection;
            (url, headersCollection, queryCollection) = this.InitializationRestService(url, parameters);

            this.consoleOutputHelper.WriteLine($"url (сериализован): {url}");
            using (var rest = new Rest(url, webServiceMethod: method))
            {
                rest.Headers = headersCollection;
                rest.Credentials = credentials as ICredentials;

                var (statusCode, errors) = rest.CallWebService();

                this.serviceContext.Services.Add(service, new WebService()
                {
                    Url = url,
                    Service = (Core.Data.Services.Service)rest.Clone(),
                    Body = string.Empty,
                    HeadersCollection = headersCollection,
                    ParametersCollection = queryCollection,
                    StatusCode = statusCode,
                    Errors = errors,
                });
            }
        }

        /// <summary>
        /// Вызов Rest сервиса с телом.
        /// </summary>
        /// <param name="url">Ссылка на сервис.</param>
        /// <param name="method">Метод сервиса.</param>
        /// <param name="body">Тело сервиса.</param>
        /// <param name="service">Название сервиса.</param>
        /// <param name="parameters">Параметры вызова.</param>
        [Scope(Tag = "WebService")]
        [When(@"я вызываю веб-сервис на REST по адресу \""(.+)\"" с методом \""(POST|GET|PUT|DELETE)\"", телом из переменной \""(.+)\"", названием \""([A-z]+)\"" и параметрами:")]
        public void SendToRestServiceWithBody(string url, WebServiceMethod method, string body, string service,  Table parameters)
        {
            this.serviceContext.Services.SingleOrDefault(key => key.Key == service).Value.Should()
                .BeNull($"Сервис с названием '{service}' уже существует");
            url.Should().NotBeEmpty("Ссылка на сервис не задана");
            parameters.Should().NotBeNull("Параметры не заданы");
            this.variableContext.Variables.ContainsKey(body).Should().BeTrue($"Переменной '{body}' не существует");

            NameValueCollection headersCollection;
            Dictionary<string, string> queryCollection;
            (url, headersCollection, queryCollection) = this.InitializationRestService(url, parameters);

            var serviceBody = this.variableContext.GetVariableValueText(body);

            serviceBody = this.TransformBody(headersCollection, serviceBody);

            this.consoleOutputHelper.WriteLine($"url (сериализован): {url}");
            this.consoleOutputHelper.WriteLine($"Запрос (сериализован): {Environment.NewLine}{serviceBody}");
            using (var rest = new Rest(url, webServiceMethod: method))
            {
                rest.Headers = headersCollection;
                var (statusCode, errors) = rest.CallWebService(serviceBody);

                this.serviceContext.Services.Add(service, new WebService()
                {
                    Url = url,
                    Service = (Core.Data.Services.Service)rest.Clone(),
                    Body = serviceBody,
                    HeadersCollection = headersCollection,
                    ParametersCollection = queryCollection,
                    StatusCode = statusCode,
                    Errors = errors,
                });
            }
        }

        /// <summary>
        /// Вызов Rest сервиса с телом и полномочиями.
        /// </summary>
        /// <param name="url">Ссылка на сервис.</param>
        /// <param name="method">Метод сервиса.</param>
        /// <param name="body">Тело сервиса.</param>
        /// <param name="service">Название сервиса.</param>
        /// <param name="varCredentials">Полномочия.</param>
        /// <param name="parameters">Параметры вызова.</param>
        [Scope(Tag = "WebServiceAuth")]
        [When(@"я вызываю веб-сервис на REST по адресу \""(.+)\"" с методом \""(POST|GET|PUT|DELETE)\"", телом из переменной \""(.+)\"", названием \""([A-z]+)\"" и параметрами, используя полномочия из переменной \""(.+)\"":")]
        public void SendToRestServiceWithBodyAndAuth(string url, WebServiceMethod method, string body, string service, string varCredentials, Table parameters)
        {
            this.serviceContext.Services.SingleOrDefault(key => key.Key == service).Value.Should()
                .BeNull($"Сервис с названием '{service}' уже существует");
            url.Should().NotBeEmpty("Ссылка на сервис не задана");
            parameters.Should().NotBeNull("Параметры не заданы");
            this.variableContext.Variables.ContainsKey(body).Should().BeTrue($"Переменной '{body}' не существует");
            var credentials = this.variableContext.GetVariableValue(varCredentials);
            credentials.Should().NotBeNull("Полномочия для входа были не созданы");

            NameValueCollection headersCollection;
            Dictionary<string, string> queryCollection;
            (url, headersCollection, queryCollection) = this.InitializationRestService(url, parameters);

            var serviceBody = this.variableContext.GetVariableValueText(body);

            serviceBody = this.TransformBody(headersCollection, serviceBody);

            this.consoleOutputHelper.WriteLine($"url (сериализован): {url}");
            this.consoleOutputHelper.WriteLine($"Запрос (сериализован): {Environment.NewLine}{serviceBody}");
            using (var rest = new Rest(url, webServiceMethod: method))
            {
                rest.Headers = headersCollection;
                rest.Credentials = credentials as ICredentials;
                var (statusCode, errors) = rest.CallWebService(serviceBody);

                this.serviceContext.Services.Add(service, new WebService()
                {
                    Url = url,
                    Service = (Core.Data.Services.Service)rest.Clone(),
                    Body = serviceBody,
                    HeadersCollection = headersCollection,
                    ParametersCollection = queryCollection,
                    StatusCode = statusCode,
                    Errors = errors,
                });
            }
        }

        /// <summary>
        /// ВЫзов Soap сервиса.
        /// </summary>
        /// <param name="url">Ссылка на сервис.</param>
        /// <param name="method">Метод сервиса.</param>
        /// <param name="service">Название сервиса..</param>
        /// <param name="xml">Тело запроса.</param>
        [Scope(Tag = "WebService")]
        [Scope(Tag = "WebServiceAuth")]
        [When(@"я вызываю веб-сервис на SOAP по адресу \""(.+)\"" с методом \""(.+)\"", названием \""(.+)\"" и запросом:")]
        public void SendToWS_SOAP(string url, string method, string service, string xml)
        {
            this.serviceContext.Services.SingleOrDefault(key => key.Key == service).Value.Should()
                .BeNull($"Сервис с названием '{service}' уже существует");
            url.Should().NotBeEmpty("Ссылка на сервис не задана");
            method.Should().NotBeEmpty("Метод сервиса не задан");
            xml.Should().NotBeEmpty("Запрос к сервису не задан");

            url = this.variableContext.ReplaceVariablesInXmlBody(url, (val) => System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)));
            method = this.variableContext.ReplaceVariablesInXmlBody(method, (val) => System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)));
            xml = this.variableContext.ReplaceVariablesInXmlBody(xml, (val) => System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)));

            this.consoleOutputHelper.WriteLine($"url (сериализован): {url}");
            this.consoleOutputHelper.WriteLine($"Запрос (сериализован): {Environment.NewLine}{xml}");
            using (var soap = new Soap(url, method))
            {
                var (statusCode, errors) = soap.CallWebService(xml);

                this.serviceContext.Services.Add(service, new WebService()
                {
                    Url = url,
                    Body = xml,
                    HeadersCollection = soap.Headers,
                    ParametersCollection = null,
                    Service = (Core.Data.Services.Service)soap.Clone(),
                    StatusCode = statusCode,
                    Errors = errors,
                });
            }
        }

        /// <summary>
        /// Шаг сохранения результата вызова web сервиса как объект в переменную.
        /// </summary>
        /// <param name="service">Название сервиса.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [Scope(Tag = "WebService")]
        [Scope(Tag = "WebServiceAuth")]
        [Then(@"я сохраняю результат вызова веб-сервиса \""(.+)\"" как объект в переменную \""(.+)\""")]
        public void Then_I_StoreReceivedResultInVariable_Object(string service, string varName)
        {
            this.serviceContext.Services.SingleOrDefault(_ => _.Key == service).Value.Should()
                .NotBeNull($"Сервис с названием '{service}' не существует");
            var webService = this.serviceContext.Services[service];

            webService.Should()
                .NotBeNull($"Сервис с названием '{service}' не был вызван");

            var body = webService.Service.ToObject();

            body.Should()
                .NotBeNull($"Результат вызова сервиса с названием '{service}' не существует");

            this.variableContext.SetVariable(varName, body.GetType(), body);
        }

        /// <summary>
        /// Шаг сохранения результата вызова web сервиса как строка в переменную.
        /// </summary>
        /// <param name="service">Название сервиса.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [Scope(Tag = "WebService")]
        [Scope(Tag = "WebServiceAuth")]
        [Then(@"я сохраняю результат вызова веб-сервиса \""(.+)\"" как текст в переменную \""(.+)\""")]
        public void StoreReceivedResultInVariable_String(string service, string varName)
        {
            this.serviceContext.Services.SingleOrDefault(_ => _.Key == service).Value.Should()
                .NotBeNull($"Сервис с названием '{service}' не существует");
            var webService = this.serviceContext.Services[service];

            webService.Should()
                .NotBeNull($"Сервис с названием '{service}' не был вызван");

            var body = webService.Service.ToString();

            body.Should()
                .NotBeNull($"Результат вызова сервиса с названием '{service}' не существует");

            this.consoleOutputHelper.WriteLine($"Результат (сериализован): {Environment.NewLine}{body}");
            this.variableContext.SetVariable(varName, body.GetType(), body);
        }

        /// <summary>
        /// Шаг сохранения результата вызова web сервиса как json в переменную.
        /// </summary>
        /// <param name="service">Название сервиса.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [Scope(Tag = "WebService")]
        [Scope(Tag = "WebServiceAuth")]
        [Then(@"я сохраняю результат вызова веб-сервиса \""(.+)\"" как json в переменную \""(.+)\""")]
        public void StoreReceivedResultInVariable_Json(string service, string varName)
        {
            this.serviceContext.Services.SingleOrDefault(_ => _.Key == service).Value.Should()
                .NotBeNull($"Сервис с названием '{service}' не существует");
            var webService = this.serviceContext.Services[service];

            webService.Should()
                .NotBeNull($"Сервис с названием '{service}' не был вызван");

            var body = webService.Service.ToJson();

            body.Should()
                .NotBeNull($"Результат вызова сервиса с названием '{service}' не существует");

            this.consoleOutputHelper.WriteLine($"Результат (сериализован): {Environment.NewLine}{body.ToString()}");
            this.variableContext.SetVariable(varName, body.GetType(), body);
        }

        /// <summary>
        /// Шаг сохранения результата вызова web сервиса как xml в переменную.
        /// </summary>
        /// <param name="service">Название сервиса.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [Scope(Tag = "WebService")]
        [Scope(Tag = "WebServiceAuth")]
        [Then(@"я сохраняю результат вызова веб-сервиса \""(.+)\"" как xml в переменную \""(.+)\""")]
        public void StoreReceivedResultInVariable_Xml(string service, string varName)
        {
            this.serviceContext.Services.SingleOrDefault(_ => _.Key == service).Value.Should()
                .NotBeNull($"Сервис с названием '{service}' не существует");
            var webService = this.serviceContext.Services[service];

            webService.Should()
                .NotBeNull($"Сервис с названием '{service}' не был вызван");

            var body = webService.Service.ToXml();

            body.Should()
                .NotBeNull($"Результат вызова сервиса с названием '{service}' не существует");

            this.consoleOutputHelper.WriteLine($"Результат (сериализован): {Environment.NewLine}{body}");
            this.variableContext.SetVariable(varName, body.GetType(), body);
        }

        /// <summary>
        /// Шаг сохранения результата значения переменной, содержащей cdata в переменную.
        /// </summary>
        /// <param name="cdata">Переменная с cdata.</param>
        /// <param name="varName">Идентификатор переменной.</param>
        [Scope(Tag = "WebService")]
        [Scope(Tag = "WebServiceAuth")]
        [Then(@"я сохраняю значение переменной \""(.+)\"" из CDATA в переменную \""(.+)\""")]
        public void StoreCDataVariable_ToVariable(string cdata, string varName)
        {
            this.variableContext.Variables.ContainsKey(varName).Should().BeTrue($"Переменная '{varName}' уже существует");
            var value = (XElement)this.variableContext.GetVariableValue(cdata);
            value.Should().NotBeNull($"Значение переменной '{cdata}' является NULL");

            var data = ((XText)value.FirstNode).Value;
            data.Should().NotBeNull($"Значение переменной '{cdata}' не является CDATA");
            var xDocument = XDocument.Parse(data);
            this.variableContext.SetVariable(varName, typeof(XDocument), xDocument);
        }

        /// <summary>
        /// Шаг проверки статуса выполнения web сервиса.
        /// </summary>
        /// <param name="service">Название сервиса.</param>
        /// <param name="status">Статус.</param>
        [Scope(Tag = "WebService")]
        [Scope(Tag = "WebServiceAuth")]
        [Then(@"веб-сервис \""(.+)\"" выполнился со статусом \""(.+)\""")]
        public void Then_ReceivedService_Status(string service, HttpStatusCode status)
        {
            this.serviceContext.Services.SingleOrDefault(_ => _.Key == service).Value.Should()
                .NotBeNull($"Сервис с названием '{service}' не существует");
            var webService = this.serviceContext.Services[service];
            webService.StatusCode.Should().NotBeNull($"Статус web сервиса '{service}' равен NULL");
            status.Should().Be(webService.StatusCode, $"Статус сервиса '{service}':{webService.StatusCode} не равен '{status}'");
        }

        private (string, NameValueCollection, Dictionary<string, string>) InitializationRestService(string url, Table parameters)
        {
            var serviceParameters = parameters.CreateSet<Parameter>();
            NameValueCollection headersCollections = null;
            Dictionary<string, string> queryCollection = null;
            var paramsList = serviceParameters.ToList();
            if (paramsList.Any())
            {
                (headersCollections, queryCollection) = this.TransformParameters(paramsList);
            }

            headersCollections?.Count.Should().NotBe(0, "Не задано ни одного заголовка");

            var query = string.Empty;
            if (queryCollection.Any())
            {
                query = this.DictionaryToStringParameters(queryCollection);
            }

            url = this.variableContext.ReplaceVariablesInXmlBody(url, (val) => System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)));

            if (!string.IsNullOrEmpty(query))
            {
                url += $"?{query}";
            }

            return (url, headersCollections, queryCollection);
        }

        private (NameValueCollection HeadersCollection, Dictionary<string, string> QueryCollection) TransformParameters(IEnumerable<Parameter> parameters)
        {
            var headersCollection = new NameValueCollection();
            var queryCollection = new Dictionary<string, string>();

            foreach (var kvp in parameters)
            {
                var value = this.variableContext.ReplaceVariablesInXmlBody(kvp.Value);
                switch (kvp.ParameterType)
                {
                    case ParameterType.QUERY:
                        queryCollection.Add(kvp.Name, value);
                        break;
                    case ParameterType.HEADER:
                        headersCollection.Add(kvp.Name, value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return (headersCollection, queryCollection);
        }

        private string DictionaryToStringParameters(Dictionary<string, string> collection)
        {
            return string.Join("&", collection.Select(_ => string.Concat(_.Key, "=", _.Value)).ToArray());
        }

        private string TransformBody(NameValueCollection headersCollection, string body)
        {
            var contentType = headersCollection["Content-Type"];
            if (string.IsNullOrEmpty(contentType))
            {
                return this.variableContext.ReplaceVariablesInXmlBody(body, (val) => System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)));
            }

            return contentType.Contains("json")
                ? this.variableContext.ReplaceVariablesInJsonBody(body, (val) => System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)))
                : this.variableContext.ReplaceVariablesInXmlBody(body, (val) => System.Security.SecurityElement.Escape(Reflection.ConvertObject<string>(val)));
        }
    }
}
