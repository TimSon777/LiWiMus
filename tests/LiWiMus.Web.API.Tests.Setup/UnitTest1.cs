using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace LiWiMus.Web.API.Tests.Setup;

public class IndexPageTests :
    IClassFixture<WebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public IndexPageTests(
        WebApplicationFactory factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Test()
    {
        var message = await _client.PostAsJsonAsync("https://localhost:5011/api/albums", 
            new
            {
                title = "FromTests",
                publishedAt = new DateOnly(2000, 10, 10).ToString("yyyy-MM-dd"),
                coverLocation = "wefsc",
                artistIds = new List<int>() {1}
            });
        
        _testOutputHelper.WriteLine(message.StatusCode.ToString());
    }
}
