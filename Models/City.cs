using System;
using Seido.Utilities.SeedGenerator;
using System.Collections.Generic;

namespace Models;

public class City : ICity, ISeed<City>
{
    public virtual Guid CityId { get; set; }
    public virtual string Name { get; set; }
    
    //Foreign Key referencing the parent Country
    public virtual Guid CountryId {get;set;}

    //Model relationship: One City belongs to one Country
    public virtual ICountry Country { get; set; }
    public virtual List<IAddress> Addresses {get; set;}
    
    public bool Seeded { get; set; } = false;

    public City(){}

    
    public City(City other)
    {
        this.CityId = other.CityId;
        this.Name = other.Name;

        this.CountryId = other.CountryId;
        this.Seeded = other.Seeded;

        this.Addresses = (other.Addresses != null) ? other.Addresses.Select(c => new Address((Address)c)).ToList<IAddress>() : null;
    }

    public virtual City Seed(SeedGenerator sgen)
    {
        this.CityId = Guid.NewGuid();

        //Created own custom data from app-seeds.json and custom SeedGenerator methods
        this.Name = sgen.City();
        this.Seeded = true;
        return this;
    }
}
