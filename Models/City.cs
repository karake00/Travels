using System;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class City : ICity, ISeed<City>
{
    public virtual Guid CityId { get; set; }
    public virtual string Name { get; set; }
    
    public virtual Guid CountryId {get;set;}
    public virtual ICountry Country { get; set; }
    
    public bool Seeded { get; set; }

    public City(){}

    public City(City other)
    {
        this.CityId = other.CityId;
        this.Name = other.Name;

        this.CountryId = other.CountryId;
        this.Country = other.Country;
        this.Seeded = other.Seeded;
    }

    public virtual City Seed(SeedGenerator sgen)
    {
        this.CityId = Guid.NewGuid();
        this.Name = sgen.FromString("Stormwind, Orgrimmar, Dalaran, Ironforge, Undercity, Shattrath, Darnassus, Thunder Bluff");
        this.Seeded = true;
        return this;
    }
}
