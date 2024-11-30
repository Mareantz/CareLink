using Application;
using Application.Utils;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSepeficOrigins= "MyAllowSepeficOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSepeficOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseRouting();
app.UseCors("MyAllowSepeficOrigins");
app.UseHttpsRedirection();
app.MapControllers();
await app.RunAsync();