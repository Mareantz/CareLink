using Application;
using Application.Utils;
using Infrastructure;
using Identity;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
//var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
//builder.WebHost.UseUrls($"http://*:{port}");

var AllowFrontend = "AllowFrontend";

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: AllowFrontend,
		builder =>
		{
			builder.WithOrigins("https://healthcaremanagement-fe.vercel.app","http://localhost:4200")
				.AllowAnyHeader()
				.AllowAnyMethod();
		});

});

//builder.Configuration["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("DefaultConnection");


//builder.Services.AddHealthChecks();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
	});
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.UseHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors(AllowFrontend);
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();
