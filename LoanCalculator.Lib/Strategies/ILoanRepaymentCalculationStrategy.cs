using LoanCalculator.Lib.DomainObjects;

namespace LoanCalculator.Lib.Strategies
{
    public interface ILoanRepaymentCalculationStrategy
    {
        LoanRepaymentResult Execute(int loanAmount, decimal rate, int termInMonths);
    }
}
