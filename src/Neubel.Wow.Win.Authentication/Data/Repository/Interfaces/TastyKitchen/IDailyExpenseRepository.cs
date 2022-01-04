using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using PagedList;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Data.Repository.Interfaces.TastyKitchen
{
    public interface IDailyExpenseRepository
    {
        int Insert(DailyExpense expense);
        int Update(DailyExpense expense);
        List<DailyExpense> Get();
        IPagedList<DailyExpense> GetPages(int pageIndex = 1, int pageSize = 10);
        DailyExpense Get(int id);
        bool Delete(int id);
        int InsertCollection(List<DailyExpense> expenses);
        List<DailyExpense> Get(DateTime from, DateTime to);
    }
}
