namespace CalculadoraCdb.Api.Model
{
    /// <summary>
    /// Representa os dados de saída do cálculo do CDB.
    /// </summary>
    public class CalculaCdbResponse
    {

        /// <summary>
        /// Valor do investimento informado sem a aplicação das taxas, ou seja, o valor bruto.
        /// </summary>
        public decimal ValorBruto { get; set; }


        /// <summary>
        /// Valor do investimento após a aplicação dos juros, ou seja, o valor liquido.
        /// </summary>
        public decimal ValorLiquido { get; set; }
    }
}
