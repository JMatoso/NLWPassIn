using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PassIn.Application.UseCases.Attendees.GetAllByEventId;
using PassIn.Application.UseCases.Checkins.DoCheckin;
using PassIn.Application.UseCases.Events.GetAll;
using PassIn.Application.UseCases.Events.Register;
using PassIn.Application.UseCases.Events.RegisterAttendee;
using PassIn.Infrastructure;
using System.Reflection;

namespace PassIn.Application.Configure;

public static class Configurations
{
    public static IServiceCollection AddPassInConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlite<PassInDbContext>(configuration.GetConnectionString("DefaultConnection"), options =>
        {
            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        });

        services.AddScoped<IGetAllEventsUseCase, GetAllEventsUseCase>();
        services.AddScoped<IGetEventByIdUseCase, GetEventByIdUseCase>();
        services.AddScoped<IRegisterEventUseCase, RegisterEventUseCase>();
        services.AddScoped<IDoAttendeeCheckinUseCase, DoAttendeeCheckinUseCase>();
        services.AddScoped<IRegisterAttendeeOnEventUseCase, RegisterAttendeeOnEventUseCase>();
        services.AddScoped<IGetAllAttendeesByEventIdUseCase, GetAllAttendeesByEventIdUseCase>();

        return services;
    }
}
