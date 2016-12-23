using CsvHelper.Configuration;
using LoanCalculator.Lib.DomainObjects;

namespace LoanCalculator.Lib.Repositories.Implementations
{
    internal class CsvLenderDefinitionMap : CsvClassMap<Lender>
    {
        public CsvLenderDefinitionMap()
        {
            Map(def => def.Name).Name("Lender");
            Map(def => def.Rate).Name("Rate");
            Map(def => def.FundsAvailable).Name("Available");
        }
    }
}
