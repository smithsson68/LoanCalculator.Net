using System;
using System.Collections.Generic;
using System.Linq;
using LoanCalculator.Lib;
using LoanCalculator.Lib.DomainObjects;
using LoanCalculator.Lib.Repositories;
using LoanCalculator.Lib.Strategies;
using LoanCalculator.Lib.Validators;
using LoanCalculator.Lib.Views.Implementation;
using Moq;
using NUnit.Framework;

namespace LoanCalculator.Test.Unit
{
    [TestFixture]
    public class LoanCalculationControllerTests
    {
        private Mock<ILoanAmountValidator> _mockLoanAmountValidator;
        private Mock<ILenderRepository> _mockLenderRepository;
        private Mock<ILoanFulfilmentStrategy> _mockLoanFulfilmentStrategy;
        private Mock<IInterestRateCalculationStrategy> _mockInterestRateCalculationStrategy;
        private Mock<ILoanRepaymentCalculationStrategy> _mockLoanRepaymentCalculationStrategy;
        private ValidationResult _loanAmountValidationResult;
        private LoanCalculationController _sut;

        [SetUp]
        public void Setup()
        {
            _mockLoanAmountValidator = new Mock<ILoanAmountValidator>();
            _mockLenderRepository = new Mock<ILenderRepository>();
            _mockLoanFulfilmentStrategy = new Mock<ILoanFulfilmentStrategy>();
            _mockInterestRateCalculationStrategy = new Mock<IInterestRateCalculationStrategy>();
            _mockLoanRepaymentCalculationStrategy = new Mock<ILoanRepaymentCalculationStrategy>();
            _loanAmountValidationResult = new ValidationResult {Valid = true};

            _sut = new LoanCalculationController(
                _mockLoanAmountValidator.Object,
                _mockLenderRepository.Object,
                _mockLoanFulfilmentStrategy.Object,
                _mockInterestRateCalculationStrategy.Object,
                _mockLoanRepaymentCalculationStrategy.Object);
        }

        [Test]
        public void Execute_ShouldCallLoanAmountValidator_ExactlyOnce()
        {
            //  Arrange
            const int loanAmount = 5000;
            _mockLoanAmountValidator.Setup(v => v.ValidateLoanAmount(loanAmount)).Returns(_loanAmountValidationResult);

            //  Act
            _sut.Execute(loanAmount);

            //  Verify
            _mockLoanAmountValidator.Verify(v => v.ValidateLoanAmount(loanAmount), Times.Once);
        }

        [Test]
        public void Execute_ShouldReturnErrorView_WhenLoanAmountValidatorReturnsFalse()
        {
            //  Arrange
            const int loanAmount = 5000;
            _mockLoanAmountValidator.Setup(v => v.ValidateLoanAmount(loanAmount))
                .Returns(new ValidationResult {Valid = false});

            //  Act
            var view = _sut.Execute(loanAmount);

            //  Assert
            Assert.IsInstanceOf(typeof(ErrorView), view);
        }

        [Test]
        public void Execute_ShouldCallLenderRepositoryList_ExactlyOnce()
        {
            //  Arrange
            const int loanAmount = 1300;
            _mockLoanAmountValidator.Setup(v => v.ValidateLoanAmount(loanAmount)).Returns(_loanAmountValidationResult);
            _mockLenderRepository.Setup(r => r.List()).Returns(Enumerable.Empty<Lender>());
            
            //  Act
            _sut.Execute(loanAmount);

            //  Assert
            _mockLenderRepository.Verify(r => r.List(), Times.Once);
        }

        [Test]
        public void Execute_ShouldCallLoanFulfilmentStrategy_WithInputProvided()
        {
            //  Arrange
            const int loanAmount = 1500;
            _mockLoanAmountValidator.Setup(v => v.ValidateLoanAmount(loanAmount)).Returns(_loanAmountValidationResult);

            var lenders = new[]
            {
                new Lender() {Name = "A", FundsAvailable = 300, Rate = 0.055m},
                new Lender() {Name = "B", FundsAvailable = 200, Rate = 0.060m},
                new Lender() {Name = "C", FundsAvailable = 700, Rate = 0.070m}
            };

            _mockLenderRepository.Setup(r => r.List()).Returns(lenders);
            _mockLoanFulfilmentStrategy.Setup(s => s.Execute(loanAmount, lenders));

            //  Act
            _sut.Execute(loanAmount);

            //  Assert
            _mockLoanFulfilmentStrategy.Verify(s => s.Execute(loanAmount, lenders), Times.Once);
        }

        [Test]
        public void Execute_ShouldCallInterestRateCalculationStrategy_WhenLoanFulfilmentsProvided()
        {
            //  Arrange
            const int loanAmount = 1500;
            _mockLoanAmountValidator.Setup(v => v.ValidateLoanAmount(loanAmount)).Returns(_loanAmountValidationResult);

            var loanFulfilments = new[]
            {
                new LoanFulfilment() {Amount = 300, Lender = new Lender() {Name = "A", FundsAvailable = 300, Rate = 0.03m }},
                new LoanFulfilment() {Amount = 200, Lender = new Lender() {Name = "C", FundsAvailable = 600, Rate = 0.035m }},
            };

            _mockLoanFulfilmentStrategy.Setup(s => s.Execute(It.IsAny<int>(), It.IsAny<IEnumerable<Lender>>()))
                .Returns(loanFulfilments);

            _mockInterestRateCalculationStrategy.Setup(c => c.Execute(loanFulfilments)).Returns(0.032m);

            //  Act
            _sut.Execute(loanAmount);

            //  Assert
            _mockInterestRateCalculationStrategy.Verify(c => c.Execute(loanFulfilments), Times.Once);
        }

        [Test]
        public void Execute_ShouldNotCallInterestRateCalculationStrategy_WhenNoLoanFulfilmentsProvided()
        {
            //  Arrange
            const int loanAmount = 1500;
            _mockLoanAmountValidator.Setup(v => v.ValidateLoanAmount(loanAmount)).Returns(_loanAmountValidationResult);

            _mockLoanFulfilmentStrategy.Setup(s => s.Execute(It.IsAny<int>(), It.IsAny<IEnumerable<Lender>>()))
                .Returns(Enumerable.Empty<LoanFulfilment>());

            _mockInterestRateCalculationStrategy.Setup(c => c.Execute(It.IsAny<IEnumerable<LoanFulfilment>>()));

            //  Act
            _sut.Execute(loanAmount);

            //  Assert
            _mockInterestRateCalculationStrategy.Verify(c => c.Execute(It.IsAny<IEnumerable<LoanFulfilment>>()), Times.Never);
        }

        [Test]
        public void Execute_ShouldCallLoanReplaymentCalculationStrategy_WhenLoanFulfilmentsProvided()
        {
            //  Arrange
            const int loanAmount = 1400;
            const decimal rate = 0.07m;

            _mockLoanAmountValidator.Setup(v => v.ValidateLoanAmount(loanAmount)).Returns(_loanAmountValidationResult);

            _mockLoanFulfilmentStrategy.Setup(s => s.Execute(It.IsAny<int>(), It.IsAny<IEnumerable<Lender>>()))
                .Returns(new[] { new LoanFulfilment() });

            _mockInterestRateCalculationStrategy.Setup(c => c.Execute(It.IsAny<IEnumerable<LoanFulfilment>>()))
                .Returns(rate);

            _mockLoanRepaymentCalculationStrategy.Setup(c => c.Execute(loanAmount, rate, 36));

            //  Act
            _sut.Execute(loanAmount);

            //  Assert
            _mockLoanRepaymentCalculationStrategy.Verify(c => c.Execute(loanAmount, rate, 36), Times.Once);
        }

        [Test]
        public void Execute_ShouldNotCallLoanReplaymentCalculatorStrategy_WhenNoLoanFulfilmentsProvided()
        {
            //  Arrange
            const int loanAmount = 1400;
            const decimal rate = 0.07m;

            _mockLoanAmountValidator.Setup(v => v.ValidateLoanAmount(loanAmount)).Returns(_loanAmountValidationResult);

            _mockLoanFulfilmentStrategy.Setup(s => s.Execute(It.IsAny<int>(), It.IsAny<IEnumerable<Lender>>()))
                .Returns(Enumerable.Empty<LoanFulfilment>());

            _mockLoanRepaymentCalculationStrategy.Setup(c => c.Execute(loanAmount, rate, 36));

            //  Act
            _sut.Execute(loanAmount);

            //  Assert
            _mockLoanRepaymentCalculationStrategy.Verify(c => c.Execute(loanAmount, rate, 36), Times.Never);
        }

        [Test]
        public void Execute_ShouldReturnSimpleLoanCalculationView_WhenLoanFulfilmentsProvided()
        {
            //  Arrange
            const int loanAmount = 1000;
            _mockLoanAmountValidator.Setup(v => v.ValidateLoanAmount(loanAmount)).Returns(_loanAmountValidationResult);
            
            _mockLoanFulfilmentStrategy.Setup(s => s.Execute(It.IsAny<int>(), It.IsAny<IEnumerable<Lender>>()))
                .Returns(new[] { new LoanFulfilment() });

            //  Act
            var view = _sut.Execute(loanAmount);

            //  Assert
            Assert.IsInstanceOf(typeof(SimpleLoanCalculationView), view);
        }

        [Test]
        public void Execute_ShouldReturnSimpleLoanCalculationViewWithCorrectValues_WhenLoanFulfilmentsProvided()
        {
            //  Arrange
            var loanReplaymentResult = new LoanRepaymentResult();
            const decimal rate = 0.07m;
            const int loanAmount = 14900;

            _mockLoanAmountValidator.Setup(v => v.ValidateLoanAmount(loanAmount)).Returns(_loanAmountValidationResult);

            _mockLoanFulfilmentStrategy.Setup(s => s.Execute(It.IsAny<int>(), It.IsAny<IEnumerable<Lender>>()))
                .Returns(new[] { new LoanFulfilment() });

            _mockInterestRateCalculationStrategy.Setup(s => s.Execute(It.IsAny<IEnumerable<LoanFulfilment>>()))
                .Returns(rate);

            _mockLoanRepaymentCalculationStrategy.Setup(c => c.Execute(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<int>()))
                .Returns(loanReplaymentResult);

            //  Act
            var view = (SimpleLoanCalculationView)_sut.Execute(loanAmount);

            //  Assert
            Assert.AreEqual(loanAmount, view.RequestedAmount);
            Assert.AreEqual(rate, view.Rate);
            Assert.AreSame(loanReplaymentResult, view.LoanRepaymentResult);
        }

        [Test]
        public void Execute_ShouldReturnNoLoanPossibleView_WhenNoLoanFulfilmentsProvided()
        {
            //  Arrange
            const int loanAmount = 15000;
            _mockLoanAmountValidator.Setup(v => v.ValidateLoanAmount(loanAmount)).Returns(_loanAmountValidationResult);

            _mockLoanFulfilmentStrategy.Setup(s => s.Execute(It.IsAny<int>(), It.IsAny<IEnumerable<Lender>>()))
                .Returns(Enumerable.Empty<LoanFulfilment>());

            //  Act
            var view = _sut.Execute(loanAmount);

            //  Assert
            Assert.IsInstanceOf(typeof(NoLoanPossibleView), view);
        }
    }
}
