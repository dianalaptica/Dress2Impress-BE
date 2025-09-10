using Dress2Impress.BusinessLogic.IServices;
using Dress2Impress.Domain.Requests;
using Dress2Impress.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dress2Impress.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClothingItemsController : ControllerBase
{
    private readonly IClothingItemService _service;

    public ClothingItemsController(IClothingItemService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClothingItemResponse>> Create([FromBody] AddClothingItemRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdStr, out var userId))
            return Unauthorized();

        var created = await _service.AddAsync(request, userId);
        return Created($"/api/clothingitems/{created.Id}", created);
    }

    [Authorize]
    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClothingItemResponse>>> GetMyItems()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var items = await _service.GetAllForUserAsync(userId);
        return Ok(items);
    }

    [Authorize]
    [HttpGet("by-category/{categoryId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClothingItemResponse>>> GetByCategory([FromRoute] int categoryId)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _service.GetAllByCategoryAsync(userId, categoryId);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("top-worn")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClothingItemResponse>>> GetTopWorn([FromQuery] int? limit)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var count = (limit.HasValue && limit.Value > 0) ? limit.Value : 4;
        var result = await _service.GetTopWornAsync(userId, count);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("{id:int}/wear-now")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClothingItemResponse>> WearNow([FromRoute] int id)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _service.WearNowAsync(userId, id);
        if (result == null) return NotFound();

        return Ok(result);
    }

    [Authorize]
    [HttpPatch("{id:int}/flags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClothingItemResponse>> UpdateFlags([FromRoute] int id, [FromBody] UpdateItemFlagsRequest request)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _service.UpdateFlagsAsync(userId, id, request);
        if (result == null) return NotFound();

        return Ok(result);
    }

    [Authorize]
    [HttpGet("dirty")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClothingItemResponse>>> GetDirty()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _service.GetDirtyAsync(userId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("wash")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> WashItems([FromBody] WashItemsRequest request)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

        var updated = await _service.WashItemsAsync(userId, request.ItemIds ?? Enumerable.Empty<int>());
        return Ok(new { updated });
    }
}
