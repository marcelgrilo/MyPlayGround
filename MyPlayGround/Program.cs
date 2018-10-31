using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MyPlayGround
{

    //https://github.com/AutoMapper/AutoMapper.Extensions.Microsoft.DependencyInjection/blob/master/test/AutoMapper.Extensions.Microsoft.DependencyInjection.Tests/Profiles.cs
    public interface ILoggerFactory { }

    public class LoggerFactory : ILoggerFactory { }

    public interface IMarketplaceFreightService { void IMarketplaceFreightServiceMethod(); }

    public interface IMGLFreightService : IMarketplaceFreightService { void IMGLFreightServiceMethod(); }

    public class MGL : IMGLFreightService
    {
        public MGL(IEnumerable<IERPFreightService> erpServices)
        {

        }

        public void IMarketplaceFreightServiceMethod()
        {
            throw new NotImplementedException();
        }

        public void IMGLFreightServiceMethod()
        {
            throw new NotImplementedException();
        }
    }

    public interface IERPFreightService { void IERPFreightServiceMethod(); }

    public interface ITRAYFreightService : IERPFreightService { void ITrayFreightServiceMethod(); }

    public class TrayService : ITRAYFreightService
    {
        public TrayService(ITrayRestProvider trayRestProvider)
        {

        }

        public void IERPFreightServiceMethod()
        {
            throw new NotImplementedException();
        }

        public void ITrayFreightServiceMethod()
        {
            throw new NotImplementedException();
        }
    }

    public interface IRestProvider { void IRestProviderMethod(); }

    public interface ITrayRestProvider : IERPFreightService { void ITrayRestProviderMethod(); }

    public class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection()               
                .AddSingleton<ILoggerFactory, LoggerFactory>()
                .AddSingleton(typeof(ILogger<>), typeof(Logger<>))
                .AddLogging((builder)
                    => builder
                    .SetMinimumLevel(LogLevel.Trace)
                    
                );


            services.AddScoped<IMGLFreightService, MGL>();
            var serviceProvider =  services.BuildServiceProvider();


        }
    }
}
