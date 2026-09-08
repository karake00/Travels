using System;
using System.Collections.Generic;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Country : ICountry, ISeed<Country>
{
    public virtual Guid CountryId {get; set;}
    public virtual string Name {get; set;}

    public virtual List<ICity> Cities {get; set;} = new List<ICity>();

    public bool Seeded { get; set; }

    public Country() {}

    public Country(Country other)
    {
        this.CountryId = other.CountryId;
        this.Name = other.Name;
        this.Cities = (other.Cities != null) ? new List<ICity>(other.Cities) : new List<ICity>();
        this.Seeded = other.Seeded;
    }

    public virtual Country Seed(SeedGenerator sgen)
    {
        this.CountryId = Guid.NewGuid();
        this.Name = sgen.FromString("Azeroth, Kalimdor, Outland, Northrend");
        this.Seeded = true;
        return this;
    }
}