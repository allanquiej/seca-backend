namespace SecaBackend.Models
{
    // Datos que el usuario ingresa para calcular el Bono 14
    public class Bono14Input
{
    public decimal SalarioPromedio { get; set; }

    // NUEVO: rango de fechas
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}


    // Datos que devolvemos al frontend
    public class Bono14Result
    {
        public decimal MontoBono14 { get; set; }
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}
