using System;
using LoanCalculator.Lib;

namespace LoanCalculator.App
{
    public class ConsoleDisplay : IDisplay  
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}
