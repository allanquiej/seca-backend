namespace SecaBackend.Models
{
    // ===============================================
    // ENTRADA: Calculadora de ISO Trimestral
    // ✅ ACTUALIZADO: Agregado CostoDeVentas para verificación margen 4%
    // ===============================================
    public class ISOTrimestralInput
    {
        // ========================================
        // DATOS PARA VERIFICAR AFECTACIÓN (Margen 4%)
        // ========================================
        
        // Total de ingresos brutos del año (servicios + ventas)
        public decimal IngresosBrutosAnuales { get; set; }
        
        // ✅ NUEVO: Costo de ventas anual (para calcular margen)
        public decimal CostoDeVentas { get; set; }
        
        
        // ========================================
        // DATOS PARA CÁLCULO DE ACTIVO NETO
        // ========================================
        
        // Componentes del Activo Neto
        public decimal ActivoTotal { get; set; }                           // Total de activos
        public decimal DepreciacionAmortizacionAcumulada { get; set; }     // Menos: Dep. y Amort. Acumulada
        public decimal ReservaCuentasIncobrables { get; set; }             // Menos: Reserva Ctas. Incobrables
        public decimal CreditosReinversion { get; set; }                   // Menos: Créditos por Reinversión
        
        // Deducción IUSI (solo aplica para Activo Neto)
        public decimal IUSIPagado { get; set; }                            // IUSI pagado en el trimestre
    }

    // ===============================================
    // SALIDA: Calculadora de ISO Trimestral
    // ✅ ACTUALIZADO: Agregados campos para mostrar verificación margen 4%
    // ===============================================
    public class ISOTrimestralResult
    {
        // ========================================
        // PASO 1: VERIFICACIÓN DE AFECTACIÓN (Margen 4%)
        // ========================================
        public decimal IngresosBrutos { get; set; }                       // Ingresos brutos anuales
        public decimal CostoDeVentas { get; set; }                        // Costo de ventas anual
        public decimal ResultadoBruto { get; set; }                       // Ingresos - Costos
        public decimal MargenPorcentaje { get; set; }                     // (Resultado / Ingresos) × 100
        public bool EstaAfectoISO { get; set; }                           // true si Margen ≥ 4%
        
        // ========================================
        // PASO 2: DETERMINACIÓN DEL MÉTODO
        // ========================================
        public decimal ActivoNeto { get; set; }                           // Activo Total - Deducciones
        public decimal ComparacionActivo { get; set; }                    // Activo Neto vs 4×Ingresos
        public string MetodoSeleccionado { get; set; } = string.Empty;    // "Ingresos" o "Activo Neto"
        public string RazonMetodo { get; set; } = string.Empty;           // Explicación del método
        
        // ========================================
        // CÁLCULO SOBRE INGRESOS (si aplica)
        // ========================================
        public decimal BaseTrimestralIngresos { get; set; }               // Ingresos / 4
        public decimal ISOSobreIngresos { get; set; }                     // (Ingresos / 4) × 1%
        
        // ========================================
        // CÁLCULO SOBRE ACTIVO NETO (si aplica)
        // ========================================
        public decimal ActivoTotal { get; set; }
        public decimal DepreciacionAmortizacionAcumulada { get; set; }
        public decimal ReservaCuentasIncobrables { get; set; }
        public decimal CreditosReinversion { get; set; }
        public decimal BaseTrimestralActivo { get; set; }                 // Activo Neto / 4
        public decimal ISOSobreActivoNeto { get; set; }                   // (Activo Neto / 4) × 1%
        public decimal IUSIPagado { get; set; }
        public decimal ISOSobreActivoNetoFinal { get; set; }              // ISO Activo - IUSI
        
        // ========================================
        // RESULTADO FINAL
        // ========================================
        public decimal ISOAPagar { get; set; }                            // Monto a pagar
        
        // Detalle
        public string DetalleCalculo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string RecomendacionLegal { get; set; } = string.Empty;
    }
}