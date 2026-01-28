namespace SecaBackend.Models
{
    // Enum para los tipos de régimen de IVA
    public enum RegimenIVA
    {
        General = 1,           // Régimen General (12%)
        PequenoContribuyente = 2,  // Pequeño Contribuyente (Cuota fija)
        Exento = 3            // Exento de IVA
    }

    // ===============================================
    // ENTRADA: Calculadora de IVA
    // ===============================================
    public class IVAInput
    {
        // Tipo de régimen
        public RegimenIVA Regimen { get; set; }
        
        // Para Régimen General
        public decimal VentasMes { get; set; }      // Total ventas con IVA incluido
        public decimal ComprasMes { get; set; }     // Total compras con IVA incluido
        public decimal Retenciones { get; set; }    // Retenciones de IVA
        
        // Para Pequeño Contribuyente
        public decimal IngresosMensuales { get; set; }  // Para validar si aplica
        public decimal IngresosAnuales { get; set; }    // Para validar si aplica (≤ Q150,000)
    }

    // ===============================================
    // SALIDA: Calculadora de IVA
    // ===============================================
    public class IVAResult
    {
        public string RegimenNombre { get; set; } = string.Empty;
        
        // Para Régimen General
        public decimal DebitoFiscal { get; set; }     // IVA en ventas
        public decimal CreditoFiscal { get; set; }    // IVA en compras
        public decimal IVABruto { get; set; }         // Débito - Crédito
        public decimal IVAAPagar { get; set; }        // IVA Bruto - Retenciones
        
        // Para Pequeño Contribuyente
        public decimal CuotaFija { get; set; }        // Q150
        
        // Para todos
        public bool Aplica { get; set; }              // Si el régimen aplica
        public string Mensaje { get; set; } = string.Empty;
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}