using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Core.Services;
using Umbraco.Web.HealthCheck.Checks.Config;

namespace Umbraco.Web.HealthCheck
{
    /// <summary>
    /// The abstract health check class
    /// </summary>
    [DataContract(Name = "healtCheck", Namespace = "")]
    public abstract class HealthCheck : IDiscoverable
    {
        protected readonly ILocalizedTextService _textService;

        protected HealthCheck(HealthCheckContext healthCheckContext)
        {
            HealthCheckContext = healthCheckContext;
            //Fill in the metadata
            var thisType = this.GetType();
            var meta = thisType.GetCustomAttribute<HealthCheckAttribute>(false);
            if (meta == null)
                throw new InvalidOperationException(
                    string.Format("The health check {0} requires a {1}", thisType, typeof(HealthCheckAttribute)));
            Name = meta.Name;
            Description = meta.Description;
            Group = meta.Group;
            Id = meta.Id;
            _textService = healthCheckContext.ApplicationContext.Services.TextService;
        }

        [IgnoreDataMember]
        public HealthCheckContext HealthCheckContext { get; private set; }

        [DataMember(Name = "id")]
        public Guid Id { get; private set; }

        [DataMember(Name = "name")]
        public string Name { get; private set; }

        [DataMember(Name = "description")]
        public string Description { get; private set; }

        [DataMember(Name = "group")]
        public string Group { get; private set; }

        /// <summary>
        /// Get the status for this health check
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<HealthCheckStatus> GetStatus();

        /// <summary>
        /// Executes the action and returns it's status
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public abstract HealthCheckStatus ExecuteAction(HealthCheckAction action);

        public Dictionary<string, string> GetSettings( ILocalizedTextService textService)
        {
            var configFilePath = IOHelper.MapPath("~/config/HealthChecks.config");
            string xPath = "/HealthChecks/checkSettings/check[@id='" + Id + "']/add";
            var settings = new Dictionary<string, string>();

            if (File.Exists(configFilePath))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(configFilePath);

                var xmlNodes = xmlDocument.SelectNodes(xPath);
                if (xmlNodes != null && xmlNodes.Count > 0)
                {
                    foreach (XmlNode node in xmlNodes)
                    {
                        settings[node.Attributes["key"].Value] = node.Attributes["value"].Value;
                    }
                }
            }

            return settings;

        }
    }
}
