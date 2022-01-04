using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neubel.Wow.Win.Authentication.Core.Interfaces.TastyKitchen;
using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TastyKitchen.Web.UI.Models;

namespace TastyKitchen.Web.UI.Controllers
{
    [Authorize(Roles= "Sysadmin,Admin")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DailySaleController> _logger;
        private readonly IDailySaleService _dailySaleService;
        private readonly IDailyExpenseService _dailyExpenseService;
        public DashboardController(ILogger<DailySaleController> logger, IDailySaleService dailySaleService, IDailyExpenseService dailyExpenseService)
        {
            _logger = logger;
            _dailySaleService = dailySaleService;
            _dailyExpenseService = dailyExpenseService;
        }
        // GET: DashboardController
        public ActionResult Index()
        {
            DashboardViewModel dashboard = new DashboardViewModel();
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var firstDayOfPreviousMonth = firstDayOfMonth.AddMonths(-1);
            var lastDayOfPreviousMonth = firstDayOfPreviousMonth.AddMonths(1).AddDays(-1);
            List<BillWiseSaleReport> previousMonthSalesData = _dailySaleService.GetBillWiseSaleReport(firstDayOfPreviousMonth, lastDayOfPreviousMonth);
            var expense = _dailyExpenseService.Get(firstDayOfPreviousMonth, lastDayOfPreviousMonth);

            //Mapping to UI Model
            expense.ForEach(x =>
            {
                dashboard.TotalExpenseAmount += x.Amount;
            });

            previousMonthSalesData.ForEach(x => 
            { 
                dashboard.TotalSaleAmount += x.Amount;
                dashboard.TotalNoOfOrders += x.BillWiseSales.Count;
            });

            dashboard.AverageOrderAmountPerOrder = Math.Round(dashboard.TotalSaleAmount / dashboard.TotalNoOfOrders, 2);

            List<MenuItemWiseSaleReport> menuItemWiseSaleReports = _dailySaleService.GetMenuItemWiseSaleReport(firstDayOfPreviousMonth, lastDayOfPreviousMonth);

            var orderedMenuItems = menuItemWiseSaleReports.SelectMany(x => x.MenuItemWiseSales);

            var menuItemWiseTotalSaleDTO = orderedMenuItems.GroupBy(t => t.Name).Select(z =>
            new MenuItemWiseTotalSaleByAmountDTO
            {
                Name = z.Key,
                TotalAmount = z.Sum(s => s.Amount)
            }).OrderByDescending(o => o.TotalAmount).Take(30).ToList();
            dashboard.MenuItemWiseTotalSaleByAmountDTO = menuItemWiseTotalSaleDTO.ToArray();

            var menuItemWiseTotalSaleByQuantityDTO = orderedMenuItems.GroupBy(t => t.Name).Select(z =>
            new MenuItemWiseTotalSaleByQuantityDTO
            {
                Name = z.Key,
                TotalQuantity = z.Sum(s => s.Quantity)
            }).OrderByDescending(o => o.TotalQuantity).Take(30).ToList();
            dashboard.MenuItemWiseTotalSaleByQuantityDTO = menuItemWiseTotalSaleByQuantityDTO.ToArray();

            return View(dashboard);
        }

        // GET: DashboardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DashboardController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DashboardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DashboardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DashboardController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
