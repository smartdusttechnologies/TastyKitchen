using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces.TastyKitchen
{
    public interface IDailyExpenseService
    {
        List<DailyExpense> Get();
        List<DailyExpense> GetPages(int pageIndex);
        DailyExpense Get(int id);
        RequestResult<int> Add(DailyExpense expense);
        RequestResult<int> Update(int id, DailyExpense expense);
        bool Delete(int id);
        RequestResult<int> AddCollection(List<DailyExpense> expenses);
        List<DailyExpense> Get(DateTime from, DateTime to);
    }
}
