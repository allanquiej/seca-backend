namespace SecaBackend.Models
{
    // ===============================================
    // MODELOS VIEJOS (mantener para compatibilidad)
    // ===============================================
    
    public class ISRTrimestralInput
    {
        public decimal IngresosTrimestrales { get; set; }
    }

    public class ISRTrimestralResult
    {
        public decimal ISRCalculado { get; set; }
        public string DetalleCalculo { get; set; } = string.Empty;
    }

    // ===============================================
    // 🆕 MODELOS NUEVOS - ISR TRIMESTRAL V2 (CORRECTO)
    // ===============================================
    
    public class ISRTrimestralV2Input
    {
        // Para Opción 1 (Acumulado)
        public decimal VentasAcumuladas { get; set; }
        public decimal GastosAcumulados { get; set; }
        
        // Para Opción 2 (Solo trimestre)
        public decimal VentasTrimestre { get; set; }
        
        // Común para ambas opciones
        public decimal ISOPendiente { get; set; }
        
        // Tipo de cálculo
        public bool UsarOpcionAcumulada { get; set; } // true = Opción 1, false = Opción 2
    }
    
    public class ISRTrimestralV2Result
    {
        public string OpcionUtilizada { get; set; } = string.Empty; // "Opción 1 - Acumulado" o "Opción 2 - Trimestre"
        public decimal BaseCalculo { get; set; } // Ventas - Gastos (Opción 1) o Ventas (Opción 2)
        public decimal ISRCalculado { get; set; } // Base × 25%
        public decimal ISOAcreditar { get; set; }
        public decimal ISRAPagar { get; set; } // ISR - ISO
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}