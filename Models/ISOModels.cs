namespace SecaBackend.Models
{
    public class ISOInput
    {
        public decimal IngresosTrimestrales { get; set; }
    }

    public class ISOResult
    {
        public decimal ISOCalculado { get; set; }
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}
