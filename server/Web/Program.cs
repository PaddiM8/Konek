using Konek.Server.Core;
using Konek.Server.Core.Bridges;
using Konek.Server.Web.Serialization;

//var bridge = await BridgeSetup.FindHueBridge();
var loggerFactory = LoggerFactory.Create(builder => {
        builder.AddConsole();
    }
);
var bridge = new LoggingBridge(loggerFactory.CreateLogger("Bridge"));
var hub = new DeviceHub(bridge);
await hub.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter()));
builder.Services.AddSingleton(_ => hub);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();