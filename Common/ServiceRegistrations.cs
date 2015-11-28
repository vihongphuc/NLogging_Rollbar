using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ServiceRegistrations
    {
        public static readonly int SystemOrder = 0;
        public static readonly int ProgramOrder = 1;
        public static readonly int LastOrder = Int32.MaxValue;

        private static ServiceContainer Container;
        private static List<ServiceRegistration> serviceRegistrations = new List<ServiceRegistration>();

        public static void PerformRegistrations(int order, Action<ServiceContainer> registerAction)
        {
            if (serviceRegistrations == null)
                throw new InvalidOperationException("Cannot register anymore services at this stage");

            serviceRegistrations.Add(new ServiceRegistration
            {
                Order = order,
                Action = registerAction
            });
        }
        public static void Init(Action<ServiceContainer> initAction)
        {
            if (serviceRegistrations == null)
                throw new InvalidOperationException("Cannot start services at this stage");

            if (Container == null)
                Container = new ServiceContainer();

            if (initAction != null)
                initAction(Container);
        }
        public static void Apply()
        {
            if (serviceRegistrations == null)
                throw new InvalidOperationException("Cannot apply services at this stage");

            Init(null);
            foreach (var sr in serviceRegistrations.OrderBy(d => d.Order))
            {
                sr.Action(Container);
            }

            serviceRegistrations = null;
        }
        public static void Dispose()
        {
            if (Container != null)
            {
                Container.Dispose();
                Container = null;
            }
        }

        private struct ServiceRegistration
        {
            public int Order { get; set; }
            public Action<ServiceContainer> Action { get; set; }
        }
    }
}
