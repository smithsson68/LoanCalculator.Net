using System.Collections.Generic;
using LoanCalculator.Lib.DomainObjects;

namespace LoanCalculator.Lib.Strategies
{
    public interface IInterestRateCalculationStrategy
    {
        decimal Execute(IEnumerable<LoanFulfilment> loanFulfilments);
    }
}
