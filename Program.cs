using System.Diagnostics;
using Azure.Storage.Blobs;
using GameStore.API.Authorization;
using GameStore.API.Cors;
using GameStore.API.Data;
using GameStore.API.Endpoints;
using GameStore.API.ErrorHandling;
using GameStore.API.ImageUpload;
using GameStore.API.Middleware;
using GameStore.API.OpenAPI;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRespositories(builder.Configuration);
builder.Services.AddAuthentication()
                .AddJwtBearer()
                .AddJwtBearer("Auth0");
builder.Services.AddGameStoreAuthorization();
builder.Services.AddApiVersioning(option => {
    option.DefaultApiVersion = new (1.0);
    option.AssumeDefaultVersionWhenUnspecified = true;
})
.AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");
// builder.Logging.AddJsonConsole(option =>
// {
//     option.JsonWriterOptions = new()
//     {
//         Indented = true
//     };
// });
builder.Services.AddGameStoreCors(builder.Configuration);
builder.Services.AddSwaggerGen()
                .AddTransient<IConfigureOptions<SwaggerGenOptions>,ConfigureSwaggerOptions>()
                .AddEndpointsApiExplorer();
builder.Services.AddSingleton<IImageUploader>(
    new ImageUploader(
        new BlobContainerClient(
            builder.Configuration.GetConnectionString("AzureStorage"),
            "images"
            )
    )
);
builder.Logging.AddAzureWebAppDiagnostics();
var app = builder.Build();
app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.ConfigureExceptionHandler());
app.UseMiddleware<RequestTimingMiddleware>();
await app.Services.InitializeDbAsync();
app.UseHttpLogging();
app.MapGamesEndpoints();
app.MapImagesEndpoints();
app.UseCors();
app.UseGameStoreSwagger();
app.Run();
