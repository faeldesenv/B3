using CalculadoraCdb.Api.Interface;
using System.Diagnostics.Metrics;

namespace CalculadoraCdb.Api.Service
{
    public class CalculaTaxaService : ICalculaTaxaService
    {       
        public decimal GetTaxaImposto(int meses)
        {
            return meses switch
            {
                <= 6 => 0.225m,
                <= 12 => 0.20m,
                <= 24 => 0.175m,
                _ => 0.15m
            };
        }
    }
}
