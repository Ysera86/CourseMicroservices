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

// bu uygulamaya private key ile imzalanm�� bir token gelecek ve bu da arkaplanda public key ile do�rulamas�n� yapcak.
// public keyi de builder.Configuration["IdentityServerURL"] dan alacak. token� ald��� public key ile kontrol edecek  do�ruysa ok dicek.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
{
    // token� kim veriyor
    opts.Authority = builder.Configuration["IdentityServerURL"];

    // gelen token i�indeki audience i�inde bu de�er varsa "ok, istek yapabilirsin" dicek. 
    opts.Audience = "resource_catalog"; // IdentityServer.Config i�inde tan�ml�, rastgele de�il. 


    // claim bazl� kimlik do�rulama olsayd� da scope a da bakacakt�k


    // builder.Configuration["IdentityServerURL"] https de�ilse varsay�lan olarak https bekleme dedik
    opts.RequireHttpsMetadata = false;
});

#endregion

#region OptionsPattern ile DataBaseSettings okuma

/*-->> OptionsPattern
IOptions<DatabaseSettings> options

options.Value 

�eklinde kullanabilirdim ama classlar�nctoruna DI olarak IOptions ge�mek yerine OptionsPattern ile DatabaseSettings ve IDatabaseSettings olu�turup okuyup DI olarak IDatabaseSettings ge�ebilirim art�k
 */
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

#endregion



#region Authorize 1 kere ekleyip t�m controllerlara etki etmesi i�in 

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
