using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FFL.Data.Tests.Integration
{
    public class PlayerRepositoryTests
    {
        private HttpClient _client;
        private PlayerRepository _repository;
        private IWebHostBuilder _builder;
        private RequestDelegate _handler;
        private IEnumerable<Player> _players;

        [Fact]
        public async Task WhenPlayersAreRequested_PlayersAreRetrievedAndMapped()
        {
            _handler = CreateWebHostHandler200();
            Arrange();
            await Act();

            Assert.NotNull(_players);
            Assert.NotEmpty(_players);
            Assert.Equal("Kevin De Bruyne", _players.First().first_name);
        }

        [Fact]
        public async Task WhenPlayersAreRequestedButSomethingGoesWrong_AnEmptyPlayerCollectionIsReturned()
        {
            _handler = CreateWebHostHandler404();
            Arrange();
            await Act();

            Assert.NotNull(_players);
            Assert.Empty(_players);
        }

        private void Arrange()
        {
            _builder = CreateWebHostBuilder();
            TestServer server = new TestServer(_builder);
            _client = server.CreateClient();
        }

        private async Task Act()
        {
            _repository = new PlayerRepository(_client);
            _players = await _repository.Get();
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
