using Application;
using Application.Utils;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

var AllowFrontend = "AllowFrontend";

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: AllowFrontend,
		builder =>
		{
			builder.WithOrigins("https://healthcaremanagement-fe.vercel.app")
				.AllowAnyHeader()
				.AllowAnyMethod();
		});

});

builder.Services.AddHealthChecks();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.MapControllers();
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseStaticFiles();
app.UseRouting();
await app.RunAsync();