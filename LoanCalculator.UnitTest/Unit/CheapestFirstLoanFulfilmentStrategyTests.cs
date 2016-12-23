using System.Linq;
using LoanCalculator.Lib.DomainObjects;
using LoanCalculator.Lib.Strategies.Implementation;
using NUnit.Framework;

namespace LoanCalculator.Test.Unit
{
    [TestFixture]
    public class CheapestFirstLoanFulfilmentStrategyTests
    {
        [Test]
        public void Execute_LoanCannotBeFulfilled_WhenNoLendersAreAvailable()
        {
            //  Arrange
            var sut = new CheapestFirstLoanFulfilmentStrategy();

            //  Act
            var results = sut.Execute(3000, Enumerable.Empty<Lender>());

            //  Assert
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Execute_LoanCannotBeFulfilled_WhenLoanIsGreaterThanTotalLendersAvailableAmount()
        {
            //  Arrange
            var lenders = new[]
            {
                new Lender() {Name = "A", FundsAvailable = 2900, Rate = 0.04m},
                new Lender() {Name = "B", FundsAvailable = 200, Rate = 0.035m}
            };

            var sut = new CheapestFirstLoanFulfilmentStrategy();

            //  Act
            var results = sut.Execute(3200, lenders);

            //  Assert
            Assert.AreEqual(0, results.Count());
        }

        [TestCase(2800)]
        [TestCase(3000)]
        public void Execute_LoanIsFulfilledByCheapestLender_WhenCheapestLenderHasSufficientFunds(int loanAmount)
        {
            //  Arrange
            var lenders = new[]
            {
                new Lender() {Name = "A", FundsAvailable = 300, Rate = 0.045m},
                new Lender() {Name = "B", FundsAvailable = 3000, Rate = 0.04m}
            };

            var sut = new CheapestFirstLoanFulfilmentStrategy();

            //  Act
            var results = sut.Execute(loanAmount, lenders).ToList();

            //  Assert
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("B", results.ElementAt(0).Lender.Name);
            Assert.AreEqual(loanAmount, results.ElementAt(0).Amount);
        }

        [TestCase(2800)]
        [TestCase(3000)]
        [TestCase(3200)]
        [TestCase(3400)]
        public void Execute_LoanIsFulfilledByMultipleLendersInCorrectOrder_WhenNoSingleLenderHasSufficientFunds(int loanAmount)
        {
            //  Arrange
            var lenders = new[]
            {
                new Lender() {Name = "A", FundsAvailable = 300, Rate = 0.045m},
                new Lender() {Name = "B", FundsAvailable = 2700, Rate = 0.04m},
                new Lender() {Name = "C", FundsAvailable = 400, Rate = 0.05m}
            }.OrderBy(l => l.Rate);

            var sut = new CheapestFirstLoanFulfilmentStrategy();

            //  Act
            var results = sut.Execute(loanAmount, lenders).ToList();

            //  Assert
            Assert.AreEqual(loanAmount, results.Sum(r => r.Amount));

            for (var i = 0; i < results.Count; i++)
            {
                Assert.AreEqual(lenders.ElementAt(i), results.ElementAt(i).Lender);
            }
        }
    }
}
