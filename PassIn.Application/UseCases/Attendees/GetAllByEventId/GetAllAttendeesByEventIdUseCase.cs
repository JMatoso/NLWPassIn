using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Attendees.GetAllByEventId;

public interface IGetAllAttendeesByEventIdUseCase
{
    Task<ResponseAllAttendeesJson> ExecuteAsync(Guid eventId);
}

public class GetAllAttendeesByEventIdUseCase(PassInDbContext passInDbContext) : IGetAllAttendeesByEventIdUseCase
{
    private readonly PassInDbContext _dbContext = passInDbContext;

    public async Task<ResponseAllAttendeesJson> ExecuteAsync(Guid eventId)
    {
        var entity = await _dbContext
            .Events
            .Include(ev => ev.Attendees)
            .ThenInclude(attendee => attendee.CheckIn)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == eventId);

        return entity is null ?
            throw new NotFoundException("An event with this id does not exist.") :
            new ResponseAllAttendeesJson
            {
                Attendees = entity.Attendees.Select(attendee => new ResponseAttendeeJson
                {
                    Id = attendee.Id,
                    Name = attendee.Name,
                    Email = attendee.Email,
                    CheckedInAt = attendee.CheckIn?.CreatedAt
                }).ToList()
            };
    }
}