// En este archivo creo un controlador llamado "StatusController".
// Los controladores son las clases que responden a las peticiones HTTP (GET, POST, etc.)

using Microsoft.AspNetCore.Mvc;

namespace SecaBackend.Controllers
{
    // Este atributo indica que esta clase es un controlador de tipo API.
    [ApiController]
    // Esta ruta significa que la URL para este controlador será: /api/status
    // porque ASP.NET reemplaza [controller] por el nombre de la clase sin la palabra "Controller".
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        // Esta acción responde a las peticiones GET que lleguen a /api/status
        [HttpGet]
        public IActionResult GetStatus()
        {
            // Aquí armo un objeto con información básica del estado de la API.
            // Más adelante puedo añadir más datos si lo necesito.
            var respuesta = new
            {
                mensaje = "Hola, esta es la API de SECA",
                version = "1.0.0",
                fechaServidor = DateTime.Now
            };

            // Ok(...) devuelve código HTTP 200 (éxito) junto con el contenido.
            return Ok(respuesta);
        }
    }
}
