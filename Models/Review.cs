using System;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Review : IReview, ISeed<Review>
{
    public virtual Guid ReviewId {get;set;}

    public virtual string Review_Title { get; set; }
    public virtual string Review_Comment { get; set; }
    public virtual int Review_Rating { get; set; }
    
    //Foreign Keys referencing the parent Country
    public virtual Guid AttractionId { get; set; }
    public virtual Guid UserId {get; set;}

    //Model relationship: One Review belongs to one Attraction, One Review belongs to one User
    public virtual IAttraction Attraction { get; set; }
    public virtual IUser User {get; set;}

    
    public bool Seeded { get; set; } = false;

    public Review(){}

    
    public Review(Review other)
    {
        this.ReviewId = other.ReviewId;

        this.Review_Title = other.Review_Title;
        this.Review_Comment = other.Review_Comment;
        this.Review_Rating = other.Review_Rating;

        this.ReviewId = other.ReviewId;
        this.Seeded = other.Seeded;
    }

    public virtual Review Seed(SeedGenerator sgen)
    {
        this.ReviewId = Guid.NewGuid();

        //Created own custom data from app-seeds.json and custom SeedGenerator methods
        this.Review_Title = sgen.Review.Title;
        this.Review_Comment = sgen.Review.Comment;
        this.Review_Rating = sgen.Review.Rating;

        this.Seeded = true;
        return this;
    }
}
