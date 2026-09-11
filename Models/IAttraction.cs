using System;

namespace Models;

public interface IAttraction
{
    Guid AttractionId { get; set; }
    string Attraction_Name { get; set; }
    string Attraction_Info { get; set; }

    Guid AddressId {get;set;}

    IAddress Address { get; set; }
    List<IReview> Reviews {get; set;}
}