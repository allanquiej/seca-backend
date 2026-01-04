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
            $"FechaInicio={inicio:yyyy-MM-dd}; FechaFin={fin:yyyy-MM-dd}; " +
            $"Dias={dias}; AniosEquivalentes={decimal.Round(aniosEquivalentes, 6)}; " +
            $"Formula=SalarioMensual*AniosEquivalentes"
    };

    // Log a DB (sin cambiar BD)
    var log = new CalculatorLog
    {
        TipoCalculadora = "Indemnizacion",
        DatosEntrada = $"Salario={input.SalarioMensual}; FechaInicio={inicio:yyyy-MM-dd}; FechaFin={fin:yyyy-MM-dd}",
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
// CALCULADORA #2 → BONO 14
// Ruta: POST /api/calculadoras/bono14
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

    // Meses equivalentes aproximados
    // (365/12 = 30.4166667)
    decimal mesesEquivalentes = (decimal)dias / (365m / 12m);

    // Bono 14 es proporcional hasta máximo 12 meses
    if (mesesEquivalentes > 12m) mesesEquivalentes = 12m;

    decimal monto = (input.SalarioPromedio / 12m) * mesesEquivalentes;

    var result = new Bono14Result
    {
        MontoBono14 = decimal.Round(monto, 2),
        DetalleCalculo =
            $"SalarioPromedio={input.SalarioPromedio}; " +
            $"FechaInicio={inicio:yyyy-MM-dd}; FechaFin={fin:yyyy-MM-dd}; " +
            $"Dias={dias}; MesesEquivalentes={decimal.Round(mesesEquivalentes, 6)}; " +
            $"Formula=(SalarioPromedio/12)*MesesEquivalentes (cap 12)"
    };

    var log = new CalculatorLog
    {
        TipoCalculadora = "Bono14",
        DatosEntrada = $"SalarioPromedio={input.SalarioPromedio}; FechaInicio={inicio:yyyy-MM-dd}; FechaFin={fin:yyyy-MM-dd}",
        Resultado = $"Monto={result.MontoBono14}; Dias={dias}; MesesEq={decimal.Round(mesesEquivalentes, 6)}",
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
// CALCULADORA #3 → AGUINALDO
// Ruta: POST /api/calculadoras/aguinaldo
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

    decimal mesesEquivalentes = (decimal)dias / (365m / 12m);
    if (mesesEquivalentes > 12m) mesesEquivalentes = 12m;

    decimal monto = (input.SalarioPromedio / 12m) * mesesEquivalentes;

    var result = new AguinaldoResult
    {
        MontoAguinaldo = decimal.Round(monto, 2),
        DetalleCalculo =
            $"SalarioPromedio={input.SalarioPromedio}; " +
            $"FechaInicio={inicio:yyyy-MM-dd}; FechaFin={fin:yyyy-MM-dd}; " +
            $"Dias={dias}; MesesEquivalentes={decimal.Round(mesesEquivalentes, 6)}; " +
            $"Formula=(SalarioPromedio/12)*MesesEquivalentes (cap 12)"
    };

    var log = new CalculatorLog
    {
        TipoCalculadora = "Aguinaldo",
        DatosEntrada = $"SalarioPromedio={input.SalarioPromedio}; FechaInicio={inicio:yyyy-MM-dd}; FechaFin={fin:yyyy-MM-dd}",
        Resultado = $"Monto={result.MontoAguinaldo}; Dias={dias}; MesesEq={decimal.Round(mesesEquivalentes, 6)}",
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


// ===========================================================
// CALCULADORA #4 → ISR LABORAL (Empleado)
// Ruta: POST /api/calculadoras/isr-laboral
// ===========================================================
[HttpPost("isr-laboral")]
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

    // Fórmula simplificada:
    // ISR = 5% del salario mensual
    decimal isr = input.SueldoMensual * 0.05m;

    var result = new ISRResult
    {
        ISRCalculado = isr,
        DetalleCalculo = $"ISR = {input.SueldoMensual} × 0.05"
    };

    // Registrar log en la base de datos
    var log = new CalculatorLog
    {
        TipoCalculadora = "ISR Laboral",
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
        mensaje = "ISR laboral calculado con éxito."
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
// CALCULADORA #7 → ISO TRIMESTRAL
// Ruta: POST /api/calculadoras/iso-trimestral
// ===========================================================
[HttpPost("iso-trimestral")]
public async Task<IActionResult> CalcularISOTrimestral([FromBody] ISOInput input)
{
    if (input.IngresosTrimestrales <= 0)
    {
        return BadRequest(new
        {
            exito = false,
            mensaje = "Los ingresos trimestrales deben ser mayores a 0."
        });
    }

    // Fórmula del ISO: 1% de ingresos trimestrales
    decimal iso = input.IngresosTrimestrales * 0.01m;

    var result = new ISOResult
    {
        ISOCalculado = iso,
        DetalleCalculo = $"ISO = {input.IngresosTrimestrales} × 0.01"
    };

    // Guardar el log en la base
    var log = new CalculatorLog
    {
        TipoCalculadora = "ISO Trimestral",
        DatosEntrada = $"IngresosTrimestrales={input.IngresosTrimestrales}",
        Resultado = $"ISO={result.ISOCalculado}",
        Fecha = DateTime.Now
    };

    _context.CalculatorLogs.Add(log);
    await _context.SaveChangesAsync();

    return Ok(new
    {
        exito = true,
        datos = result,
        mensaje = "ISO trimestral calculado con éxito."
    });
}


        }
    }

