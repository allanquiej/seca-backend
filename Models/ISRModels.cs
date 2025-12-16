namespace SecaBackend.Models
{
    public class ISRInput
    {
        public decimal SueldoMensual { get; set; }
    }

    public class ISRResult
    {
        public decimal ISRCalculado { get; set; }
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}
