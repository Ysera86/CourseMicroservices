using FreeCourse.Services.Basket.Services;
using FreeCourse.Services.Basket.Settings;
using FreeCourse.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//.

builder.Services.AddScoped<IBasketService, BasketService>();    

#region HttpContextAccessor : user Id eriþimi için : Context.User.Claims içinden UserId alýcam. Contexte mvc edpointlerde eriþim ama servis düzeyinde eriþmek için HttpContextAccessor  gerekli, onu da Shared içinde DI ile ctordan aabilmek için, bunu kullanan APIlerin servislerine DI geçmeliyim : BURADAKÝ GÝBÝ

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
