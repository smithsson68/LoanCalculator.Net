using LoanCalculator.Lib;
using LoanCalculator.Lib.DomainObjects;
using LoanCalculator.Lib.Views.Implementation;
using Moq;
using NUnit.Framework;

namespace LoanCalculator.Test.Unit
{
    [TestFixture]
    public class SimpleLoanCalculationViewTests
    {
        [Test]
        public void Render_ShouldWriteToDisplay_WithGivenValues()
        {
            //  Arrange
            const int requestedAmount = 3400;
            const decimal rate = 0.062m;
            var loanRepaymentResult = new LoanRepaymentResult()
            {
                MonthlyRepayment = 103.74m,
                TotalPayable = 3734.75m
            };

            var mockDisplay = new Mock<IDisplay>();
            mockDisplay.Setup(d => d.WriteLine("Requested amount: £3400"));
            mockDisplay.Setup(d => d.WriteLine("Rate: 6.2%"));
            mockDisplay.Setup(d => d.WriteLine("Monthly repayment: £103.74"));
            mockDisplay.Setup(d => d.WriteLine("Total repayment: £3734.75"));

            var sut = new SimpleLoanCalculationView(requestedAmount, rate, loanRepaymentResult);

            //  Act
            sut.Render(mockDisplay.Object);

            //  Assert
            mockDisplay.VerifyAll();
        }
    }
}
