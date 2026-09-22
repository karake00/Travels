﻿using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using Models.DTO;
using DbModels;
using DbContext;
using Configuration;
using Microsoft.Data.SqlClient;

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
        var info = new GstUsrInfoAllDto
        {
            Db = await _dbContext.InfoDbView.FirstAsync()
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

        await _dbContext.SaveChangesAsync();

        return await DbInfo();
    }

    public async Task<ResponseItemDto<GstUsrInfoAllDto>> RemoveSeedAsync(bool seeded)
    {
        // Create parameters based on database provider
        var connection = _dbContext.Database.GetDbConnection();
        using var command = connection.CreateCommand();

        // Tell that it is a stored procedure and point to supusr.spDeleteAll
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "supusr.spDeleteAll";

        // Create the parameter
        var parameter = new SqlParameter("seededParam", seeded);
        command.Parameters.Add(parameter);

        // Open connection if it is closed
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        // Execute procedure in SQL server
        await command.ExecuteNonQueryAsync();

        // Return the updated data from the view
        return await DbInfo();
    }

}