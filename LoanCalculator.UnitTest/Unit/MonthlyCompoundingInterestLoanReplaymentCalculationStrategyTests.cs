using LoanCalculator.Lib.Strategies.Implementation;
using NUnit.Framework;

namespace LoanCalculator.Test.Unit
{
    using System;

    [TestFixture]
    public class MonthlyCompoundingInterestLoanReplaymentCalculationStrategyTests
    {
        // Figures calculated using http://www.calculator.net/loan-calculator.html
        // and verified using http://www.calculatorsoup.com/calculators/financial/apr-calculator.php
        //[TestCase(1000, 0.07, 36, 30.88, 1111.58)]
        //[TestCase(3400, 0.062, 36, 103.74, 3734.75)]
        //[TestCase(15000, 0.048, 36, 448.22, 16135.84)]
        //  The following test case uses the figures from the specification
        //  The test does not pass because the figures seem to be calculated
        //  using Annual Percentage Yield instead of monthly compounding
        [TestCase(1000, 0.07, 36, 30.78, 1108.10)]
        public void Execute_CorrectReplaymentValuesAreCalculated_ForTheParametersProvided(
            int loanAmount,
            decimal rate, 
            int termInMonths,
            decimal expectedMonthlyRepayment,
            decimal expectedTotalRepayment)
        {
            //  Arrange
            var sut = new MonthlyCompoundingInterestLoanReplaymentCalculationStrategy();

            //  Act
            var result = sut.Execute(loanAmount, rate, termInMonths);
            
            //  Assert
            Assert.AreEqual(expectedMonthlyRepayment, Math.Round(result.MonthlyRepayment, 2));
            Assert.AreEqual(expectedTotalRepayment, Math.Round(result.TotalPayable, 2));
        }
    }
}
