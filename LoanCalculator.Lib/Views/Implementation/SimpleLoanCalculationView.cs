using LoanCalculator.Lib.DomainObjects;

namespace LoanCalculator.Lib.Views.Implementation
{
    public class SimpleLoanCalculationView : IView
    {
        public SimpleLoanCalculationView(int requestedAmount, decimal rate, LoanRepaymentResult loanRepaymentResult)
        {
            RequestedAmount = requestedAmount;
            Rate = rate;
            LoanRepaymentResult = loanRepaymentResult;
        }

        public int RequestedAmount { get; private set; }
        public decimal Rate { get; private set; }
        public LoanRepaymentResult LoanRepaymentResult { get; private set; }

        public void Render(IDisplay display)
        {
            display.WriteLine(string.Format("Requested amount: £{0}", RequestedAmount.ToString("F0")));
            display.WriteLine(string.Format("Rate: {0}%", (Rate*100).ToString("F1")));
            display.WriteLine(string.Format("Monthly repayment: £{0}", LoanRepaymentResult.MonthlyRepayment.ToString("F2")));
            display.WriteLine(string.Format("Total repayment: £{0}", LoanRepaymentResult.TotalPayable.ToString("F2")));
        }
    }
}
