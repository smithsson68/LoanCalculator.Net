using System;
using LoanCalculator.Lib.DomainObjects;

namespace LoanCalculator.Lib.Strategies.Implementation
{
    public class MonthlyCompoundingInterestLoanReplaymentCalculationStrategy : ILoanRepaymentCalculationStrategy
    {
        public LoanRepaymentResult Execute(int loanAmount, decimal rate, int termInMonths)
        {
            //  Algorith derived from https://en.wikipedia.org/wiki/Amortization_calculator#The_formula

            //var periodicRate = (double) rate/12;
            var monthlyRate = Math.Pow((double)(1 + rate), 1.0 / 12.00) - 1;
            //var factor = monthlyRate + (monthlyRate / (Math.Pow(1 + monthlyRate, termInMonths)-1));
            var monthlyRepayment = loanAmount * monthlyRate * Math.Pow(monthlyRate + 1, termInMonths) / (Math.Pow(monthlyRate + 1, termInMonths) -1);
            var totalPayable = monthlyRepayment*termInMonths;

            return new LoanRepaymentResult()
            {
                MonthlyRepayment = (decimal)monthlyRepayment,
                TotalPayable = (decimal)totalPayable
            };
        }
    }
}