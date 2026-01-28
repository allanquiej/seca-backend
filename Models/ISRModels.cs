namespace SecaBackend.Models
{
    // ===============================================
    // MODELOS VIEJOS (mantener para compatibilidad)
    // ===============================================
    
    public class ISRInput
    {
        public decimal SueldoMensual { get; set; }
    }

    public class ISRResult
    {
        public decimal ISRCalculado { get; set; }
        public string DetalleCalculo { get; set; } = string.Empty;
    }

    // ===============================================
    // 🆕 MODELOS NUEVOS - ISR ASALARIADO (CORRECTO)
    // ===============================================
    
    public class ISRAsalariadoInput
    {
        // Ingresos anuales (enero-diciembre)
        public decimal SalariosAnuales { get; set; }
        public decimal Bono14 { get; set; }
        public decimal Aguinaldo { get; set; }
        public decimal OtrosBonos { get; set; }
        
        // Tipo de cálculo
        public bool EsProyectado { get; set; } // true = mensual, false = definitiva anual
    }
    
    public class ISRAsalariadoResult
    {
        public decimal TotalIngresos { get; set; }
        public decimal DeduccionPersonal { get; set; } // Q48,000
        public decimal BaseImponible { get; set; }
        public decimal ISRTotal { get; set; } // 5% sobre base
        public decimal ISRMensual { get; set; } // Solo si es proyectado
        public string TipoCalculo { get; set; } = string.Empty; // "Proyectado" o "Definitiva"
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}