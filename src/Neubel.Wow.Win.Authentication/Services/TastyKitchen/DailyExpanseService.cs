using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces.TastyKitchen;
using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using Neubel.Wow.Win.Authentication.Data.Repository.Interfaces.TastyKitchen;
using System;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Services.TastyKitchen
{
    public class DailyExpanseService : IDailyExpenseService
    {
        private readonly IDailyExpenseRepository _dailyExpenseRepository;

        public DailyExpanseService(IDailyExpenseRepository dailyExpenseRepository)
        {
            _dailyExpenseRepository = dailyExpenseRepository;
        }

        public RequestResult<int> Add(DailyExpense expense)
        {
            _dailyExpenseRepository.Insert(expense);
            return new RequestResult<int>(1);
        }

        public RequestResult<int> AddCollection(List<DailyExpense> expenses)
        {
            _dailyExpenseRepository.InsertCollection(expenses);
            return new RequestResult<int>(1);
        }

        public bool Delete(int id)
        {
            _dailyExpenseRepository.Delete(id);
            return true;
        }

        public List<DailyExpense> Get()
        {
            return _dailyExpenseRepository.Get();
        }
        public List<DailyExpense> Get(DateTime from, DateTime to)
        {
            return _dailyExpenseRepository.Get(from, to);
        }

        public List<DailyExpense> GetPages(int pageIndex)
        {
            return _dailyExpenseRepository.GetPages(pageIndex).ToNonNullList();
        }

        public DailyExpense Get(int id)
        {
            return _dailyExpenseRepository.Get(id);
        }

        public RequestResult<int> Update(int id, DailyExpense expense)
        {
           _dailyExpenseRepository.Update(expense);
            return new RequestResult<int>(1);
        }
    }
}
