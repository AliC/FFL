using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
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
        private IList<Player> _players;

        [Fact]
        public async Task PlayersRequestIsMadeToCorrectUrl()
        {
            // uses HttpClient testing approach from https://github.com/dotnet/corefx/issues/1624#issuecomment-100755941
            Uri requestUri = new Uri("https://fantasy.premierleague.com/drf/elements/");
            string response = "[{\"first_name\":\"Kevin De Bruyne\"}]";

            Mock<HttpClientHandler> handler = new Mock<HttpClientHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(response) });

            _client = new HttpClient(handler.Object);
            await Act();

            handler.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(m => m.RequestUri == requestUri), ItExpr.IsAny<CancellationToken>());
        }

        // int test
        [Fact]
        public async Task WhenPlayersAreRequested_PlayersAreRetrievedAndMapped()
        {
            _handler = CreateWebHostHandler200();
            Arrange();
            await Act();

            Assert.NotNull(_players);
            Assert.NotEmpty(_players);
            Assert.Equal("Kevin De Bruyne", _players[0].first_name);
        }

        // int test
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
