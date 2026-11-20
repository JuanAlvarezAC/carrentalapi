using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using MySqlConnector;

namespace CarRentalServices.Repositories;

public class CustomerRepository(IMySqlDataService dataService) : ICustomerRepository
{
    public IEnumerable<Customer> GetCustomers()
    {
        return ReadRows(dataService.ExecSP("GetCustomers"))
            .Select(MapCustomer)
            .ToList();
    }

    public Customer? GetCustomerById(int id)
    {
        var parameters = new[] { new MySqlParameter("p_IdCustomer", id) };
        var result = dataService.ExecSP("GetCustomerById", parameters);

        return ReadRows(result).Select(MapCustomer).SingleOrDefault();
    }

    public bool InsertCustomer(Customer customer)
    {
        var result = dataService.ExecSP("InsertCustomer", CustomerParameters(customer));
        var row = ReadRows(result).Single();

        customer.IdCustomer = Convert.ToInt32(row["NewCustomerId"]);
        return customer.IdCustomer > 0;
    }

    public bool UpdateCustomer(Customer customer)
    {
        var parameters = CustomerParameters(customer);
        parameters.Add(new MySqlParameter("p_IdCustomer", customer.IdCustomer));

        var result = dataService.ExecSP("UpdateCustomer", parameters);
        return Convert.ToInt32(ReadRows(result).Single()["Found"]) == 1;
    }

    public bool DeleteCustomer(int id)
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<DataRow> ReadRows(DataSet result)
    {
        if (result == null || result.Tables.Count == 0)
        {
            throw new InvalidOperationException(
                "El procedimiento de clientes no devolvió una tabla. Revisa Output.");
        }

        return result.Tables[0].AsEnumerable();
    }

    private static Customer MapCustomer(DataRow row)
    {
        return new Customer
        {
            IdCustomer = Convert.ToInt32(row["IdCustomer"]),
            FirstName = row.Field<string>("FirstName") ?? "",
            LastName = row.Field<string>("LastName") ?? "",
            SocialSecurityNumber = row.Field<string>("SSN") ?? "",
            Email = row.Field<string>("Email") ?? "",
            CountryId = Convert.ToInt32(row["CountryId"]),
            CountryName = row.Field<string>("CountryName") ?? "",
            MobilePhone = row.Field<string>("Mobile") ?? "",
            StateName = row.Field<string>("StateName") ?? ""
        };
    }

    private static List<MySqlParameter> CustomerParameters(Customer customer)
    {
        return new List<MySqlParameter>
        {
            new("p_FirstName", customer.FirstName),
            new("p_LastName", customer.LastName),
            new("p_SSN", customer.SocialSecurityNumber),
            new("p_Email", customer.Email),
            new("p_CountryId", customer.CountryId),
            new("p_Mobile", customer.MobilePhone),
            new("p_StateName", customer.StateName)
        };
    }
}