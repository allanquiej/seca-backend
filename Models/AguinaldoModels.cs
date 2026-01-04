namespace SecaBackend.Models
{
public class AguinaldoInput
{
    public decimal SalarioPromedio { get; set; }

    // NUEVO: rango de fechas
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}

    public class AguinaldoResult
    {
        public decimal MontoAguinaldo { get; set; }
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}
