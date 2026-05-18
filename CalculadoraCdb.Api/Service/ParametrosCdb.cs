using CalculadoraCdb.Domain.Interfaces;

namespace CalculadoraCdb.Api.Service
{
    public class ParametrosCdb(IConfiguration configuration) : IParametrosCdb
    {
        public decimal Cdi { get; } = configuration.GetValue("ParametrosCdb:Cdi", 0.009m);
        public decimal Tb { get; } = configuration.GetValue("ParametrosCdb:Tb", 1.08m);
    }
}
