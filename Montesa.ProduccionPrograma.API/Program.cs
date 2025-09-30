using Microsoft.EntityFrameworkCore;
using Montesa.ProduccionPrograma.API.Data;



var builder = WebApplication.CreateBuilder(args);

const string MyCors = "FrontLocal";

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyCors, policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000",
                "https://localhost:3000",
                "http://127.0.0.1:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
        // .AllowCredentials(); // solo si usarás cookies/credenciales
    });
});
// ---------- fin CORS ----------

// Add services to the container.

builder.Services.AddControllers();
//Desde aqui voy a llamar la conexion
builder.Services.AddDbContext<ProduccionDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConexion")));

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7253/api/");
});

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

app.UseHttpsRedirection();
// CORS debe ir antes de Authorization y antes de MapControllers
app.UseCors(MyCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
