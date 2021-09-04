using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoSchedule.Core.Helpers;
using AutoSchedule.Core.Models;
using AutoSchedule.UI.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Blazor;
using BlazorFluentUI;

namespace AutoSchedule.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDk2MTQ1QDMxMzkyZTMyMmUzMFpRRnBWWEpOamd5TldMNVlQbnE2MExlMWhvRFYyNG8yRVo2RVZabVpVb3c9;NDk2MTQ2QDMxMzkyZTMyMmUzMEpTMTNXTWtFWmpOeVJRbjV4a3BMWkxlTW9YSUxuSy9hcEhBeUc5VmpUcnM9;NDk2MTQ3QDMxMzkyZTMyMmUzMFBRdDRDTmNKalYxdGQzdk1XOENSQ0dLeXhtR1lLQUVkZEJvN3JnQTZMbU09;NDk2MTQ4QDMxMzkyZTMyMmUzMElOM1FHV1JlMGMxaTE0VS9CR1JNaGZuQ2ptUWxFUmlHcE9nZG1uM2J5Uzg9;NDk2MTQ5QDMxMzkyZTMyMmUzMFIzVnVjVXdySStTMmxhT25Gbi9TWklGcWpVQmxWRktiSHhHNUI1UUIxeWc9;NDk2MTUwQDMxMzkyZTMyMmUzMFlvVzFQQnVueENGYTloWGNhRDJvM010OEFEZHRtSEVQVDhuaStvQnl6RGM9;NDk2MTUxQDMxMzkyZTMyMmUzMG9BZ3owSWdLODdyTC90bWJlRUFjV2p4b2NVVTN3bkpOdjd0WjZySEg4bkU9;NDk2MTUyQDMxMzkyZTMyMmUzMFN1Q0FNV3JXQk9IOWZqOGRaSThiUmcyWEhkQUk3cnJBSFVTc2V0MTZPbjg9;NDk2MTUzQDMxMzkyZTMyMmUzMFZlQXpkRzk1RnZjQ2dLM3pyd2JkRlFEcnNHeWVvcXZwRm1XdDdwSWszZjQ9;NDk2MTU0QDMxMzkyZTMyMmUzMGZWcUc3M250bDdBcWFoa1dsNXA5TGg1MS8wUFdlMFBWVGZBOTN0TC9PRDQ9");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddBlazorFluentUI();
            builder.Services.AddSingleton<IDataProvider<IEnumerable<Session>>, WebAPIDataProvider>();
            builder.Services.AddSingleton<AppDataServiceSingleton>();
            builder.Services.AddScoped<AppDataService>();

            await builder.Build().RunAsync();
        }
    }
}
