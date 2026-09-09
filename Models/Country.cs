using System;
using System.Collections.Generic;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Country : ICountry, ISeed<Country>
{
    public virtual Guid CountryId {get; set;}
    public virtual string Name {get; set;}

    //Model relationship: One Country has many Cities
    public virtual List<ICity> Cities {get; set;}

    public bool Seeded { get; set; } = false;

    public Country() {}

    //Copy constructor: Creating independent deep copies
    //which means that it takes an existing object and creates a new independent copy.
    //With deep copy it also creates memory for the things inside(cities)
    //Deep copy avoids the original and the copy from affecting each other
    public Country(Country other)
    {
        this.CountryId = other.CountryId;
        this.Name = other.Name;

        //Creating a deep copy of the cities if there is any Cities
        //Else keep it null
        this.Cities = (other.Cities != null) ? other.Cities.Select(c => new City((City)c)).ToList<ICity>() : null;
        this.Seeded = other.Seeded;
    }

    public virtual Country Seed(SeedGenerator sgen)
    {
        this.CountryId = Guid.NewGuid();

        //Created own custom data from app-seeds.json and custom SeedGenerator methods
        this.Name = sgen.Country;
        this.Seeded = true;
        return this;
    }
}