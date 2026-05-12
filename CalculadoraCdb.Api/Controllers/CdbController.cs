using CalculadoraCdb.Api.Interface;
using CalculadoraCdb.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCdb.Api.Controllers
{
    [Tags("Cálculos do CDB")]
    [ApiController]
    [Route("api/[controller]")]
    public class CdbController : ControllerBase
    {
        private readonly ICalculadoraCdbService _calculadoraCdbService;

        /// <summary>
        /// Initializes a new instance of <see cref="CdbController"/>.
        /// </summary>
        /// <param name="calculatorService">The CDB calculation service.</param>
        public CdbController(ICalculadoraCdbService calculadoraCdbService)
        {
            _calculadoraCdbService = calculadoraCdbService;
        }

        /// <summary>
        /// Calculates gross and net CDB investment values.
        /// </summary>
        /// <param name="request">The calculation request with initial value and period.</param>
        /// <returns>Gross and net values of the investment.</returns>
        [HttpPost("calculate")]
        [ProducesResponseType(typeof(CalculaCdbRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult Calculate([FromBody] CalculaCdbRequest request)
        {
            if (request.ValorInvestido <= 0)
            {
                ModelState.AddModelError(nameof(request.ValorInvestido), "O valor investido deve ser positivo.");
                return ValidationProblem();
            }

            if (request.Meses <= 1)
            {
                ModelState.AddModelError(nameof(request.Meses), "O prazo deve ser maior que 1 mês.");
                return ValidationProblem();
            }

            var result = _calculadoraCdbService.Calculate(request.ValorInvestido, request.Meses);
            return Ok(result);
        }
    }
}
