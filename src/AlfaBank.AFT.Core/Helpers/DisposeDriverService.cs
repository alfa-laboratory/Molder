using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AlfaBank.AFT.Core.Helpers
{
    public static class DisposeDriverService
    {
        private static readonly List<string> _processesToCheck =
            new List<string>
            {
                    "chrome",
                    "firefox",
                    "webdriver"
            };
        public static DateTime? TestRunStartTime { get; set; }
        public static void FinishHim(IWebDriver driver)
        {
            driver?.Dispose();
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                try
                {
                    Debug.WriteLine(process.ProcessName);
                    if (process.StartTime > TestRunStartTime)
                    {
                        var shouldKill = false;
                        foreach (var processName in _processesToCheck)
                        {
                            if (process.ProcessName.ToLower().Contains(processName))
                            {
                                shouldKill = true;
                                break;
                            }
                        }
                        if (shouldKill)
                        {
                            process.Kill();
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
    }
}
