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
public class AddressesController : ControllerBase
{
    private readonly ILogger<AddressesController> _logger;
    private readonly IAddressesService _service;

    public AddressesController(IAddressesService service, ILogger<AddressesController> logger = null)
    {
        _logger = logger;
        _service = service;
    }

    // GET: api/Addresses
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ResponsePageDto<IAddress>))]
    public async Task<IActionResult> ReadAddresses()
    {
        try
        {
            var result = await _service.ReadAddressesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{nameof(ReadAddresses)}: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
}