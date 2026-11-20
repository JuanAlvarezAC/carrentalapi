using CarRental.Controllers;
using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CarRental.Tests.Customers;

public class CustomersControllerTests
{
    private readonly Mock<ICustomerRepository> repository = new();
    private readonly Customer customer = new() { IdCustomer = 1, FirstName = "Ana" };

    [Fact]
    public void GetCustomers_ReturnsList()
    {
        var customers = new[] { customer };
        repository.Setup(r => r.GetCustomers()).Returns(customers);

        var result = new CustomersController(repository.Object).GetCustomers();

        Assert.Same(customers, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public void GetCustomerById_ReturnsCustomer()
    {
        repository.Setup(r => r.GetCustomerById(1)).Returns(customer);

        var result = new CustomersController(repository.Object).GetCustomerById(1);

        Assert.Same(customer, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public void CreateCustomer_ReturnsCreated()
    {
        var newCustomer = new Customer { FirstName = "Ana" };
        repository.Setup(r => r.InsertCustomer(newCustomer))
            .Callback<Customer>(c => c.IdCustomer = 1)
            .Returns(true);

        var result = new CustomersController(repository.Object).CreateCustomer(newCustomer);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, created.StatusCode);
        Assert.Equal(1, Assert.IsType<int>(created.RouteValues!["id"]));
    }

    [Fact]
    public void UpdateCustomer_ReturnsOk()
    {
        repository.Setup(r => r.UpdateCustomer(customer)).Returns(true);

        var result = new CustomersController(repository.Object).UpdateCustomer(customer);

        Assert.IsType<OkResult>(result);
        repository.Verify(r => r.UpdateCustomer(customer), Times.Once);
    }
}