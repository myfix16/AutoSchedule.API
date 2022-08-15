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
        private readonly Dictionary<string, ObservableCollection<Session>> _availableSessionsFlatContainer = new();

        IDataProviderAsync<IEnumerable<Session>> _sessionDataProvider;
        IDataProviderAsync<IEnumerable<string>> _termDataProvider;

        public const string Version = "1.1.0";

        public IEnumerable<string> Terms;

        bool _initialized = false;
        Task _initializeTask;

        /// <summary>
        /// Retrieve session data from source.
        /// </summary>
        public async Task InitializeAsync()
        {
            if (!_initialized)
            {
                _initializeTask ??= InitializeAsyncActual();
                await _initializeTask;
            }
        }

        private async Task InitializeAsyncActual()
        {
#if DEBUG
            _sessionDataProvider = new WebAPIDataProvider<IEnumerable<Session>>("https://localhost:44320/api/sessions");
            _termDataProvider = new WebAPIDataProvider<IEnumerable<string>>("https://localhost:44320/api/terms");
#else
            SessionDataProvider = new WebAPIDataProvider<IEnumerable<Session>>("https://api-autoschedule.azurewebsites.net/api/sessions");
            TermDataProvider = new WebAPIDataProvider<IEnumerable<string>>("https://api-autoschedule.azurewebsites.net/api/terms");
#endif

            Terms = await _termDataProvider.GetDataAsync();

            _initialized = true;
#if DEBUG
            Console.WriteLine("DataService singleton initialized.");
#endif
        }

        public async Task<ObservableCollection<Session>> GetSessionData(string term)
        {
            if (!_availableSessionsFlatContainer.ContainsKey(term))
            {
                await LoadSessionData(term);
            }
            return _availableSessionsFlatContainer[term];
        }

        private async Task LoadSessionData(string term)
        {
            ObservableCollection<Session> sessionsFlat = new();
            foreach (var item in await _sessionDataProvider.GetDataAsync())
            {
                sessionsFlat.Add(item);
            }
            _availableSessionsFlatContainer[term] = sessionsFlat;
#if DEBUG
            Console.WriteLine($"Term {term} loaded for the first time.");
#endif
        }
    }
}
