using FreeCourse.Services.Basket.Services;
using FreeCourse.Services.Basket.Settings;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//.

builder.Services.AddScoped<IBasketService, BasketService>();


#region JWT Protection 

var requiredAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); // i�inde jwt.io gibi biyeren bak�nca sub diye g�r�n�rken user.claims i�inde bu claim "" diye geliyor, o nedenle maplee ki sub diye okuyabileyim dedik

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerURL"];
    opt.Audience = "resource_basket";
    opt.RequireHttpsMetadata = false;
});

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requiredAuthorizePolicy));
});

#endregion

#region HttpContextAccessor : user Id eri�imi i�in : Context.User.Claims i�inden UserId al�cam. Contexte mvc edpointlerde eri�im ama servis d�zeyinde eri�mek i�in HttpContextAccessor  gerekli, onu da Shared i�inde DI ile ctordan aabilmek i�in, bunu kullanan APIlerin servislerine DI ge�meliyim : BURADAK� G�B�

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();

#endregion

#region OptionsPattern ile RedisSettings okuma

builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));

builder.Services.AddSingleton<RedisService>(sp => 
{
    var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
    var redis = new RedisService(redisSettings.Host, redisSettings.Port);

    redis.Connect();

    return redis;
});

#endregion


//..

//centralize authorization
//builder.Services.AddControllers();


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

//..

app.UseAuthentication();

//..

app.UseAuthorization();

app.MapControllers();

app.Run();
