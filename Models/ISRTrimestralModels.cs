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
        
        // ✅ NUEVOS CAMPOS - Agregados según imágenes
        public decimal RentasExentas { get; set; }
        public decimal ISRPagadoAnteriorTrimestre { get; set; }
        
        // Común para ambas opciones
        public decimal ISOPendiente { get; set; }
        
        // Tipo de cálculo
        public bool UsarOpcionAcumulada { get; set; } // true = Opción 1, false = Opción 2
    }
    
    public class ISRTrimestralV2Result
    {
        public string OpcionUtilizada { get; set; } = string.Empty; // "Opción 1 - Acumulado" o "Opción 2 - Trimestre"
        public decimal BaseCalculo { get; set; } // Ventas - Rentas Exentas - Gastos (Opción 1) o Ventas - Rentas Exentas (Opción 2)
        public decimal ISRCalculado { get; set; } // Base × 25% (Opción 1) o Base × 2% (Opción 2)
        
        // ✅ NUEVOS CAMPOS - Para mostrar cálculos intermedios en Opción 2
        public decimal ISR25Porciento { get; set; }  // Solo para Opción 2: Base × 25%
        public decimal ISR8Porciento { get; set; }   // Solo para Opción 2: ISR25% × 8%
        
        public decimal ISOAcreditar { get; set; }
        public decimal ISRPagadoAnterior { get; set; } // Solo para Opción 1
        public decimal ISRAPagar { get; set; } // ISR - ISO - ISR Anterior (si aplica)
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}