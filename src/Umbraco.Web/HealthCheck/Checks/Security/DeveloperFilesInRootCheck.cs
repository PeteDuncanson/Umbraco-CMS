using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.HealthChecks;

namespace Umbraco.Web.HealthCheck.Checks.Security
{
    [HealthCheck(
        "a3949eab-3932-4600-989e-24a6c5a127e4",
        "Developer files in website root",
        Description = "Checks to see if you have any common developers files in on your website which might leak sensitive data.",
        Group = "Security")]
    public class DeveloperFilesInRootCheck : HealthCheck
    {
        private readonly ILocalizedTextService _textService;

        public DeveloperFilesInRootCheck(HealthCheckContext healthCheckContext) : base(healthCheckContext)
        {
            _textService = healthCheckContext.ApplicationContext.Services.TextService;
        }

        /// <summary>
        /// Get the status for this health check
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<HealthCheckStatus> GetStatus()
        {
            var customSetting = GetSettings();

            // We should get a dictionary of values that we can use as regexp so just cast this to a List<string> for ease
            return new[] { CheckForFiles( customSetting.Values.ToList<string>() ) };
        }

        /// <summary>
        /// Executes the action and returns it's status
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public override HealthCheckStatus ExecuteAction(HealthCheckAction action)
        {
            throw new InvalidOperationException("DeveloperFilesInRootCheck has no executable actions");
        }

        private List<string> FindFiles( List<string> whiteListedFiles )
        {
            var foundFiles = new List<string>();

            var filesInRoot = Directory.GetFiles(HostingEnvironment.MapPath("~"));

            List<Regex> whiteListedRegExpressions = new List<Regex>();
            foreach( var whiteListedFile in whiteListedFiles )
            {
                if (!String.IsNullOrEmpty(whiteListedFile))
                {
                    Regex whiteListedRegExp = new Regex(whiteListedFile, RegexOptions.IgnoreCase);
                    whiteListedRegExpressions.Add(whiteListedRegExp);
                }
            }

            foreach (var file in filesInRoot )
            {
                var inWhiteList = false;

                foreach( var whiteListedRegEx in whiteListedRegExpressions )
                {
                    if ( whiteListedRegEx.IsMatch( file ) )
                    {
                        inWhiteList = true;
                        continue;
                    }
                }

                if (!inWhiteList)
                {
                    foundFiles.Add(file);
                }
            }

            return foundFiles;
        }

        private HealthCheckStatus CheckForFiles( List<string> whitelistedFiles )
        {
            var message = string.Empty;
            var foundFiles = FindFiles(whitelistedFiles);

            if (foundFiles.Any())
            {
                message = _textService.Localize("healthcheck/developerFilesInRootFound", new[] { String.Join("</li><li>", foundFiles.ToArray<string>()) });
            }
            else
            {
                message = _textService.Localize("healthcheck/developerFilesInRootNotFound");
            }

            var actions = new List<HealthCheckAction>();
            return
                new HealthCheckStatus(message)
                {
                    ResultType = !foundFiles.Any() ? StatusResultType.Success : StatusResultType.Warning,
                    Actions = actions
                };
        }
    }
}


