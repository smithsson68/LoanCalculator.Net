namespace LoanCalculator.Lib.Views.Implementation
{
    public class ErrorView : IView
    {
        private readonly string _message;

        public ErrorView(string message)
        {
            _message = message;
        }
        public void Render(IDisplay display)
        {
            display.WriteLine(_message);
        }
    }
}
