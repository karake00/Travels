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
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUsersService _service;

    public UsersController(IUsersService service, ILogger<UsersController> logger = null)
    {
        _logger = logger;
        _service = service;
    }

    // GET: api/Users
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ResponsePageDto<IUser>))]
    public async Task<IActionResult> ReadUsers(bool seeded = true, bool flat = false, string filter = null, int pageNumber = 0, int pageSize = 10)
    {
        try
        {
            var result = await _service.ReadUsersAsync(seeded, flat, filter, pageNumber, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{nameof(ReadUsers)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
}