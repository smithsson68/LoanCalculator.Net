using LoanCalculator.Lib.Validators.Implementation;
using NUnit.Framework;

namespace LoanCalculator.Test.Unit
{
    [TestFixture]
    public class HardCodedLoanAmountValidatorTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(999)]
        [TestCase(15001)]
        public void ValidateLoanAmount_ShouldReturnInvalidResult_WhenValueOutOfRange(int value)
        {
            //  Arrange
            var sut = new HardCodedLoanAmountValidator();

            //  Act - Assert
            Assert.IsFalse(sut.ValidateLoanAmount(value).Valid);
        }

        [TestCase(1001)]
        [TestCase(2020)]
        [TestCase(5050)]
        [TestCase(14999)]
        public void ValidateLoanAmount_ShouldReturnInvalidResult_WhenValueInRangeButNotMulipleOf100(int value)
        {
            //  Arrange
            var sut = new HardCodedLoanAmountValidator();

            //  Act - Assert
            Assert.IsFalse(sut.ValidateLoanAmount(value).Valid);
        }

        [TestCase(1000)]
        [TestCase(1100)]
        [TestCase(6600)]
        [TestCase(15000)]
        public void ValidateLoanAmount_ShouldReturnValidResult_WhenValueIsInRangeAndMultipleOf100(int value)
        {
            //  Arrange
            var sut = new HardCodedLoanAmountValidator();

            //  Act - Assert
            Assert.IsTrue(sut.ValidateLoanAmount(value).Valid);
        }
    }
}
