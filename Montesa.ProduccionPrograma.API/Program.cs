using Microsoft.EntityFrameworkCore;
using Montesa.ProduccionPrograma.API.Data;
using Montesa.ProduccionPrograma.API.Services;

var builder = WebApplication.CreateBuilder(args);
const string CorsPolicyName = "AllowViteReact";
builder.Services.AddCors(options =>
{
  
    options.AddPolicy("AllowViteReact",
      b => b
          .WithOrigins("http://localhost:5173")
          .AllowAnyHeader()
          .AllowAnyMethod()
  );
});
// ---------- fin CORS ----------
builder.Services.AddControllers();
/*Desde aqui voy a llamar la conexion
builder.Services.AddDbContext<ProduccionDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConexion")));
*/
//Agrega un factory de conexiones para Dapper
builder.Services.AddSingleton<ISqlConnectionFactory, DapperContext>();
builder.Services.AddScoped<ICargarProgramaService, CargarProgramaService>();
builder.Services.AddScoped<IProdProgramaSpRepository, ProdProgramaSpRepository>();
builder.Services.AddScoped<IProgramaCargaRepository, ProgramaCargaRepository>();
builder.Services.AddScoped<IAsignacionMaquinaService, AsignacionMaquinaService>();
builder.Services.AddScoped<IProdMaquinaRepository, ProdMaquinaRepository>();


builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7253/api/");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DapperContext>();


var app = builder.Build();
app.UseCors(CorsPolicyName);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
