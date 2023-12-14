using Spark.Library.Environment;
using Spark.Library.Config;
using RoomCoder.Application.Startup;
using RoomCoder.Application.Database;
using Vite.AspNetCore.Extensions;
using Microsoft.Extensions.FileProviders;

EnvManager.LoadConfig();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetupSparkConfig();

// Add all services to the container.
builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
	// Do something only in dev environments
	app.UseViteDevMiddleware();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Services.RegisterScheduledJobs();
app.Services.RegisterEvents();

app.Run();
