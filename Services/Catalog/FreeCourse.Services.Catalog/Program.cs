using AutoMapper;
using FreeCourse.Services.Catalog.Mapping;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Services.Catalog.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//..
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseService, CourseService>();


#region JWT

// bu uygulamaya private key ile imzalanmýþ bir token gelecek ve bu da arkaplanda public key ile doðrulamasýný yapcak.
// public keyi de builder.Configuration["IdentityServerURL"] dan alacak. tokený aldýðý public key ile kontrol edecek  doðruysa ok dicek.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
{
    // tokený kim veriyor
    opts.Authority = builder.Configuration["IdentityServerURL"];

    // gelen token içindeki audience içinde bu deðer varsa "ok, istek yapabilirsin" dicek. 
    opts.Audience = "resource_catalog"; // IdentityServer.Config içinde tanýmlý, rastgele deðil. 


    // claim bazlý kimlik doðrulama olsaydý da scope a da bakacaktýk


    // builder.Configuration["IdentityServerURL"] https deðilse varsayýlan olarak https bekleme dedik
    opts.RequireHttpsMetadata = false;
});

#endregion

#region OptionsPattern ile DataBaseSettings okuma

/*-->> OptionsPattern
IOptions<DatabaseSettings> options

options.Value 

þeklinde kullanabilirdim ama classlarýnctoruna DI olarak IOptions geçmek yerine OptionsPattern ile DatabaseSettings ve IDatabaseSettings oluþturup okuyup DI olarak IDatabaseSettings geçebilirim artýk
 */
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

#endregion



#region Authorize 1 kere ekleyip tüm controllerlara etki etmesi için 

builder.Services.AddControllers(opts =>
{
    opts.Filters.Add(new AuthorizeFilter());
});

#endregion

//..

//builder.Services.AddControllers(); // > opt ekledik


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

//..
app.UseAuthentication();
//..

app.UseAuthorization();

app.MapControllers();

app.Run();
