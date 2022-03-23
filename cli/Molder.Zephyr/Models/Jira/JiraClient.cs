using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using Molder.Zephyr.Infrastructure;
using Molder.Zephyr.Models.Dto;
using Newtonsoft.Json.Linq;

namespace Molder.Zephyr.Models.Jira
{
    public class JiraClient
    {
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Version { get; set; }
        
        public async Task<int?> GetIssueIdBy(string issueKey)
        {
            int? issueId = null;
            try
            {
                var response = await $"{Url}/{API.JIRA}/{Version}/issue/{issueKey}".WithBasicAuth(Login, Password).GetAsync();
                var content = response.ResponseMessage.Content;
                var str = await content.ReadAsStringAsync();
                var token = JToken.Parse(str);
                var id = token.SelectToken("id");
                issueId = int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return issueId;
        }
        
        public async Task<List<TestStep>> GetTestStepsBy(int issueId)
        {
            var testSteps = new List<TestStep>();
            try
            {
                var response = await $"{Url}/{API.ZEPHYR}/{Version}/teststep/{issueId}".WithBasicAuth(Login, Password).GetAsync();
                var content = response.ResponseMessage.Content;
                var str = content.ReadAsStringAsync().Result;
                var token = JToken.Parse(str);

                if (token.SelectToken("stepBeanCollection") is JArray stepBeanCollection)
                {
                    testSteps.AddRange(stepBeanCollection.Select(
                        stepBean => new TestStep()
                        {
                            Id = int.Parse(stepBean.SelectToken("id").ToString()), 
                            Step = stepBean.SelectToken("step").ToString(), 
                            Data = stepBean.SelectToken("data").ToString(), 
                            OrderId = int.Parse(stepBean.SelectToken("orderId").ToString())
                        }));
                }
            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return testSteps;
        }

        public async Task<int?> CreateNewTestStepBy(int issueId, string step)
        {
            int? testStepId = null;
            try
            {
                var response = await $"{Url}/{API.ZEPHYR}/{Version}/teststep/{issueId}".WithBasicAuth(Login, Password)
                    .PostJsonAsync(
                        new 
                        { 
                                step = $"{step}", 
                                data = "",
                                result = ""
                        });

                var statusCode = response.ResponseMessage.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    var content = response.ResponseMessage.Content;
                    var str = content.ReadAsStringAsync().Result;
                    var token = JToken.Parse(str);
                    var id = token.SelectToken("id");
                    testStepId = int.Parse(id.ToString());
                }
            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return testStepId;
        }

        public async Task<bool> UpdateTestStepBy(int issueId, int id, string step, string data)
        {
            try
            {
                var response = await $"{Url}/{API.ZEPHYR}/{Version}/teststep/{issueId}/{id}".WithBasicAuth(Login, Password)
                    .PutJsonAsync(
                        new 
                        { 
                            step = $"{step}", 
                            data = $"{data}"
                        });
                var statusCode = response.ResponseMessage.StatusCode;
                
                return statusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return false;
        }

        public async Task<int?> GetProjectIdBy(string projectKey)
        {
            int? projectId = null;
            try
            {
                var response = await $"{Url}/{API.JIRA}/{Version}/project/{projectKey}".WithBasicAuth(Login, Password)
                    .GetAsync();
                var content = response.ResponseMessage.Content;
                var str = content.ReadAsStringAsync().Result;
                var token = JToken.Parse(str);
                var id = token.SelectToken("id");
                projectId = int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return projectId;
        }

        public async Task<int?> GetVersionBy(int? projectId, string versionName)
        {
            int? versionId = null;
            try
            {
                var response = await $"{Url}/{API.JIRA}/{Version}/project/{projectId}/versions".WithBasicAuth(Login, Password).GetAsync();
                var content = response.ResponseMessage.Content;
                var str = content.ReadAsStringAsync().Result;
                var array = JToken.Parse(str) as JArray;
                
                var versions = new List<Dto.Version>();
                versions.AddRange(array!.Select(
                    el => new Dto.Version
                    {
                        Id = int.Parse(el.SelectToken("id").ToString()),
                        Name = el.SelectToken("name").ToString()
                    }));

                var version = versions.FirstOrDefault(v => v.Name == versionName);
                if (version is not null)
                    versionId = version.Id;
            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return versionId;
        }
        
        public async Task<int?> CreateCycle(int? projectId, int? versionId, string cycle)
        {
            int? cycleId = null;
            try
            {
                var response = await $"{Url}/{API.ZEPHYR}/{Version}/cycle".WithBasicAuth(Login, Password)
                    .PostJsonAsync(
                        new 
                        { 
                            name = $"{cycle}", 
                            projectId = projectId,
                            versionId = versionId
                        });
                var statusCode = response.ResponseMessage.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    var content = response.ResponseMessage.Content;
                    var str = content.ReadAsStringAsync().Result;
                    var token = JToken.Parse(str);
                    var id = token.SelectToken("id");
                    cycleId = int.Parse(id.ToString());
                }

            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return cycleId;
        }
        
        public async Task<bool> AddTests(int? projectId, int? versionId, int? cycleId, IEnumerable<string> issues)
        {
            try
            {
                var response = await $"{Url}/{API.ZEPHYR}/{Version}/execution/addTestsToCycle".WithBasicAuth(Login, Password)
                    .PostJsonAsync(new
                    {
                        versionId = versionId,
                        method = "1",
                        cycleId = cycleId,
                        projectId = projectId,
                        issues = issues
                    });
                var statusCode = response.ResponseMessage.StatusCode;

                return statusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return false;
        }

        public async Task<int?> CreateExecution(int? projectId, int? versionId, int? issueId, int? cycleId,
            string assignee)
        {
            int? executionId = null;
            try
            {
                var data = new
                {
                    cycleId = cycleId,
                    issueId = issueId,
                    projectId = projectId,
                    versionId = versionId,
                    assigneeType = "assignee",
                    assignee = assignee,
                };
                var response = await $"{Url}/{API.ZEPHYR}/{Version}/execution".WithBasicAuth(Login, Password)
                    .PostJsonAsync(
                        new 
                        { 
                            cycleId = cycleId, 
                            issueId = issueId,
                            projectId = projectId,
                            versionId = versionId,
                            assigneeType = "assignee",
                            assignee = assignee,
                        });
                var statusCode = response.ResponseMessage.StatusCode;

                if (statusCode == HttpStatusCode.OK)
                {
                    var content = response.ResponseMessage.Content;
                    var str = content.ReadAsStringAsync().Result;
                    var token = JObject.Parse(str);
                    var properties = token.Properties();
                    var key = properties.FirstOrDefault()?.Name;
                    executionId = int.Parse(key!);
                }

            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return executionId;
        }

        public async Task<bool> ExecutionNavigator(int? executionId, string project, string fixVersion, string cycle)
        {
            try
            {
                var response = await $"{Url}/{API.ZEPHYR}/{Version}/execution/navigator/{executionId}?zql=project = \"{project}\" AND fixVersion = \"{fixVersion}\" AND cycleName in (\"{cycle}\") AND folderName is EMPTY ORDER BY Execution ASC&expand=executionStatus,checksteps"
                    .WithBasicAuth(Login, Password).GetAsync();
                var statusCode = response.ResponseMessage.StatusCode;

                return statusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return false;
        }
        
        public async Task<List<TestStep>> GetStepResult(int? executionId)
        {
            var testSteps = new List<TestStep>();
            try
            {
                var response = await $"{Url}/{API.ZEPHYR}/{Version}/stepResult?executionId={executionId}".WithBasicAuth(Login, Password).GetAsync();
                var content = response.ResponseMessage.Content;
                var str = content.ReadAsStringAsync().Result;
                var array = JToken.Parse(str) as JArray;
                
                testSteps.AddRange(array.Select(
                    el => new TestStep()
                    {
                        Id = int.Parse(el.SelectToken("id").ToString()), 
                        Step = el.SelectToken("step").ToString(), 
                        Data = el.SelectToken("data").ToString(), 
                        OrderId = int.Parse(el.SelectToken("orderId").ToString())
                    }));
            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return testSteps;
        }
        
        public async Task<bool> UpdateStepResult(int stepResultId, int status, string comment)
        {
            try
            {
                var response = await $"{Url}/{API.ZEPHYR}/{Version}/stepResult/{stepResultId}".WithBasicAuth(Login, Password)
                    .PutJsonAsync(
                        new 
                        { 
                            status = $"{status}",
                            comment = status == 2 ? comment : ""
                        });
                var statusCode = response.ResponseMessage.StatusCode;
                
                return statusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return false;
        }
        
        public async Task<bool> ExecutionStatus(int? executionId, int status)
        {
            try
            {
                var response = await $"{Url}/{API.ZEPHYR}/{Version}/execution/{executionId}/execute".WithBasicAuth(Login, Password)
                    .PutJsonAsync(
                        new 
                        { 
                            status = $"{status}"
                        });
                var statusCode = response.ResponseMessage.StatusCode;
                
                return statusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
            }

            return false;
        }
    }
}