using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Register;

public interface IGetEventByIdUseCase
{
    Task<ResponseEventJson> ExecuteAsync(Guid id);
}

public class GetEventByIdUseCase(PassInDbContext passInDbContext) : IGetEventByIdUseCase
{
    private readonly PassInDbContext _dbContext = passInDbContext;

    public async Task<ResponseEventJson> ExecuteAsync(Guid id)
    {
        var entity = await _dbContext.Events.Include(x => x.Attendees).FirstOrDefaultAsync(x => x.Id == id);

        return entity is null ?
            throw new NotFoundException("An event with this id doesn't exist.") :
            new ResponseEventJson
            {
                Id = entity.Id,
                Slug = entity.Slug,
                Title = entity.Title,
                Details = entity.Details,
                MaximumAttendees = entity.MaximumAttendees,
                AttendeesAmount = entity.Attendees.Count,
                Attendees = entity.Attendees.Select(x => new ResponseAttendeeJson
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    CreatedAt = x.CreatedAt,
                }).ToList(),
            };
    }
}