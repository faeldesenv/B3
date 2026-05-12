using System.ComponentModel.DataAnnotations;

namespace CalculadoraCdb.Api.Model
{
    /// <summary>
    /// Responsavel por representar os dados de entrada para o cálculo do CDB, incluindo o valor investido e o período em meses.
    /// </summary>
    public sealed class CalculaCdbRequest
    {
        /// <summary>
        /// Valor do investimento inicial (deve ser maior que zero).
        /// </summary>
        [Required(ErrorMessage = "O valor investido é obrigatório.")]
        public decimal? ValorInvestido { get; set; }

        /// <summary>
        /// Periodo de meses do investimento (deve ser maior que 1 mês).
        /// </summary>
        [Required(ErrorMessage = "O numero de meses é obrigatório.")]
        public int? Meses { get; set; }  
    }
}
