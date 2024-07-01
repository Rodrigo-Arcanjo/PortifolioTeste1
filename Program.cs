using Microsoft.OpenApi.Models;
using PortifolioTeste1.Controllers;
using PortifolioTeste1.Models;
using PortifolioTeste1.Security;

var builder = WebApplication.CreateBuilder(args);

//TALVEZ PRECISE COMENTAR LINHA ABAIXO
builder.Services.AddTransient<TokenService>();

// Add services to the container.

//var connectionString = builder.Configuration.GetConnectionString("DevEventsCs");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Protótipo API ASP.NET Core 7",
        Version = "v1",
        Contact = new OpenApiContact 
        {
            Name = "Rodrigo Arcanjo",
            Email = "rdarcanjo@hotmail.com",
            Url = new Uri("https://www.linkedin.com/in/rodrigo-arcanjo-9b211b2a2/")
        }
    });

    var xmlPath = Path.Combine(System.AppContext.BaseDirectory, "GrupoRecursosApiSimplificaMobile.xml");
    s.IncludeXmlComments(xmlPath);
});

//builder.Services.AddAuthentication().AddJwtBearer();


var app = builder.Build();

/*app.MapGet("/", (TokenService service) => service.Generate(
        new User("99901", "1234", "Rodarc", "1234567890987656453", "sucesso", 
            new[] {"dev", "mobile"}
        )));*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //  Permite acesso ao Swagger para ver documentação e realizar testes apenas rodando na máquina, ou seja, no modo desenvolvedor como especificado no 'if'
    app.UseSwagger(); 
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
