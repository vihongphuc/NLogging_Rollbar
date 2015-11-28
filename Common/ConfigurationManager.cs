using Common;
using Common.Configurations;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ConfigurationManager
    {
        private static readonly Dictionary<string, Dictionary<string, Configuration>> allConfigs = new Dictionary<string, Dictionary<string, Configuration>>(StringComparer.OrdinalIgnoreCase);
        public static void ApplyConfiguration(IConfigurationStore configurationStore)
        {
            lock (allConfigs)
            {
                foreach (var configGroup in configurationStore.GetConfigurations().GroupBy(d => d.Group))
                {
                    Dictionary<string, Configuration> group;
                    if (!allConfigs.TryGetValue(configGroup.Key, out group))
                    {
                        group = new Dictionary<string, Configuration>(StringComparer.OrdinalIgnoreCase);
                        allConfigs.Add(configGroup.Key, group);
                    }

                    foreach (var config in configGroup)
                    {
                        group[config.Key] = config;
                    }
                }
            }
        }

        public static string Require(string group, string key)
        {
            Dictionary<string, Configuration> groupConfig;
            if (allConfigs.TryGetValue(group, out groupConfig))
            {
                Configuration configResult;
                if (groupConfig.TryGetValue(key, out configResult))
                {
                    return configResult.Value;
                }

                throw new ArgumentException("The key cannot be found", "key");
            }

            throw new ArgumentException("The group cannot be found", "group");
        }

        public static string Get(string group, string key)
        {
            Dictionary<string, Configuration> groupConfig;
            if (allConfigs.TryGetValue(group, out groupConfig))
            {
                Configuration configResult;
                if (groupConfig.TryGetValue(key, out configResult))
                {
                    return configResult.Value;
                }
            }

            return null;
        }
        public static T Get<T>(string group, string key)
        {
            return (T)Convert.ChangeType(Get(group, key), typeof(T));
        }
        public static T Get<T>(string group, string key, T defaultValue)
        {
            return Get<T>(group, key, () => defaultValue);
        }
        public static T Get<T>(string group, string key, Func<T> fnDefaultValue)
        {
            var val = Get(group, key);
            if (String.IsNullOrWhiteSpace(val))
                return fnDefaultValue();
            return (T)Convert.ChangeType(val, typeof(T));
        }

        public static bool TryGet(string group, string key, out string value)
        {
            Dictionary<string, Configuration> groupConfig;
            if (allConfigs.TryGetValue(group, out groupConfig))
            {
                Configuration configResult;
                if (groupConfig.TryGetValue(key, out configResult))
                {
                    value = configResult.Value;
                    return true;
                }
            }

            value = null;
            return false;
        }
        public static bool TryGet<T>(string group, string key, out T value)
        {
            string sValue;
            if (TryGet(group, key, out sValue))
            {
                value = (T)Convert.ChangeType(sValue, typeof(T));
                return true;
            }

            value = default(T);
            return false;
        }

        public static bool IsFalse(string group, string key, bool defaultValue = false)
        {
            var val = Get(group, key);
            if (!String.IsNullOrWhiteSpace(val))
            {
                return String.Equals(val, "0") ||
                       String.Equals(val, "false", StringComparison.OrdinalIgnoreCase);
            }

            return !defaultValue;
        }
        public static bool IsTrue(string group, string key, bool defaultValue = false)
        {
            var val = Get(group, key);
            if (!String.IsNullOrWhiteSpace(val))
            {
                return String.Equals(val, "1") ||
                       String.Equals(val, "true", StringComparison.OrdinalIgnoreCase);
            }

            return defaultValue;
        }
    }
}
