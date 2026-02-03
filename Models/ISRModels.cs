// ✅ ARCHIVO ACTUALIZADO - REEMPLAZAR COMPLETO
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
    // ✅ MODELOS ACTUALIZADOS - ISR ASALARIADO (SAT-1901 COMPLETO)
    // ===============================================
    
    public class ISRAsalariadoInput
    {
        // ========================================
        // SECCIÓN 1: IDENTIFICACIÓN
        // ========================================
        public string NitEmpleado { get; set; } = string.Empty;
        
        // ========================================
        // SECCIÓN 2: PERÍODO DE IMPOSICIÓN
        // ========================================
        public int AnioImposicion { get; set; }
        
        // ========================================
        // SECCIÓN 3: FECHA INICIO DE LABORES
        // ========================================
        public int MesInicio { get; set; }  // 1-12
        public int AnioInicio { get; set; }
        public bool EsProyectado { get; set; }
        
        // ========================================
        // SECCIÓN 4: RENTAS BRUTAS
        // ========================================
        
        // ¿Cuántos patronos?
        public int NumeroPatronos { get; set; }  // 1 = Uno, 2+ = Dos o más
        
        // --- SI TUVO UN PATRONO ---
        public string? NitPatronoPrincipal { get; set; }
        public bool SueldoIgualDurante12Meses { get; set; }
        
        // Salario ordinario mensual (SIN bonificación)
        public decimal SalarioOrdinarioMensual { get; set; }
        
        // Bonificación incentivo (Decreto 37-2001)
        public decimal BonificacionIncentivo { get; set; }
        
        // Salarios mensuales individuales (si SueldoIgualDurante12Meses = false)
        public decimal? SalarioEnero { get; set; }
        public decimal? SalarioFebrero { get; set; }
        public decimal? SalarioMarzo { get; set; }
        public decimal? SalarioAbril { get; set; }
        public decimal? SalarioMayo { get; set; }
        public decimal? SalarioJunio { get; set; }
        public decimal? SalarioJulio { get; set; }
        public decimal? SalarioAgosto { get; set; }
        public decimal? SalarioSeptiembre { get; set; }
        public decimal? SalarioOctubre { get; set; }
        public decimal? SalarioNoviembre { get; set; }
        public decimal? SalarioDiciembre { get; set; }
        
        // --- SI TUVO DOS O MÁS PATRONOS ---
        public decimal? SalariosPatronoPrincipal { get; set; }
        public decimal? SalariosOtrosPatronos { get; set; }
        
        // Otros ingresos anuales
        public decimal Bono14 { get; set; }
        public decimal Aguinaldo { get; set; }
        public decimal HorasExtrasAnuales { get; set; }
        public decimal OtrosBonos { get; set; }
        
        // ========================================
        // SECCIÓN 5: RENTAS EXENTAS ADICIONALES
        // ========================================
        public decimal IndemnizacionesPorMuerteOIncapacidad { get; set; }
        public decimal IndemnizacionesPorTiempoServido { get; set; }
        public decimal RemuneracionesDiplomaticos { get; set; }
        public decimal GastosRepresentacionYViaticos { get; set; }
        
        // ========================================
        // SECCIÓN 6: DEDUCCIONES ADICIONALES
        // ========================================
        public decimal DeduccionesPersonalesComprobadas { get; set; }
        public decimal Donaciones { get; set; }
        public decimal PrimasSeguroVida { get; set; }
    }
    
    public class ISRAsalariadoResult
    {
        // ========================================
        // INFORMACIÓN GENERAL
        // ========================================
        public string NitEmpleado { get; set; } = string.Empty;
        public int AnioImposicion { get; set; }
        public int NumeroPatronos { get; set; }
        public decimal SalarioOrdinarioMensual { get; set; }
        
        // ========================================
        // SECCIÓN 4: RENTA BRUTA
        // ========================================
        public decimal SalariosAnuales { get; set; }
        public decimal BonificacionAnual { get; set; }
        public decimal Aguinaldo { get; set; }
        public decimal Bono14 { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal OtrosBonos { get; set; }
        public decimal TotalRentaBruta { get; set; }
        
        // ========================================
        // SECCIÓN 5: RENTAS EXENTAS
        // ========================================
        public decimal IndemnizacionesPorMuerteOIncapacidad { get; set; }
        public decimal IndemnizacionesPorTiempoServido { get; set; }
        public decimal RemuneracionesDiplomaticos { get; set; }
        public decimal GastosRepresentacionYViaticos { get; set; }
        public decimal AguinaldoExento { get; set; }
        public decimal Bono14Exento { get; set; }
        public decimal TotalRentasExentas { get; set; }
        
        // ========================================
        // SECCIÓN 5: RENTA NETA
        // ========================================
        public decimal RentaNeta { get; set; }
        
        // ========================================
        // SECCIÓN 6: DEDUCCIONES
        // ========================================
        public decimal GastosPersonales { get; set; }  // Q48,000
        public decimal DeduccionesPersonalesComprobadas { get; set; }
        public decimal Donaciones { get; set; }
        public decimal CuotaIGSS { get; set; }  // 4.83%
        public decimal PrimasSeguroVida { get; set; }
        public decimal TotalDeducciones { get; set; }
        
        // ========================================
        // RESULTADO
        // ========================================
        public decimal RentaImponible { get; set; }
        public decimal ExcedenteDeducciones { get; set; }
        public decimal ISRAnual { get; set; }
        public decimal RetencionMensual { get; set; }
        
        // ========================================
        // METADATOS
        // ========================================
        public string TipoCalculo { get; set; } = string.Empty;
        public string DetalleCalculo { get; set; } = string.Empty;
    }
}