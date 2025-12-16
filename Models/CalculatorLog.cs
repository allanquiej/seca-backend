// Esta clase representa un registro del uso de una calculadora.

namespace SecaBackend.Models
{
    public class CalculatorLog
    {
        public int Id { get; set; }

        // Tipo de calculadora usada: "Indemnizacion", "Bono15", etc.
        public string TipoCalculadora { get; set; } = string.Empty;

        // Datos que el usuario ingresó (los podemos guardar como texto o JSON).
        public string DatosEntrada { get; set; } = string.Empty;

        // Resultado que devolvió la calculadora.
        public string Resultado { get; set; } = string.Empty;

        // Fecha y hora del uso.
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
