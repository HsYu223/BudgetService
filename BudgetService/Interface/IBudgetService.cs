using System;

namespace BudgetService.Interface
{
    public interface IBudgetService
    {
        decimal Query(DateTime start, DateTime end);
    }
}