﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoSchedule.Core.Helpers;
using AutoSchedule.Core.Models;

namespace AutoSchedule.UI.Services
{
    public class AppDataService
    {
        /// <summary>
        /// All available sessions such as ACT2111 L01.
        /// </summary>
        public ObservableCollection<Session> AvailableSessionsFlat = new();

        /// <summary>
        /// All available sessions, grouped by class name.
        /// </summary>
        public IEnumerable<IEnumerable<Session>> AvailableSessions;

        /// <summary>
        /// All available class, container of sessions, such as ACT2111.
        /// </summary>
        public IEnumerable<string> AvailableClasses;

        /// <summary>
        /// Currently selected schedule.
        /// </summary>
        public Schedule CurrentSchedule = new();
        public readonly ObservableCollection<string> FilteredClasses = new();
        public readonly ObservableCollection<string> SelectedClasses = new();
        /// <summary>
        /// All available schedules generated by ClassSelector.
        /// </summary>
        public readonly ObservableCollection<Schedule> AvailableSchedules = new();

        public string SelectedTerm { get; private set; }

        public string SearchBoxText = string.Empty;

        /// <summary>
        /// Indicates whether the service is initialized.
        /// </summary>
        public bool Initialized = false;
        private Task _initializingTask;

        AppDataServiceSingleton _appDataServiceSingleton;

        public async Task InitializeAsync(AppDataServiceSingleton appDataServiceSingleton)
        {
            if (!Initialized)
            {
                // If _initializingTask is not null (i.e. one initializing task is performing), simply
                // await for the result. Else, start am initializing task and await for the result.
                _initializingTask ??= InitializeAsyncActual(appDataServiceSingleton);
                await _initializingTask;
            }
        }

        private async Task InitializeAsyncActual(AppDataServiceSingleton appDataServiceSingleton)
        {
            _appDataServiceSingleton = appDataServiceSingleton;
            await appDataServiceSingleton.InitializeAsync();

            // Initialize terms data
            await SetSelectedTerm(appDataServiceSingleton.Terms.Max());

            Initialized = true;
#if DEBUG
            Console.WriteLine("DataService initialized.");
#endif
        }

        private async Task LoadSessionData(string term)
        {
            Initialized = false;
            FilteredClasses.Clear();
            SelectedClasses.Clear();

            AvailableSessionsFlat = await _appDataServiceSingleton.GetSessionData(term);
            AvailableSessions = AvailableSessionsFlat.GroupBy(s => s.GetClassifiedName());
            AvailableClasses = AvailableSessions
                .Select(l => l.First().GetClassifiedName()).Distinct().OrderBy(s => s);

            foreach (var item in AvailableClasses)
            {
                FilteredClasses.Add(item);
            }

            Initialized = true;
#if DEBUG
            Console.WriteLine($"Term {term} loaded.");
#endif
        }

        internal async Task SetSelectedTerm(string term)
        {
            SelectedTerm = term;
            await LoadSessionData(term);
        }
    }
}
