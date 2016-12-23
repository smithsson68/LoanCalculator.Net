using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using LoanCalculator.Lib.DomainObjects;

namespace LoanCalculator.Lib.Repositories.Implementations
{
    public class CsvLenderRepository : ILenderRepository
    {
        private readonly IEnumerable<Lender> _lenders;
         
        public CsvLenderRepository(TextReader textReader)
        {
            var reader = new CsvReader(textReader);
            reader.Configuration.RegisterClassMap<CsvLenderDefinitionMap>();
            _lenders = reader.GetRecords<Lender>().ToList();
        }

        public IEnumerable<Lender> List()
        {
            return _lenders;
        }
    }
}
