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
    public class UnitTest1
    {
        [Fact]
        public async Task Test1Async()
        {
            var builder = foo();
            TestServer server = new TestServer(builder);
            HttpClient client = server.CreateClient();
            PlayerRepository repo = new PlayerRepository(client);
            IList<PlayerProperties> playerStats = await repo.GetStatsAsync();

            Assert.NotNull(playerStats);
            Assert.NotEmpty(playerStats);
            Assert.Equal("Kevin De Bruyne", playerStats[0].first_name);
        }

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .Build();

        private IWebHostBuilder foo()
        {
            string[] args = new string[] { };

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .AddCommandLine(args)
                
                .Build();

            return WebHost.CreateDefaultBuilder()
                //.UseUrls("http://*:5000")
                .UseConfiguration(config)
                .Configure(app =>
                {
                    //app.Map(,)
                    app.Run(context =>
                        context.Response.WriteAsync("[{\"first_name\":\"Kevin De Bruyne\"}]"));
                });
        }
    }
}
