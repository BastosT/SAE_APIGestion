using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.DataManger;
using SAE_APIGestion.Models.EntityFramework;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GlobalDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GlobalDBContext")));

// evite l'erreur de cycle de références 
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// data repo
builder.Services.AddScoped<IDataRepository<Batiment>, BatimentManager>();
builder.Services.AddScoped<IDataRepository<Capteur>, CapteurManager>();
builder.Services.AddScoped<IDataRepository<TypeDonneesCapteur>, TypeDonneesCapteurManager>();
builder.Services.AddScoped<IDataRepository<DonneesCapteur>, DonneesCapteurManager>();
builder.Services.AddScoped<IDataRepository<Equipement>, EquipementManager>();
builder.Services.AddScoped<IDataRepository<Mur>, MurManager>();
builder.Services.AddScoped<IDataRepository<Salle>, SalleManager>();
builder.Services.AddScoped<IDataRepository<TypeEquipement>, TypeEquipementManager>();
builder.Services.AddScoped<IDataRepository<TypeSalle>, TypeSalleManager>();

//cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
