using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using DateOnlyTimeOnly.AspNet.Converters;
using LiWiMus.Infrastructure.Data;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace LiWiMus.Web.API.Tests.Setup;

public class IndexPageTests : 
    IClassFixture<WebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _testOutputHelper;

    public IndexPageTests(
        WebApplicationFactory factory, 
        ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Test()
    {
        var message = await _client.PostAsJsonAsync("https://localhost:5011/api/albums", new Albums.Create.Request
        {
            Title = "AlbumTest",
            CoverLocation = "wescd",
            ArtistIds = new List<int> {1},
            PublishedAt = new DateOnly(2000, 10, 10)
        }, new JsonOptions().UseDateOnlyTimeOnlyStringConverters());
        
        Assert.True(message.IsSuccessStatusCode);
    }
}

public static class JsonOptionsExtensions
{
    /// <summary>
    /// Adds <see cref="DateOnly"/> and <see cref="TimeOnly"/> serializers to System.Text.Json.
    /// </summary>
    public static JsonSerializerOptions UseDateOnlyTimeOnlyStringConverters(this JsonOptions options)
    {
        options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        return options.SerializerOptions;
    }
}