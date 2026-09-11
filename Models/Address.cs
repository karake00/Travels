using System;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Address : IAddress, ISeed<Address>
{
    public virtual Guid AddressId { get; set; }
    public virtual string Street_Name { get; set; }
    public virtual int ZipCode {get; set;}
    
    //Foreign Key referencing the parent Country
    public virtual Guid CityId {get;set;}

    //Model relationship: One Address belongs to one Country
    public virtual ICity City { get; set; }

    public virtual List<IAttraction> Attractions {get; set;}
    
    public bool Seeded { get; set; } = false;

    public Address(){}

    
    public Address(Address other)
    {
        this.AddressId = other.AddressId;
        this.Street_Name = other.Street_Name;

        this.CityId = other.CityId;
        this.Seeded = other.Seeded;

        this.Attractions = (other.Attractions != null) ? other.Attractions.Select(c => new Attraction((Attraction)c)).ToList<IAttraction>() : null;
    }

    public virtual Address Seed(SeedGenerator sgen)
    {
        this.AddressId = Guid.NewGuid();

        //Created own custom data from app-seeds.json and custom SeedGenerator methods
        this.Street_Name = sgen.StreetAddress();
        this.ZipCode = sgen.ZipCode;
        this.Seeded = true;
        return this;
    }
}
