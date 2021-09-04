using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoSchedule.Core.Helpers;
using AutoSchedule.Core.Models;

namespace AutoSchedule.UI.Services
{
    public class AppDataServiceSingleton
    {
        /// <summary>
        /// All available sessions such as ACT2111 L01.
        /// </summary>
        public readonly ObservableCollection<Session> AvailableSessionsFlat = new();

        /// <summary>
        /// All available sessions, grouped by class name.
        /// </summary>
        public IEnumerable<IGrouping<string, Session>> AvailableSessions;

        public IEnumerable<string> AvailableClasses;

        readonly IDataProvider<IEnumerable<Session>> DataProvider;

        public const string Version = "1.1.0";

        public const string Term = "2021-2022 Term 1";

        public bool Initialized = false;

        public AppDataServiceSingleton(IDataProvider<IEnumerable<Session>> dataProvider)
        {
            DataProvider = dataProvider;
        }

        /// <summary>
        /// Retrieve session data from source.
        /// </summary>
        public async Task InitializeAsync()
        {
            if (!Initialized)
            {
                foreach (var item in await DataProvider.GetSessionsAsync())
                    AvailableSessionsFlat.Add(item);
                AvailableSessions = AvailableSessionsFlat.GroupBy(s => s.GetClassifiedName());
                AvailableClasses = AvailableSessions.Select(g => g.Key);
                Initialized = true;

#if DEBUG
                Console.WriteLine("DataService singleton initialized.");
#endif
            }
        }
    }
}
