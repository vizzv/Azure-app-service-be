using System.Text.Json.Serialization;
using WebApi.Helpers;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;
 
    services.AddDbContext<DataContext>();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params (e.g. User update)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // configure DI for application services
    services.AddScoped<IUserService, UserService>();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApp", builder =>
    {
        builder.WithOrigins("https://green-flower-0dfce0b0f.1.azurestaticapps.net")
               .AllowAnyMethod()
               .AllowAnyHeader();
               .AllowCredentials();
    });
});
}

var app = builder.Build();

// configure HTTP request pipeline
{
    // global cors policy
app.UseCors("AllowFrontendApp");

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();
}

app.Run();
