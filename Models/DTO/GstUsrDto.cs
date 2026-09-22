namespace Models.DTO;

public class GstUsrInfoDbDto
{
    //Countries
    public int NrSeededCountries { get; set; } = 0;
    public int NrUnseededCountries { get; set; } = 0;

    //Cities
    public int NrSeededCities { get; set; } = 0;
    public int NrUnseededCities { get; set; } = 0;

    //Addresses
    public int NrSeededAddresses { get; set; } = 0; 
    public int NrUnseededAddresses { get; set; } = 0;

    // Attractions 
    public int NrSeededAttractions { get; set; } = 0; 
    public int NrUnseededAttractions { get; set; } = 0; 
    
    // Users 
    public int NrSeededUsers { get; set; } = 0; 
    public int NrUnseededUsers { get; set; } = 0; 
    
    // Reviews 
    public int NrSeededReviews { get; set; } = 0; 
    public int NrUnseededReviews { get; set; } = 0;
}

//Cities per Country
public class GstUsrInfoCitiesDto
{
    public string Country { get; set; } = null;
    public int NrCities { get; set; } = 0;
}

//Attractions per City
public class GstUsrInfoAttractionsDto 
{ 
    public string Country { get; set; } = null; 
    public string City { get; set; } = null; 
    public int NrAttractions { get; set; } = 0; 
}

//Reviews per User
public class GstUsrInfoReviewsDto 
{ 
    public string UserName { get; set; } = null; 
    public int NrReviews { get; set; } = 0; 
}


public class GstUsrInfoAllDto
{
    public GstUsrInfoDbDto Db { get; set; } = null;
    public List<GstUsrInfoCitiesDto> Cities { get; set; } = null;
    public List<GstUsrInfoAttractionsDto> Attractions { get; set; } = null;
    public List<GstUsrInfoReviewsDto> Reviews { get; set; } = null;
}