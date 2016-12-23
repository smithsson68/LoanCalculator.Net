using System;
using System.Linq;
using LoanCalculator.Lib.DomainObjects;
using LoanCalculator.Lib.Repositories;
using LoanCalculator.Lib.Strategies;
using LoanCalculator.Lib.Validators;
using LoanCalculator.Lib.Views;
using LoanCalculator.Lib.Views.Implementation;

namespace LoanCalculator.Lib
{
    public class LoanCalculationController
    {
        private const int LoanTermInMonths = 36;

        private readonly ILoanAmountValidator _loanAmountValidator;
        private readonly ILenderRepository _lenderRepository;
        private readonly ILoanFulfilmentStrategy _loanFulfilmentStrategy;
        private readonly IInterestRateCalculationStrategy _interestRateCalculationStrategy;
        private readonly ILoanRepaymentCalculationStrategy _loanRepaymentCalculationStrategy;

        public LoanCalculationController(
            ILoanAmountValidator loanAmountValidator,
            ILenderRepository lenderRepository,
            ILoanFulfilmentStrategy loanFulfilmentStrategy,
            IInterestRateCalculationStrategy interestRateCalculationStrategy,
            ILoanRepaymentCalculationStrategy loanRepaymentCalculationStrategy)
        {
            _loanAmountValidator = loanAmountValidator;
            _lenderRepository = lenderRepository;
            _loanFulfilmentStrategy = loanFulfilmentStrategy;
            _interestRateCalculationStrategy = interestRateCalculationStrategy;
            _loanRepaymentCalculationStrategy = loanRepaymentCalculationStrategy;
        }

        public IView Execute(int loanAmount)
        {
            var validationResult = _loanAmountValidator.ValidateLoanAmount(loanAmount);

            if (!validationResult.Valid)
            {
                return new ErrorView(validationResult.ErrorMessage);
            }

            var fulfilments = (_loanFulfilmentStrategy.Execute(loanAmount, _lenderRepository.List()) 
                ?? Enumerable.Empty<LoanFulfilment>()).ToList();

            if (!fulfilments.Any())
            {
                return new NoLoanPossibleView();
            }

            var rate = _interestRateCalculationStrategy.Execute(fulfilments);
            var repaymentResult = _loanRepaymentCalculationStrategy.Execute(loanAmount, rate, LoanTermInMonths);
            return new SimpleLoanCalculationView(loanAmount, rate, repaymentResult);
        }
    }
}
