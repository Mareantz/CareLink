using Application;
using Application.Utils;
using Infrastructure;
using Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Repositories;
using Infrastructure.Repositories;
using PredictiveHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

var AllowFrontend = "AllowFrontend";

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: AllowFrontend,
		builder =>
		{
			builder.WithOrigins("https://healthcaremanagement-fe.vercel.app","http://localhost:4200")
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
		});

});

builder.Services.AddAuthorization();

builder.Configuration["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("DefaultConnection");
builder.Configuration["ConnectionStrings:UserConnection"] = Environment.GetEnvironmentVariable("UserConnection");
builder.Configuration["Jwt:Key"] = Environment.GetEnvironmentVariable("Jwt__Key");
builder.Services.AddHealthChecks();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
	});
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IMedicalHistoryRepository, MedicalHistoryRepository>();

builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer <token>'",
		Name = "Authorization",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
	{
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	try
	{
		var context = services.GetRequiredService<ApplicationDbContext>();
		context.Database.Migrate();
		// Optionally, you can seed the database here if needed
		// DbSeeder.Seed(context);
	}
	catch (Exception ex)
	{
		// Log the error or handle it as per your requirements
		var logger = services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred while migrating or initializing the database.");
		// Optionally, rethrow or handle the exception
		throw;
	}
}
app.UseHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(AllowFrontend);
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
public partial class Program { }