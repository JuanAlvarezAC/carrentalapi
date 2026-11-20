using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController(ICustomerRepository repository) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCustomers()
    {
        return Ok(repository.GetCustomers());
    }

    [HttpGet("{id:int}")]
    public IActionResult GetCustomerById(int id)
    {
        var customer = repository.GetCustomerById(id);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPost]
    public IActionResult CreateCustomer([FromBody] Customer customer)
    {
        if (!repository.InsertCustomer(customer))
        {
            return Problem("No se pudo confirmar la inserción del cliente.");
        }

        return CreatedAtAction(
            nameof(GetCustomerById),
            new { id = customer.IdCustomer },
            customer);
    }

    [HttpPut]
    public IActionResult UpdateCustomer([FromBody] Customer customer)
    {
        if (!repository.UpdateCustomer(customer))
        {
            return NotFound();
        }

        return Ok();
    }
}