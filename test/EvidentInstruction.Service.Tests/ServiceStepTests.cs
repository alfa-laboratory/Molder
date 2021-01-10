using EvidentInstruction.Controllers;
using EvidentInstruction.Service.Controllers;
using EvidentInstruction.Service.Models;
using EvidentInstruction.Service.Models.Interfaces;
using EvidentInstruction.Service.Steps;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using TechTalk.SpecFlow;
using Xunit;

namespace EvidentInstruction.Service.Tests
{

    [ExcludeFromCodeCoverage]
    public class ServiceStepTests
    {        
        private ServiceController serviceController;
        private VariableController variableController;
        private ServiceSteps step;
        private RequestInfo requestInfo;

        public ServiceStepTests()
        {
            serviceController = new ServiceController();
            variableController = new VariableController();
            step = new ServiceSteps(variableController, serviceController);
            requestInfo = new RequestInfo() { Url = "http://test", Method = HttpMethod.Get, Headers = new Dictionary<string, string>() { { "Content-type", "application/json" } }, Content = new StringContent("test") };
        }

        [Theory]
        [InlineData("Content-type", "application/json", "header")]
        [InlineData("Content-type", "text", "BODY")]
        [InlineData("", "test/test", "QuerY")]       
        public void GetRequestDTO_CorrectTable_ReturnRequestDTO(string name, string value, string style)
        {
            var table = new Table(new string[] {"Name", "Value", "Style"});
            table.AddRow(name, value, style);

            var result = step.TableToRequestDTO(table);

            result.Should().NotBeNull();           
        }

        [Theory]
        [InlineData("Content-type", "<p>Test</p>", "BODY")]
        [InlineData("Content-type", "<b><i>Test</i></b>", "BODY")]        
        public void GetRequestDTO_CorrectTableWithBODY_ReturnRequestDTO(string name, string value, string style)
        {
            var table = new Table(new string[] { "Name", "Value", "Style" });
            table.AddRow(name, value, style);

            var result = step.TableToRequestDTO(table);

            result.Content.Headers.ContentType.ToString().Should().Be("text/xml; charset=utf-8");
        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData(null, null, null)]             
        public void GetRequestDTO_IncorrectTable_ReturnEmpty(string name, string value, string style)
        {
            var table = new Table(new string[] { "Name", "Value", "Style" });
            table.AddRow(name, value, style);

            var result = step.TableToRequestDTO(table);

            result.Header.Should().BeEmpty();
            result.Body.Should().BeEmpty();
            result.Query.Should().BeEmpty();
            result.Content.Headers.Should().NotBeEmpty();
        }

        [Theory]        
        [InlineData("test", "test/test", "")]
        [InlineData("", "test/test", "QuerrY")]
        public void GetRequestDTO_IncorrectTable_ReturnEmptyQuery(string name, string value, string style)
        {
            var table = new Table(new string[] { "Name", "Value", "Style" });
            table.AddRow(name, value, style);

            var result = step.TableToRequestDTO(table);
            result.Query.Should().BeEmpty();
        }

#if ToDOSendToRestServiceWithBody
        [Fact]        
        public void SendToRestServiceWithBody_EmptyUrl_ReturnVariable()
        {            
            var table = new Table(new string[] { "Name", "Value", "Style" });
            table.AddRow("Content-type", "application/json", "header");           

            var requestDto = step.TableToRequestDTO(table);
         
            var mockWebService = new Mock<IWebService>();            
            IWebService webService = new WebService(requestInfo);

            mockWebService
                .Setup(u => u.SendMessage(It.IsAny<RequestInfo>())).Returns(new ResponceInfo() { StatusCode = HttpStatusCode.OK, Request= requestInfo });

            webService = mockWebService.Object;

            step.SendToRestServiceWithBody("test", "http://test", Infrastructures.HTTPMethodType.GET, requestDto);

            this.serviceController.Should().NotBeNull();
            this.variableController.Variables.Should().NotBeEmpty();
           
        }
#endif
    }
}
