// En este archivo defino la configuración principal de mi API.
// Aquí se inicia el servidor web y se registran los servicios que voy a usar.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecaBackend.Data;
using SecaBackend.Models;

var builder = WebApplication.CreateBuilder(args);

// =========================
// 1) REGISTRO DE SERVICIOS
// =========================

// Con esta línea agrego soporte para "Controllers".
// Los controllers son clases donde voy a escribir las rutas de mi API,
// por ejemplo: /api/status, /api/calculadoras, /api/chatbot, etc.
builder.Services.AddControllers();

// Aquí configuro CORS.
// CORS evita que el navegador bloquee las peticiones desde mi frontend (Vercel)
// hacia mi backend (Railway).
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPermitido", policyBuilder =>
    {
        // IMPORTANTE:
        // Más adelante, cuando tenga el dominio real del frontend en Vercel,
        // debo colocar ese dominio aquí en lugar de "https://mi-dominio-vercel.com".
        // Por ahora dejo localhost para trabajar en mi computadora.
        policyBuilder
            .WithOrigins(
                "https://mi-dominio-vercel.com",  // dominio real del frontend en producción
                "http://localhost:5173"           // frontend en desarrollo con Vite
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Aquí registro el DbContext de Entity Framework Core.
// Le digo que use SQL Server y que tome la cadena de conexión llamada "SecaDb"
// desde el archivo appsettings.json.
builder.Services.AddDbContext<SecaDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SecaDb"));
});


// Más adelante, aquí también registraré:
// - El DbContext para la base de datos en Azure SQL
// - Servicios de negocio (calculadoras, chatbot, etc.)

var app = builder.Build();

// =========================
// 2) CONFIGURACIÓN DEL PIPELINE HTTP
// =========================

// Esta línea redirige el tráfico HTTP a HTTPS en desarrollo.
// Es una buena práctica para que la comunicación sea más segura.
app.UseHttpsRedirection();

// Activo CORS con la política que definí arriba.
// Esto permite que mi frontend pueda consumir esta API sin ser bloqueado.
app.UseCors("CorsPermitido");

// Aquí se manejará la autorización.
// Más adelante, cuando tenga roles y usuarios, esta parte será importante.
// Por ahora, simplemente la dejo activada aunque no tenga autenticación configurada.
app.UseAuthorization();

// Esta línea conecta automáticamente los controladores que yo cree
// con las rutas de la API (por ejemplo: /api/status).
app.MapControllers();

// Este endpoint simple es como un "latido" del servidor.
// Sirve para comprobar que la API está viva desde un navegador o desde el frontend.
app.MapGet("/", () => "API de SECA funcionando correctamente.");

// Finalmente, aquí arranca la aplicación.
app.Run();
