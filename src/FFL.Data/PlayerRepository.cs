using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FFL.Data
{
    public class PlayerRepository : IPlayerRepository
    {
        private HttpClient _httpClient;

        public PlayerRepository(HttpClient client) => _httpClient = client;

        public async Task<IEnumerable<Player>> Get()
        {
            var uri = "https://fantasy.premierleague.com/drf/elements/";
            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<Player>>(responseContent);
                }
            }

            return new List<Player>();
        }
    }
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> Get();
    }
}