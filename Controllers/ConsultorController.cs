using Microsoft.AspNetCore.Mvc;
using SecaBackend.Data;
using SecaBackend.Models;

namespace SecaBackend.Controllers
{
    [ApiController]
    [Route("api/consultor")]
    public class ConsultorController : ControllerBase
    {
        private readonly SecaDbContext _context;

        public ConsultorController(SecaDbContext context)
        {
            _context = context;
        }

        // Diccionario de preguntas rápidas y respuestas inmediatas
        private readonly Dictionary<string, string> respuestas = new()
        {
            { "que es iva", "El IVA (Impuesto al Valor Agregado) es un impuesto del 12% aplicado a la venta de bienes y servicios en Guatemala." },
            { "que es isr", "El ISR (Impuesto Sobre la Renta) es un impuesto que grava los ingresos de personas y empresas." },
            { "que es iso", "El ISO (Impuesto de Solidaridad) es un impuesto trimestral equivalente al 1% de los ingresos brutos o activo neto." },

            { "como se inscribe una empresa", "Para inscribir una empresa en Guatemala se debe registrar en la SAT, obtener RTU, NIT y cumplir con los requisitos legales correspondientes." },
            { "requisitos para inscribirse", "Requisitos: DPI, dirección fiscal, número de teléfono, correo electrónico y actividad económica declarada." },

            { "crear una sociedad", "Para crear una sociedad se requiere escritura pública, nombramiento de representante legal, inscripción en SAT y Registro Mercantil." },
            { "crear una sociedad en emprendimiento", "El régimen de emprendimiento permite crear una empresa simplificada con requisitos mínimos para nuevos negocios." },
            { "crear una ong", "Para crear una ONG se necesita escritura pública, estatutos, nombramiento de junta directiva e inscripción en el Registro de Personas Jurídicas." },

            { "que regimen me conviene", "Depende de tus ingresos: Pequeño Contribuyente (menos de Q150,000 al año) o Régimen sobre utilidades para mayores ingresos." },

            { "obligaciones al inscribir una empresa", "Incluyen: emitir facturas, llevar contabilidad, presentar declaraciones y cumplir con impuestos según régimen." },

            { "obligaciones tributarias pequeño contribuyente", "Pagar el 5% mensual del total facturado y emitir facturas debidamente." },

            { "obligaciones para negocios", "Toda empresa debe declarar impuestos, llevar registros contables y estar al día en obligaciones con SAT." }
        };

        // Endpoint principal del consultor
        [HttpPost("preguntar")]
        public async Task<IActionResult> Preguntar([FromBody] ConsultorInput input)
        {
            string preguntaNormalizada = input.Pregunta.ToLower().Trim();

            // Buscar coincidencia exacta en el diccionario
            string respuesta = respuestas
                .FirstOrDefault(x => preguntaNormalizada.Contains(x.Key)).Value 
                ?? "Lo siento, no tengo información exacta sobre esa pregunta.";

            // Guardar log
            var log = new ChatLog
            {
                Usuario = "consultor-web",
                Pregunta = input.Pregunta,
                Respuesta = respuesta,
                Fecha = DateTime.Now
            };

            _context.ChatLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new ConsultorRespuesta
            {
                PreguntaOriginal = input.Pregunta,
                Respuesta = respuesta
            });
        }
    }
}
