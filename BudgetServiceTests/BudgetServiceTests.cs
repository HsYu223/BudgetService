using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace BudgetService.Tests
{
    [TestFixture()]
    public class BudgetServiceTests
    {
        private IBudgetService _budgetService;
        private IBudgetRepo _budgetRepo;

        [SetUp]
        public void SetUp()
        {
            _budgetService = Substitute.For<IBudgetService>();
            _budgetRepo = Substitute.For<IBudgetRepo>();
        }

        [Test()]
        public void ueryTest()
        {
            // arrange
            var start = new DateTime(2022, 09, 01);
            var end = new DateTime(2022, 04, 01);


            var expected = 0;
            GetAll();

            // actual
            var actual = this._budgetService.uery(start, end);

            // assert
            actual.Should().Be(expected);
        }

        private void GetAll()
        {
            _budgetRepo.GetAll().Returns(
                new List<Budget>()
                {
                    new Budget
                    {
                        YearMoth = "202209",
                        Amount = 30
                    },
                    new Budget
                    {
                        YearMoth = "202211",
                        Amount = 300
                    },
                    new Budget
                    {
                        YearMoth = "202212",
                        Amount = 3100
                    }
                });
        }
    }
}