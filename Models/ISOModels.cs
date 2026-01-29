namespace SecaBackend.Models
{
    // ===============================================
    // ENTRADA: Calculadora de ISO Trimestral
    // ===============================================
    public class ISOTrimestralInput
    {
        // ========================================
        // OPCIÓN 1: ISO SOBRE INGRESOS BRUTOS
        // ========================================
        
        // Total de ingresos brutos del año (suma de todos los trimestres)
        public decimal IngresosBrutosAnuales { get; set; }
        
        
        // ========================================
        // OPCIÓN 2: ISO SOBRE ACTIVO NETO
        // ========================================
        
        // Componentes del Activo Neto
        public decimal ActivoTotal { get; set; }                           // Total de activos
        public decimal DepreciacionAmortizacionAcumulada { get; set; }     // Menos: Dep. y Amort. Acumulada
        public decimal ReservaCuentasIncobrables { get; set; }            // Menos: Reserva Ctas. Incobrables
        public decimal CreditosReinversion { get; set; }                  // Menos: Créditos por Reinversión
        
        // Deducción IUSI (solo aplica para Activo Neto)
        public decimal IUSIPagado { get; set; }                           // IUSI pagado en el trimestre
    }

    // ===============================================
    // SALIDA: Calculadora de ISO Trimestral
    // ===============================================
    public class ISOTrimestralResult
    {
        // Cálculo sobre Ingresos Brutos
        public decimal IngresosBrutosAnuales { get; set; }
        public decimal BaseTrimestralIngresos { get; set; }               // Ingresos / 4
        public decimal ISOSobreIngresos { get; set; }                     // (Ingresos / 4) × 1%
        
        // Cálculo sobre Activo Neto
        public decimal ActivoTotal { get; set; }
        public decimal DepreciacionAmortizacionAcumulada { get; set; }
        public decimal ReservaCuentasIncobrables { get; set; }
        public decimal CreditosReinversion { get; set; }
        public decimal ActivoNeto { get; set; }                           // Activo Total - Deducciones
        public decimal BaseTrimestralActivo { get; set; }                 // Activo Neto / 4
        public decimal ISOSobreActivoNeto { get; set; }                   // (Activo Neto / 4) × 1%
        public decimal IUSIPagado { get; set; }
        public decimal ISOSobreActivoNetoFinal { get; set; }              // ISO Activo - IUSI
        
        // Resultado Final
        public decimal ISOAPagar { get; set; }                            // El mayor entre las dos opciones
        public string MetodoUtilizado { get; set; } = string.Empty;       // "Ingresos Brutos" o "Activo Neto"
        
        // Detalle
        public string DetalleCalculoIngresos { get; set; } = string.Empty;
        public string DetalleCalculoActivo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string RecomendacionLegal { get; set; } = string.Empty;
    }
}