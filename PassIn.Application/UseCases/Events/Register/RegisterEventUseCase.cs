using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Register;

public interface IRegisterEventUseCase
{
    Task<ResponseEventJson> ExecuteAsync(RequestEventJson request);
}

public class RegisterEventUseCase(PassInDbContext dbContext) : IRegisterEventUseCase
{
    private readonly PassInDbContext _dbContext = dbContext;

    public async Task<ResponseEventJson> ExecuteAsync(RequestEventJson request)
    {
        Validate(request);

        var entity = new Infrastructure.Entities.Event
        {
            Title = request.Title,
            Details = request.Details,
            MaximumAttendees = request.MaximumAttendees,
            Slug = request.Title,
        };

        await _dbContext.Events.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return new ResponseEventJson
        {
            Id = entity.Id
        };
    }

    private static void Validate(RequestEventJson request)
    {
        if (request.MaximumAttendees <= 0)
        {
            throw new ErrorOnValidationException("The Maximum attendes is invalid.");
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ErrorOnValidationException("The title is invalid.");
        }

        if (string.IsNullOrWhiteSpace(request.Details))
        {
            throw new ErrorOnValidationException("The title is invalid.");
        }
    }
}