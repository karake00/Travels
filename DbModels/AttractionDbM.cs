using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Models;
using Seido.Utilities.SeedGenerator;

namespace DbModels;


//Mapping the class to table "Cities" and place it in "supusr"
//So that access rights within the datebase can be managed later.
[Table("Attractions", Schema = "supusr")]
public class AttractionDbM : Attraction, ISeed<AttractionDbM>
{
    //Tell EF Core that this is the primarykey (PK)
    [Key]
    public override Guid AttractionId {get; set;}

    //Tell EF Core that the name of the Attraction can not be empty (not null)
    [Required]
    public override string Attraction_Name {get; set;}

    [Required]
    public override string Attraction_Info {get; set;}

    //Tell EF Core to not create this list (database can not create a list with interface)
    [NotMapped]
    public override IAddress Address { get => AddressDbM; set => new NotImplementedException(); }  

    [NotMapped]
    public override List<IReview> Reviews {get => ReviewsDbM?.ToList<IReview>(); set => new NotImplementedException();}

    //Preventing infinite loops
    [JsonIgnore]
    public virtual List<ReviewDbM> ReviewsDbM {get; set;}

    //Tell EF Core which column stores the ID
    //JsonIgnore preventing infinite loops
    [ForeignKey("AddressId")]
    [JsonIgnore]
    public virtual AddressDbM AddressDbM {get; set;}

    public override AttractionDbM Seed(SeedGenerator sgen)
    {
        base.Seed(sgen);
        return this;
    }

    public AttractionDbM() {}
}