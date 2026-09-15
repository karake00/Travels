﻿using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using Models.DTO;
using DbModels;
using DbContext;
using Configuration;

namespace DbRepos;

public class AdminDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AdminDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    public AdminDbRepos(ILogger<AdminDbRepos> logger, Encryptions encryptions, MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }

    public async Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync() => await DbInfo();


    private async Task<ResponseItemDto<GstUsrInfoAllDto>> DbInfo()
    {
        var info = new GstUsrInfoAllDto();
        info.Db = new GstUsrInfoDbDto
        {
            NrSeededCountries = await _dbContext.Countries.Where(c => c.Seeded).CountAsync(),
            NrUnseededCountries = await _dbContext.Countries.Where(c => !c.Seeded).CountAsync(),
            NrCountriesWithCities = await _dbContext.Countries.Where(c => c.CitiesDbM.Count > 0).CountAsync(),
            
            NrSeededCities = await _dbContext.Cities.Where(c => c.Seeded).CountAsync(),
            NrUnseededCities = await _dbContext.Cities.Where(c => !c.Seeded).CountAsync(),
            
            NrSeededAddresses = await _dbContext.Addresses.Where(a => a.Seeded).CountAsync(),
            NrUnseededAddresses = await _dbContext.Addresses.Where(a => !a.Seeded).CountAsync(),
            
            NrSeededAttractions = await _dbContext.Attractions.Where(a => a.Seeded).CountAsync(),
            NrUnseededAttractions = await _dbContext.Attractions.Where(a => !a.Seeded).CountAsync(),
            
            NrSeededUsers = await _dbContext.Users.Where(u => u.Seeded).CountAsync(),
            NrUnseededUsers = await _dbContext.Users.Where(u => !u.Seeded).CountAsync(),
            
            NrSeededReviews = await _dbContext.Reviews.Where(r => r.Seeded).CountAsync(),
            NrUnseededReviews = await _dbContext.Reviews.Where(r => !r.Seeded).CountAsync()
        };

        return new ResponseItemDto<GstUsrInfoAllDto>
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = info
        };
    }


    public async Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrItems)
    {
        // Clear database from all seeded data
        await RemoveSeedAsync(true);

        // Create a seeder
        var fn = Path.GetFullPath(_seedSource);
        var seeder = new SeedGenerator(fn);

        #region Full seeding

        //Generate a list with data to every table (except reviews)
        var users = seeder.ItemsToList<UserDbM>(50); //Generates 50 users
        var countries = seeder.UniqueItemsToList<CountryDbM>(5); //Generates 5 unique countries
        var cities = seeder.ItemsToList<CityDbM>(100); //Generates 100 cities 
        var addresses = seeder.ItemsToList<AddressDbM>(200); //Generates 200 addresses
        var attractions = seeder.ItemsToList<AttractionDbM>(nrItems < 1000 ? 1000 : nrItems); //Generates 1000 attractions if input is under 1000, else it generates nrItems

        //Link cities to country
        foreach (var city in cities)
        {
            city.CountryDbM = seeder.FromList(countries);
        }

        //Link addresses to cities
        foreach (var address in addresses)
        {
            address.CityDbM = seeder.FromList(cities);
        }

        //Link attractions to addresses and reviews to users
        foreach (var attraction in attractions)
        {
            //Link attraction to address
            attraction.AddressDbM = seeder.FromList(addresses);

            //Generate data to reviews, 0 to 20 reviews per attraction
            var reviews = seeder.ItemsToList<ReviewDbM>(seeder.Next(0, 21));

            //Link reviews to users
            foreach (var review in reviews)
            {
                review.UserDbM = seeder.FromList(users);
            }
            //Link attraction to reviews
            attraction.ReviewsDbM = reviews;
        }

        //Save everything in EF Core
        await _dbContext.Users.AddRangeAsync(users);
        await _dbContext.Countries.AddRangeAsync(countries);
        await _dbContext.Cities.AddRangeAsync(cities);
        await _dbContext.Addresses.AddRangeAsync(addresses);
        await _dbContext.Attractions.AddRangeAsync(attractions);

        #endregion

        LogChangeTracker();
        await _dbContext.SaveChangesAsync();
        LogChangeTracker();

        return await DbInfo();
    }

    public async Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded)
    {
        _dbContext.Reviews.RemoveRange(_dbContext.Reviews.Where(f => f.Seeded == seeded));
        _dbContext.Users.RemoveRange(_dbContext.Users.Where(f => f.Seeded == seeded));
        _dbContext.Attractions.RemoveRange(_dbContext.Attractions.Where(f => f.Seeded == seeded));
        _dbContext.Addresses.RemoveRange(_dbContext.Addresses.Where(f => f.Seeded == seeded));
        _dbContext.Cities.RemoveRange(_dbContext.Cities.Where(c => c.Seeded == seeded));
        _dbContext.Countries.RemoveRange(_dbContext.Countries.Where(c => c.Seeded == seeded));

        LogChangeTracker();
        await _dbContext.SaveChangesAsync();
        LogChangeTracker();

        return await DbInfo();
    }

    private void LogChangeTracker()
    {
        foreach (var e in _dbContext.ChangeTracker.Entries())
        {
            var id = e.Entity switch
            {
                CountryDbM countryDbM => countryDbM.CountryId,
                CityDbM cityDbM => cityDbM.CityId,
                AddressDbM addressDbM => addressDbM.AddressId,
                AttractionDbM attractionDbM => attractionDbM.AttractionId,
                ReviewDbM reviewDbM => reviewDbM.ReviewId,
                UserDbM userDbM => userDbM.UserId,
                _ => Guid.Empty
            };

            _logger.LogInformation($"{nameof(LogChangeTracker)}: {e.Entity.GetType().Name}: {id} - {e.State}");
        }
    }
}