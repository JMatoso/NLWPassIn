using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Checkins.DoCheckin;

public interface IDoAttendeeCheckinUseCase
{
    Task<ResponseRegisterJson> ExecuteAsync(Guid attendeeId);
}

public class DoAttendeeCheckinUseCase(PassInDbContext passInDbContext) : IDoAttendeeCheckinUseCase
{
    private readonly PassInDbContext _dbContext = passInDbContext;

    public async Task<ResponseRegisterJson> ExecuteAsync(Guid attendeeId)
    {
        await ValidateAsync(attendeeId);

        var entity = new CheckIn
        {
            AttendeeId = attendeeId,
            CreatedAt = DateTime.UtcNow,
        };

        await _dbContext.CheckIns.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return new ResponseRegisterJson
        {
            Id = attendeeId
        };
    }

    private async Task ValidateAsync(Guid attendeeId)
    {
        var existAttendee = await _dbContext.Attendees.AsNoTracking().AnyAsync(attendee => attendee.Id == attendeeId);
        if (existAttendee == false)
        {
            throw new NotFoundException("The attendee with this Id was not founf.");
        }

        var existCheckin = await _dbContext.CheckIns.AsNoTracking().AnyAsync(ch => ch.AttendeeId == attendeeId);
        if (existCheckin)
        {
            throw new ConflictException("Attendee can not do checking twice in the same event.");
        }
    }
}