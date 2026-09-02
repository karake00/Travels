using System.ComponentModel.DataAnnotations;
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
}