using System;

namespace SecaBackend.Models
{
// Modelo que recibe el usuario desde el frontend
public class IndemnizacionInput
{
    // Salario mensual del trabajador
    public decimal SalarioMensual { get; set; }

    // NUEVO: rango de fechas
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}


    // Modelo que enviamos como respuesta al frontend
    public class IndemnizacionResult
    {
        // Monto total calculado de la indemnización
        public decimal MontoIndemnizacion { get; set; }

        // Descripción del cálculo
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}
