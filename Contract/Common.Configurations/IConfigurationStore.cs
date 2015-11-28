using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configurations
{
    public interface IConfigurationStore : IDisposable
    {
        IEnumerable<Configuration> GetConfigurations();
    }
}
