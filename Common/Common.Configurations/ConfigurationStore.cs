using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configurations
{
    public abstract class ConfigurationStore : IConfigurationStore
    {
        public static readonly string GlobalSectionName = CommonConfigurations.Group;

        public IEnumerable<Models.Configuration> GetConfigurations()
        {
            var iniData = GetIniData();
            return GetConfigurationsCore(iniData).ToArray();
        }

        protected abstract IniData GetIniData();
        protected virtual IEnumerable<Models.Configuration> GetConfigurationsCore(IniData mergedIniData)
        {
            foreach (var globalData in mergedIniData.Global)
            {
                yield return new Models.Configuration
                {
                    Group = GlobalSectionName,
                    Key = globalData.KeyName,
                    Value = globalData.Value
                };
            }
            foreach (var section in mergedIniData.Sections)
            {
                foreach (var keyValue in section.Keys)
                {
                    yield return new Models.Configuration
                    {
                        Group = section.SectionName,
                        Key = keyValue.KeyName,
                        Value = keyValue.Value
                    };
                }
            }
        }

        public void Dispose()
        {
            Dispose(isDiposing: true);
            GC.SuppressFinalize(this);
        }
        ~ConfigurationStore()
        {
            Dispose(isDiposing: false);
        }
        protected virtual void Dispose(bool isDiposing)
        {
        }
    }
}
