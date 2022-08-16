using System.Collections.Generic;
using System.Linq;
using AutoSchedule.API.Helpers;
using AutoSchedule.Core.Helpers;
using AutoSchedule.Core.Models;
using Azure.Cosmos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AutoSchedule.API
{
    // todo: support for multi terms
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        internal static IEnumerable<Session> Sessions;

        internal static IEnumerable<IGrouping<string, Session>> GroupedSessions;

        internal static IEnumerable<string> ClassNames;

        internal static IEnumerable<string> Terms;

        static readonly IDataProviderAsync<IEnumerable<Session>> DataProvider = new AzureCosmosDBDataProvider("SessionsData", "2021-2022-Term2");

        public static void Main(string[] args)
        {
            Terms = DBHelper.GetTables(DBHelper.GetDB()).Result;
            Sessions = DataProvider.GetDataAsync().Result;
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
