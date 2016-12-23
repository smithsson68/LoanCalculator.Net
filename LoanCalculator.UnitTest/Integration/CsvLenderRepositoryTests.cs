using System;
using System.IO;
using System.Linq;
using CsvHelper;
using LoanCalculator.Lib.Repositories.Implementations;
using NUnit.Framework;

namespace LoanCalculator.Test.Integration
{
    /// <summary>
    /// These are considered integration tests because the CsvLenderRepository has a dependency on CsvHelper
    /// </summary>
    [TestFixture]
    public class CsvLenderRepositoryTests
    {
        [Test]
        public void Ctor_ShouldThrowArgumentNullException_WhenPassedNullTextReader()
        {
            //  Arrange - Act - Assert
            Assert.Throws<ArgumentNullException>(() => new CsvLenderRepository(null));
        }

        [Test]
        public void Ctor_ShouldThrowCsvMissingFieldException_WhenTextReaderContainsNonLenderHeadings()
        {
            //  Arrange
            var sr = new StringReader("Col1,Col2\r\nA,B");

            //  Act - Assert
            Assert.Throws<CsvMissingFieldException>(() => new CsvLenderRepository(sr));
        }

        [Test]
        public void Ctor_ShouldThrowCsvMissingFieldException_WhenTextReaderContainsToFewLenderHeadings()
        {
            //  Arrange
            var sr = new StringReader("Lender,Rate\r\nA,0.065");

            //  Act - Assert
            Assert.Throws<CsvMissingFieldException>(() => new CsvLenderRepository(sr));
        }

        [TestCase("Lender,Rate,Available\r\nA,0.065,400", 1)]
        [TestCase("Lender,Rate,Available\r\nA,0.065,400\r\nB,0.055,1200", 2)]
        [TestCase("Lender,Rate,Available\r\nA,0.065,400\r\nB,0.055,1200\r\nC,0.04,300", 3)]
        public void List_ListShouldNotBeEmpty_WhenTextReaderContainsValidHeadingsAndRows(string csvContent, int expectedRows)
        {
            //  Arrange
            var sr = new StringReader(csvContent);
            var sut = new CsvLenderRepository(sr);

            //  Act
            var actualLenderCount = sut.List().Count();

            //  Assert
            Assert.AreEqual(expectedRows, actualLenderCount);
        }
    }
}
