using FFL.UI.Web.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFL.Data;
using Moq;
using Xunit;

namespace FFL.UI.Web.Mvc.Tests
{
    public class PlayerControllerTests
    {
        [Fact]
        public async Task VieWModelContainsPlayers()
        {
            var player1 = new Data.Player { first_name = "Kevin", second_name = "De Bruyne" };
            var player2 = new Data.Player { first_name = "Dele", second_name = "Alli" };
            var expectedPlayer1Name = player1.first_name + " " + player1.second_name;
            var expectedPlayer2Name = player2.first_name + " " + player2.second_name;
            var players = new[] { player1, player2 };

            var repository = new Mock<IPlayerRepository>();
            repository.Setup(r => r.Get()).ReturnsAsync(players);

            var playerController = new PlayerController(repository.Object);
            var result = await playerController.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Models.Player>>(result.Model);

            var actualPlayers = (IEnumerable<Models.Player>)result.Model;
            Assert.Collection(actualPlayers,
                player => { Assert.Equal(expectedPlayer1Name, player.Name); },
                player => { Assert.Equal(expectedPlayer2Name, player.Name); });

        }
    }
}
