


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("https://localhost:7226");
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("HelpdeskCors", policy =>
    {
        policy.WithOrigins(
            "https://helpdesk-web-ere2edcnh8grd6ab.germanywestcentral-01.azurewebsites.net",
            "https://localhost:7226"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger immer aktivieren
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("HelpdeskCors");
app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapControllers();

app.Run();