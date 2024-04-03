using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Attendees.GetAllByEventId;
using PassIn.Application.UseCases.Events.RegisterAttendee;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using System.Net.Mime;

namespace PassIn.Api.Controllers;

/// <summary>
/// Attendees controller.
/// </summary>
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class AttendeesController(IRegisterAttendeeOnEventUseCase registerAttendeeOnEventUseCase, IGetAllAttendeesByEventIdUseCase getAllAttendeesByEventIdUseCase) : ControllerBase
{
    private readonly IRegisterAttendeeOnEventUseCase _registerAttendeeOnEventUseCase = registerAttendeeOnEventUseCase;
    private readonly IGetAllAttendeesByEventIdUseCase _getAllAttendeesByEventIdUseCase = getAllAttendeesByEventIdUseCase;

    /// <summary>
    /// Register an attendee on an event.
    /// </summary>
    /// <param name="eventId">Event id.</param>
    [HttpPost]
    [Route("{eventId}/register")]
    [ProducesResponseType(typeof(ResponseRegisterJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterAsync([FromRoute] Guid eventId, [FromBody] RequestRegisterEventJson request)
    {
        var response = await _registerAttendeeOnEventUseCase.ExecuteAsync(eventId, request);
        return Created(string.Empty, response);
    }

    /// <summary>
    /// Get all attendees by event id.
    /// </summary>
    /// <param name="eventId">Event id.</param>
    [HttpGet]
    [Route("{eventId}")]
    [ProducesResponseType(typeof(ResponseRegisterJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync([FromRoute] Guid eventId)
    {
        var response = await _getAllAttendeesByEventIdUseCase.ExecuteAsync(eventId);
        return Ok(response);
    }
}