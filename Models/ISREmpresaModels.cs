namespace SecaBackend.Models
{
    public class ISREmpresaInput
    {
        public decimal IngresosMensuales { get; set; }
    }

    public class ISREmpresaResult
    {
        public decimal ISRCalculado { get; set; }
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}
