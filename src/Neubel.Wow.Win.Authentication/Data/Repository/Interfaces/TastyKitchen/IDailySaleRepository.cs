using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using PagedList;
using System;
using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Data.Repository.Interfaces.TastyKitchen
{
    public interface IDailySaleRepository
    {
        int Insert(DailySale dailySale);
        int InsertCollection(List<DailySale> dailySales);
        int Update(DailySale dailySale);
        List<DailySale> Get();
        IPagedList<DailySale> GetPages(int pageIndex = 1, int pageSize = 10);
        DailySale Get(int id);
        bool Delete(int id);
        int InsertBillWiseSaleReport(BillWiseSaleReport billWiseSaleReport);
        int InsertMenuCategoryWiseSaleReport(MenuCategoryWiseSaleReport menuCategoryWiseSaleReport);
        int InsertMenuItemWiseSaleReport(MenuItemWiseSaleReport menuItemWiseSaleReport);
        List<MenuItemWiseSaleReport> GetMenuItemWiseSaleReport(DateTime from, DateTime to);
        List<BillWiseSaleReport> GetBillWiseSaleReport(DateTime from, DateTime to);
        List<MenuCategoryWiseSaleReport> GetMenuCategoryWiseSaleReport(DateTime from, DateTime to);
    }
}
