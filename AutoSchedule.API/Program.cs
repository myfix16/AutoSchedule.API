using System.Collections.Generic;
using System.Linq;
using AutoSchedule.API.Helpers;
using AutoSchedule.Core.Helpers;
using AutoSchedule.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AutoSchedule.API
{
    // todo: support for multi terms
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        internal static readonly Dictionary<string, IEnumerable<Session>> Sessions = new();

        internal static readonly Dictionary<string, IEnumerable<IGrouping<string, Session>>> GroupedSessions = new();

        internal static readonly Dictionary<string, IEnumerable<string>> ClassNames = new();

        internal static IEnumerable<string> Terms;

        public static void Main(string[] args)
        {
            Terms = DBHelper.GetTables(DBHelper.GetDB()).Result;
            PrepareData(Terms.Max());

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }

        internal static void PrepareData(string term)
        {
            IDataProviderAsync<IEnumerable<Session>> dataProvider = new AzureCosmosDBDataProvider("SessionsData", term);
            var sessions = dataProvider.GetDataAsync().Result;
            var groupedSessions = sessions.GroupBy(s => s.GetClassifiedName()).ToList();
            var classNames = groupedSessions.Select(g => g.Key).ToList();
            Sessions[term] = sessions;
            GroupedSessions[term] = groupedSessions;
            ClassNames[term] = classNames;
        }
    }
}
