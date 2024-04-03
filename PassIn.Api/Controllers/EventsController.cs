using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Events.GetAll;
using PassIn.Application.UseCases.Events.Register;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using System.Net.Mime;

namespace PassIn.Api.Controllers;

/// <summary>
/// Events controller.
/// </summary>
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class EventsController(IGetAllEventsUseCase getAllEventsUseCase, IGetEventByIdUseCase getEventByIdUseCase, IRegisterEventUseCase registerEventUseCase) : ControllerBase
{
    private readonly IGetAllEventsUseCase _getAllEventsUseCase = getAllEventsUseCase;
    private readonly IGetEventByIdUseCase _getEventByIdUseCase = getEventByIdUseCase;
    private readonly IRegisterEventUseCase _registerEventUseCase = registerEventUseCase;

    /// <summary>
    /// Register a new event.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAsync([FromBody] RequestEventJson request)
    {
        var response = await _registerEventUseCase.ExecuteAsync(request);
        return Created(string.Empty, response);
    }

    /// <summary>
    /// Get an event by id.
    /// </summary>
    /// <param name="id">Event id.</param>
    [HttpPost]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var response = await _getEventByIdUseCase.ExecuteAsync(id);
        return Ok(response); 
    }

    /// <summary>
    /// Get all events.
    /// </summary>
    /// <param name="searchKey">Filter by search key.</param>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync([FromQuery] string? searchKey)
    {
        var response = await _getAllEventsUseCase.ExecuteAsync(searchKey);
        return Ok(response);
    }
}
