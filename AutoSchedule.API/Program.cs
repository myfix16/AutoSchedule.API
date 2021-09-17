using System.Collections.Generic;
using System.Linq;
using AutoSchedule.Core.Helpers;
using AutoSchedule.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AutoSchedule.API
{
    public class Program
    {
        internal static IEnumerable<Session> Sessions;

        internal static IEnumerable<IGrouping<string, Session>> GroupedSessions;

        internal static readonly IDataProvider<IEnumerable<Session>> DataProvider = new AzureCosmosDBDataProvider();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration((context, config) =>
                //{
                //    var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
                //    config.AddAzureKeyVault(
                //    keyVaultEndpoint,
                //    new DefaultAzureCredential());
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
