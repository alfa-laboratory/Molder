using FluentAssertions;
using Molder.Service.Models;
using Molder.Service.Models.Provider;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Molder.Service.Tests
{
    [ExcludeFromCodeCoverage]
    public class WebServiceTests
    {
        private RequestInfo requestInfo;

        public WebServiceTests()
        {
            requestInfo = new RequestInfo() { Url = "http://test", Method = HttpMethod.Get, Headers = new Dictionary<string, string>() { { "Content-type", "application/json" } }, Content = new StringContent("test")  };
        }

        [Fact]
        public void SendMessage_IncorrectRequest_ReturnNull()
        {    
            var mockFlurlProvider = new Mock<IFlurlProvider>();

            var webService = new WebService();

            mockFlurlProvider
                .Setup(u => u.SendRequestAsync(It.IsAny<RequestInfo>())).Throws<Exception>();

            webService.Provider = mockFlurlProvider.Object;
            var result = webService.SendMessage(requestInfo).Result;     
            result.Should().BeNull();
        }

        [Fact]
        public void SendMessage_CorrectRequest_ReturnOK()
        {
            var mockFlurlProvider = new Mock<IFlurlProvider>();
            var response =  new HttpResponseMessage() {  StatusCode = HttpStatusCode.OK, Content = new StringContent("test")};
            var responseTask = Task.FromResult(response);
            var webService = new WebService();

            mockFlurlProvider
                .Setup(u => u.SendRequestAsync(It.IsAny<RequestInfo>())).Returns(responseTask);

            webService.Provider = mockFlurlProvider.Object;
            var result = webService.SendMessage(requestInfo);

            result.Result.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Result.Content.ToString().Should().Be("test");
        }

#if False
        [Fact]
        public void SendMessage_IncorrectRequest_ReturnError()
        {
            var mockSqlProvider = new Mock<IFlurlProvider>();
            var flurlCall = new FlurlCall() { HttpRequestMessage = new HttpRequestMessage() { Content = new StringContent("test") } };
            var ex = new Exception() { HelpLink = "test" };
            var webService = new WebService(requestInfo);

            mockSqlProvider
                .Setup(u => u.SendRequest(It.IsAny<RequestInfo>())).Throws(new FlurlHttpTimeoutException(flurlCall, ex));
                
            webService.fprovider = mockSqlProvider.Object;
            var result = webService.SendMessage(requestInfo);

            result.StatusCode.ToString().Should().Be("OK");
            result.Content.ToString().Should().Be("test");
        }
#endif

    }
}
