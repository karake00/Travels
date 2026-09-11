using System;

namespace Models;

public interface IReview
{
    Guid ReviewId { get; set; }
    string Review_Title { get; set; }
    string Review_Comment { get; set; }
    int Review_Rating {get; set;}

    Guid AttractionId {get;set;}

    IAttraction Attraction { get; set; }

    Guid UserId {get;set;}

    IUser User { get; set; }
}