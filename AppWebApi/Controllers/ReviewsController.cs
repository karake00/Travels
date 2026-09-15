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
public class ReviewsController : ControllerBase
{
    private readonly ILogger<ReviewsController> _logger;
    private readonly IReviewsService _service;

    public ReviewsController(IReviewsService service, ILogger<ReviewsController> logger = null)
    {
        _logger = logger;
        _service = service;
    }

    // GET: api/Reviews
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ResponsePageDto<IReview>))]
    public async Task<IActionResult> ReadReviews()
    {
        try
        {
            var result = await _service.ReadReviewsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{nameof(ReadReviews)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
}