using CarRental.Controllers;
using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CarRental.Tests.Countries;

public class IsoCountriesControllerTests
{
    private readonly Mock<ICountryRepository> repository = new();
    private readonly Country country = new() { IdCountry = 4, Name = "Francia" };

    [Fact]
    public void GetCountries_ReturnsList()
    {
        var countries = new[] { country };
        repository.Setup(r => r.GetCountries()).Returns(countries);

        var result = new IsoCountriesController(repository.Object).GetCountries();

        Assert.Same(countries, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public void GetCountryById_ReturnsCountry()
    {
        repository.Setup(r => r.GetCountryById(4)).Returns(country);

        var result = new IsoCountriesController(repository.Object).GetCountryById(4);

        Assert.Same(country, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public void CreateCountry_ReturnsCreated()
    {
        repository.Setup(r => r.InsertCountry(country)).Returns(true);

        var result = new IsoCountriesController(repository.Object).CreateCountry(country);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, created.StatusCode);
        Assert.Equal(4, Assert.IsType<int>(created.RouteValues!["id"]));
    }

    [Fact]
    public void UpdateCountry_ReturnsOk()
    {
        repository.Setup(r => r.UpdateCountry(country)).Returns(true);

        var result = new IsoCountriesController(repository.Object).UpdateCountry(country);

        Assert.IsType<OkResult>(result);
        repository.Verify(r => r.UpdateCountry(country), Times.Once);
    }
}