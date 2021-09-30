using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Molder.Helpers;
using Molder.Models.Configuration;
using Molder.ReportPortal.Helper;
using Molder.ReportPortal.Infrastructures;
using Molder.ReportPortal.Models.Settings;
using ReportPortal.Client;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.SpecFlowPlugin;
using ReportPortal.SpecFlowPlugin.EventArguments;
using System;
using TechTalk.SpecFlow;

namespace Molder.ReportPortal.Hooks
{
    [Binding]
    class Hooks : Steps
    {
        [BeforeTestRun(Order = -9000000)]

        public static void InitializeConfiguration()
        {
            var settings = ConfigOptionsFactory.Create(ConfigurationExtension.Instance.Configuration);

            if (settings.Value is null)
            {
                Log.Logger().LogInformation($@"appsettings is not contains {Constants.CONFIG_BLOCK} block.");
            }
            else
            {
                Log.Logger().LogInformation($@"appsettings contains {Constants.CONFIG_BLOCK} block. Settings selected.");
                ReportPortalSettings.Settings = settings.Value;
                AddCustomHandlers();
            }
        }

        private static void AddCustomHandlers()
        {
            if (ReportPortalSettings.Settings.Enabled)
            {
                ReportPortalAddin.Initializing += ReportPortalAddin_Initializing;
                ReportPortalAddin.BeforeRunStarted += ReportPortalAddin_BeforeRunStarted;
            } 
        }

        /// <summary>
        /// set up RP server properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ReportPortalAddin_Initializing(object sender, InitializingEventArgs e)
        {
            e.Service = new Service(
                new Uri(ReportPortalSettings.Settings.ServerSettings.Url),
                ReportPortalSettings.Settings.ServerSettings.Project,
                ReportPortalSettings.Settings.ServerSettings.Token);
        }

        /// <summary>
        /// set up RP launch properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ReportPortalAddin_BeforeRunStarted(object sender, RunStartedEventArgs e)
        {
            e.StartLaunchRequest.Description = ReportPortalSettings.Settings.LaunchSettings.Description;
            e.StartLaunchRequest.Name = ReportPortalSettings.Settings.LaunchSettings.Name;
            e.StartLaunchRequest.IsRerun = ReportPortalSettings.Settings.LaunchSettings.IsRerun;
            e.StartLaunchRequest.RerunOfLaunchUuid = ReportPortalSettings.Settings.LaunchSettings.RerunOfLaunchUuid;
            e.StartLaunchRequest.StartTime = ReportPortalSettings.Settings.LaunchSettings.StartTime;

#if DEBUG
            e.StartLaunchRequest.Mode = LaunchMode.Debug;
#else
            e.StartLaunchRequest.Mode = LaunchMode.Default;
#endif
            ReportPortalSettings.Settings.LaunchSettings.Tags.ForEach(t =>
            e.StartLaunchRequest.Attributes.Add(new ItemAttribute { Value = t }));
        }

        
    }
}
