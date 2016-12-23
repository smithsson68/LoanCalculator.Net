
using System;

namespace LoanCalculator.Lib.Validators.Implementation
{
    public class HardCodedLoanAmountValidator : ILoanAmountValidator
    {
        private const int MinLoanAmount = 1000;
        private const int MaxLoanAmount = 15000;

        public ValidationResult ValidateLoanAmount(int loanAmount)
        {
            var result = new ValidationResult
            {
                Valid = IsLoanAmountInRange(loanAmount) && IsLoanAmountIncrementOf100(loanAmount)
            };

            if (!result.Valid)
            {
                result.ErrorMessage = string.Format(
                    "Loan amount is invalid - please supply a value between £{0} and £{1} to the nearest £100",
                    MinLoanAmount, MaxLoanAmount);
            }

            return result;
        }

        private static bool IsLoanAmountInRange(int loanAmount)
        {
            return loanAmount >= MinLoanAmount && loanAmount <= MaxLoanAmount;
        }

        private static bool IsLoanAmountIncrementOf100(int loanAmount)
        {
            return loanAmount%100 == 0;
        }
    }
}
