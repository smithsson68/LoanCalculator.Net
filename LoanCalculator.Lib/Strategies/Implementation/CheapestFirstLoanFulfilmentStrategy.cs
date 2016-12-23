using System;
using System.Collections.Generic;
using System.Linq;
using LoanCalculator.Lib.DomainObjects;

namespace LoanCalculator.Lib.Strategies.Implementation
{
    public class CheapestFirstLoanFulfilmentStrategy : ILoanFulfilmentStrategy
    {
        public IEnumerable<LoanFulfilment> Execute(int loanAmount, IEnumerable<Lender> lenders)
        {
            if (lenders.Sum(l => l.FundsAvailable) < loanAmount)
            {
                yield break;
            }

            var lendersByPrice = lenders.OrderBy(l => l.Rate);
            var loanAmountFulfilled = 0;

            foreach (var lender in lendersByPrice)
            {
                var loanToFulfil = loanAmount - loanAmountFulfilled;

                if (loanToFulfil == 0)
                {
                    break;
                }

                var loanFromThisLender = Math.Min(loanToFulfil, lender.FundsAvailable);
                loanAmountFulfilled += loanFromThisLender;
                yield return new LoanFulfilment() { Amount = loanFromThisLender, Lender = lender };
            }
        }
    }
}
