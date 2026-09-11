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
[Table("Cities", Schema = "supusr")]
public class CityDbM : City, ISeed<CityDbM>
{
    //Tell EF Core that this is the primarykey (PK)
    [Key]
    public override Guid CityId {get; set;}

    //Tell EF Core that the name of the country can not be empty (not null)
    [Required]
    public override string Name {get; set;}

    //Tell EF Core to not create this list (database can not create a list with interface)
    [NotMapped]
    public override ICountry Country { get => CountryDbM; set => new NotImplementedException(); }

    [NotMapped]
    public override List<IAddress> Addresses { get => AddressDbM?.ToList<IAddress>(); set => new NotImplementedException(); }

    [JsonIgnore]
    public List<AddressDbM> AddressDbM { get; set; } = null;

    //Tell EF Core which column stores the ID
    //JsonIgnore preventing infinite loops
    [ForeignKey("CountryId")]
    [JsonIgnore]
    public virtual CountryDbM CountryDbM {get; set;}

    public override CityDbM Seed(SeedGenerator sgen)
    {
        base.Seed(sgen);
        return this;
    }

    public CityDbM() {}
}