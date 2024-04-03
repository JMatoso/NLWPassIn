using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Infrastructure;
namespace PassIn.Application.UseCases.Events.GetAll;

public interface IGetAllEventsUseCase
{
    Task<List<ResponseEventJson>> ExecuteAsync(string? searchKey = "");
}

public class GetAllEventsUseCase(PassInDbContext passInDbContext) : IGetAllEventsUseCase
{
    private readonly PassInDbContext _dbContext = passInDbContext;

    public async Task<List<ResponseEventJson>> ExecuteAsync(string? searchKey = "")
    {
        var query = _dbContext.Events.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchKey))
        {
            query = query.Where(e => e.Title.ToLower().Contains(searchKey.ToLower()) || 
                                     e.Details.ToLower().Contains(searchKey.ToLower()));
        }

        query = query.Include(e => e.Attendees);

        return await query.Select(e => new ResponseEventJson
        {
            Id = e.Id,
            Slug = e.Slug,
            Title = e.Title,
            Details = e.Details,
            MaximumAttendees = e.MaximumAttendees,
            AttendeesAmount = e.Attendees.Count
        }).ToListAsync();
    }
}
