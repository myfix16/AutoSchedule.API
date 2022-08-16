using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AutoSchedule.Core.Models;

namespace AutoSchedule.Core.Helpers
{
    public class WebApiManager
    {
        readonly string _baseUrl;
        readonly HttpClient _client;
        readonly JsonSerializerOptions _serializerOptions;

        public WebApiManager(string baseUrl)
        {
            this._baseUrl = baseUrl;
            this._client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }

        ~WebApiManager()
        {
            _client.Dispose();
        }

        public async Task<IEnumerable<Session>> GetSessions(string term)
            => await _client.GetFromJsonAsync<IEnumerable<Session>>($"{_baseUrl}/sessions/?term={term}");

        public async Task<IEnumerable<Session>> GetClassNames(string term)
            => await _client.GetFromJsonAsync<IEnumerable<Session>>($"{_baseUrl}/ClassNames/?term={term}");

        public async Task<IEnumerable<string>> GetTerms()
            => await _client.GetFromJsonAsync<IEnumerable<string>>($"{_baseUrl}/terms");

        public async Task<IEnumerable<Schedule>> GenerateSchedules(string term, IEnumerable<PriorityClass> selectedClasses, int maxSchedules = -1)
        {
            var result = await _client.PostAsJsonAsync(
                $"{_baseUrl}/ClassSelection/?term={term}&maxSchedules={maxSchedules}",
                selectedClasses);
            return JsonSerializer.Deserialize<List<Schedule>>(result.Content.ReadAsStream(), _serializerOptions);
        }
    }
}
