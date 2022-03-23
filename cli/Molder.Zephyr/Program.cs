using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Molder.Zephyr.Models;
using Molder.Zephyr.Models.Jira;
using Newtonsoft.Json;

namespace Molder.Zephyr
{
    class Program
    {
        static Task Main(string[] args)
        {
            return Task.Run(() => 
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(RunPublish)
                .WithNotParsed(HandleParseError));
        }
        
        private static void RunPublish(CommandLineOptions options)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            
            // 1. распарсить документ на статусы и задачи
            
            ProcessBar.ProcessBar.Write("Парсинг документа с отчетом прогона тестов...");
            
            string content;
            if (File.Exists(options.Path))
            {
                content = File.ReadAllTextAsync(options.Path).Result;
            }
            else
            {
                ProcessBar.ProcessBar.Error("Файл с отчетом не найден");
                return;
            }

            List<Feature> report;
            try
            {
                report = JsonConvert.DeserializeObject<List<Feature>>(content);
            }
            catch (Exception ex)
            {
                ProcessBar.ProcessBar.Error(ex.Message);
                return;
            }

            ProcessBar.ProcessBar.Done();
            
            // _ создать подключение к Jira
            
            ProcessBar.ProcessBar.Write("Подключение к Jira...");
            var jiraClient = new JiraClient
            {
                Url = options.Url,
                Login = options.Login,
                Password = options.Password,
                Version = options.Version
            };
            
            ProcessBar.ProcessBar.Done();
            
            // 2. в задачах обновить TestStep's 

            ProcessBar.ProcessBar.Write("Обновление задач в Jira...");
            ProcessBar.ProcessBar.Done();
            
            foreach (var feature in report)
            {
                if (feature.Task is not null)
                {
                    var issueId = jiraClient.GetIssueIdBy(feature.Task).Result;
                    if (issueId is not null)
                    {
                        ProcessBar.ProcessBar.Information($"Задача для feature файла {feature.Name} найдена.");
                        
                        feature.IssueId = (int) issueId;
                        
                        var testSteps = jiraClient.GetTestStepsBy((int) issueId).Result;
                        if (testSteps.Any())
                        {
                            ProcessBar.ProcessBar.Information($"В задаче \"{feature.Task}\" имеются зафиксированные шаги.");
                            var orderedTestSteps = testSteps.OrderBy(ts => ts.OrderId);
                            if (orderedTestSteps.Any() && feature.Scenarios.Any())
                            {
                                feature.Scenarios = feature.Scenarios.OrderByDescending(sc => sc.OrderId.HasValue).ThenBy(sc=>sc.OrderId);
                                
                                foreach (var scenario in feature.Scenarios)
                                {
                                    var scenariosById = orderedTestSteps.FirstOrDefault(ts => ts.OrderId == scenario.OrderId);
                                    if (scenariosById is not null)
                                    {
                                        if (!scenariosById.Step.Equals(scenario.Name))
                                        {
                                            var updateTestStep = jiraClient.UpdateTestStepBy((int) issueId,
                                                    scenariosById.Id, scenario.Name, scenariosById.Data).Result;
                                            
                                            if (!updateTestStep)
                                            {
                                                ProcessBar.ProcessBar.Warning($"Для сценария \"{scenario.Name}\" в задаче \"{feature.Task}\" не обновился шаг.");
                                                continue;
                                            }
                                            scenario.TestStepId = scenariosById.Id;
                                            ProcessBar.ProcessBar.Information($"Для сценария \"{scenario.Name}\" в задаче \"{feature.Task}\" обновился шаг.");
                                        }
                                    }
                                    else
                                    {
                                        var testStepId = jiraClient.CreateNewTestStepBy((int)issueId, scenario.Name).Result;
                                        if (testStepId is null)
                                        {
                                            ProcessBar.ProcessBar.Warning($"Для сценария \"{scenario.Name}\" в задаче \"{feature.Task}\" не создался шаг.");
                                            continue;
                                        }

                                        scenario.TestStepId = (int)testStepId;
                                        ProcessBar.ProcessBar.Information($"Для сценария \"{scenario.Name}\" в задаче \"{feature.Task}\" создался шаг.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            ProcessBar.ProcessBar.Information($"В задаче \"{feature.Task}\" отсутствуют шаги.");
                            foreach (var scenario in feature.Scenarios)
                            {
                                var testStepId = jiraClient.CreateNewTestStepBy((int) issueId, scenario.Name).Result;
                                if (testStepId is null)
                                {
                                    ProcessBar.ProcessBar.Warning($"Для сценария \"{scenario.Name}\" в задачу \"{feature.Task}\" не был добавлен шаг.");
                                }
                                ProcessBar.ProcessBar.Information($"Для сценария \"{scenario.Name}\" в задачу \"{feature.Task}\" был добавлен шаг.");
                                scenario.TestStepId = (int) testStepId;
                            }
                        }
                    }
                    else
                    {
                        ProcessBar.ProcessBar.Warning($"Задача \"{feature.Task}\" для feature файла {feature.Name} не найдена.");
                    }
                }
                else
                {
                    ProcessBar.ProcessBar.Warning($"Feature \"{feature.Name}\" без связанной задачи.");
                }
            }
            
            // 3. найти ProjectId по Key
            
            ProcessBar.ProcessBar.Write("Получение проекта из Jira...");
            var projectId = jiraClient.GetProjectIdBy("RCVK").Result;
            ProcessBar.ProcessBar.Done();

            // 4. найти релиз
            ProcessBar.ProcessBar.Write("Получение релиза из Jira...");
            var versionId = jiraClient.GetVersionBy(projectId, options.Release).Result;
            ProcessBar.ProcessBar.Done();

            // 5. создать Cycle по имени

            ProcessBar.ProcessBar.Write("Создание цикла в релизе...");
            var cycleId = jiraClient.CreateCycle(projectId, versionId, options.TestCycle).Result;
            ProcessBar.ProcessBar.Done();
            
            // 6. добавить в цикл задачи

            ProcessBar.ProcessBar.Write("Добавление в цикл задач...");
            var issues = report.Select(f => f.Task);
            ProcessBar.ProcessBar.Done();
            
            ProcessBar.ProcessBar.Write($"Добавление тестов в цикл \"{options.TestCycle}\"...", jiraClient.AddTests(projectId, versionId, cycleId, issues).Result);
            ProcessBar.ProcessBar.Done();
            
            // 7. для каждой задачи
            // 7.1 создать запуск для задачи
            // 7.2 поменять статусы для TestStep's
            // 7.3 поменять статус задачи

            foreach (var feature in report)
            {
                var executionId =
                    jiraClient.CreateExecution(projectId, versionId, feature.IssueId, cycleId, options.Login).Result;
                
                // 8. поменять статус для запуска
                ProcessBar.ProcessBar.Write("Смена статуса для задачи...", 
                    jiraClient.ExecutionStatus(executionId, (int) feature.Status).Result);
                ProcessBar.ProcessBar.Done();
                
                ProcessBar.ProcessBar.Write("Обновление задачи в Jira...", 
                    jiraClient.ExecutionNavigator(executionId, options.Project, options.Release, options.TestCycle).Result);
                ProcessBar.ProcessBar.Done();
                
                var stepResults = jiraClient.GetStepResult(executionId).Result;
                
                foreach (var scenario in feature.Scenarios)
                {
                    var stepResult = stepResults.FirstOrDefault(sr => sr.OrderId == scenario.OrderId);
                    if (stepResult is not null)
                    {
                        ProcessBar.ProcessBar.Write($"Смена статуса для \"{scenario.Name}\"...", 
                            jiraClient.UpdateStepResult(stepResult.Id, (int) scenario.Status, scenario.Error).Result);
                        ProcessBar.ProcessBar.Done();
                    }
                }
            }
            
            ProcessBar.ProcessBar.Write("Последние приготовления..."); ProcessBar.ProcessBar.Done();
        }
        
        static void HandleParseError(IEnumerable<Error> errs)
        {
            var result = -2;
            Console.WriteLine("errors {0}", errs.Count());
            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
                result = -1;
            Console.WriteLine("Exit code {0}", result);
        }
    }
}