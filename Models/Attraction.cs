using System;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Attraction : IAttraction, ISeed<Attraction>
{
    public virtual Guid AttractionId { get; set; }
    public virtual string Attraction_Name { get; set; }
    public virtual string Attraction_Info {get; set;}
    
    //Foreign Key referencing the parent Country
    public virtual Guid AddressId {get;set;}

    //Model relationship: One Attraction belongs to one Address
    public virtual IAddress Address { get; set; }

    public virtual List<IReview> Reviews {get; set;}
    
    public bool Seeded { get; set; } = false;

    public Attraction(){}

    
    public Attraction(Attraction other)
    {
        this.AttractionId = other.AttractionId;
        this.Attraction_Name = other.Attraction_Name;

        this.AddressId = other.AddressId;
        this.Seeded = other.Seeded;

        this.Reviews = (other.Reviews != null) ? other.Reviews.Select(c => new Review((Review)c)).ToList<IReview>() : null;
   
    }

    public virtual Attraction Seed(SeedGenerator sgen)
    {
        this.AttractionId = Guid.NewGuid();

        //Created own custom data from app-seeds.json and custom SeedGenerator methods
        this.Attraction_Name = sgen.Attraction.AttractionName;
        this.Attraction_Info = sgen.Attraction.AttractionDescription;
        this.Seeded = true;
        return this;
    }
}
