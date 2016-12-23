namespace LoanCalculator.Lib.Views.Implementation
{
    public class NoLoanPossibleView : IView
    {
        public void Render(IDisplay display)
        {
            display.WriteLine("It is not possible to provide a quote at this time");
        }
    }
}
