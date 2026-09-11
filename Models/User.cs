using System;
using System.Collections.Generic;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class User : IUser, ISeed<User>
{
    public virtual Guid UserId {get; set;}
    public virtual string User_Name {get; set;}

    //Model relationship: One User has many Reviews
    public virtual List<IReview> Reviews {get; set;}

    public bool Seeded { get; set; } = false;

    public User() {}

    //Copy constructor: Creating independent deep copies
    //which means that it takes an existing object and creates a new independent copy.
    //With deep copy it also creates memory for the things inside(Reviews)
    //Deep copy avoids the original and the copy from affecting each other
    public User(User other)
    {
        this.UserId = other.UserId;
        this.User_Name = other.User_Name;

        //Creating a deep copy of the Reviews if there is any Reviews
        //Else keep it null
        this.Reviews = (other.Reviews != null) ? other.Reviews.Select(c => new Review((Review)c)).ToList<IReview>() : null;
        this.Seeded = other.Seeded;
    }

    public virtual User Seed(SeedGenerator sgen)
    {
        this.UserId = Guid.NewGuid();

        //Created own custom data from app-seeds.json and custom SeedGenerator methods
        this.User_Name = sgen.FirstName;
        this.Seeded = true;
        return this;
    }
}