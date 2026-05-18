namespace CalculadoraCdb.Domain.Interfaces
{
    /// <summary>
    /// Parâmetros de mercado utilizados no cálculo do CDB (CDI e Taxa do Banco).
    /// </summary>
    public interface IParametrosCdb
    {
        decimal Cdi { get; }
        decimal Tb { get; }
    }
}
