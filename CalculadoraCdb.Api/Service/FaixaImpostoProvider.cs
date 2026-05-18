using CalculadoraCdb.Domain.Interfaces;
using CalculadoraCdb.Domain.Models;

namespace CalculadoraCdb.Api.Service
{
    public class FaixaImpostoProvider : IFaixaImpostoProvider
    {
        private readonly IConfiguration _configuration;

        public FaixaImpostoProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IReadOnlyList<FaixaImposto> GetFaixas()
        {
            return _configuration.GetSection("TabelaImposto:Faixas")
                .Get<List<FaixaImposto>>() ?? [];
        }
    }
}
