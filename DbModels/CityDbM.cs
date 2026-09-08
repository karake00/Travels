using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models;

namespace DbModels;

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
    public override ICountry Country {get; set;}
}