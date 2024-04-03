using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Checkins.DoCheckin;
using PassIn.Communication.Responses;
using System.Net.Mime;

namespace PassIn.Api.Controllers;

/// <summary>
/// Check-in controller.
/// </summary>
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class CheckInController(IDoAttendeeCheckinUseCase doCheckInUseCase) : ControllerBase
{
    private readonly IDoAttendeeCheckinUseCase _doCheckInUseCase = doCheckInUseCase;

    /// <summary>
    /// Check-in an attendee.
    /// </summary>
    /// <param name="attendeeId">Attendee id.</param>
    [HttpPost]
    [Route("{attendeeId}")]
    [ProducesResponseType(typeof(ResponseRegisterJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CheckinAsync([FromRoute] Guid attendeeId)
    {
        var response = await _doCheckInUseCase.ExecuteAsync(attendeeId);
        return Created(string.Empty, response);
    }
}