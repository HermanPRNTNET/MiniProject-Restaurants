using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RestaurantControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact()]
    public async void GetAll_ForInvalidRequest_ReturnInvalid400BadRequest()
    {
        var client = _factory.CreateClient();

        var result = await client.GetAsync("/api/restaurants");

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact()]
    public async void GetAll_ForValidRequest_Returns200OK()
    {
        var client = _factory.CreateClient();

        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}