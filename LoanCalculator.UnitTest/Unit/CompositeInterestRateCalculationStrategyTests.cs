using System.CodeDom;
using LoanCalculator.Lib.DomainObjects;
using LoanCalculator.Lib.Strategies.Implementation;
using NUnit.Framework;

namespace LoanCalculator.Test.Unit
{
    [TestFixture]
    public class CompositeInterestRateCalculationStrategyTests
    {
        [Test]
        public void Execute_CorrectRateIsCalculated_FromSingleLoanFulfilment()
        {
            //  Arrange
            var loanFulfilments = new[]
            {
                new LoanFulfilment() { Amount = 300, Lender = new Lender() { Name = "A", FundsAvailable = 400, Rate = 0.045m }}
            };

            var sut = new CompositeInterestRateCalculationStrategy();

            //  Act
            var rate = sut.Execute(loanFulfilments);

            //  Assert
            Assert.AreEqual(0.045m, rate);
        }

        [Test]
        public void Execute_CorrectRateIsCalculated_FromMultipleFulfilments()
        {
            //  Arrange
            var loanFulfilments = new[]
            {
                new LoanFulfilment() { Amount = 300, Lender = new Lender() { Name = "A", FundsAvailable = 300, Rate = 0.045m }},
                new LoanFulfilment() { Amount = 600, Lender = new Lender() { Name = "B", FundsAvailable = 600, Rate = 0.05m }},
                new LoanFulfilment() { Amount = 1000, Lender = new Lender() { Name = "C", FundsAvailable = 2000, Rate = 0.0675m }}
            };

            var sut = new CompositeInterestRateCalculationStrategy();

            //  Act
            var rate = sut.Execute(loanFulfilments);

            //  Assert
            Assert.AreEqual(0.058m, rate);
        }
    }
}
