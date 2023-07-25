using AutoMapper;
using FreeCourse.Services.Catalog.Mapping;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Services.Catalog.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//..
builder.Services.AddScoped<ICategoryService,CategoryService>();

/*
 * 
-->> OptionsPattern
IOptions<DatabaseSettings> options

options.Value 

þeklinde kullanabilirdim ama classlarýnctoruna DI olarak IOptions geçmek yerine OptionsPattern ile DatabaseSettings ve IDatabaseSettings oluþturup okuyup DI olarak IDatabaseSettings geçebilirim artýk
 */
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});


//..

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
