// ✅ ARCHIVO COMPLETO - REEMPLAZAR TODO
// Ubicación: SECA-BACKEND/Models/ISRModels.cs

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
    // ✅ MODELOS NUEVOS - ISR ASALARIADO (CORREGIDO)
    // ===============================================
    
    public class ISRAsalariadoInput
    {
        // ✅ CAMPOS NUEVOS:
        public decimal SalarioOrdinarioMensual { get; set; }
        public decimal BonificacionIncentivo { get; set; }
        
        // Campos existentes
        public decimal Bono14 { get; set; }
        public decimal Aguinaldo { get; set; }
        public decimal OtrosBonos { get; set; }
        
        // Tipo de cálculo
        public bool EsProyectado { get; set; }
    }
    
    public class ISRAsalariadoResult
    {
        // ✅ ESTRUCTURA COMPLETAMENTE NUEVA
        
        // Renta Bruta
        public decimal SalariosAnuales { get; set; }
        public decimal BonificacionAnual { get; set; }
        public decimal Aguinaldo { get; set; }
        public decimal Bono14 { get; set; }
        public decimal OtrosBonos { get; set; }
        public decimal TotalRentaBruta { get; set; }
        
        // Rentas Exentas
        public decimal AguinaldoExento { get; set; }
        public decimal Bono14Exento { get; set; }
        public decimal TotalRentasExentas { get; set; }
        
        // Renta Neta
        public decimal RentaNeta { get; set; }
        
        // Deducciones
        public decimal GastosPersonales { get; set; }
        public decimal CuotaIGSS { get; set; }
        public decimal TotalDeducciones { get; set; }
        
        // Resultado
        public decimal RentaImponible { get; set; }
        public decimal ISRAnual { get; set; }
        public decimal RetencionMensual { get; set; }
        
        // Metadatos
        public string TipoCalculo { get; set; } = string.Empty;
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}