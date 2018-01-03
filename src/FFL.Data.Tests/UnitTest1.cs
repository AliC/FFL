using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FFL.Data.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1Async()
        {


            PlayerRepository repo = new PlayerRepository();
            IList<PlayerProperties> playerStats = await repo.GetStatsAsync();

            Assert.NotNull(playerStats);
        }
    }
}
