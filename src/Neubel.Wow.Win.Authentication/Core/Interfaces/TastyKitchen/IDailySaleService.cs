using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using System;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Core.Interfaces.TastyKitchen
{
    public interface IDailySaleService
    {
        List<DailySale> Get();
        List<DailySale> GetPages(int pageIndex);
        DailySale Get(int id);
        RequestResult<int> Add(DailySale dailySale);
        RequestResult<int> AddCollection(List<DailySale> dailySales);
        RequestResult<int> Update(int id, DailySale dailySale);
        bool Delete(int id);
        int AddBillWiseSaleReport(BillWiseSaleReport billWiseSaleReport);
        int AddMenuCategoryWiseSaleReport(MenuCategoryWiseSaleReport menuCategoryWiseSaleReport);
        int AddMenuItemWiseSaleReport(MenuItemWiseSaleReport menuItemWiseSaleReport);
        List<MenuItemWiseSaleReport> GetMenuItemWiseSaleReport(DateTime from, DateTime to);
        List<BillWiseSaleReport> GetBillWiseSaleReport(DateTime from, DateTime to);
        List<MenuCategoryWiseSaleReport> GetMenuCategoryWiseSaleReport(DateTime from, DateTime to);
    }
}
