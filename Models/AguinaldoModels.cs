namespace SecaBackend.Models
{
    public class AguinaldoInput
    {
        public decimal SalarioPromedio { get; set; }
        public decimal MesesTrabajados { get; set; }  // Máximo 12
    }

    public class AguinaldoResult
    {
        public decimal MontoAguinaldo { get; set; }
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}
