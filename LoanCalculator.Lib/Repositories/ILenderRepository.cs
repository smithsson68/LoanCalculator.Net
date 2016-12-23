using System.Collections.Generic;
using LoanCalculator.Lib.DomainObjects;

namespace LoanCalculator.Lib.Repositories
{
    public interface ILenderRepository
    {
        IEnumerable<Lender> List();
    }
}
