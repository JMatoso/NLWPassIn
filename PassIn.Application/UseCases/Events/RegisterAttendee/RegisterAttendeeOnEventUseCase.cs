using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Events.RegisterAttendee;

public interface IRegisterAttendeeOnEventUseCase
{
    Task<ResponseRegisterJson> ExecuteAsync(Guid eventId, RequestRegisterEventJson request);
}

public class RegisterAttendeeOnEventUseCase(PassInDbContext passInDbContext) : IRegisterAttendeeOnEventUseCase
{
    private readonly PassInDbContext _dbContext = passInDbContext;

    public async Task<ResponseRegisterJson> ExecuteAsync(Guid eventId, RequestRegisterEventJson request)
    {
        await ValidateAsync(eventId, request);

        var entity = new Attendee
        {
            Email = request.Email,
            Name = request.Name,
            EventId = eventId,
            CreatedAt = DateTime.UtcNow,
        };

        await _dbContext.Attendees.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return new ResponseRegisterJson
        {
            Id = entity.Id,
        };
    }

    private async Task ValidateAsync(Guid eventId, RequestRegisterEventJson request)
    {
        var eventEntity = await _dbContext.Events.FindAsync(eventId) ??
                throw new NotFoundException("An event with this id does not exist.");

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ErrorOnValidationException("The name is invalid.");
        }

        if (!EmailIsValid(request.Email))
        {
            throw new ErrorOnValidationException("The e-mail is invalid.");
        }

        var attendeeAlreadyRegistered = await _dbContext
            .Attendees
            .AsNoTracking()
            .AnyAsync(attendee => attendee.Email.Equals(request.Email) && attendee.EventId == eventId);

        if (attendeeAlreadyRegistered)
        {
            throw new ConflictException("You can not register twice on the event.");
        }

        var attendeesForEvent = await _dbContext
            .Attendees
            .AsNoTracking()
            .CountAsync(attendee => attendee.EventId == eventId);

        if (attendeesForEvent == eventEntity.MaximumAttendees)
        {
            throw new ErrorOnValidationException("There is no room for this event.");
        }
    }

    private static bool EmailIsValid(string email)
    {
        try
        {
            _ = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}