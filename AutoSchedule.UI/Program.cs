using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoSchedule.UI.Services;
using BlazorFluentUI;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Blazor;

namespace AutoSchedule.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHJqVVhjWlpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF9jTnxXdkZhXHpad3dRTg==;Mgo+DSMBPh8sVXJ0S0R+XE9HcFRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3xTcUdnWXxcdXFRQWVcWQ==;ORg4AjUWIQA/Gnt2VVhiQlFadVlJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdkJhXX9dcnFQQmdaU0A=;NjgyOTEzQDMyMzAyZTMyMmUzMG9TWDYzVzdVOWNQTWNrUElERm1vZUVMdSs2bmhHNnp3WXR6VHREdnRpRUE9;NjgyOTE0QDMyMzAyZTMyMmUzME56WU5FbE1wcmcyYm8wVDkwbW9HbVRnd3FGSSs5MENqdEUvdnJDSFNPakE9;NRAiBiAaIQQuGjN/V0Z+Xk9EaFxEVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdEVkW3tec3ZQQ2RUUkBy;NjgyOTE2QDMyMzAyZTMyMmUzMGExU3FmVXJjUm5VSXFPYndBUU91b3h2SzB6YmV4WkgzcjVhVkJiNmNiS0U9;NjgyOTE3QDMyMzAyZTMyMmUzMEMvQnVGb1BsNnBUbFhKNUtBeWpoUGl2dFFMWk56M1R6S0ZWTnBsT1g2N3c9;Mgo+DSMBMAY9C3t2VVhiQlFadVlJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdkJhXX9dcnFQQmlcUEE=;NjgyOTE5QDMyMzAyZTMyMmUzMEZiNW9VYkhyOVVZVWFBUUNOTkY2VlpPL1ZaK21UYkozT1duWTN5bDRTT0U9;NjgyOTIwQDMyMzAyZTMyMmUzMEhpVTdKdXVVeEN3NmYrVHlzN1JneGZLMVJDWEJRYWVIaGdyZDhYOStGZ2M9;NjgyOTIxQDMyMzAyZTMyMmUzMGExU3FmVXJjUm5VSXFPYndBUU91b3h2SzB6YmV4WkgzcjVhVkJiNmNiS0U9");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddBlazorFluentUI();
            builder.Services.AddSingleton<AppDataServiceSingleton>();
            builder.Services.AddScoped<AppDataService>();

            await builder.Build().RunAsync();
        }
    }
}
