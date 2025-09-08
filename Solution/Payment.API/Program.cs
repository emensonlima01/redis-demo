var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddDatabaseServices(builder.Configuration)
    .AddBusinessServices()
    .AddExternalServices(builder.Configuration);

var app = builder.Build();

app.ConfigurePipeline()
   .ConfigureMiddlewares()
   .ConfigureEndpoints();

app.Run();
