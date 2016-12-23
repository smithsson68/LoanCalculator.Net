using System.Collections.Generic;
using LoanCalculator.Lib.DomainObjects;

namespace LoanCalculator.Lib.Strategies
{
    public interface ILoanFulfilmentStrategy
    {
        IEnumerable<LoanFulfilment> Execute(int loanAmount, IEnumerable<Lender> lenders);
    }
}
