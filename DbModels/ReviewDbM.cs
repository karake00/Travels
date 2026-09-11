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
[Table("Reviews", Schema = "supusr")]
public class ReviewDbM : Review, ISeed<ReviewDbM>
{
    //Tell EF Core that this is the primarykey (PK)
    [Key]
    public override Guid ReviewId {get; set;}

    //Tell EF Core that the name of the Review can not be empty (not null)
    [Required]
    public override string Review_Title {get; set;}

    [Required]
    public override string Review_Comment {get; set;}

    [Required]
    public override int Review_Rating {get; set;}

    //Tell EF Core to not create this list (database can not create a list with interface)
    [NotMapped]
    public override IAttraction Attraction { get => AttractionDbM; set => new NotImplementedException(); }  
    [NotMapped]
    public override IUser User { get => UserDbM; set => new NotImplementedException(); }  


    //Tell EF Core which column stores the ID
    //JsonIgnore preventing infinite loops
    [ForeignKey("AttractionId")]
    [JsonIgnore]
    public virtual AttractionDbM AttractionDbM {get; set;}

    [ForeignKey("UserId")]
    [JsonIgnore]
    public virtual UserDbM UserDbM {get; set;}

    public override ReviewDbM Seed(SeedGenerator sgen)
    {
        base.Seed(sgen);
        return this;
    }

    public ReviewDbM() {}
}