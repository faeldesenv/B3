using CalculadoraCdb.Api.Model;
using CalculadoraCdb.Domain.Interfaces;
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
        /// <param name="calculadoraCdbService">The CDB calculation service.</param>
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
            try
            {
                var result = _calculadoraCdbService.Calculate(
                    request.ValorInvestido!.Value,
                    request.Meses!.Value);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(ex.ParamName ?? string.Empty, ex.Message);
                return ValidationProblem();
            }
        }
    }
}
