namespace LoanCalculator.Lib.Validators
{
    public interface ILoanAmountValidator
    { 
        ValidationResult ValidateLoanAmount(int loanAmount);
    }   
}