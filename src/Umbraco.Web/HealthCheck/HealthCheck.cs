using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.HealthChecks;

namespace Umbraco.Web.HealthCheck
{
    /// <summary>
    /// The abstract health check class
    /// </summary>
    [DataContract(Name = "healtCheck", Namespace = "")]
    public abstract class HealthCheck : IDiscoverable
    {
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

        public Dictionary<string, string> GetSettings()
        {
            var settings = UmbracoConfig.For.HealthCheck().CustomCheckSettings.Settings;
            var checkSetting = new Dictionary<string, string>();

            foreach (CustomCheckSetting settingObj in settings)
            {
                // Only return the settings for the current Check
                if (settingObj.Id == this.Id)
                {
                    // We just return the setting Object as a dictionary here, this is all the <add key="" value="" /> items in the config for this Check
                    checkSetting = settingObj.GetSettingsDictionary();
                    break;
                }
            }

            return checkSetting;
        }
    }
}
