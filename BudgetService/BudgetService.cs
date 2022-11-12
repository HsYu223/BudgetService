using BudgetService.Interface;
using System;
using System.Linq;

namespace BudgetService
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepo _bugBudgetRepo;

        public BudgetService(IBudgetRepo bugBudgetRepo)
        {
            _bugBudgetRepo = bugBudgetRepo;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            // 結束時間小於開始時間
            if (end < start) return 0;

            var budgets = _bugBudgetRepo.GetAll();
            var startTYarMonth = Convert.ToInt32($"{start:yyyyMM}");
            var endYearMonth = Convert.ToInt32($"{end:yyyyMM}");
            var totalBudget = budgets.Where(
                rows => Convert.ToInt32(rows.YearMoth) >= startTYarMonth &&
                        Convert.ToInt32(rows.YearMoth) <= endYearMonth);
            var day = end.Subtract(start).Days + 1;

            var monthDay = DateTime.DaysInMonth(start.Year, start.Month);

            // 當日查詢
            if (day.Equals(1))
            {
                return Convert.ToDecimal(totalBudget.Select(x => x.Amount).FirstOrDefault() / monthDay);
            }

            // 當月查詢
            if (startTYarMonth.Equals(endYearMonth) && day.Equals(monthDay))
            {
                return Convert.ToDecimal(totalBudget.Select(x => x.Amount).FirstOrDefault());
            }

            // 部分月查詢
            if (startTYarMonth.Equals(endYearMonth) && !day.Equals(monthDay))
            {
                return Convert.ToDecimal(totalBudget.Select(x => x.Amount).FirstOrDefault() / monthDay * day);
            }

            var tmp = new DateTime(start.Year, start.Month, 1);
            var tmpAmount = (decimal)0;

            while (tmp <= end)
            {
                var tmpDay = end.Subtract(start).Days + 1;

                if (tmp.ToString("yyyyMM") == startTYarMonth.ToString())
                {
                    var tmpMonthDay = DateTime.DaysInMonth(start.Year, start.Month) - start.Day + 1;
                    var monthAmount = totalBudget.Where(x => x.YearMoth.Equals(tmp.ToString("yyyyMM")))
                        .Select(x => x.Amount).FirstOrDefault();
                    tmpAmount += Convert.ToDecimal(monthAmount / monthDay * tmpMonthDay);
                }
                else if (tmp.ToString("yyyyMM") == endYearMonth.ToString())
                {
                    var tmpMonthDay = end.Day;
                    var monthAmount = totalBudget.Where(x => x.YearMoth.Equals(tmp.ToString("yyyyMM")))
                        .Select(x => x.Amount).FirstOrDefault();
                    var daysInMonth = DateTime.DaysInMonth(end.Year, end.Month);
                    tmpAmount += Convert.ToDecimal(monthAmount / daysInMonth * tmpMonthDay);
                }
                else
                {
                    tmpAmount += Convert.ToDecimal(totalBudget.Where(x => x.YearMoth.Equals(tmp.ToString("yyyyMM")))
                        .Select(x => x.Amount).FirstOrDefault());
                }

                tmp = tmp.AddMonths(1);
            }


            return tmpAmount;
        }
    }
}