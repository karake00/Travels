namespace Models.DTO;

// Summary and statistics for the entire database
public class GstUsrInfoDbDto
{
    public int NrSeededCountries { get; set; } = 0;
    public int NrUnseededCountries { get; set; } = 0;
    public int NrCountriesWithCities { get; set; } = 0;

    public int NrSeededCities { get; set; } = 0;
    public int NrUnseededCities { get; set; } = 0;
}

// Statistics showing the number of cities per country
public class GstUsrInfoCitiesDto
{
    public string Country { get; set; } = null;
    public int NrCities { get; set; } = 0;
}

// Main DTO aggregating all statistics for the guest user view
public class GstUsrInfoAllDto
{
    public GstUsrInfoDbDto Db { get; set; } = null;
    public List<GstUsrInfoCitiesDto> Cities { get; set; } = null;
}