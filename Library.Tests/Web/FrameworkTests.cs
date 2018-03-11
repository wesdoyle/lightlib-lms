using System.Net.Http;
using System.Threading.Tasks;
using Library.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace Library.Tests.Web
{

    [TestFixture]
    public class FrameworkTests
    {
        private readonly HttpClient _client;

        public FrameworkTests()
        {
            var server = new TestServer(new WebHostBuilder().UseEnvironment("Development").UseStartup<Startup>());
            _client = server.CreateClient();
        }

        // Test]
        // ublic async Task Default_Web_Request_Should_Return_OK()
        // 
        //    // var response = await _client.GetAsync("/");
        //    // var responseString = await response.Content.ReadAsStringAsync();
        //    // response.EnsureSuccessStatusCode();
        // }
    }
}