using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//..

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
{
    opts.Authority = builder.Configuration["IdentityServerURL"];
    opts.Audience = "resource_photo_stock";
    opts.RequireHttpsMetadata = false;
});

builder.Services.AddControllers(opts =>
{
    opts.Filters.Add(new AuthorizeFilter());    
});
//builder.Services.AddControllers();

//..

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//.. resimleri dýþ dünyaya açýcaz db yok :  resimler static dosyalar,  bunun için wwwroot klasörü oluþturduk ,içine de photos
// bu middleware ile wwwroot içindeki dosyalara dýþarýdan public olarak eriþebiliriz.
app.UseStaticFiles();


app.UseAuthentication();
//..

app.UseAuthorization();

app.MapControllers();

app.Run();
