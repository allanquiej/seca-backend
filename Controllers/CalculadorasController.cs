// Este controlador contiene todas las calculadoras del sistema SECA.
// Cada calculadora será un método POST separado.
// Comenzamos con la calculadora de indemnización.

using Microsoft.AspNetCore.Mvc;
using SecaBackend.Data;
using SecaBackend.Models;

namespace SecaBackend.Controllers
{
    [ApiController]
    [Route("api/calculadoras")]
    public class CalculadorasController : ControllerBase
    {
        private readonly SecaDbContext _context;

        // El DbContext nos permite escribir logs en la base de datos.
        public CalculadorasController(SecaDbContext context)
        {
            _context = context;
        }

        // ===========================================================
        // CALCULADORA #1 → INDEMNIZACIÓN
        // Ruta: POST /api/calculadoras/indemnizacion
        // ===========================================================
        [HttpPost("indemnizacion")]
        public async Task<IActionResult> CalcularIndemnizacion([FromBody] IndemnizacionInput input)
        {
            // Validaciones básicas
            if (input.SalarioMensual <= 0)
            {
                return BadRequest(new { exito = false, mensaje = "El salario mensual debe ser mayor a 0." });
            }

            var inicio = input.FechaInicio.Date;
            var fin = input.FechaFin.Date;

            if (fin < inicio)
            {
                return BadRequest(new { exito = false, mensaje = "La fecha fin no puede ser menor que la fecha inicio." });
            }

            // Días trabajados (incluyendo ambos días)
            var dias = (fin - inicio).TotalDays + 1;
            if (dias <= 0)
            {
                return BadRequest(new { exito = false, mensaje = "El rango de fechas no es válido." });
            }

            // Años equivalentes (promedio con año bisiesto)
            decimal aniosEquivalentes = (decimal)dias / 365.25m;

            // Indemnización aproximada: salario mensual * años equivalentes
            decimal monto = input.SalarioMensual * aniosEquivalentes;

            var result = new IndemnizacionResult
            {
                MontoIndemnizacion = decimal.Round(monto, 2),
                DetalleCalculo =
                    $"SalarioMensual={input.SalarioMensual}; " +
                    $"FechaInicio={inicio:dd-MM-yyyy}; FechaFin={fin:dd-MM-yyyy}; " +
                    $"Dias={dias}; AniosEquivalentes={decimal.Round(aniosEquivalentes, 6)}; " +
                    $"Formula=SalarioMensual*AniosEquivalentes"
            };

            // Log a DB (sin cambiar BD)
            var log = new CalculatorLog
            {
                TipoCalculadora = "Indemnizacion",
                DatosEntrada = $"Salario={input.SalarioMensual}; FechaInicio={inicio:dd-MM-yyyy}; FechaFin={fin:dd-MM-yyyy}",
                Resultado = $"Monto={result.MontoIndemnizacion}; Dias={dias}; AniosEq={decimal.Round(aniosEquivalentes, 6)}",
                Fecha = DateTime.Now
            };

            _context.CalculatorLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                exito = true,
                datos = result,
                mensaje = "Cálculo de indemnización realizado con éxito."
            });
        }


        // ===========================================================
        // CALCULADORA #2 → BONO 14 (✅ CORREGIDO)
        // Ruta: POST /api/calculadoras/bono14
        // Fórmula: (Salario Promedio ÷ 365) × Días Laborados
        // ===========================================================
        [HttpPost("bono14")]
        public async Task<IActionResult> CalcularBono14([FromBody] Bono14Input input)
        {
            if (input.SalarioPromedio <= 0)
            {
                return BadRequest(new { exito = false, mensaje = "El salario promedio debe ser mayor a 0." });
            }

            var inicio = input.FechaInicio.Date;
            var fin = input.FechaFin.Date;

            if (fin < inicio)
            {
                return BadRequest(new { exito = false, mensaje = "La fecha fin no puede ser menor que la fecha inicio." });
            }

            // Días trabajados (incluye ambos)
            var dias = (fin - inicio).TotalDays + 1;
            if (dias <= 0)
            {
                return BadRequest(new { exito = false, mensaje = "El rango de fechas no es válido." });
            }

            // ✅ FÓRMULA OFICIAL DEL MINISTERIO DE TRABAJO:
            // Bono 14 = (Salario Promedio ÷ 365) × Días Laborados
            decimal monto = (input.SalarioPromedio / 365m) * (decimal)dias;

            var result = new Bono14Result
            {
                MontoBono14 = decimal.Round(monto, 2),
                DetalleCalculo =
                    $"SalarioPromedio={input.SalarioPromedio}; " +
                    $"FechaInicio={inicio:dd-MM-yyyy}; FechaFin={fin:dd-MM-yyyy}; " +
                    $"Dias={dias}; " +
                    $"Formula=(SalarioPromedio/365)*Dias"
            };

            var log = new CalculatorLog
            {
                TipoCalculadora = "Bono14",
                DatosEntrada = $"SalarioPromedio={input.SalarioPromedio}; FechaInicio={inicio:dd-MM-yyyy}; FechaFin={fin:dd-MM-yyyy}",
                Resultado = $"Monto={result.MontoBono14}; Dias={dias}",
                Fecha = DateTime.Now
            };

            _context.CalculatorLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                exito = true,
                datos = result,
                mensaje = "Cálculo de Bono 14 realizado con éxito."
            });
        }


        // ===========================================================
        // CALCULADORA #3 → AGUINALDO (✅ CORREGIDO)
        // Ruta: POST /api/calculadoras/aguinaldo
        // Fórmula: (Salario Promedio ÷ 365) × Días Laborados
        // ===========================================================
        [HttpPost("aguinaldo")]
        public async Task<IActionResult> CalcularAguinaldo([FromBody] AguinaldoInput input)
        {
            if (input.SalarioPromedio <= 0)
            {
                return BadRequest(new { exito = false, mensaje = "El salario promedio debe ser mayor a 0." });
            }

            var inicio = input.FechaInicio.Date;
            var fin = input.FechaFin.Date;

            if (fin < inicio)
            {
                return BadRequest(new { exito = false, mensaje = "La fecha fin no puede ser menor que la fecha inicio." });
            }

            var dias = (fin - inicio).TotalDays + 1;
            if (dias <= 0)
            {
                return BadRequest(new { exito = false, mensaje = "El rango de fechas no es válido." });
            }

            // ✅ FÓRMULA OFICIAL DEL MINISTERIO DE TRABAJO:
            // Aguinaldo = (Salario Promedio ÷ 365) × Días Laborados
            decimal monto = (input.SalarioPromedio / 365m) * (decimal)dias;

            var result = new AguinaldoResult
            {
                MontoAguinaldo = decimal.Round(monto, 2),
                DetalleCalculo =
                    $"SalarioPromedio={input.SalarioPromedio}; " +
                    $"FechaInicio={inicio:dd-MM-yyyy}; FechaFin={fin:dd-MM-yyyy}; " +
                    $"Dias={dias}; " +
                    $"Formula=(SalarioPromedio/365)*Dias"
            };

            var log = new CalculatorLog
            {
                TipoCalculadora = "Aguinaldo",
                DatosEntrada = $"SalarioPromedio={input.SalarioPromedio}; FechaInicio={inicio:dd-MM-yyyy}; FechaFin={fin:dd-MM-yyyy}",
                Resultado = $"Monto={result.MontoAguinaldo}; Dias={dias}",
                Fecha = DateTime.Now
            };

            _context.CalculatorLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                exito = true,
                datos = result,
                mensaje = "Cálculo de aguinaldo realizado con éxito."
            });
        }


// ✅ MÉTODO ACTUALIZADO - REEMPLAZAR el método CalcularISRAsalariado en CalculadorasController.cs
// Buscar desde línea ~861 y reemplazar TODO el método

// ===========================================================
// ✅ CALCULADORA ISR ASALARIADO - ACTUALIZADO SEGÚN SAT-1901
// Ruta: POST /api/calculadoras/isr-asalariado
// ===========================================================
[HttpPost("isr-asalariado")]
public async Task<IActionResult> CalcularISRAsalariado([FromBody] ISRAsalariadoInput input)
{
    // ========================================
    // VALIDACIONES
    // ========================================
    
    if (input.AnioImposicion < 2020 || input.AnioImposicion > 2030)
    {
        return BadRequest(new { exito = false, mensaje = "Año de imposición no válido." });
    }
    
    if (input.MesInicio < 1 || input.MesInicio > 12)
    {
        return BadRequest(new { exito = false, mensaje = "Mes de inicio no válido (1-12)." });
    }
    
    if (input.NumeroPatronos < 1)
    {
        return BadRequest(new { exito = false, mensaje = "Debe especificar al menos un patrono." });
    }

    // ========================================
    // PASO 1: CALCULAR TOTAL DE SALARIOS ANUALES
    // ========================================
    
    decimal totalSalariosAnuales = 0m;
    decimal bonificacionAnual = 0m;
    
    if (input.NumeroPatronos == 1)
    {
        // UN SOLO PATRONO
        if (input.SueldoIgualDurante12Meses)
        {
            // Sueldo igual durante 12 meses
            totalSalariosAnuales = input.SalarioOrdinarioMensual * 12m;
            bonificacionAnual = input.BonificacionIncentivo * 12m;
        }
        else
        {
            // Suma de los 12 meses (si están proporcionados)
            totalSalariosAnuales = 
                (input.SalarioEnero ?? 0m) +
                (input.SalarioFebrero ?? 0m) +
                (input.SalarioMarzo ?? 0m) +
                (input.SalarioAbril ?? 0m) +
                (input.SalarioMayo ?? 0m) +
                (input.SalarioJunio ?? 0m) +
                (input.SalarioJulio ?? 0m) +
                (input.SalarioAgosto ?? 0m) +
                (input.SalarioSeptiembre ?? 0m) +
                (input.SalarioOctubre ?? 0m) +
                (input.SalarioNoviembre ?? 0m) +
                (input.SalarioDiciembre ?? 0m);
            
            bonificacionAnual = input.BonificacionIncentivo * 12m;
        }
    }
    else
    {
        // DOS O MÁS PATRONOS
        totalSalariosAnuales = 
            (input.SalariosPatronoPrincipal ?? 0m) + 
            (input.SalariosOtrosPatronos ?? 0m);
        
        bonificacionAnual = input.BonificacionIncentivo * 12m;
    }

    // ========================================
    // PASO 2: CALCULAR RENTA BRUTA
    // ========================================
    
    decimal totalRentaBruta = 
        totalSalariosAnuales +
        bonificacionAnual +
        input.Bono14 +
        input.Aguinaldo +
        input.HorasExtrasAnuales +
        input.OtrosBonos;

    // ========================================
    // PASO 3: CALCULAR SALARIO ORDINARIO MENSUAL
    // (Para determinar límite de rentas exentas)
    // ========================================
    
    decimal salarioOrdinarioMensual = input.SalarioOrdinarioMensual;
    
    // Si no se proporcionó, calculamos el promedio
    if (salarioOrdinarioMensual == 0m && input.NumeroPatronos == 1 && !input.SueldoIgualDurante12Meses)
    {
        salarioOrdinarioMensual = totalSalariosAnuales / 12m;
    }
    
    if (salarioOrdinarioMensual == 0m && input.NumeroPatronos > 1)
    {
        // Para múltiples patronos, usamos el promedio sin bonificación
        salarioOrdinarioMensual = (totalSalariosAnuales / 12m);
    }

    // ========================================
    // PASO 4: CALCULAR RENTAS EXENTAS
    // ========================================
    
    // Aguinaldo exento: hasta 100% del salario ordinario mensual
    decimal aguinaldoExento = Math.Min(input.Aguinaldo, salarioOrdinarioMensual);
    
    // Bono 14 exento: hasta 100% del salario ordinario mensual
    decimal bono14Exento = Math.Min(input.Bono14, salarioOrdinarioMensual);
    
    // Suma de todas las rentas exentas
    decimal totalRentasExentas = 
        input.IndemnizacionesPorMuerteOIncapacidad +
        input.IndemnizacionesPorTiempoServido +
        input.RemuneracionesDiplomaticos +
        input.GastosRepresentacionYViaticos +
        aguinaldoExento +
        bono14Exento;

    // ========================================
    // PASO 5: CALCULAR RENTA NETA
    // ========================================
    
    decimal rentaNeta = totalRentaBruta - totalRentasExentas;
    
    if (rentaNeta < 0)
    {
        rentaNeta = 0m;
    }

    // ========================================
    // PASO 6: CALCULAR DEDUCCIONES
    // ========================================
    
    // 1. Deducciones personales sin comprobación (Art. 72): Q48,000
    decimal gastosPersonales = 48000m;
    
    // 2. Deducciones personales comprobadas (opcional)
    decimal deduccionesPersonalesComprobadas = input.DeduccionesPersonalesComprobadas;
    
    // 3. Donaciones (opcional, máximo 5% de la renta bruta)
    decimal donaciones = input.Donaciones;
    decimal limiteDonaciones = totalRentaBruta * 0.05m;
    if (donaciones > limiteDonaciones)
    {
        donaciones = limiteDonaciones;
    }
    
    // 4. Cuota IGSS: 4.83% sobre la renta neta
    decimal cuotaIGSS = rentaNeta * 0.0483m;
    
    // 5. Primas de seguro de vida (opcional)
    decimal primasSeguroVida = input.PrimasSeguroVida;
    
    // Total deducciones
    decimal totalDeducciones = 
        gastosPersonales +
        deduccionesPersonalesComprobadas +
        donaciones +
        cuotaIGSS +
        primasSeguroVida;

    // ========================================
    // PASO 7: CALCULAR RENTA IMPONIBLE
    // ========================================
    
    decimal rentaImponible = rentaNeta - totalDeducciones;
    
    // Excedente de deducciones
    decimal excedenteDeducciones = 0m;
    if (rentaImponible < 0)
    {
        excedenteDeducciones = Math.Abs(rentaImponible);
        rentaImponible = 0m;
    }
    else
    {
        excedenteDeducciones = gastosPersonales;
    }

    // ========================================
    // PASO 8: CALCULAR ISR ANUAL (TABLA PROGRESIVA)
    // ========================================
    
    decimal isrAnual = 0m;
    
    if (rentaImponible <= 300000m)
    {
        // Rango I: Hasta Q300,000 → 5%
        isrAnual = rentaImponible * 0.05m;
    }
    else
    {
        // Rango II: Más de Q300,000 → Q15,000 + 7% sobre excedente
        decimal excedente = rentaImponible - 300000m;
        isrAnual = 15000m + (excedente * 0.07m);
    }

    // ========================================
    // PASO 9: CALCULAR RETENCIÓN MENSUAL
    // ========================================
    
    decimal retencionMensual = isrAnual / 12m;

    // ========================================
    // CONSTRUIR RESULTADO
    // ========================================
    
    var result = new ISRAsalariadoResult
    {
        // Información general
        NitEmpleado = input.NitEmpleado,
        AnioImposicion = input.AnioImposicion,
        NumeroPatronos = input.NumeroPatronos,
        SalarioOrdinarioMensual = decimal.Round(salarioOrdinarioMensual, 2),
        
        // Sección 4: Renta Bruta
        SalariosAnuales = decimal.Round(totalSalariosAnuales, 2),
        BonificacionAnual = decimal.Round(bonificacionAnual, 2),
        Aguinaldo = decimal.Round(input.Aguinaldo, 2),
        Bono14 = decimal.Round(input.Bono14, 2),
        HorasExtras = decimal.Round(input.HorasExtrasAnuales, 2),
        OtrosBonos = decimal.Round(input.OtrosBonos, 2),
        TotalRentaBruta = decimal.Round(totalRentaBruta, 2),
        
        // Sección 5: Rentas Exentas
        IndemnizacionesPorMuerteOIncapacidad = decimal.Round(input.IndemnizacionesPorMuerteOIncapacidad, 2),
        IndemnizacionesPorTiempoServido = decimal.Round(input.IndemnizacionesPorTiempoServido, 2),
        RemuneracionesDiplomaticos = decimal.Round(input.RemuneracionesDiplomaticos, 2),
        GastosRepresentacionYViaticos = decimal.Round(input.GastosRepresentacionYViaticos, 2),
        AguinaldoExento = decimal.Round(aguinaldoExento, 2),
        Bono14Exento = decimal.Round(bono14Exento, 2),
        TotalRentasExentas = decimal.Round(totalRentasExentas, 2),
        RentaNeta = decimal.Round(rentaNeta, 2),
        
        // Sección 6: Deducciones
        GastosPersonales = decimal.Round(gastosPersonales, 2),
        DeduccionesPersonalesComprobadas = decimal.Round(deduccionesPersonalesComprobadas, 2),
        Donaciones = decimal.Round(donaciones, 2),
        CuotaIGSS = decimal.Round(cuotaIGSS, 2),
        PrimasSeguroVida = decimal.Round(primasSeguroVida, 2),
        TotalDeducciones = decimal.Round(totalDeducciones, 2),
        
        // Resultado
        RentaImponible = decimal.Round(rentaImponible, 2),
        ExcedenteDeducciones = decimal.Round(excedenteDeducciones, 2),
        ISRAnual = decimal.Round(isrAnual, 2),
        RetencionMensual = decimal.Round(retencionMensual, 2),
        
        // Metadatos
        TipoCalculo = input.EsProyectado ? "Proyectada" : "Definitiva",
        DetalleCalculo = $"NIT: {input.NitEmpleado}; Período: {input.AnioImposicion}; " +
                        $"Patronos: {input.NumeroPatronos}; Salario Ordinario Mensual: Q{salarioOrdinarioMensual:F2}; " +
                        $"Total Salarios Anuales: Q{totalSalariosAnuales:F2}; Renta Bruta: Q{totalRentaBruta:F2}; " +
                        $"Rentas Exentas: Q{totalRentasExentas:F2}; Renta Neta: Q{rentaNeta:F2}; " +
                        $"Deducciones: Q{totalDeducciones:F2}; Renta Imponible: Q{rentaImponible:F2}; " +
                        $"ISR Anual: Q{isrAnual:F2}; Retención Mensual: Q{retencionMensual:F2}"
    };

    // ========================================
    // GUARDAR LOG
    // ========================================
    
    var log = new CalculatorLog
    {
        TipoCalculadora = "ISR Asalariado",
        DatosEntrada = $"NIT={input.NitEmpleado}; Periodo={input.AnioImposicion}; " +
                      $"Patronos={input.NumeroPatronos}; SueldoIgual={input.SueldoIgualDurante12Meses}; " +
                      $"SalarioOrdinario={input.SalarioOrdinarioMensual}; " +
                      $"Bonificacion={input.BonificacionIncentivo}",
        Resultado = $"ISRAnual={result.ISRAnual}; RetencionMensual={result.RetencionMensual}; " +
                   $"RentaImponible={result.RentaImponible}",
        Fecha = DateTime.Now
    };

    _context.CalculatorLogs.Add(log);
    await _context.SaveChangesAsync();

    // ========================================
    // RETORNAR RESPUESTA
    // ========================================
    
    return Ok(new
    {
        exito = true,
        datos = result,
        mensaje = $"ISR Asalariado calculado correctamente según formulario SAT-1901 ({result.TipoCalculo})."
    });
}


// ===========================================================
// ⚠️ MANTENER ENDPOINT VIEJO PARA COMPATIBILIDAD
// (Deprecated - usar /isr-asalariado en su lugar)
// ===========================================================
[HttpPost("isr-laboral")]
[Obsolete("Use /isr-asalariado endpoint instead. This simplified version will be removed in future versions.")]
public async Task<IActionResult> CalcularISRLaboral([FromBody] ISRInput input)
{
    if (input.SueldoMensual <= 0)
    {
        return BadRequest(new
        {
            exito = false,
            mensaje = "El sueldo mensual debe ser mayor a 0."
        });
    }

    // Fórmula simplificada (DEPRECATED):
    // ISR = 5% del salario mensual
    decimal isr = input.SueldoMensual * 0.05m;

    var result = new ISRResult
    {
        ISRCalculado = isr,
        DetalleCalculo = $"[DEPRECADO] Fórmula simplificada: ISR = {input.SueldoMensual} × 0.05. " +
                        $"Para cálculo correcto según SAT, use /api/calculadoras/isr-asalariado"
    };

    // Registrar log en la base de datos
    var log = new CalculatorLog
    {
        TipoCalculadora = "ISR Laboral (Deprecated)",
        DatosEntrada = $"SueldoMensual={input.SueldoMensual}",
        Resultado = $"ISR={result.ISRCalculado}",
        Fecha = DateTime.Now
    };

    _context.CalculatorLogs.Add(log);
    await _context.SaveChangesAsync();

    return Ok(new
    {
        exito = true,
        datos = result,
        mensaje = "ISR laboral calculado con éxito. NOTA: Este endpoint está deprecado, use /isr-asalariado para cálculo oficial según SAT.",
        advertencia = "Este endpoint usa una fórmula simplificada y será removido en versiones futuras. Use /api/calculadoras/isr-asalariado"
    });
}

        // ===========================================================
        // CALCULADORA #5 → ISR EMPRESAS / EMPRENDEDORES (Mensual)
        // Ruta: POST /api/calculadoras/isr-empresa-mensual
        // ===========================================================
        [HttpPost("isr-empresa-mensual")]
        public async Task<IActionResult> CalcularISREmpresaMensual([FromBody] ISREmpresaInput input)
        {
            if (input.IngresosMensuales <= 0)
            {
                return BadRequest(new
                {
                    exito = false,
                    mensaje = "Los ingresos deben ser mayores a 0."
                });
            }

            // Fórmula simplificada: ISR = 5% de los ingresos mensuales
            decimal isr = input.IngresosMensuales * 0.05m;

            var result = new ISREmpresaResult
            {
                ISRCalculado = isr,
                DetalleCalculo = $"ISR = {input.IngresosMensuales} × 0.05"
            };

            // Registrar log en la base de datos
            var log = new CalculatorLog
            {
                TipoCalculadora = "ISR Empresa Mensual",
                DatosEntrada = $"IngresosMensuales={input.IngresosMensuales}",
                Resultado = $"ISR={result.ISRCalculado}",
                Fecha = DateTime.Now
            };

            _context.CalculatorLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                exito = true,
                datos = result,
                mensaje = "ISR mensual para empresas calculado con éxito."
            });
        }

        // ===========================================================
        // CALCULADORA #6 → ISR TRIMESTRAL EMPRESAS
        // Ruta: POST /api/calculadoras/isr-empresa-trimestral
        // ===========================================================
        [HttpPost("isr-empresa-trimestral")]
        public async Task<IActionResult> CalcularISREmpresaTrimestral([FromBody] ISRTrimestralInput input)
        {
            if (input.IngresosTrimestrales <= 0)
            {
                return BadRequest(new
                {
                    exito = false,
                    mensaje = "Los ingresos trimestrales deben ser mayores a 0."
                });
            }

            // Fórmula: ISR = 5% del total trimestral
            decimal isr = input.IngresosTrimestrales * 0.05m;

            var result = new ISRTrimestralResult
            {
                ISRCalculado = isr,
                DetalleCalculo = $"ISR = {input.IngresosTrimestrales} × 0.05"
            };

            // Registro en base de datos
            var log = new CalculatorLog
            {
                TipoCalculadora = "ISR Empresa Trimestral",
                DatosEntrada = $"IngresosTrimestrales={input.IngresosTrimestrales}",
                Resultado = $"ISR={result.ISRCalculado}",
                Fecha = DateTime.Now
            };

            _context.CalculatorLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                exito = true,
                datos = result,
                mensaje = "ISR trimestral para empresas calculado con éxito."
            });
        }

        // ===========================================================
// ===========================================================
// CALCULADORA ISO TRIMESTRAL - ✅ ACTUALIZADO según Video YouTube
// Ruta: POST /api/calculadoras/iso-trimestral
// 
// NUEVA LÓGICA según video:
// 1. Verificar margen 4% → Si < 4% NO paga ISO
// 2. Si Activo > 4×Ingresos → Calcular sobre Ingresos
// 3. Si Activo <= 4×Ingresos → Calcular sobre Activo Neto (1/4)
// ===========================================================
[HttpPost("iso-trimestral")]
public async Task<IActionResult> CalcularISOTrimestral([FromBody] ISOTrimestralInput input)
{
    // ========================================
    // VALIDACIONES BÁSICAS
    // ========================================
    if (input.IngresosBrutosAnuales < 0)
    {
        return BadRequest(new { exito = false, mensaje = "Los ingresos brutos no pueden ser negativos." });
    }
    
    if (input.CostoDeVentas < 0)
    {
        return BadRequest(new { exito = false, mensaje = "El costo de ventas no puede ser negativo." });
    }
    
    if (input.ActivoTotal < 0)
    {
        return BadRequest(new { exito = false, mensaje = "El activo total no puede ser negativo." });
    }


    // ========================================
    // PASO 1: VERIFICAR SI ESTÁ AFECTO AL ISO (MARGEN 4%)
    // ========================================
    // Fórmula según video:
    // Margen = (Ingresos Brutos - Costo de Ventas) / Ingresos Brutos
    // Si Margen < 4% → NO está afecto al ISO
    
    // Si no hay ingresos, no está afecto
    if (input.IngresosBrutosAnuales == 0)
    {
        return Ok(new 
        { 
            exito = true, 
            datos = new ISOTrimestralResult
            {
                IngresosBrutos = 0,
                CostoDeVentas = 0,
                ResultadoBruto = 0,
                MargenPorcentaje = 0,
                EstaAfectoISO = false,
                ISOAPagar = 0,
                MetodoSeleccionado = "N/A",
                RazonMetodo = "No hay ingresos brutos",
                DetalleCalculo = "No hay ingresos brutos registrados.",
                Mensaje = "La empresa NO está afecta al Impuesto de Solidaridad porque no registra ingresos.",
                RecomendacionLegal = ""
            },
            mensaje = "No está afecto al ISO (sin ingresos)." 
        });
    }
    
    // Calcular margen
    decimal resultadoBruto = input.IngresosBrutosAnuales - input.CostoDeVentas;
    decimal margenDecimal = resultadoBruto / input.IngresosBrutosAnuales;
    decimal margenPorcentaje = margenDecimal * 100m;
    
    // Verificar si está afecto (margen >= 4%)
    bool estaAfectoISO = margenPorcentaje >= 4m;
    
    // Si NO está afecto, retornar inmediatamente
    if (!estaAfectoISO)
    {
        return Ok(new 
        { 
            exito = true, 
            datos = new ISOTrimestralResult
            {
                IngresosBrutos = decimal.Round(input.IngresosBrutosAnuales, 2),
                CostoDeVentas = decimal.Round(input.CostoDeVentas, 2),
                ResultadoBruto = decimal.Round(resultadoBruto, 2),
                MargenPorcentaje = decimal.Round(margenPorcentaje, 2),
                EstaAfectoISO = false,
                ActivoNeto = 0,
                ComparacionActivo = 0,
                ISOAPagar = 0,
                MetodoSeleccionado = "N/A",
                RazonMetodo = "Margen menor al 4%",
                DetalleCalculo = $"Ingresos: Q{input.IngresosBrutosAnuales:F2}; " +
                                $"Costos: Q{input.CostoDeVentas:F2}; " +
                                $"Resultado: Q{resultadoBruto:F2}; " +
                                $"Margen: {margenPorcentaje:F2}%",
                Mensaje = $"La empresa NO está afecta al ISO porque su margen de utilidad " +
                         $"({margenPorcentaje:F2}%) es menor al 4% requerido.",
                RecomendacionLegal = "Según el Decreto 73-2008, las empresas con margen de utilidad menor al 4% " +
                                    "no están afectas al Impuesto de Solidaridad."
            },
            mensaje = "No está afecto al ISO (margen < 4%)." 
        });
    }


    // ========================================
    // PASO 2: CALCULAR ACTIVO NETO
    // ========================================
    decimal activoNeto = input.ActivoTotal 
                       - input.DepreciacionAmortizacionAcumulada 
                       - input.ReservaCuentasIncobrables 
                       - input.CreditosReinversion;
    
    if (activoNeto < 0) activoNeto = 0;


    // ========================================
    // PASO 3: DETERMINAR MÉTODO DE CÁLCULO
    // ========================================
    // Según video:
    // Si Activo Neto > 4 × Ingresos Brutos → Calcular sobre INGRESOS
    // Si Activo Neto <= 4 × Ingresos Brutos → Calcular sobre 1/4 ACTIVO NETO
    
    decimal cuatroVecesIngresos = 4m * input.IngresosBrutosAnuales;
    bool usarMetodoIngresos = activoNeto > cuatroVecesIngresos;
    
    decimal baseCalculo;
    decimal isoCalculado;
    decimal isoAPagar;
    string metodoSeleccionado;
    string razonMetodo;
    string detalleCalculo;


    // ========================================
    // PASO 4: CALCULAR ISO SEGÚN MÉTODO DETERMINADO
    // ========================================
    
    if (usarMetodoIngresos)
    {
        // ========================================
        // MÉTODO 1: CALCULAR SOBRE INGRESOS
        // ========================================
        // Fórmula: (Ingresos Brutos / 4) × 1%
        
        baseCalculo = input.IngresosBrutosAnuales;
        decimal baseTrimestral = baseCalculo / 4m;
        isoCalculado = baseTrimestral * 0.01m;
        isoAPagar = isoCalculado;  // No se resta IUSI en este método
        
        metodoSeleccionado = "ISO sobre Ingresos Brutos";
        razonMetodo = $"Activo Neto (Q{activoNeto:F2}) > 4×Ingresos (Q{cuatroVecesIngresos:F2})";
        
        detalleCalculo = $"Margen: {margenPorcentaje:F2}% (≥4% ✓); " +
                        $"Activo Neto: Q{activoNeto:F2}; " +
                        $"4×Ingresos: Q{cuatroVecesIngresos:F2}; " +
                        $"Ingresos Anuales: Q{input.IngresosBrutosAnuales:F2}; " +
                        $"Base Trimestral (÷4): Q{baseTrimestral:F2}; " +
                        $"ISO 1%: Q{isoCalculado:F2}";
    }
    else
    {
        // ========================================
        // MÉTODO 2: CALCULAR SOBRE 1/4 DEL ACTIVO NETO
        // ========================================
        // Fórmula: (Activo Neto / 4) × 1% - IUSI
        
        baseCalculo = activoNeto;
        decimal baseTrimestral = baseCalculo / 4m;
        isoCalculado = baseTrimestral * 0.01m;
        
        // Restar IUSI pagado (solo en este método)
        isoAPagar = isoCalculado - input.IUSIPagado;
        if (isoAPagar < 0) isoAPagar = 0;
        
        metodoSeleccionado = "ISO sobre 1/4 del Activo Neto";
        razonMetodo = $"Activo Neto (Q{activoNeto:F2}) ≤ 4×Ingresos (Q{cuatroVecesIngresos:F2})";
        
        detalleCalculo = $"Margen: {margenPorcentaje:F2}% (≥4% ✓); " +
                        $"Activo Neto: Q{activoNeto:F2}; " +
                        $"4×Ingresos: Q{cuatroVecesIngresos:F2}; " +
                        $"Base Trimestral (÷4): Q{baseTrimestral:F2}; " +
                        $"ISO 1%: Q{isoCalculado:F2}; " +
                        $"IUSI: Q{input.IUSIPagado:F2}; " +
                        $"ISO a Pagar: Q{isoAPagar:F2}";
    }


    // ========================================
    // PASO 5: CONSTRUIR RESULTADO
    // ========================================
    var result = new ISOTrimestralResult
    {
        // Paso 1: Verificación margen 4%
        IngresosBrutos = decimal.Round(input.IngresosBrutosAnuales, 2),
        CostoDeVentas = decimal.Round(input.CostoDeVentas, 2),
        ResultadoBruto = decimal.Round(resultadoBruto, 2),
        MargenPorcentaje = decimal.Round(margenPorcentaje, 2),
        EstaAfectoISO = estaAfectoISO,
        
        // Paso 2: Activo
        ActivoTotal = decimal.Round(input.ActivoTotal, 2),
        DepreciacionAmortizacionAcumulada = decimal.Round(input.DepreciacionAmortizacionAcumulada, 2),
        ReservaCuentasIncobrables = decimal.Round(input.ReservaCuentasIncobrables, 2),
        CreditosReinversion = decimal.Round(input.CreditosReinversion, 2),
        ActivoNeto = decimal.Round(activoNeto, 2),
        
        // Paso 3: Decisión del método
        ComparacionActivo = decimal.Round(cuatroVecesIngresos, 2),
        MetodoSeleccionado = metodoSeleccionado,
        RazonMetodo = razonMetodo,
        
        // Paso 4: Cálculo (solo el método usado)
        BaseTrimestralIngresos = usarMetodoIngresos ? decimal.Round(input.IngresosBrutosAnuales / 4m, 2) : 0,
        ISOSobreIngresos = usarMetodoIngresos ? decimal.Round(isoCalculado, 2) : 0,
        
        BaseTrimestralActivo = !usarMetodoIngresos ? decimal.Round(activoNeto / 4m, 2) : 0,
        ISOSobreActivoNeto = !usarMetodoIngresos ? decimal.Round(isoCalculado, 2) : 0,
        IUSIPagado = !usarMetodoIngresos ? decimal.Round(input.IUSIPagado, 2) : 0,
        ISOSobreActivoNetoFinal = !usarMetodoIngresos ? decimal.Round(isoAPagar, 2) : 0,
        
        // Resultado
        ISOAPagar = decimal.Round(isoAPagar, 2),
        
        // Detalles
        DetalleCalculo = detalleCalculo,
        Mensaje = $"Se utiliza el método de {metodoSeleccionado} porque {razonMetodo}.",
        RecomendacionLegal = "La tasa del ISO es del 1% sobre los ingresos por servicios prestados. " +
                           "Cuando los activos son más de 4 veces los ingresos brutos, el cálculo se hace sobre los ingresos. " +
                           "De lo contrario, se realiza sobre la cuarta parte del monto de activo neto."
    };


    // ========================================
    // GUARDAR LOG EN BASE DE DATOS
    // ========================================
    var log = new CalculatorLog
    {
        TipoCalculadora = "ISO Trimestral",
        DatosEntrada = $"Ingresos={input.IngresosBrutosAnuales}; Costos={input.CostoDeVentas}; " +
                      $"Margen={margenPorcentaje:F2}%; ActivoNeto={activoNeto}",
        Resultado = $"Afecto={estaAfectoISO}; ISOAPagar={result.ISOAPagar}; Método={metodoSeleccionado}",
        Fecha = DateTime.Now
    };
    
    _context.CalculatorLogs.Add(log);
    await _context.SaveChangesAsync();


    // ========================================
    // RETORNAR RESPUESTA
    // ========================================
    return Ok(new 
    { 
        exito = true, 
        datos = result, 
        mensaje = "ISO trimestral calculado correctamente según normativa actualizada (margen 4% + regla activos)." 
    });
}

        // ===========================================================
        // 🆕 CALCULADORA #8 → PRESTACIONES LABORALES COMPLETAS
        // Ruta: POST /api/calculadoras/prestaciones-completas
        // ===========================================================
        [HttpPost("prestaciones-completas")]
        public async Task<IActionResult> CalcularPrestacionesCompletas(
            [FromBody] PrestacionesCompletasInput input)
        {
            // Validaciones básicas
            if (input.SalarioOrdinario <= 0)
            {
                return BadRequest(new { 
                    exito = false, 
                    mensaje = "El salario ordinario debe ser mayor a 0." 
                });
            }

            var inicio = input.FechaInicio.Date;
            var fin = input.FechaFin.Date;

            if (fin < inicio)
            {
                return BadRequest(new { 
                    exito = false, 
                    mensaje = "La fecha fin no puede ser menor que la fecha inicio." 
                });
            }

            // Días trabajados
            var diasTotales = (fin - inicio).TotalDays + 1;

            var result = new PrestacionesCompletasResult();

            // ========================================
            // 1. CALCULAR INDEMNIZACIÓN
            // ========================================
            result.Indemnizacion = CalcularComponenteIndemnizacion(
                input.TipoTerminacion,
                input.SalarioOrdinario,
                input.SalariosUltimos6Meses,
                diasTotales,
                input.MontoPensionIGSS
            );

            // ========================================
            // 2. CALCULAR AGUINALDO
            // ========================================
            result.Aguinaldo = CalcularComponenteAguinaldo(
                input.SalarioOrdinario,
                inicio,
                fin,
                input.YaRecibioAguinaldo
            );

            // ========================================
            // 3. CALCULAR BONO 14
            // ========================================
            result.Bono14 = CalcularComponenteBono14(
                input.SalarioOrdinario,
                inicio,
                fin,
                input.YaRecibiBono14
            );

            // ========================================
            // 4. CALCULAR VACACIONES
            // ========================================
            result.Vacaciones = CalcularComponenteVacaciones(
                input.SalarioOrdinario,
                input.DiasVacacionesPendientes
            );

            // ========================================
            // 5. CALCULAR BONIFICACIÓN Q250
            // ========================================
            result.Bonificacion250 = CalcularComponenteBonificacion250(fin);

            // ========================================
            // CALCULAR TOTAL
            // ========================================
            result.TotalLiquidacion = 
                result.Indemnizacion.Monto +
                result.Aguinaldo.Monto +
                result.Bono14.Monto +
                result.Vacaciones.Monto +
                result.Bonificacion250.Monto;

            // ========================================
            // ADVERTENCIAS Y NOTAS LEGALES
            // ========================================
            AgregarAdvertenciasYNotas(result, input.TipoTerminacion);

            // Log a DB
            var log = new CalculatorLog
            {
                TipoCalculadora = "PrestacionesCompletas",
                DatosEntrada = $"Salario={input.SalarioOrdinario}; " +
                               $"FechaInicio={inicio:dd-MM-yyyy}; " +
                               $"FechaFin={fin:dd-MM-yyyy}; " +
                               $"Tipo={input.TipoTerminacion}",
                Resultado = $"Total={result.TotalLiquidacion}; " +
                            $"Indemnizacion={result.Indemnizacion.Monto}; " +
                            $"Aguinaldo={result.Aguinaldo.Monto}; " +
                            $"Bono14={result.Bono14.Monto}",
                Fecha = DateTime.Now
            };

            _context.CalculatorLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                exito = true,
                datos = result,
                mensaje = "Cálculo de prestaciones completas realizado con éxito."
            });
        }

        // ========================================
        // MÉTODOS AUXILIARES PRIVADOS
        // ========================================

        private ComponenteIndemnizacion CalcularComponenteIndemnizacion(
            TipoTerminacion tipo,
            decimal salarioOrdinario,
            List<decimal> salarios6Meses,
            double diasTotales,
            decimal? pensionIGSS)
        {
            var componente = new ComponenteIndemnizacion();

            // Determinar si aplica indemnización según tipo
            if (tipo == TipoTerminacion.DespidoJustificado || 
                tipo == TipoTerminacion.RenunciaVoluntaria)
            {
                componente.Aplica = false;
                componente.Monto = 0;
                componente.Detalle = "No aplica indemnización según tipo de terminación.";
                return componente;
            }

            componente.Aplica = true;

            // Calcular salario promedio (últimos 6 meses o actual)
            decimal salarioPromedio = salarioOrdinario;
            if (salarios6Meses != null && salarios6Meses.Count > 0)
            {
                salarioPromedio = salarios6Meses.Average();
            }

            // Años trabajados
            decimal aniosEquivalentes = (decimal)diasTotales / 365.25m;

            // Indemnización base
            decimal indemnizacion = salarioPromedio * aniosEquivalentes;

            // Mínimo 3 meses si trabajó más de 3 años
            if (aniosEquivalentes > 3 && indemnizacion < (salarioPromedio * 3))
            {
                indemnizacion = salarioPromedio * 3;
            }

            // Caso especial: Pensión IGSS
            if (tipo == TipoTerminacion.PensionIGSS && pensionIGSS.HasValue)
            {
                if (pensionIGSS.Value >= indemnizacion)
                {
                    componente.Aplica = false;
                    componente.Monto = 0;
                    componente.Detalle = "Pensión IGSS cubre la indemnización completa.";
                    return componente;
                }
                else
                {
                    indemnizacion -= pensionIGSS.Value;
                    componente.Detalle = $"Salario promedio: Q{salarioPromedio:F2}; " +
                                        $"Años: {aniosEquivalentes:F2}; " +
                                        $"Pensión IGSS: Q{pensionIGSS:F2}; " +
                                        $"Diferencia a pagar";
                }
            }
            else
            {
                componente.Detalle = $"Salario promedio: Q{salarioPromedio:F2}; " +
                                    $"Años trabajados: {aniosEquivalentes:F2}; " +
                                    $"Fórmula: Salario × Años";
            }

            componente.Monto = decimal.Round(indemnizacion, 2);
            return componente;
        }

        private ComponenteAguinaldo CalcularComponenteAguinaldo(
            decimal salario,
            DateTime inicio,
            DateTime fin,
            bool yaRecibio)
        {
            var componente = new ComponenteAguinaldo { Aplica = true };

            if (yaRecibio)
            {
                componente.Aplica = false;
                componente.Monto = 0;
                componente.Detalle = "Ya recibió aguinaldo del período actual.";
                return componente;
            }

            // Periodo aguinaldo: 1 dic - 30 nov
            var periodoInicio = new DateTime(fin.Year - 1, 12, 1);
            var periodoFin = new DateTime(fin.Year, 11, 30);

            // Ajustar si el inicio es posterior
            if (inicio > periodoInicio)
                periodoInicio = inicio;

            // Ajustar si el fin es anterior
            if (fin < periodoFin)
                periodoFin = fin;

            // Calcular días
            var dias = (periodoFin - periodoInicio).TotalDays + 1;
            
            if (dias <= 0)
            {
                componente.Aplica = false;
                componente.Monto = 0;
                componente.Detalle = "No hay días en período de aguinaldo.";
                return componente;
            }

            decimal monto = (salario / 365m) * (decimal)dias;
            componente.Monto = decimal.Round(monto, 2);
            componente.Detalle = $"Período: {periodoInicio:dd-MM-yyyy} a {periodoFin:dd-MM-yyyy}; " +
                                $"Días: {dias}; Fórmula: (Q{salario}/365)×{dias}";

            return componente;
        }

        private ComponenteBono14 CalcularComponenteBono14(
            decimal salario,
            DateTime inicio,
            DateTime fin,
            bool yaRecibio)
        {
            var componente = new ComponenteBono14 { Aplica = true };

            if (yaRecibio)
            {
                componente.Aplica = false;
                componente.Monto = 0;
                componente.Detalle = "Ya recibió Bono 14 del período actual.";
                return componente;
            }

            // Periodo bono 14: 1 julio - 30 junio
            var periodoInicio = new DateTime(fin.Year - 1, 7, 1);
            var periodoFin = new DateTime(fin.Year, 6, 30);

            if (inicio > periodoInicio)
                periodoInicio = inicio;

            if (fin < periodoFin)
                periodoFin = fin;

            var dias = (periodoFin - periodoInicio).TotalDays + 1;
            
            if (dias <= 0)
            {
                componente.Aplica = false;
                componente.Monto = 0;
                componente.Detalle = "No hay días en período de Bono 14.";
                return componente;
            }

            decimal monto = (salario / 365m) * (decimal)dias;
            componente.Monto = decimal.Round(monto, 2);
            componente.Detalle = $"Período: {periodoInicio:dd-MM-yyyy} a {periodoFin:dd-MM-yyyy}; " +
                                $"Días: {dias}; Fórmula: (Q{salario}/365)×{dias}";

            return componente;
        }

        private ComponenteVacaciones CalcularComponenteVacaciones(
            decimal salario,
            int diasPendientes)
        {
            var componente = new ComponenteVacaciones { Aplica = true };

            if (diasPendientes <= 0)
            {
                componente.Aplica = false;
                componente.Monto = 0;
                componente.Detalle = "No hay días de vacaciones pendientes.";
                return componente;
            }

            decimal monto = (salario / 30m) * diasPendientes;
            componente.Monto = decimal.Round(monto, 2);
            componente.Detalle = $"Días pendientes: {diasPendientes}; " +
                                $"Fórmula: (Q{salario}/30)×{diasPendientes}";

            return componente;
        }

        private ComponenteBonificacion250 CalcularComponenteBonificacion250(DateTime fechaFin)
        {
            var componente = new ComponenteBonificacion250 { Aplica = true };

            int diasDelMes = fechaFin.Day;
            decimal monto = (250m / 30m) * diasDelMes;
            
            componente.Monto = decimal.Round(monto, 2);
            componente.Detalle = $"Días trabajados en {fechaFin:MMMM}: {diasDelMes}; " +
                                $"Fórmula: (Q250/30)×{diasDelMes}";

            return componente;
        }

        private void AgregarAdvertenciasYNotas(
            PrestacionesCompletasResult result, 
            TipoTerminacion tipo)
        {
            // Advertencias
            if (!result.Indemnizacion.Aplica)
            {
                result.Advertencias.Add(
                    "No se incluye indemnización según el tipo de terminación seleccionado."
                );
            }

            if (!result.Aguinaldo.Aplica && !result.Bono14.Aplica)
            {
                result.Advertencias.Add(
                    "Ya recibió aguinaldo y bono 14 del período actual."
                );
            }

            // Notas legales
            result.NotasLegales.Add("Este cálculo es una estimación basada en el Código de Trabajo de Guatemala.");
            result.NotasLegales.Add("El pago debe realizarse el último día laboral.");
            result.NotasLegales.Add("Plazo para reclamar indemnización: 30 días hábiles.");
            result.NotasLegales.Add("Plazo para reclamar otras prestaciones: 2 años.");
            result.NotasLegales.Add("Consulte con un abogado laboralista para casos específicos.");
        }
   

        // ===========================================================
        // 🆕 CALCULADORA #9 → ISR EMPRESA MENSUAL V2 - ✅ CORREGIDO
        // Ruta: POST /api/calculadoras/isr-empresa-mensual-v2
        // ===========================================================
        [HttpPost("isr-empresa-mensual-v2")]
        public async Task<IActionResult> CalcularISREmpresaMensualV2([FromBody] ISREmpresaMensualV2Input input)
        {
            if (input.TotalFacturacionMes <= 0)
            {
                return BadRequest(new { exito = false, mensaje = "La facturación debe ser mayor a 0." });
            }
            if (input.TotalRetenciones < 0)
            {
                return BadRequest(new { exito = false, mensaje = "Las retenciones no pueden ser negativas." });
            }

            decimal baseCalculo = input.TotalFacturacionMes / 1.12m;
            decimal iva = baseCalculo * 0.12m;
            
            decimal isrPrimerosTreintaMil = 0;
            decimal isrExcedente = 0;
            
            if (baseCalculo <= 30000m)
            {
                isrPrimerosTreintaMil = baseCalculo * 0.05m;
            }
            else
            {
                isrPrimerosTreintaMil = 30000m * 0.05m;
                decimal excedente = baseCalculo - 30000m;
                isrExcedente = excedente * 0.07m;
            }

            decimal isrTotal = isrPrimerosTreintaMil + isrExcedente;
            decimal isrAPagar = isrTotal - input.TotalRetenciones;
            if (isrAPagar < 0) isrAPagar = 0;

            var result = new ISREmpresaMensualV2Result
            {
                Base = decimal.Round(baseCalculo, 2),
                IVA = decimal.Round(iva, 2),
                ISRPrimerosTreintaMil = decimal.Round(isrPrimerosTreintaMil, 2),
                ISRExcedente = decimal.Round(isrExcedente, 2),
                ISRTotal = decimal.Round(isrTotal, 2),
                ISRAPagar = decimal.Round(isrAPagar, 2),
                DetalleCalculo = $"Facturación: Q{input.TotalFacturacionMes:F2}; Base: Q{baseCalculo:F2}; ISR 5%: Q{isrPrimerosTreintaMil:F2}; ISR 7%: Q{isrExcedente:F2}; Total: Q{isrTotal:F2}; A pagar: Q{isrAPagar:F2}"
            };

            var log = new CalculatorLog
            {
                TipoCalculadora = "ISR Empresa Mensual V2",
                DatosEntrada = $"Facturación={input.TotalFacturacionMes}; Retenciones={input.TotalRetenciones}",
                Resultado = $"ISRTotal={result.ISRTotal}; ISRAPagar={result.ISRAPagar}",
                Fecha = DateTime.Now
            };
            _context.CalculatorLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new { exito = true, datos = result, mensaje = "ISR mensual calculado." });
        }

// ===========================================================
        // 🆕 CALCULADORA #10 → ISR TRIMESTRAL V2 - ✅ ACTUALIZADO
        // Ruta: POST /api/calculadoras/isr-empresa-trimestral-v2
        // ✅ CAMBIOS: Agregados RentasExentas, ISRPagadoAnteriorTrimestre
        // ✅ Opción 2 ahora usa cálculo correcto: (Base × 25%) × 8%
        // ===========================================================
        [HttpPost("isr-empresa-trimestral-v2")]
        public async Task<IActionResult> CalcularISRTrimestralV2([FromBody] ISRTrimestralV2Input input)
        {
            // Validaciones básicas
            if (input.ISOPendiente < 0)
            {
                return BadRequest(new { exito = false, mensaje = "El ISO pendiente no puede ser negativo." });
            }

            if (input.RentasExentas < 0)
            {
                return BadRequest(new { exito = false, mensaje = "Las rentas exentas no pueden ser negativas." });
            }

            if (input.ISRPagadoAnteriorTrimestre < 0)
            {
                return BadRequest(new { exito = false, mensaje = "El ISR pagado anterior no puede ser negativo." });
            }

            ISRTrimestralV2Result result;

            // ========================================
            // OPCIÓN 1: CIERRES PARCIALES (ACUMULADO)
            // ========================================
            if (input.UsarOpcionAcumulada)
            {
                if (input.VentasAcumuladas < 0 || input.GastosAcumulados < 0)
                {
                    return BadRequest(new { exito = false, mensaje = "Las ventas y gastos no pueden ser negativos." });
                }

                // Base = Ventas - Rentas Exentas - Gastos
                decimal baseCalculo = input.VentasAcumuladas - input.RentasExentas - input.GastosAcumulados;
                
                if (baseCalculo <= 0)
                {
                    result = new ISRTrimestralV2Result
                    {
                        OpcionUtilizada = "Opción 1 - Acumulado",
                        BaseCalculo = 0,
                        ISRCalculado = 0,
                        ISR25Porciento = 0,
                        ISR8Porciento = 0,
                        ISOAcreditar = 0,
                        ISRPagadoAnterior = 0,
                        ISRAPagar = 0,
                        DetalleCalculo = $"Ventas: Q{input.VentasAcumuladas:F2}; Rentas Exentas: -Q{input.RentasExentas:F2}; Gastos: -Q{input.GastosAcumulados:F2}; No aplica ISR"
                    };
                }
                else
                {
                    // ISR = Base × 25%
                    decimal isrCalculado = baseCalculo * 0.25m;
                    
                    // ISO a acreditar (no puede ser mayor al ISR calculado)
                    decimal isoAcreditar = Math.Min(input.ISOPendiente, isrCalculado);
                    
                    // ISR a pagar = ISR - ISO - ISR Anterior
                    decimal isrAPagar = isrCalculado - isoAcreditar - input.ISRPagadoAnteriorTrimestre;
                    if (isrAPagar < 0) isrAPagar = 0;

                    result = new ISRTrimestralV2Result
                    {
                        OpcionUtilizada = "Opción 1 - Acumulado",
                        BaseCalculo = decimal.Round(baseCalculo, 2),
                        ISRCalculado = decimal.Round(isrCalculado, 2),
                        ISR25Porciento = 0, // No se muestra en Opción 1
                        ISR8Porciento = 0,  // No aplica en Opción 1
                        ISOAcreditar = decimal.Round(isoAcreditar, 2),
                        ISRPagadoAnterior = decimal.Round(input.ISRPagadoAnteriorTrimestre, 2),
                        ISRAPagar = decimal.Round(isrAPagar, 2),
                        DetalleCalculo = $"Ventas: Q{input.VentasAcumuladas:F2}; Rentas Exentas: -Q{input.RentasExentas:F2}; Gastos: -Q{input.GastosAcumulados:F2}; Resultado: Q{baseCalculo:F2}; Resultado x25%: Q{isrCalculado:F2}; ISO: -Q{isoAcreditar:F2}; ISR anterior: -Q{input.ISRPagadoAnteriorTrimestre:F2}; ISR x pagar: Q{isrAPagar:F2}"
                    };
                }
            }
            // ========================================
            // OPCIÓN 2: TRIMESTRE DIRECTO
            // ========================================
            else
            {
                if (input.VentasTrimestre < 0)
                {
                    return BadRequest(new { exito = false, mensaje = "Las ventas no pueden ser negativas." });
                }

                if (input.VentasTrimestre == 0)
                {
                    result = new ISRTrimestralV2Result
                    {
                        OpcionUtilizada = "Opción 2 - Trimestre",
                        BaseCalculo = 0,
                        ISRCalculado = 0,
                        ISR25Porciento = 0,
                        ISR8Porciento = 0,
                        ISOAcreditar = 0,
                        ISRPagadoAnterior = 0,
                        ISRAPagar = 0,
                        DetalleCalculo = "Ventas: Q0.00; No aplica ISR"
                    };
                }
                else
                {
                    // Base = Ventas - Rentas Exentas
                    decimal baseCalculo = input.VentasTrimestre - input.RentasExentas;
                    
                    if (baseCalculo <= 0)
                    {
                        result = new ISRTrimestralV2Result
                        {
                            OpcionUtilizada = "Opción 2 - Trimestre",
                            BaseCalculo = 0,
                            ISRCalculado = 0,
                            ISR25Porciento = 0,
                            ISR8Porciento = 0,
                            ISOAcreditar = 0,
                            ISRPagadoAnterior = 0,
                            ISRAPagar = 0,
                            DetalleCalculo = $"Ventas: Q{input.VentasTrimestre:F2}; Rentas Exentas: -Q{input.RentasExentas:F2}; No aplica ISR"
                        };
                    }
                    else
                    {
                        // Paso 1: ISR 25% = Base × 25%
                        decimal isr25Porciento = baseCalculo * 0.25m;
                        
                        // Paso 2: ISR 8% = ISR 25% × 8% (equivale a Base × 2%)
                        decimal isr8Porciento = isr25Porciento * 0.08m;
                        
                        // ISO a acreditar (no puede ser mayor al ISR calculado)
                        decimal isoAcreditar = Math.Min(input.ISOPendiente, isr8Porciento);
                        
                        // ISR a pagar = ISR 8% - ISO
                        decimal isrAPagar = isr8Porciento - isoAcreditar;
                        if (isrAPagar < 0) isrAPagar = 0;

                        result = new ISRTrimestralV2Result
                        {
                            OpcionUtilizada = "Opción 2 - Trimestre",
                            BaseCalculo = decimal.Round(baseCalculo, 2),
                            ISRCalculado = decimal.Round(isr8Porciento, 2), // El ISR final es el 8%
                            ISR25Porciento = decimal.Round(isr25Porciento, 2),
                            ISR8Porciento = decimal.Round(isr8Porciento, 2),
                            ISOAcreditar = decimal.Round(isoAcreditar, 2),
                            ISRPagadoAnterior = 0, // No aplica en Opción 2
                            ISRAPagar = decimal.Round(isrAPagar, 2),
                            DetalleCalculo = $"Ventas: Q{input.VentasTrimestre:F2}; Rentas Exentas: -Q{input.RentasExentas:F2}; Resultado: Q{baseCalculo:F2}; Resultado x25%: Q{isr25Porciento:F2}; Resultado x8%: Q{isr8Porciento:F2}; ISO: -Q{isoAcreditar:F2}; Total: Q{isrAPagar:F2}"
                        };
                    }
                }
            }

            // Log en base de datos
            var log = new CalculatorLog
            {
                TipoCalculadora = "ISR Trimestral V2",
                DatosEntrada = input.UsarOpcionAcumulada 
                    ? $"Ventas={input.VentasAcumuladas}; Rentas Exentas={input.RentasExentas}; Gastos={input.GastosAcumulados}; ISO={input.ISOPendiente}; ISR Anterior={input.ISRPagadoAnteriorTrimestre}"
                    : $"Ventas={input.VentasTrimestre}; Rentas Exentas={input.RentasExentas}; ISO={input.ISOPendiente}",
                Resultado = $"ISR={result.ISRCalculado}; APagar={result.ISRAPagar}",
                Fecha = DateTime.Now
            };
            _context.CalculatorLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new { exito = true, datos = result, mensaje = $"ISR trimestral calculado ({result.OpcionUtilizada})." });
        }
// ===========================================================
        // CALCULADORA IVA - ACTUALIZADO
        // Ruta: POST /api/calculadoras/iva
        // Calcula el IVA según el régimen: General, Pequeño Contribuyente, Exento
        // 
        // CAMBIOS:
        // - Agregados 3 campos de deducciones separados
        // - Cálculo actualizado: IVA Bruto - IVACredito - IVARetenido - IVAExento
        // - Muestra si es "IVA POR PAGAR" o "IVA CRÉDITO"
        // ===========================================================
        [HttpPost("iva")]
        public async Task<IActionResult> CalcularIVA([FromBody] IVAInput input)
        {
            IVAResult result;

            switch (input.Regimen)
            {
                case RegimenIVA.General:
                    // ====================================
                    // RÉGIMEN GENERAL (12%)
                    // ====================================
                    
                    if (input.VentasMes < 0 || input.ComprasMes < 0)
                    {
                        return BadRequest(new { exito = false, mensaje = "Los montos de ventas y compras no pueden ser negativos." });
                    }
                    
                    if (input.IVACredito < 0 || input.IVARetenido < 0 || input.IVAExento < 0)
                    {
                        return BadRequest(new { exito = false, mensaje = "Las deducciones no pueden ser negativas." });
                    }

                    // Calcular base y débito fiscal (IVA en ventas)
                    decimal baseVentas = input.VentasMes / 1.12m;
                    decimal debitoFiscal = baseVentas * 0.12m;

                    // Calcular base y crédito fiscal (IVA en compras)
                    decimal baseCompras = input.ComprasMes / 1.12m;
                    decimal creditoFiscal = baseCompras * 0.12m;

                    // IVA bruto = Débito - Crédito
                    decimal ivaBruto = debitoFiscal - creditoFiscal;

                    // Total de deducciones (3 campos separados)
                    decimal totalDeducciones = input.IVACredito + input.IVARetenido + input.IVAExento;

                    // IVA a pagar = IVA Bruto - Total Deducciones
                    decimal ivaAPagar = ivaBruto - totalDeducciones;
                    
                    // Determinar si es pago o crédito
                    bool esCredito = ivaAPagar < 0;
                    string tipoResultado = esCredito ? "IVA CRÉDITO" : "IVA POR PAGAR";
                    
                    // Valor absoluto si es crédito para mostrarlo positivo
                    decimal ivaFinal = esCredito ? Math.Abs(ivaAPagar) : ivaAPagar;

                    result = new IVAResult
                    {
                        RegimenNombre = "Régimen General (IVA 12%)",
                        
                        // Bases
                        BaseVentas = decimal.Round(baseVentas, 2),
                        BaseCompras = decimal.Round(baseCompras, 2),
                        
                        // IVA
                        DebitoFiscal = decimal.Round(debitoFiscal, 2),
                        CreditoFiscal = decimal.Round(creditoFiscal, 2),
                        IVABruto = decimal.Round(ivaBruto, 2),
                        
                        // Deducciones separadas
                        IVACredito = decimal.Round(input.IVACredito, 2),
                        IVARetenido = decimal.Round(input.IVARetenido, 2),
                        IVAExento = decimal.Round(input.IVAExento, 2),
                        TotalDeducciones = decimal.Round(totalDeducciones, 2),
                        
                        // Resultado
                        IVAAPagar = decimal.Round(ivaFinal, 2),
                        
                        CuotaFija = 0,
                        Aplica = true,
                        
                        Mensaje = esCredito 
                            ? $"{tipoResultado}: Tienes un saldo a favor de Q{ivaFinal:F2}. Puedes solicitarlo en devolución o acreditarlo al siguiente mes."
                            : $"{tipoResultado}: Debes pagar Q{ivaFinal:F2} este mes.",
                            
                        DetalleCalculo = $"Ventas: Q{input.VentasMes:F2}; Base ventas: Q{baseVentas:F2}; " +
                                       $"Débito fiscal (12%): Q{debitoFiscal:F2}; " +
                                       $"Compras: Q{input.ComprasMes:F2}; Base compras: Q{baseCompras:F2}; " +
                                       $"Crédito fiscal (12%): Q{creditoFiscal:F2}; " +
                                       $"IVA bruto (Débito - Crédito): Q{ivaBruto:F2}; " +
                                       $"(-) IVA crédito: Q{input.IVACredito:F2}; " +
                                       $"(-) IVA retenido: Q{input.IVARetenido:F2}; " +
                                       $"(-) IVA exento: Q{input.IVAExento:F2}; " +
                                       $"Total deducciones: Q{totalDeducciones:F2}; " +
                                       $"{tipoResultado}: Q{ivaFinal:F2}"
                    };
                    break;

                case RegimenIVA.PequenoContribuyente:
                    // ====================================
                    // PEQUEÑO CONTRIBUYENTE (Cuota fija)
                    // ====================================
                    
                    if (input.IngresosAnuales < 0)
                    {
                        return BadRequest(new { exito = false, mensaje = "Los ingresos anuales no pueden ser negativos." });
                    }

                    const decimal limitePequenoContribuyente = 150000m;
                    const decimal cuotaFija = 150m;

                    if (input.IngresosAnuales > limitePequenoContribuyente)
                    {
                        result = new IVAResult
                        {
                            RegimenNombre = "Pequeño Contribuyente",
                            BaseVentas = 0,
                            BaseCompras = 0,
                            DebitoFiscal = 0,
                            CreditoFiscal = 0,
                            IVABruto = 0,
                            IVACredito = 0,
                            IVARetenido = 0,
                            IVAExento = 0,
                            TotalDeducciones = 0,
                            IVAAPagar = 0,
                            CuotaFija = 0,
                            Aplica = false,
                            Mensaje = $"No aplicas para Pequeño Contribuyente. Tus ingresos anuales (Q{input.IngresosAnuales:F2}) " +
                                    $"superan el límite de Q{limitePequenoContribuyente:F2}. Debes inscribirte en Régimen General.",
                            DetalleCalculo = $"Ingresos anuales: Q{input.IngresosAnuales:F2}; " +
                                           $"Límite: Q{limitePequenoContribuyente:F2}; No aplica este régimen."
                        };
                    }
                    else
                    {
                        result = new IVAResult
                        {
                            RegimenNombre = "Pequeño Contribuyente",
                            BaseVentas = 0,
                            BaseCompras = 0,
                            DebitoFiscal = 0,
                            CreditoFiscal = 0,
                            IVABruto = 0,
                            IVACredito = 0,
                            IVARetenido = 0,
                            IVAExento = 0,
                            TotalDeducciones = 0,
                            IVAAPagar = cuotaFija,
                            CuotaFija = cuotaFija,
                            Aplica = true,
                            Mensaje = "Como Pequeño Contribuyente, pagas una cuota fija mensual de Q150.00",
                            DetalleCalculo = $"Ingresos anuales: Q{input.IngresosAnuales:F2}; " +
                                           $"Límite: Q{limitePequenoContribuyente:F2}; " +
                                           $"Cuota fija mensual: Q{cuotaFija:F2}"
                        };
                    }
                    break;

                case RegimenIVA.Exento:
                    // ====================================
                    // EXENTO DE IVA
                    // ====================================
                    
                    result = new IVAResult
                    {
                        RegimenNombre = "Exento de IVA",
                        BaseVentas = 0,
                        BaseCompras = 0,
                        DebitoFiscal = 0,
                        CreditoFiscal = 0,
                        IVABruto = 0,
                        IVACredito = 0,
                        IVARetenido = 0,
                        IVAExento = 0,
                        TotalDeducciones = 0,
                        IVAAPagar = 0,
                        CuotaFija = 0,
                        Aplica = true,
                        Mensaje = "Tu actividad está exenta de IVA. No debes pagar este impuesto.",
                        DetalleCalculo = "Actividad exenta según la Ley del IVA. IVA a pagar: Q0.00"
                    };
                    break;

                default:
                    return BadRequest(new { exito = false, mensaje = "Régimen de IVA no válido." });
            }

            // Log a DB
            var log = new CalculatorLog
            {
                TipoCalculadora = "IVA",
                DatosEntrada = $"Regimen={input.Regimen}; Ventas={input.VentasMes}; Compras={input.ComprasMes}; " +
                              $"IVACredito={input.IVACredito}; IVARetenido={input.IVARetenido}; IVAExento={input.IVAExento}; " +
                              $"IngresosAnuales={input.IngresosAnuales}",
                Resultado = $"IVAAPagar={result.IVAAPagar}; TipoResultado={((result.IVABruto - result.TotalDeducciones) < 0 ? "CREDITO" : "PAGAR")}; Aplica={result.Aplica}",
                Fecha = DateTime.Now
            };
            _context.CalculatorLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new { exito = true, datos = result, mensaje = "IVA calculado con éxito." });
        }
    }
}