using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Models;

namespace DbModels;


//Mapping the class to table "Cities" and place it in "supusr"
//So that access rights within the datebase can be managed later.
[Table("Cities", Schema = "supusr")]
public class CityDbM : City
{
    //Tell EF Core that this is the primarykey (PK)
    [Key]
    public override Guid CityId {get; set;}

    //Tell EF Core that the name of the country can not be empty (not null)
    [Required]
    public override string Name {get; set;}

    //Tell EF Core to not create this list (database can not create a list with interface)
    [NotMapped]
    public override ICountry Country 
    {
        get => CountryDbM; 
        set => CountryDbM = value as CountryDbM;
    }


    //Tell EF Core which column stores the ID
    //JsonIgnore preventing infinite loops
    [ForeignKey("CountryId")]
    [JsonIgnore]
    public virtual CountryDbM CountryDbM {get; set;}
}