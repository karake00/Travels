using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Services;
using Configuration;
using Configuration.Options;
using Models.DTO;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly DbConnectionSetsOptions _dbSetOptions;
    private readonly AesEncryptionOptions _aesOptions;
    private readonly JwtOptions _jwtOptions;
    private readonly VersionOptions _versionOptions;
    private readonly IConfiguration _configuration;
    private readonly Encryptions _encryptions;
    private readonly DatabaseConnections _dbConnections;
    private readonly IAdminService _service;

    public AdminController(
        ILogger<AdminController> logger,
        IConfiguration configuration,
        IOptions<DbConnectionSetsOptions> dbSetOptions,
        IOptions<AesEncryptionOptions> aesOptions,
        IOptions<JwtOptions> jwtOptions,
        IOptions<VersionOptions> versionOptions,
        Encryptions encryptions,
        DatabaseConnections dbConnections,
        IAdminService service)
    {
        _logger = logger;
        _configuration = configuration;
        _dbSetOptions = dbSetOptions?.Value;
        _aesOptions = aesOptions?.Value;
        _jwtOptions = jwtOptions?.Value;
        _versionOptions = versionOptions?.Value;
        _encryptions = encryptions;
        _dbConnections = dbConnections;
        _service = service;
    }

    // GET: api/admin/Environment
    [HttpGet]
    [ActionName("Environment")]
    [ProducesResponseType(200, Type = typeof(DatabaseConnections.SetupInformation))]
    public IActionResult Environment()
    {
        try
        {
            var info = _dbConnections.SetupInfo;
            _logger.LogInformation($"{nameof(Environment)}:\n{JsonConvert.SerializeObject(info)}");
            return Ok(info);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(Environment)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    // GET: api/admin/Version
    [HttpGet]
    [ActionName("Version")]
    [ProducesResponseType(typeof(VersionOptions), 200)]
    public IActionResult Version()
    {
        try
        {
            _logger.LogInformation($"{nameof(Version)}:\n{JsonConvert.SerializeObject(_versionOptions)}");
            return Ok(_versionOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving version information");
            return BadRequest(ex.Message);
        }
    }

    // GET: api/admin/Info
    [HttpGet]
    [ActionName("Info")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<GstUsrInfoAllDto>))]
    public async Task<IActionResult> Info()
    {
        try
        {
            var result = await _service.GuestInfoAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(Info)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    // GET: api/admin/Seed?nrItems=50
    [HttpGet]
    [ActionName("Seed")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<GstUsrInfoAllDto>))]
    public async Task<IActionResult> Seed(int nrItems = 50)
    {
        try
        {
            _logger.LogInformation($"{nameof(Seed)}");
            var result = await _service.SeedAsync(nrItems);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(Seed)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/admin/RemoveSeed?seeded=true
    [HttpDelete]
    [ActionName("RemoveSeed")]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<GstUsrInfoAllDto>))]
    public async Task<IActionResult> RemoveSeed(bool seeded = true)
    {
        try
        {
            _logger.LogInformation($"{nameof(RemoveSeed)}");
            var result = await _service.RemoveSeedAsync(seeded);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(RemoveSeed)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    // GET: api/admin/Log
    [HttpGet]
    [ActionName("Log")]
    public async Task<IActionResult> Log([FromServices] ILoggerProvider loggerProvider)
    {
        if (loggerProvider is InMemoryLoggerProvider cl)
        {
            return Ok(await cl.MessagesAsync);
        }
        return Ok("No messages in log");
    }
}
