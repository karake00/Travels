using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using Models;


namespace DbModels;

public class CountryDbM : Country
{
    //Tell EF Core that this is the primarykey (PK)
    [Key]
    public override Guid CountryId {get; set;}

    //Tell EF Core that the name of the country can not be empty (not null)
    [Required]
    public override string Name {get; set;}

    //Tell EF Core to not create this list (database can not create a list with interface)
    [NotMapped]
    public override List<ICity> Cities 
    {
        get => CitiesDbM?.Cast<ICity>().ToList(); 
        set => CitiesDbM = value?.Cast<CityDbM>().ToList();
    }

    //Preventing infinite loops
    [JsonIgnore]
    public virtual List<CityDbM> CitiesDbM {get; set;}
}