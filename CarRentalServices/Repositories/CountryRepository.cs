using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CarRentalServices.Contracts;
using CarRentalServices.Entities;
using MySqlConnector;

namespace CarRentalServices.Repositories;

public class CountryRepository(IMySqlDataService dataService) : ICountryRepository
{
    public IEnumerable<Country> GetCountries()
    {
        var result = dataService.ExecSP("GetCountries");
        return ReadRows(result).Select(MapCountry).ToList();
    }

    public Country? GetCountryById(int id)
    {
        var parameters = new[] { new MySqlParameter("p_IdCountry", id) };
        var result = dataService.ExecSP("GetCountryById", parameters);
        return ReadRows(result).Select(MapCountry).SingleOrDefault();
    }

    public bool InsertCountry(Country country)
    {
        var result = dataService.ExecSP("InsertCountry", CountryParameters(country));
        return result.Tables.Count > 0 && result.Tables[0].Rows.Count == 1;
    }

    public bool UpdateCountry(Country country)
    {
        var result = dataService.ExecSP("UpdateCountry", CountryParameters(country));
        var row = ReadRows(result).SingleOrDefault()
            ?? throw new InvalidOperationException("UpdateCountry no devolvió el indicador Found.");
        return Convert.ToInt32(row["Found"]) == 1;
    }

    private static IEnumerable<DataRow> ReadRows(DataSet result)
    {
        if (result.Tables.Count == 0)
        {
            throw new InvalidOperationException("El procedimiento no devolvió una tabla.");
        }

        return result.Tables[0].AsEnumerable();
    }

    private static Country MapCountry(DataRow row)
    {
        return new Country
        {
            IdCountry = Convert.ToInt32(row["IdCountry"]),
            Name = row.Field<string>("Name") ?? "",
            IsoCode = row.Field<string>("IsoCode") ?? "",
            IsoCode3 = row.Field<string>("IsoCode3") ?? ""
        };
    }

    private static MySqlParameter[] CountryParameters(Country country)
    {
        return new[]
        {
            new MySqlParameter("p_IdCountry", country.IdCountry),
            new MySqlParameter("p_Name", country.Name),
            new MySqlParameter("p_IsoCode", country.IsoCode),
            new MySqlParameter("p_IsoCode3", country.IsoCode3)
        };
    }
}