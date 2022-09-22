using Microsoft.Extensions.FileProviders;
using TestTask.WebApi.DependencyInjection;
using TestTask.WebApi.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Bootstrapper.Register(builder.Services);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var staticFilesService = app.Services.GetRequiredService<IStaticFilesService>();

var cacheMaxAgeOneWeek = (60 * 60 * 24 * 7).ToString();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(staticFilesService.GetFilesDirectoryPath()),
    RequestPath = staticFilesService.GetFilesRequestPath(),
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
            "Cache-Control", $"public, max-age={cacheMaxAgeOneWeek}");
    }
});

app.MapControllers();

app.Run();