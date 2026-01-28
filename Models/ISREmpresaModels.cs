namespace SecaBackend.Models
{
    // ===============================================
    // MODELOS VIEJOS (mantener para compatibilidad)
    // ===============================================
    
    public class ISREmpresaInput
    {
        public decimal IngresosMensuales { get; set; }
    }

    public class ISREmpresaResult
    {
        public decimal ISRCalculado { get; set; }
        public string DetalleCalculo { get; set; } = string.Empty;
    }

    // ===============================================
    // 🆕 MODELOS NUEVOS - ISR EMPRESA MENSUAL V2 (CORRECTO)
    // ===============================================
    
    public class ISREmpresaMensualV2Input
    {
        public decimal TotalFacturacionMes { get; set; }
        public decimal TotalRetenciones { get; set; } // Puede ser 0
    }
    
    public class ISREmpresaMensualV2Result
    {
        public decimal Base { get; set; } // Total / 1.12
        public decimal IVA { get; set; } // Base × 0.12
        public decimal ISRPrimerosTreintaMil { get; set; } // 5% sobre primeros Q30k
        public decimal ISRExcedente { get; set; } // 7% sobre excedente
        public decimal ISRTotal { get; set; }
        public decimal ISRAPagar { get; set; } // ISRTotal - Retenciones
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}