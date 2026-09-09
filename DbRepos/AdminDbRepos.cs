﻿using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Linq;

using DbModels;
using DbContext;
using Configuration;
using Models;
using Models.DTO;
using Seido.Utilities.SeedGenerator;

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

    // Retrieves database overview and statistics for guest user view
    public async Task<ResponseItemDto<GstUsrInfoAllDto>> InfoAsync()
    {
        var info = new GstUsrInfoAllDto();
        info.Db = new GstUsrInfoDbDto
        {
            NrSeededCountries = await _dbContext.Countries.Where(c => c.Seeded).CountAsync(),
            NrUnseededCountries = await _dbContext.Countries.Where(c => !c.Seeded).CountAsync(),
            NrCountriesWithCities = await _dbContext.Countries.Where(c => c.CitiesDbM.Count > 0).CountAsync(),

            NrSeededCities = await _dbContext.Cities.Where(c => c.Seeded).CountAsync(),
            NrUnseededCities = await _dbContext.Cities.Where(c => !c.Seeded).CountAsync()
        };

        return new ResponseItemDto<GstUsrInfoAllDto>
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = info
        };
    }

    // Seeds the database with custom WoW countries and cities, then returns updated statistics
    public async Task<ResponseItemDto<GstUsrInfoAllDto>> SeedAsync(int nrItems)
    {
        // Remove existing seeded data before generating new items
        await RemoveSeedAsync(true);

        // Create the seeder pointing to the full path of the JSON seed file
        var fn = Path.GetFullPath(_seedSource);
        var seeder = new SeedGenerator(fn);

        // Retrieve unique countries from the custom seed generator
        _logger.LogInformation("Generating countries...");
        var countries = seeder.UniqueItemsToList<CountryDbM>(5);

        // Assign matching cities to each generated country
        _logger.LogInformation("Assigning matching cities to countries...");
        int countPerCountry = nrItems > 0 ? (nrItems / 5) : 20;

        foreach (var country in countries)
        {
            var citiesForCountry = new List<CityDbM>();
            for (int i = 0; i < countPerCountry; i++)
            {
                citiesForCountry.Add(new CityDbM
                {
                    CityId = Guid.NewGuid(),

                    // Sending the country name to seeder.City() to fetch custom WoW city data for that specific country
                    Name = seeder.City(country.Name),
                    Seeded = true
                });
            }

            // Assign the list of cities to the parent country's navigation property
            country.CitiesDbM = citiesForCountry;
        }

        _logger.LogInformation("Adding countries...");

        // Add parent list to context. EF Core Change Tracker automatically handles related entities
        await _dbContext.Countries.AddRangeAsync(countries);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Database successfully seeded!");

        // Return updated database statistics after seeding
        return await InfoAsync();
    }

    // Removes seeded or unseeded data from the database
    public async Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded)
    {
        _logger.LogInformation("Clearing database from seeded data...");
        _dbContext.Cities.RemoveRange(_dbContext.Cities.Where(c => c.Seeded == seeded));
        _dbContext.Countries.RemoveRange(_dbContext.Countries.Where(c => c.Seeded == seeded));

        await _dbContext.SaveChangesAsync();

        // Return updated database statistics after cleanup
        return await InfoAsync();
    }
}