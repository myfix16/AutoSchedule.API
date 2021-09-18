using System.Collections.Generic;
using System.Linq;
using AutoSchedule.Core.Helpers;
using AutoSchedule.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AutoSchedule.API
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        internal static IEnumerable<Session> Sessions;

        internal static IEnumerable<IGrouping<string, Session>> GroupedSessions;

        internal static IEnumerable<string> ClassNames;

        static readonly IDataProvider<IEnumerable<Session>> DataProvider = new AzureCosmosDBDataProvider();

        public static void Main(string[] args)
        {
            Sessions = DataProvider.GetSessionsAsync().Result;
            GroupedSessions = Sessions.GroupBy(s => s.GetClassifiedName()).ToList();
            ClassNames = GroupedSessions.Select(g => g.Key).ToList();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
