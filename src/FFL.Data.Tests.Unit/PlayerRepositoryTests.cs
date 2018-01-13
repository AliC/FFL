using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FFL.Data.Tests.Unit
{
    public class PlayerRepositoryTests
    {
        private HttpClient _client;
        private Mock<HttpClientHandler> _handler = new Mock<HttpClientHandler>();
        private PlayerRepository _repository;
        private IList<Player> _players;

        [Fact]
        public async Task PlayersRequestIsMadeWithCorrectVerb()
        {
            HttpMethod expectedMethod = HttpMethod.Get;
            
            Arrange();
            await Act();

            _handler.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(m => m.Method == expectedMethod), ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task PlayersRequestIsMadeToCorrectUrl()
        {
            // uses HttpClient testing approach from https://github.com/dotnet/corefx/issues/1624#issuecomment-100755941
            Uri expectedUri = new Uri("https://fantasy.premierleague.com/drf/elements/");

            Arrange();
            await Act();

            _handler.Protected().Verify("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(m => m.RequestUri == expectedUri), ItExpr.IsAny<CancellationToken>());
        }

        private void Arrange()
        {
            _handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            _client = new HttpClient(_handler.Object);
        }

        private async Task Act()
        {
            _repository = new PlayerRepository(_client);
            _players = await _repository.Get();
        }
    }
}
