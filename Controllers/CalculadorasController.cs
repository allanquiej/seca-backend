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
        // 🆕 CALCULADORA #8 → ISR ASALARIADO - ✅ CORREGIDO
        // Ruta: POST /api/calculadoras/isr-asalariado
        // ===========================================================
        [HttpPost("isr-asalariado")]
        public async Task<IActionResult> CalcularISRAsalariado([FromBody] ISRAsalariadoInput input)
        {
            if (input.SalariosAnuales < 0 || input.Bono14 < 0 || input.Aguinaldo < 0 || input.OtrosBonos < 0)
            {
                return BadRequest(new { exito = false, mensaje = "Los ingresos no pueden ser negativos." });
            }

            decimal totalIngresos = input.SalariosAnuales + input.Bono14 + input.Aguinaldo + input.OtrosBonos;
            const decimal deduccionPersonal = 48000m;
            decimal baseImponible = totalIngresos - deduccionPersonal;
            
            if (baseImponible <= 0)
            {
                var resultSinISR = new ISRAsalariadoResult
                {
                    TotalIngresos = totalIngresos,
                    DeduccionPersonal = deduccionPersonal,
                    BaseImponible = 0,
                    ISRTotal = 0,
                    ISRMensual = 0,
                    TipoCalculo = input.EsProyectado ? "Proyectado" : "Definitiva",
                    DetalleCalculo = $"Total ingresos: Q{totalIngresos:F2}; Deducción: Q{deduccionPersonal:F2}; No aplica ISR"
                };
                return Ok(new { exito = true, datos = resultSinISR, mensaje = "No aplica ISR." });
            }

            decimal isrTotal = baseImponible * 0.05m;
            decimal isrMensual = input.EsProyectado ? (isrTotal / 12m) : 0;

            var result = new ISRAsalariadoResult
            {
                TotalIngresos = decimal.Round(totalIngresos, 2),
                DeduccionPersonal = decimal.Round(deduccionPersonal, 2),
                BaseImponible = decimal.Round(baseImponible, 2),
                ISRTotal = decimal.Round(isrTotal, 2),
                ISRMensual = decimal.Round(isrMensual, 2),
                TipoCalculo = input.EsProyectado ? "Proyectado" : "Definitiva",
                DetalleCalculo = $"Total: Q{totalIngresos:F2}; Deducción: Q{deduccionPersonal:F2}; Base: Q{baseImponible:F2}; ISR: Q{isrTotal:F2}" +
                               (input.EsProyectado ? $"; Mensual: Q{isrMensual:F2}" : "")
            };

            var log = new CalculatorLog
            {
                TipoCalculadora = "ISR Asalariado",
                DatosEntrada = $"Salarios={input.SalariosAnuales}; Bono14={input.Bono14}; Aguinaldo={input.Aguinaldo}",
                Resultado = $"ISRTotal={result.ISRTotal}; ISRMensual={result.ISRMensual}",
                Fecha = DateTime.Now
            };
            _context.CalculatorLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new { exito = true, datos = result, mensaje = "ISR asalariado calculado." });
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
        // 🆕 CALCULADORA #10 → ISR TRIMESTRAL V2 - ✅ CORREGIDO
        // Ruta: POST /api/calculadoras/isr-empresa-trimestral-v2
        // ===========================================================
        [HttpPost("isr-empresa-trimestral-v2")]
        public async Task<IActionResult> CalcularISRTrimestralV2([FromBody] ISRTrimestralV2Input input)
        {
            if (input.ISOPendiente < 0)
            {
                return BadRequest(new { exito = false, mensaje = "El ISO pendiente no puede ser negativo." });
            }

            ISRTrimestralV2Result result;

            if (input.UsarOpcionAcumulada)
            {
                if (input.VentasAcumuladas < 0 || input.GastosAcumulados < 0)
                {
                    return BadRequest(new { exito = false, mensaje = "Las ventas y gastos no pueden ser negativos." });
                }

                decimal baseCalculo = input.VentasAcumuladas - input.GastosAcumulados;
                
                if (baseCalculo <= 0)
                {
                    result = new ISRTrimestralV2Result
                    {
                        OpcionUtilizada = "Opción 1 - Acumulado",
                        BaseCalculo = 0,
                        ISRCalculado = 0,
                        ISOAcreditar = 0,
                        ISRAPagar = 0,
                        DetalleCalculo = $"Ventas: Q{input.VentasAcumuladas:F2}; Gastos: Q{input.GastosAcumulados:F2}; No aplica ISR"
                    };
                }
                else
                {
                    decimal isrCalculado = baseCalculo * 0.25m;
                    decimal isoAcreditar = Math.Min(input.ISOPendiente, isrCalculado);
                    decimal isrAPagar = isrCalculado - isoAcreditar;
                    if (isrAPagar < 0) isrAPagar = 0;

                    result = new ISRTrimestralV2Result
                    {
                        OpcionUtilizada = "Opción 1 - Acumulado",
                        BaseCalculo = decimal.Round(baseCalculo, 2),
                        ISRCalculado = decimal.Round(isrCalculado, 2),
                        ISOAcreditar = decimal.Round(isoAcreditar, 2),
                        ISRAPagar = decimal.Round(isrAPagar, 2),
                        DetalleCalculo = $"Ventas: Q{input.VentasAcumuladas:F2}; Gastos: Q{input.GastosAcumulados:F2}; Base: Q{baseCalculo:F2}; ISR 25%: Q{isrCalculado:F2}; ISO: Q{isoAcreditar:F2}; A pagar: Q{isrAPagar:F2}"
                    };
                }
            }
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
                        ISOAcreditar = 0,
                        ISRAPagar = 0,
                        DetalleCalculo = "Ventas: Q0.00; No aplica ISR"
                    };
                }
                else
                {
                    decimal baseCalculo = input.VentasTrimestre;
                    decimal isrCalculado = baseCalculo * 0.25m;
                    decimal isoAcreditar = Math.Min(input.ISOPendiente, isrCalculado);
                    decimal isrAPagar = isrCalculado - isoAcreditar;
                    if (isrAPagar < 0) isrAPagar = 0;

                    result = new ISRTrimestralV2Result
                    {
                        OpcionUtilizada = "Opción 2 - Trimestre",
                        BaseCalculo = decimal.Round(baseCalculo, 2),
                        ISRCalculado = decimal.Round(isrCalculado, 2),
                        ISOAcreditar = decimal.Round(isoAcreditar, 2),
                        ISRAPagar = decimal.Round(isrAPagar, 2),
                        DetalleCalculo = $"Ventas: Q{input.VentasTrimestre:F2}; ISR 25%: Q{isrCalculado:F2}; ISO: Q{isoAcreditar:F2}; A pagar: Q{isrAPagar:F2}"
                    };
                }
            }

            var log = new CalculatorLog
            {
                TipoCalculadora = "ISR Trimestral V2",
                DatosEntrada = input.UsarOpcionAcumulada 
                    ? $"Ventas={input.VentasAcumuladas}; Gastos={input.GastosAcumulados}; ISO={input.ISOPendiente}"
                    : $"Ventas={input.VentasTrimestre}; ISO={input.ISOPendiente}",
                Resultado = $"ISR={result.ISRCalculado}; APagar={result.ISRAPagar}",
                Fecha = DateTime.Now
            };
            _context.CalculatorLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(new { exito = true, datos = result, mensaje = $"ISR trimestral calculado ({result.OpcionUtilizada})." });
        }
    }
}