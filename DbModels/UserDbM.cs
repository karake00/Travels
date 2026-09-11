using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using Models;
using Seido.Utilities.SeedGenerator;


namespace DbModels;

//Mapping the class to table "Users" and place it in "supusr"
//So that access rights within the datebase can be managed later.
[Table("Users", Schema = "supusr")]
public class UserDbM : User, IEquatable<UserDbM>, ISeed<UserDbM>
{
    //Tell EF Core that this is the primarykey (PK)
    [Key]
    public override Guid UserId {get; set;}

    //Tell EF Core that the name of the User can not be empty (not null)
    [Required]
    public override string User_Name {get; set;}

    //Tell EF Core to not create this list (database can not create a list with interface)
    [NotMapped]
    public override List<IReview> Reviews {get => ReviewsDbM?.ToList<IReview>(); set => new NotImplementedException();}

    //Preventing infinite loops
    [JsonIgnore]
    public virtual List<ReviewDbM> ReviewsDbM {get; set;}

    public bool Equals(UserDbM other) => other != null && User_Name == other.User_Name;
    public override int GetHashCode() => User_Name?.GetHashCode() ?? 0;

    public override UserDbM Seed(SeedGenerator sgen)
    {
        this.UserId = Guid.NewGuid();
        this.User_Name = sgen.FirstName;
        this.Seeded = true;
        return this;
    }
}