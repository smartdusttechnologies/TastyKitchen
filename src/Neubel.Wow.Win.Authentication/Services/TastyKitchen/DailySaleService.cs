using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces.TastyKitchen;
using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using Neubel.Wow.Win.Authentication.Data.Repository.Interfaces.TastyKitchen;
using System;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Services.TastyKitchen
{
    public class DailySaleService : IDailySaleService
    {
        IDailySaleRepository _dailySaleRepository;
        public DailySaleService(IDailySaleRepository dailySaleRepository)
        {
            _dailySaleRepository = dailySaleRepository;
        }

        public RequestResult<int> Add(DailySale dailySale)
        {
            _dailySaleRepository.Insert(dailySale);
            return new RequestResult<int>(1);
        }

        public RequestResult<int> AddCollection(List<DailySale> dailySales)
        {
            _dailySaleRepository.InsertCollection(dailySales);
            return new RequestResult<int>(1);
        }

        public bool Delete(int id)
        {
            _dailySaleRepository.Delete(id);
            return true;
        }

        public List<DailySale> Get()
        {
            return _dailySaleRepository.Get();
        }

        public DailySale Get(int id)
        {
            return _dailySaleRepository.Get(id);
        }

        public List<DailySale> GetPages(int pageIndex)
        {

            return _dailySaleRepository.GetPages(pageIndex).ToNonNullList();
        }

        public RequestResult<int> Update(int id, DailySale dailySale)
        {
            _dailySaleRepository.Update(dailySale);
            return new RequestResult<int>(1);
        }

        public int AddBillWiseSaleReport(BillWiseSaleReport billWiseSaleReport)
        {
            return _dailySaleRepository.InsertBillWiseSaleReport(billWiseSaleReport);
        }

        public int AddMenuCategoryWiseSaleReport(MenuCategoryWiseSaleReport menuCategoryWiseSaleReport)
        {
            return _dailySaleRepository.InsertMenuCategoryWiseSaleReport(menuCategoryWiseSaleReport);
        }

        public int AddMenuItemWiseSaleReport(MenuItemWiseSaleReport menuItemWiseSaleReport)
        {
            return _dailySaleRepository.InsertMenuItemWiseSaleReport(menuItemWiseSaleReport);
        }

        public List<MenuItemWiseSaleReport> GetMenuItemWiseSaleReport(DateTime from, DateTime to)
        {
            return _dailySaleRepository.GetMenuItemWiseSaleReport(from, to);
        }
        public List<BillWiseSaleReport> GetBillWiseSaleReport(DateTime from, DateTime to)
        {
            return _dailySaleRepository.GetBillWiseSaleReport(from, to);
        }
        public List<MenuCategoryWiseSaleReport> GetMenuCategoryWiseSaleReport(DateTime from, DateTime to)
        {
            return _dailySaleRepository.GetMenuCategoryWiseSaleReport(from, to);
        }
    }
}
