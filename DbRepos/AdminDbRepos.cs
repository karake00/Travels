﻿using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using System.Collections.Generic;

using DbModels;
using DbContext;
using Configuration;

namespace DbRepos;

public class AdminDbRepos
{
    private readonly ILogger<AdminDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    public async Task SeedAsync(int nrItems)
    {
        _logger.LogInformation("Remove old countries and start add new ones...");
        _dbContext.Countries.RemoveRange(_dbContext.Countries);

        var wCountries = new List<CountryDbM>
        {
            new CountryDbM { Name = "Durotar" },
            new CountryDbM { Name = "Elwynn Forest" },
            new CountryDbM { Name = "Tirisfal Glades" },
            new CountryDbM { Name = "Mulgore" },
            new CountryDbM { Name = "Teldrassil" }
        };
        _dbContext.Countries.AddRange(wCountries);

        //Save changes to the database
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("New countries added! :)");
    }

    public AdminDbRepos(ILogger<AdminDbRepos> logger, Encryptions encryptions, MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
}