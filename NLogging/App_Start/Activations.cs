using LightInject;
using LightInject.Web;
using LightInject.Mvc;
using Common;
using Common.Configurations;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Net.Http;
//using Web.Core.Stores;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Web.Activations), "PreStart")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Web.Activations), "PostStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Web.Activations), "Shutdown")]

namespace Web
{
    internal static class Activations
    {
        private const string DefaultConfigPath = "~/configs.ini";
        private const int DefaultIDGeneratorScope = 2;

        public static void PreStart()
        {
            InitConfigurations();

            var scope = ConfigurationManager.Get<int>(CommonConfigurations.Group, CommonConfigurations.IDGeneratorScope, DefaultIDGeneratorScope);
            IDGenerator.SetProvider(new UniqueNumberProvider(scope));

            ServiceRegistrations.PerformRegistrations(ServiceRegistrations.SystemOrder, container =>
            {
                container.EnableAnnotatedConstructorInjection();
                container.EnableAnnotatedPropertyInjection();
                container.EnablePerWebRequestScope();

                // register all controllers
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (asm.FullName.StartsWith("NLogging"))
                    {
                        container.RegisterControllers(asm);
                    }
                }

                // other services
                //container.Register<HttpClient>(sf => CreateHttpClient(), new PerScopeLifetime());
                //container.Register<IPeopleStore, HttpPeopleStore>(new PerScopeLifetime());
                //container.Register<IReportStore, HttpReportStore>(new PerScopeLifetime());
                //container.Register<IAuthenticationStore, HttpAuthenticationStore>(new PerScopeLifetime());
            });

        }
        private static void InitConfigurations()
        {
            var configPath = System.Web.Hosting.HostingEnvironment.MapPath(DefaultConfigPath);
            if (File.Exists(configPath))
            {
                ConfigurationManager.ApplyConfiguration(new FileConfigurationStore(configPath));
            }
        }
       

        public static void PostStart()
        {
            ServiceRegistrations.PerformRegistrations(ServiceRegistrations.LastOrder, container =>
            {
                container.EnableMvc();
            });

            ServiceRegistrations.Apply();
        }
        public static void Shutdown()
        {
            ServiceRegistrations.Dispose();
        }

    }
}