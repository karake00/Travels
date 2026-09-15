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
public class AttractionsController : ControllerBase
{
    private readonly ILogger<AttractionsController> _logger;
    private readonly IAttractionsService _service;

    public AttractionsController(IAttractionsService service, ILogger<AttractionsController> logger = null)
    {
        _logger = logger;
        _service = service;
    }

    // GET: api/Attractions
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ResponsePageDto<IAttraction>))]
    public async Task<IActionResult> ReadAttractions()
    {
        try
        {
            var result = await _service.ReadAttractionsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{nameof(ReadAttractions)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
}