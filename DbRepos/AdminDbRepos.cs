﻿using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;

using DbModels;
using DbContext;
using Configuration;
using Models;
using Seido.Utilities.SeedGenerator;

namespace DbRepos;

public class AdminDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AdminDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    public async Task SeedAsync(int nrItems)
    {
        // Remove old data from databse
        _logger.LogInformation("Clearing database from old seeded data...");
        _dbContext.Cities.RemoveRange(_dbContext.Cities);
        _dbContext.Countries.RemoveRange(_dbContext.Countries);
        await _dbContext.SaveChangesAsync();

        // Create the seeder pointing to the full path of the JSON file
        var fn = Path.GetFullPath(_seedSource);
        var seeder = new SeedGenerator(fn);

        // Use seeder.UniqueItemsToList to retrieve 5 countries
        _logger.LogInformation("Generating countries...");
        var countries = seeder.UniqueItemsToList<CountryDbM>(5);

        // For each country, create cities and assign them to the navigation property
        _logger.LogInformation("Assigning matching cities to countries...");
        // Example 20 cities per country if 100 items were requested
        int countPerCountry = nrItems > 0 ? (nrItems / 5) : 20;

        foreach (var country in countries)
        {
            var citiesForCountry = new List<CityDbM>();
            for (int i = 0; i < countPerCountry; i++)
            {
                citiesForCountry.Add(new CityDbM
                {
                    CityId = Guid.NewGuid(),

                    // Sending the country name to seeder.City() to only randomize cities that actually belong to that specific country
                    Name = seeder.City(country.Name),
                    Seeded = true
                });
            }

            // Assign the list of cities to the country's navigation property
            country.CitiesDbM = citiesForCountry;
        }

        _logger.LogInformation("Adding countries...");

        // Only add the parent list (countries) to the context. EF Core's Change Tracker will automatically save all related cities in the correct order
        await _dbContext.Countries.AddRangeAsync(countries);

        // Saves everything to SQL server
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Database successfully seeded!");
    }

    public AdminDbRepos(ILogger<AdminDbRepos> logger, Encryptions encryptions, MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
}