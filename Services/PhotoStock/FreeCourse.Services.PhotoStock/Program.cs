var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

//.. resimleri d�� d�nyaya a��caz db yok :  resimler static dosyalar,  bunun i�in wwwroot klas�r� olu�turduk ,i�ine de photos
// bu middleware ile wwwroot i�indeki dosyalara d��ar�dan public olarak eri�ebiliriz.
app.UseStaticFiles();

//..

app.UseAuthorization();

app.MapControllers();

app.Run();
