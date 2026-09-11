using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using Models;
using Seido.Utilities.SeedGenerator;


namespace DbModels;

//Mapping the class to table "Countries" and place it in "supusr"
//So that access rights within the datebase can be managed later.
[Table("Countries", Schema = "supusr")]
public class CountryDbM : Country, IEquatable<CountryDbM>, ISeed<CountryDbM>
{
    //Tell EF Core that this is the primarykey (PK)
    [Key]
    public override Guid CountryId {get; set;}

    //Tell EF Core that the name of the country can not be empty (not null)
    [Required]
    public override string Name {get; set;}

    //Tell EF Core to not create this list (database can not create a list with interface)
    [NotMapped]
    public override List<ICity> Cities {get => CitiesDbM?.ToList<ICity>(); set => new NotImplementedException();}

    //Preventing infinite loops
    [JsonIgnore]
    public virtual List<CityDbM> CitiesDbM {get; set;}

    public bool Equals(CountryDbM other) => other != null && Name == other.Name;
    public override int GetHashCode() => Name?.GetHashCode() ?? 0;

    public override CountryDbM Seed(SeedGenerator sgen)
    {
        this.CountryId = Guid.NewGuid();
        this.Name = sgen.Country;
        this.Seeded = true;
        return this;
    }
}