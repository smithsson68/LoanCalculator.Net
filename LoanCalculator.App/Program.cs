using System;
using System.IO;
using LoanCalculator.Lib;
using LoanCalculator.Lib.Repositories.Implementations;
using LoanCalculator.Lib.Strategies.Implementation;
using LoanCalculator.Lib.Validators.Implementation;

namespace LoanCalculator.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please provide 2 arguments - the path to the market file and loan amount");
                return;
            }

            try
            {
                var loanAmount = GetLoanAmount(args[1]);

                var loanAmountValidator = new HardCodedLoanAmountValidator();
                var lenderRepository = CreateLenderRepository(args[0]);
                var loanFulfilmentStrategy = new CheapestFirstLoanFulfilmentStrategy();
                var interestCalculationStrategy = new CompositeInterestRateCalculationStrategy();
                var loanRepaymentStrategy = new MonthlyCompoundingInterestLoanReplaymentCalculationStrategy();

                var loanController = new LoanCalculationController(
                    loanAmountValidator,
                    lenderRepository,
                    loanFulfilmentStrategy,
                    interestCalculationStrategy,
                    loanRepaymentStrategy);

                var view = loanController.Execute(loanAmount);
                view.Render(new ConsoleDisplay());
            }
            catch (Exception exception)
            {
                Console.WriteLine("Ooops - an error occured processing your request.");
                Console.WriteLine("Here are the details:");
                Console.WriteLine(exception.ToString());
            }
        }

        private static CsvLenderRepository CreateLenderRepository(string marketFilePath)
        {
            try
            {
                using (var textReader = File.OpenText(marketFilePath))
                {
                    return new CsvLenderRepository(textReader);
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException("An error occured processing the market file", exception);
            }
        }

        private static int GetLoanAmount(string value)
        {
            var loanAmount = 0;
            int.TryParse(value, out loanAmount);
            return loanAmount;
        }
    }
}
