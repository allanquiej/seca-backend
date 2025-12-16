// Este controlador lo uso solo para comprobar que Entity Framework Core
// está bien configurado y puede leer y escribir en la base de datos SecaDB.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecaBackend.Data;
using SecaBackend.Models;

namespace SecaBackend.Controllers
{
    [ApiController]
    // La ruta final será: /api/eftest
    [Route("api/[controller]")]
    public class EfTestController : ControllerBase
    {
        // Aquí guardo una referencia al DbContext que configuré en Program.cs.
        private readonly SecaDbContext _context;

        // El DbContext se inyecta automáticamente gracias a la configuración de servicios.
        public EfTestController(SecaDbContext context)
        {
            _context = context;
        }

        // GET: /api/eftest
        // Este método cuenta cuántos registros hay en cada tabla para comprobar
        // que EF Core puede consultar la base de datos sin problemas.
        [HttpGet]
        public async Task<IActionResult> ProbarEfCore()
        {
            int totalChatLogs = await _context.ChatLogs.CountAsync();
            int totalCalculatorLogs = await _context.CalculatorLogs.CountAsync();
            int totalLeads = await _context.Leads.CountAsync();

            var respuesta = new
            {
                exito = true,
                mensaje = "EF Core está conectado y puede leer las tablas.",
                totales = new
                {
                    chatLogs = totalChatLogs,
                    calculatorLogs = totalCalculatorLogs,
                    leads = totalLeads
                }
            };

            return Ok(respuesta);
        }

        // POST: /api/eftest/insertar-prueba
        // Este método inserta un registro de prueba en ChatLogs
        // para verificar que EF Core también puede ESCRIBIR en la base.
        [HttpPost("insertar-prueba")]
        public async Task<IActionResult> InsertarRegistroPrueba()
        {
            var nuevoChat = new ChatLog
            {
                Usuario = "prueba-efcore",
                Pregunta = "¿Esta es una prueba de EF Core?",
                Respuesta = "Sí, la inserción funcionó correctamente.",
                Fecha = DateTime.Now
            };

            _context.ChatLogs.Add(nuevoChat);
            await _context.SaveChangesAsync();

            var respuesta = new
            {
                exito = true,
                mensaje = "Se insertó un registro de prueba en ChatLogs.",
                idGenerado = nuevoChat.Id
            };

            return Ok(respuesta);
        }
    }
}
