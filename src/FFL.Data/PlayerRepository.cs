using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FFL.Data
{
    public class PlayerRepository
    {
        private HttpClient _httpClient;

        public PlayerRepository(HttpClient client) => _httpClient = client;

        public async Task<IList<PlayerProperties>> GetStatsAsync()
        {
            IList<PlayerProperties> playerProperties = new List<PlayerProperties>();

            string uri = "https://fantasy.premierleague.com/drf/elements/";
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    playerProperties = JsonConvert.DeserializeObject<IList<PlayerProperties>>(responseContent);
                }
            }

            return playerProperties;
        }
    }
}