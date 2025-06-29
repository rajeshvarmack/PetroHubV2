using PetroHub.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// Configure the request pipeline
app.ConfigureApiPipeline();

app.Run();
