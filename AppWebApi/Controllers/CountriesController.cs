using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

using Services;
using Models;
using Models.DTO;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly ILogger<CountriesController> _logger;
    private readonly ICountriesService _service;

    public CountriesController(ICountriesService service, ILogger<CountriesController> logger = null)
    {
        _logger = logger;
        _service = service;
    }

    // GET: api/countries
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ResponsePageDto<ICountry>))]
    public async Task<IActionResult> ReadCountries()
    {
        try
        {
            var result = await _service.ReadCountriesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{nameof(ReadCountries)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
}