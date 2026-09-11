using System;

namespace Models;

public interface IAddress
{
    Guid AddressId { get; set; }
    string Street_Name { get; set; }
    int ZipCode {get; set;}

    Guid CityId {get;set;}

    ICity City { get; set; }
    List<IAttraction> Attractions {get; set;}
}