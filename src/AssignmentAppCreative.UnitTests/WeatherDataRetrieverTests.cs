using AssignmentAppCreative.WebAPI.DTOs;
using AssignmentAppCreative.WebAPI.Interfaces;
using AssignmentAppCreative.WebAPI.Services;
using Moq;
using RestSharp;
using Shouldly;

namespace AssignmentAppCreative.UnitTests;

public class WeatherDataRetrieverTests
{
    [Fact]
    public async Task WeatherDataRetriever_returns_value_from_cache_if_it_is_present_in_cache()
    {
        // Arrange
        const string randomText = nameof(randomText);
        var cacheManager = new Mock<ICacheManager>();
        cacheManager.Setup(c => c.GetValueForAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(randomText)!);

        var sut = new WeatherDataRetriever(new Mock<IWeatherService>().Object, cacheManager.Object);

        // Act
        (await sut.GetWeatherForFirstCity("-"))
            // Assert
            .ShouldBe(randomText);
    }

    [Fact]
    public async Task WeatherDataRetriever_returns_value_from_WeatherApi_if_it_is_NOT_present_in_cache()
    {
        // Arrange
        const string randomText = nameof(randomText);
        // var cacheManager = new Mock<ICacheManager>();
        // cacheManager.Setup(c => c.GetValueForAsync(It.IsAny<string>()))
        //     .Returns(Task.FromResult(string.Empty)!);

        var weatherService = new Mock<IWeatherService>();
        var x = weatherService.Setup(c 
                => c.GetWeatherData<IEnumerable<LocationData>>(It.IsAny<RestRequest>()))
            .Returns(Task.FromResult(new[] { new LocationData() }
                .AsEnumerable())!);
        var weatherInfoMock = new Mock<WeatherInfo>();
        weatherInfoMock.Setup(w => w.ToString()).Returns(randomText);

        weatherService.Setup(c => c.GetWeatherData<WeatherInfo>(It.IsAny<RestRequest>()))
            .Returns(Task.FromResult(weatherInfoMock.Object)!);

        var sut = new WeatherDataRetriever(weatherService.Object, Mock.Of<ICacheManager>());

        // Act
        (await sut.GetWeatherForFirstCity("-")) 
            // Assert
            .ShouldBe(randomText);
    }
}