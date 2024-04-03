using Microsoft.OpenApi.Models;
using PassIn.Api.Filters;
using PassIn.Application.Configure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc($"v1", new OpenApiInfo
    {
        Title = "PassIn",
        Version = $"v1",
        Description = "Back-end application in C# developed during Rocketseat's NLW Unite. For managing participants at an in-person event.",
        Contact = new OpenApiContact
        {
            Name = "PassIn",
            Email = "joaquimjose@duck.com",
            Url = new Uri("https://github.com/JMatoso/NLWPassIn")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://github.com/JMatoso/NLWPassIn/blob/master/LICENSE.txt")
        },
        TermsOfService = new Uri("https://github.com/JMatoso/NLWPassIn")
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    config.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddPassInConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
