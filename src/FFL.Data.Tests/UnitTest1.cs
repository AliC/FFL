using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FFL.Data.Tests
{
    public class PlayerRepositoryTests
    {
        private HttpClient _client;
        private PlayerRepository _repository;
        private IWebHostBuilder _builder;
        private RequestDelegate _handler;

        // unit test?
        [Fact]
        public async Task PlayerStatisticsRequestIsMadeToCorrectUrl()
        {
            _handler = CreateWebHostHandler200();
            ArrangeAndAct();

            string expectedUri = "https://fantasy.premierleague.com/drf/elements/";

            await _repository.GetStatsAsync();

            Assert.Equal(expectedUri, _client.BaseAddress.ToString());
        }

        // int test
        [Fact]
        public async Task WhenPlayerStatisticsAreRequested_PlayerStatisticsAreRetrievedAndMapped()
        {
            _handler = CreateWebHostHandler200();
            ArrangeAndAct();

            IList<PlayerProperties> playerStats = await _repository.GetStatsAsync();

            Assert.NotNull(playerStats);
            Assert.NotEmpty(playerStats);
            Assert.Equal("Kevin De Bruyne", playerStats[0].first_name);
        }

        // int test
        [Fact]
        public async Task WhenPlayerStatisticsAreRequestedButSomethingGoesWrong_AnEmptyCollectionIsReturned()
        {
            _handler = CreateWebHostHandler404();
            ArrangeAndAct();

            IList<PlayerProperties> playerStats = await _repository.GetStatsAsync();

            Assert.NotNull(playerStats);
            Assert.Empty(playerStats);
        }

        private void ArrangeAndAct()
        {
            _builder = CreateWebHostBuilder();
            TestServer server = new TestServer(_builder);
            _client = server.CreateClient();

            Act();
        }

        private void Act()
        {
            _repository = new PlayerRepository(_client);
        }

        private RequestDelegate CreateWebHostHandler200()
        {
            return async context => await context.Response.WriteAsync("[{\"first_name\":\"Kevin De Bruyne\"}]");
        }

        private RequestDelegate CreateWebHostHandler404()
        {
            return context => Task.FromResult(context.Response.StatusCode = (int)HttpStatusCode.NotFound);
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
                    app.Run(_handler);
                });
        }
    }
}
