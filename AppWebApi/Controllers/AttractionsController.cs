using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

using Services;
using Models;
using Models.DTO;

namespace AppWebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AttractionsController : ControllerBase
{
    private readonly ILogger<AttractionsController> _logger;
    private readonly IAttractionsService _service;

    public AttractionsController(IAttractionsService service, ILogger<AttractionsController> logger = null)
    {
        _logger = logger;
        _service = service;
    }

    // GET: api/Attractions/read
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ResponsePageDto<IAttraction>))]
    public async Task<IActionResult> ReadAttractions(bool seeded = true, bool flat = false, string filter = null, int pageNumber = 0, int pageSize = 10)
    {
        try
        {
            var result = await _service.ReadAttractionsAsync(seeded, flat, filter, pageNumber, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{nameof(ReadAttractions)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    // GET: api/Attractions/readWithoutComment
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ResponsePageDto<IAttraction>))]
    public async Task<IActionResult> ReadAttractionsWithoutComments(bool seeded = true, bool flat = false, int pageNumber = 0, int pageSize = 10)
    {
        try
        {
            var result = await _service.ReadAttractionsWithoutCommentsAsync(seeded, flat, pageNumber, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{nameof(ReadAttractions)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    // GET: api/Attractions/readitem?id=...
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ResponseItemDto<IAttraction>))]
    public async Task<IActionResult> ReadAttraction(Guid id, bool flat = false)
    {
        try
        {
            var result = await _service.ReadAttractionAsync(id, flat);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{nameof(ReadAttractions)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
}