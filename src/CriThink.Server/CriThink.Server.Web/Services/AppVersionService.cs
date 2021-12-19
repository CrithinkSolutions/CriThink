using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Hosting;

namespace CriThink.Server.Web.Services
{
    public class AppVersionService : IAppVersionService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly Assembly _assembly;

        public AppVersionService(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _assembly = Assembly.GetEntryAssembly();
        }

        public string Version
        {
            get
            {
                var versionAttribute = _assembly.GetCustomAttribute<AssemblyFileVersionAttribute>();
                return versionAttribute?.Version;
            }
        }

        public string Copyright
        {
            get
            {
                var copyrightAttribute = _assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
                return copyrightAttribute?.Copyright;
            }
        }

        public string CurrentEnvironment => _environment.EnvironmentName;

        public string Runtime => RuntimeInformation.FrameworkDescription;
    }
}
