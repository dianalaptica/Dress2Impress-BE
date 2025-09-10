using Dress2Impress.BusinessLogic.IServices;
using Dress2Impress.Domain.Requests;
using Dress2Impress.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dress2Impress.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OutfitsController : ControllerBase
{
    private readonly IOutfitService _service;

    public OutfitsController(IOutfitService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OutfitResponse>>> GetMyOutfits()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _service.GetAllForUserAsync(userId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("{id:int}/wear-today")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<WearTodayResponse>> WearToday([FromRoute] int id)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _service.WearTodayAsync(userId, id);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("generate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OutfitItemResponse>>> Generate([FromBody] GenerateOutfitRequest req)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        if (req.StyleId <= 0)
            return BadRequest("StyleId is required.");

        var result = await _service.GenerateForStyleAsync(userId, req.StyleId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("accept")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<OutfitResponse>> Accept([FromBody] AcceptOutfitRequest req)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _service.AcceptAsync(userId, req);
        return Ok(result);
    }
}
