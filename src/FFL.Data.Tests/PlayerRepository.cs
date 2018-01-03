using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FFL.Data.Tests
{
    public class PlayerRepository
    {
        public async Task<IList<PlayerProperties>> GetStatsAsync()
        {
            IList<PlayerProperties> playerProperties = new List<PlayerProperties>();

            using (HttpClient httpClient = new HttpClient())
            {
                string uri = "https://fantasy.premierleague.com/drf/elements/";
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        playerProperties = JsonConvert.DeserializeObject<IList<PlayerProperties>>(responseContent);
                    }
                }
            }

            return playerProperties;
        }
    }
}