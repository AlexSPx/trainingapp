using TrainingAppAPI.Middlewares;
using TrainingAppAPI.Models;
using TrainingAppAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISessionService, SessionService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//User-protected routes;
app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api/auth"),
    appBuilder => appBuilder.UseMiddleware<WithAuth>()
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
