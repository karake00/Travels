using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;
using Azure;

namespace DbRepos;

public class AttractionsDbRepos
{
    private ILogger<AttractionsDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public AttractionsDbRepos(ILogger<AttractionsDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public async Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        //Ckecks if filter input is null, if it is, sett filter to empty string "".
        filter ??= "";

        //Creates the base query and with AsNoTracking method for better perfomance
        IQueryable<AttractionDbM> query = _dbContext.Attractions.AsNoTracking();

        //If flat is false (deep reading), include related tables (Address, City, Country)
        //If flat is true (easy reading), skip the related tebles
        if (!flat)
        {
            query = query
                .Include(a => a.AddressDbM)
                .ThenInclude(c => c.CityDbM)
                .ThenInclude(co => co.CountryDbM);
        }
        
        //Filters the query by seed status and searches in Name, Info, City name and Country name.
        var filteredQuery = query.Where(i => i.Seeded == seeded && 
            (i.Attraction_Name.ToLower().Contains(filter.ToLower()) || 
                i.Attraction_Info.ToLower().Contains(filter.ToLower()) || 
                i.AddressDbM.CityDbM.Name.ToLower().Contains(filter.ToLower()) ||
                i.AddressDbM.CityDbM.CountryDbM.Name.ToLower().Contains(filter.ToLower())));

        //Creates the response object for the page items and page info.
        var ret = new ResponsePageDto<IAttraction>()
        {   
#if DEBUG
        ConnectionString = _dbContext.dbConnection,
#endif

        //Counts total matching items in the database for pagination.
        DbItemsCount = await filteredQuery.CountAsync(),

        //Fetches only the current page items
        PageItems = await filteredQuery
        .Skip(pageNumber * pageSize)
        .Take(pageSize)
        .ToListAsync<IAttraction>(),
        
        PageNr = pageNumber,
        PageSize = pageSize
        };
        return ret;
    }

    public async Task<ResponsePageDto<IAttraction>> ReadAttractionsWithoutCommentsAsync(bool seeded, bool flat, int pageNumber, int pageSize)
    {
        IQueryable<AttractionDbM> query = _dbContext.Attractions.AsNoTracking();

        if (!flat)
        {
            query = query
                .Include(a => a.AddressDbM)
                .ThenInclude(ad => ad.CityDbM)
                .ThenInclude(c => c.CountryDbM);
        }
        
        var filteredQuery = query.Where(i => i.Seeded == seeded && 
            (i.ReviewsDbM == null || i.ReviewsDbM.Count == 0));

        var ret = new ResponsePageDto<IAttraction>()
        {   
#if DEBUG
        ConnectionString = _dbContext.dbConnection,
#endif

        DbItemsCount = await filteredQuery.CountAsync(),
        PageItems = await filteredQuery
        .Skip(pageNumber * pageSize)
        .Take(pageSize)
        .ToListAsync<IAttraction>(),
        
        PageNr = pageNumber,
        PageSize = pageSize
        };
        return ret;
    }

    public async Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat)
    {
        IQueryable<AttractionDbM> query = _dbContext.Attractions.AsNoTracking();

        if (!flat)
        {
            query = query
                .Include(a => a.AddressDbM)
                .ThenInclude(ad => ad.CityDbM)
                .ThenInclude(c => c.CountryDbM)
                .Include(r => r.ReviewsDbM);
        }
        
        //Fetches the attraction matching the id input, if nothing match it will be null.
        var item = await query.FirstOrDefaultAsync(a => a.AttractionId == id);

        var ret = new ResponseItemDto<IAttraction>()
        {   
#if DEBUG
        ConnectionString = _dbContext.dbConnection,
#endif
        Item = item
        };
        return ret;
    }
}