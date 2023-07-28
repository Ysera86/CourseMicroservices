using FreeCourse.Services.Basket.Services;
using FreeCourse.Services.Basket.Settings;
using FreeCourse.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//.

builder.Services.AddScoped<IBasketService, BasketService>();    

#region HttpContextAccessor : user Id eri�imi i�in : Context.User.Claims i�inden UserId al�cam. Contexte mvc edpointlerde eri�im ama servis d�zeyinde eri�mek i�in HttpContextAccessor  gerekli, onu da Shared i�inde DI ile ctordan aabilmek i�in, bunu kullanan APIlerin servislerine DI ge�meliyim : BURADAK� G�B�

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();

#endregion

#region OptionsPattern ile RedisSettings okuma

builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));

#endregion

builder.Services.AddSingleton<RedisService>(sp => 
{
    var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
    var redis = new RedisService(redisSettings.Host, redisSettings.Port);

    redis.Connect();

    return redis;
});

//..


builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
