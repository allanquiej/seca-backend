namespace SecaBackend.Models
{
    // Enum para los tipos de regimen de IVA
    public enum RegimenIVA
    {
        General = 1,           // Regimen General (12%)
        PequenoContribuyente = 2,  // Pequeno Contribuyente (Cuota fija)
        Exento = 3            // Exento de IVA
    }

    // ===============================================
    // ENTRADA: Calculadora de IVA
    // ACTUALIZADO: 3 campos de deducciones separados
    // ===============================================
    public class IVAInput
    {
        // Tipo de regimen
        public RegimenIVA Regimen { get; set; }
        
        // Para Regimen General
        public decimal VentasMes { get; set; }      // Total ventas con IVA incluido
        public decimal ComprasMes { get; set; }     // Total compras con IVA incluido
        
        // ACTUALIZADO: 3 deducciones separadas
        public decimal IVACredito { get; set; }     // IVA credito del mes anterior
        public decimal IVARetenido { get; set; }    // Retenciones que te hicieron
        public decimal IVAExento { get; set; }      // IVA de ventas exentas
        
        // Para Pequeno Contribuyente
        public decimal IngresosMensuales { get; set; }  // Para validar si aplica
        public decimal IngresosAnuales { get; set; }    // Para validar si aplica (≤ Q150,000)
    }

    // ===============================================
    // SALIDA: Calculadora de IVA
    // ACTUALIZADO: Detalle de deducciones
    // ===============================================
    public class IVAResult
    {
        public string RegimenNombre { get; set; } = string.Empty;
        
        // Para Regimen General
        public decimal BaseVentas { get; set; }       // Base de ventas sin IVA
        public decimal BaseCompras { get; set; }      // Base de compras sin IVA
        public decimal DebitoFiscal { get; set; }     // IVA en ventas
        public decimal CreditoFiscal { get; set; }    // IVA en compras
        public decimal IVABruto { get; set; }         // Debito - Credito
        
        // ACTUALIZADO: Deducciones separadas
        public decimal IVACredito { get; set; }       // IVA credito del mes anterior
        public decimal IVARetenido { get; set; }      // Retenciones
        public decimal IVAExento { get; set; }        // IVA exento
        public decimal TotalDeducciones { get; set; } // Suma de las 3 deducciones
        
        public decimal IVAAPagar { get; set; }        // IVA Bruto - Deducciones
        
        // Para Pequeno Contribuyente
        public decimal CuotaFija { get; set; }        // Q150
        
        // Para todos
        public bool Aplica { get; set; }              // Si el regimen aplica
        public string Mensaje { get; set; } = string.Empty;
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}