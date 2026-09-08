using System.Collections.Generic;
namespace Models;

public interface ICountry
{
    Guid CountryId {get; set;}
    string Name {get; set;}

    List<ICity> Cities {get; set;}
    
}