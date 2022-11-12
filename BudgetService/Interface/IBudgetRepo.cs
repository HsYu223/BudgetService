using System.Collections.Generic;
using BudgetService.DataModel;

namespace BudgetService.Interface
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}