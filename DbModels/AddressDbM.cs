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
[Table("Addresses", Schema = "supusr")]
public class AddressDbM : Address, ISeed<AddressDbM>
{
    //Tell EF Core that this is the primarykey (PK)
    [Key]
    public override Guid AddressId {get; set;}

    //Tell EF Core that the name of the City can not be empty (not null)
    [Required]
    public override string Street_Name {get; set;}

    [Required]
    public override int ZipCode {get; set;}

    //Tell EF Core to not create this list (database can not create a list with interface)
    [NotMapped]
    public override ICity City { get => CityDbM; set => new NotImplementedException(); }  

    [NotMapped]
    public override List<IAttraction> Attractions { get => AttractionDbM?.ToList<IAttraction>(); set => new NotImplementedException(); }

    [JsonIgnore]
    public List<AttractionDbM> AttractionDbM { get; set; } = null;

    //Tell EF Core which column stores the ID
    //JsonIgnore preventing infinite loops
    [ForeignKey("CityId")]
    [JsonIgnore]
    public virtual CityDbM CityDbM {get; set;}

    public override AddressDbM Seed(SeedGenerator sgen)
    {
        base.Seed(sgen);
        return this;
    }

    public AddressDbM() {}
}