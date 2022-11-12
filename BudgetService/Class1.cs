using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetService
{
    public class BudgetService
    {
        private readonly IBudgetRepo _bugBudgetRepo;

        public BudgetService(IBudgetRepo bugBudgetRepo)
        {
            this._bugBudgetRepo = bugBudgetRepo;
        }

        public decimal uery(DateTime start, DateTime end)
        {
            // 結束時間大於開始時間
            if (end > start)
            {
                return 0;
            }

            var budgets = this._bugBudgetRepo.GetAll();
            var startTYarMonth = Convert.ToInt32($"{start:yyyyMM}");
            var endYearMonth = Convert.ToInt32($"{end:yyyyMM}");
            var totalBudget=  budgets.Where(
                rows => Convert.ToInt32(rows.YearMoth) >= startTYarMonth &&
                        Convert.ToInt32(rows.YearMoth) <= endYearMonth);
            var day = end.Subtract(start).Days;


            // 當日查詢
            if (day.Equals(1))
            {
                var monthDay = DateTime.DaysInMonth(start.Year, start.Month);

                var data = totalBudget.Select(x => x.Amount).FirstOrDefault() / monthDay;
            }



            // 當月查詢

            // 跨月查詢

            // 部分月查詢
        }
    }

    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }

    public class Budget
    {
        public string YearMoth { get; set; }

        public int Amount { get; set; }
    }
}
