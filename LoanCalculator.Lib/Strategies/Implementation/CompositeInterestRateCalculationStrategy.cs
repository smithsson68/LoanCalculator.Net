using System.Collections.Generic;
using System.Linq;
using LoanCalculator.Lib.DomainObjects;

namespace LoanCalculator.Lib.Strategies.Implementation
{
    public class CompositeInterestRateCalculationStrategy : IInterestRateCalculationStrategy
    {
        public decimal Execute(IEnumerable<LoanFulfilment> loanFulfilments)
        {
            return loanFulfilments.Sum(f => f.Amount*f.Lender.Rate)/loanFulfilments.Sum(f => f.Amount);
        }
    }
}
