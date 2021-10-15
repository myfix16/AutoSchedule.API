using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoSchedule.Core.Models;

namespace AutoSchedule.Core.Helpers
{
    public class WebAPIDataProvider : IDataProviderAsync<IEnumerable<Session>>
    {
        public async Task<IEnumerable<Session>> GetDataAsync()
        {
            using var client = new HttpClient();
            return await client.GetFromJsonAsync<IEnumerable<Session>>("https://api-autoschedule.azurewebsites.net/api/sessions");
        }
    }
}
