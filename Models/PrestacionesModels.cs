using System;
using System.Collections.Generic;

namespace SecaBackend.Models
{
    // =========================================
    // ENUM: Tipo de Terminación Laboral
    // =========================================
    public enum TipoTerminacion
    {
        DespidoInjustificado = 1,
        DespidoJustificado = 2,
        RenunciaVoluntaria = 3,
        RenunciaCausaJusta = 4,
        PensionIGSS = 5
    }

    // =========================================
    // ENTRADA: Prestaciones Completas
    // =========================================
    public class PrestacionesCompletasInput
    {
        // Datos básicos
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        
        // Salario actual (sin Q250)
        public decimal SalarioOrdinario { get; set; }
        
        // Salarios últimos 6 meses (para indemnización)
        // Si está vacío, se usa el salario ordinario
        public List<decimal> SalariosUltimos6Meses { get; set; } = new List<decimal>();
        
        // Tipo de terminación
        public TipoTerminacion TipoTerminacion { get; set; }
        
        // Vacaciones pendientes (días)
        public int DiasVacacionesPendientes { get; set; }
        
        // Ya recibió prestaciones del período actual
        public bool YaRecibioAguinaldo { get; set; }
        public bool YaRecibiBono14 { get; set; }
        
        // Solo si es pensión IGSS
        public decimal? MontoPensionIGSS { get; set; }
    }

    // =========================================
    // SALIDA: Prestaciones Completas
    // =========================================
    public class PrestacionesCompletasResult
    {
        // Componentes individuales
        public ComponenteIndemnizacion Indemnizacion { get; set; } = new ComponenteIndemnizacion();
        public ComponenteAguinaldo Aguinaldo { get; set; } = new ComponenteAguinaldo();
        public ComponenteBono14 Bono14 { get; set; } = new ComponenteBono14();
        public ComponenteVacaciones Vacaciones { get; set; } = new ComponenteVacaciones();
        public ComponenteBonificacion250 Bonificacion250 { get; set; } = new ComponenteBonificacion250();
        
        // Total
        public decimal TotalLiquidacion { get; set; }
        
        // Advertencias
        public List<string> Advertencias { get; set; } = new List<string>();
        
        // Notas legales
        public List<string> NotasLegales { get; set; } = new List<string>();
    }

    // =========================================
    // COMPONENTE: Indemnización
    // =========================================
    public class ComponenteIndemnizacion
    {
        public bool Aplica { get; set; }
        public decimal Monto { get; set; }
        public string Detalle { get; set; } = string.Empty;
    }

    // =========================================
    // COMPONENTE: Aguinaldo
    // =========================================
    public class ComponenteAguinaldo
    {
        public bool Aplica { get; set; }
        public decimal Monto { get; set; }
        public string Detalle { get; set; } = string.Empty;
    }

    // =========================================
    // COMPONENTE: Bono 14
    // =========================================
    public class ComponenteBono14
    {
        public bool Aplica { get; set; }
        public decimal Monto { get; set; }
        public string Detalle { get; set; } = string.Empty;
    }

    // =========================================
    // COMPONENTE: Vacaciones
    // =========================================
    public class ComponenteVacaciones
    {
        public bool Aplica { get; set; }
        public decimal Monto { get; set; }
        public string Detalle { get; set; } = string.Empty;
    }

    // =========================================
    // COMPONENTE: Bonificación Q250
    // =========================================
    public class ComponenteBonificacion250
    {
        public bool Aplica { get; set; }
        public decimal Monto { get; set; }
        public string Detalle { get; set; } = string.Empty;
    }
}