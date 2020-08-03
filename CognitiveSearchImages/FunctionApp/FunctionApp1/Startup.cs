using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(FunctionApp1.Startup))]

namespace FunctionApp1
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton((s) => {
                return new CustomVisionService();
            });

            builder.Services.AddSingleton((s) => {
                return new BlobStorageService();
            });
        }
    }
}
