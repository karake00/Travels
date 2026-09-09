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
public class CitiesController : ControllerBase
{
    private readonly ILogger<CitiesController> _logger;
    private readonly ICitiesService _service;

    public CitiesController(ICitiesService service, ILogger<CitiesController> logger = null)
    {
        _logger = logger;
        _service = service;
    }

    // GET: api/cities
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ResponsePageDto<ICity>))]
    public async Task<IActionResult> ReadCities()
    {
        try
        {
            var result = await _service.ReadCitiesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{nameof(ReadCities)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
}