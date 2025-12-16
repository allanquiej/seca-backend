// Este controlador existe solo para probar que mi API se puede conectar
// correctamente a la base de datos SecaDB en Azure SQL.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace SecaBackend.Controllers
{
    // Indico que esta clase es un controlador de tipo API.
    [ApiController]
    // Con esta ruta, la URL será: /api/dbtest
    // [controller] se reemplaza por el nombre de la clase sin "Controller",
    // es decir: "DbTest". Entonces la ruta queda: /api/dbtest
    [Route("api/[controller]")]
    public class DbTestController : ControllerBase
    {
        // Aquí guardo la configuración de la aplicación (incluye appsettings.json).
        private readonly IConfiguration _configuration;

        // ASP.NET Core inyecta IConfiguration automáticamente en el constructor.
        public DbTestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Este método responde a las peticiones GET que lleguen a /api/dbtest
        [HttpGet]
        public async Task<IActionResult> ProbarConexion()
        {
            // Leo la cadena de conexión que definí en appsettings.json bajo "SecaDb".
            string connectionString = _configuration.GetConnectionString("SecaDb");

            DateTime fechaDesdeSql;

            try
            {
                // Uso "using" para asegurar que la conexión se cierre correctamente.
                using (var connection = new SqlConnection(connectionString))
                {
                    // Abro la conexión con Azure SQL.
                    await connection.OpenAsync();

                    // Este comando le pide al servidor SQL su fecha/hora actual.
                    string query = "SELECT GETDATE();";

                    using (var command = new SqlCommand(query, connection))
                    {
                        // ExecuteScalarAsync ejecuta el SELECT y devuelve un solo valor.
                        object? resultado = await command.ExecuteScalarAsync();

                        fechaDesdeSql = Convert.ToDateTime(resultado);
                    }
                }

                // Si llegué hasta aquí, es porque la conexión fue exitosa.
                var respuestaOk = new
                {
                    exito = true,
                    mensaje = "Conexión a Azure SQL exitosa.",
                    fechaDesdeSql = fechaDesdeSql
                };

                return Ok(respuestaOk);
            }
            catch (Exception ex)
            {
                // Si hubo algún error de conexión, lo capturo aquí y lo devuelvo.
                var respuestaError = new
                {
                    exito = false,
                    mensaje = "Error al conectar con Azure SQL.",
                    detalle = ex.Message
                };

                return StatusCode(500, respuestaError);
            }
        }
    }
}
