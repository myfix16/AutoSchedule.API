using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoSchedule.Core.Models;

namespace AutoSchedule.Core.Helpers
{
    public class WebAPIDataProvider<T> : IDataProviderAsync<T>
    {
        private readonly string _url;

        public WebAPIDataProvider(string requestUrl)
        {
            _url = requestUrl;
        }

        public async Task<T> GetDataAsync()
        {
            using var client = new HttpClient();
            return await client.GetFromJsonAsync<T>(_url);
        }
    }
}
