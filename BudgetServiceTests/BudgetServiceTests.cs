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

            _budgetService = new BudgetService(_budgetRepo);

        }

        [Test()]
        public void 結束日期小於起始日期()
        {
            // arrange
            var start = new DateTime(2022, 09, 01);
            var end = new DateTime(2022, 04, 01);


            var expected = 0;
            GetAll();

            // actual
            var actual = this._budgetService.Query(start, end);

            // assert
            actual.Should().Be(expected);
        }

        [Test()]
        public void 當日查詢()
        {
            // arrange
            var start = new DateTime(2022, 09, 01);
            var end = new DateTime(2022, 09, 01);


            var expected = 1;
            GetAll();

            // actual
            var actual = this._budgetService.Query(start, end);

            // assert
            actual.Should().Be(expected);
        }

        [Test()]
        public void 整月查詢()
        {
            // arrange
            var start = new DateTime(2022, 09, 01);
            var end = new DateTime(2022, 09, 30);


            var expected = 30;
            GetAll();

            // actual
            var actual = this._budgetService.Query(start, end);

            // assert
            actual.Should().Be(expected);
        }

        [Test()]
        public void 部分月查詢()
        {
            // arrange
            var start = new DateTime(2022, 11, 5);
            var end = new DateTime(2022, 11, 10);


            var expected = 60;
            GetAll();

            // actual
            var actual = this._budgetService.Query(start, end);

            // assert
            actual.Should().Be(expected);
        }

        [Test()]
        public void 跨月查詢()
        {
            // arrange
            var start = new DateTime(2022, 09, 1);
            var end = new DateTime(2022, 11, 5);


            var expected = 80;
            GetAll();

            // actual
            var actual = this._budgetService.Query(start, end);

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