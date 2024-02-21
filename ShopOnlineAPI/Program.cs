using log4net.Config;
using Microsoft.EntityFrameworkCore;
using ShopOnlineAPI.Data;
using ShopOnlineAPI.Utils;

var builder = WebApplication.CreateBuilder(args);
IConfiguration config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

XmlConfigurator.Configure(new FileInfo(builder.Environment.IsDevelopment() ? "log4net.dev.config" : "log4net.config"));
log4net.Util.LogLog.InternalDebugging = true;

string databaseDriver = builder.Configuration["Database:Provider"];
switch (databaseDriver)
{
    case ConstantApi.DatabaseDriver.SqlServer: { builder.Services.AddDbContext<ApplicationDbContext, SqlDbContext>(); } break;
    default: { throw new Exception("Database driver not supported"); } break;
    // se puede implementar otros Contexts de diferentes bases de datos.
}

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddDbContext<ApplicationDbContext>();

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
app.UseCors("MyCorsPolicy");

//app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
    catch (Exception e)
    {
        app.Logger.LogError("Error running migrations. {0}. {1}", e.Message, e.StackTrace);
    }

}

app.Run();
