namespace SecaBackend.Models
{
    public class ISRTrimestralInput
    {
        public decimal IngresosTrimestrales { get; set; }
    }

    public class ISRTrimestralResult
    {
        public decimal ISRCalculado { get; set; }
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}
