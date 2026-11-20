
using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers;

[ApiController]
[Route("[controller]")]
public class IsoCountriesController(ICountryRepository repository) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCountries()
    {
        return Ok(repository.GetCountries());
    }

    [HttpGet("{id:int}")]
    public IActionResult GetCountryById(int id)
    {
        var country = repository.GetCountryById(id);

        if (country is null)
        {
            return NotFound();
        }

        return Ok(country);
    }

    [HttpPost]
    public IActionResult CreateCountry([FromBody] Country country)
    {
        if (!repository.InsertCountry(country))
        {
            return Problem("No se pudo confirmar la inserción del país.");
        }

        return CreatedAtAction(
            nameof(GetCountryById),
            new { id = country.IdCountry },
            country);
    }

    [HttpPut]
    public IActionResult UpdateCountry([FromBody] Country country)
    {
        if (!repository.UpdateCountry(country))
        {
            return NotFound();
        }

        return Ok();
    }
}