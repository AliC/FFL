using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FFL.Data.Tests
{
    public class PlayerRepositoryTests
    {
        private HttpClient _client;

        public PlayerRepositoryTests()
        {
            var builder = CreateWebHostBuilder();
            TestServer server = new TestServer(builder);
            _client = server.CreateClient();
        }

        // unit test?
        [Fact(Skip = "Complete this test")]
        public void CorrectUrl()
        {
            string expectedUri = "https://fantasy.premierleague.com/drf/elements/";


        }

        // int test
        [Fact]
        public async Task WhenPlayerStatisticsAreRequested_PlayerStatisticsAreRetrievedAndMapped()
        {
            PlayerRepository repo = new PlayerRepository(_client);
            IList<PlayerProperties> playerStats = await repo.GetStatsAsync();

            Assert.NotNull(playerStats);
            Assert.NotEmpty(playerStats);
            Assert.Equal("Kevin De Bruyne", playerStats[0].first_name);
        }

        private IWebHostBuilder CreateWebHostBuilder()
        {
            string[] args = { };

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .AddCommandLine(args)
                .Build();

            return WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .Configure(app =>
                {
                    app.Run(context => context.Response.WriteAsync("[{\"first_name\":\"Kevin De Bruyne\"}]"));
                });
        }
    }
}
