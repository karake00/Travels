using System;

namespace Models;

public interface ICity
{
    Guid CityId { get; set; }
    string Name { get; set; }

    Guid CountryId {get;set;}

    ICountry Country { get; set; }

    List<IAddress> Addresses {get; set;}
}