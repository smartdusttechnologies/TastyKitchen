using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neubel.Wow.Win.Authentication.Core.Interfaces.TastyKitchen;
using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using TastyKitchen.Web.UI.Models;

namespace TastyKitchen.Web.UI.Controllers
{
    [Authorize]
    public class DailySaleController : Controller
    {
        private readonly ILogger<DailySaleController> _logger;
        private readonly IDailySaleService _dailySaleService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DailySaleController(ILogger<DailySaleController> logger, IDailySaleService dailySaleService, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _dailySaleService = dailySaleService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Index()
        {
            var sales = _dailySaleService.GetPages(1);

            List<Models.DailySale> dailySales = new List<Models.DailySale>();

            foreach (var item in sales)
            {
                dailySales.Add(new Models.DailySale { Id = item.Id, Amount = item.Amount, Date = item.Date, BillNumber = item.BillNumber, SaleType = item.Type });
            }
            ViewBag.nextPage = 2;
            ViewBag.PreviousPage = 0;
            return View(dailySales.AsEnumerable());
        }

        [HttpPost]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Index(int pageIndex)
        {
            var expenses = _dailySaleService.GetPages(pageIndex);

            List<Models.DailySale> dailySales = new List<Models.DailySale>();

            foreach (var item in expenses)
            {
                dailySales.Add(new Models.DailySale { Id = item.Id, Amount = item.Amount, Date = item.Date, BillNumber = item.BillNumber, SaleType = item.Type });
            }
            ViewBag.nextPage = pageIndex + 1;
            ViewBag.PreviousPage = pageIndex == 1 ? 1 : pageIndex - 1;
            return View(dailySales.AsEnumerable());
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Create()
        {
            return View(new Models.DailySale { Date = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Create([Bind] Models.DailySale dailySale)
        {
            if (ModelState.IsValid)
            {
                var dailySaleBusinessModel = new Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen.DailySale
                {
                    Date = dailySale.Date,
                    Amount = dailySale.Amount,
                    Id = dailySale.Id,
                    BillNumber = dailySale.BillNumber,
                    Type = dailySale.SaleType
                };

                _dailySaleService.Add(dailySaleBusinessModel);

                return RedirectToAction("Index");
            }
            return View(dailySale);
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = _dailySaleService.Get((int)id);

            if (sale == null)
            {
                return NotFound();
            }

            var expenseUIModel = new Models.DailySale
            {
                Date = sale.Date,
                Amount = sale.Amount,
                Id = sale.Id,
                BillNumber = sale.BillNumber,
                SaleType = sale.Type
            };

            return View(expenseUIModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Edit(int id, [Bind] Models.DailySale dailySale)
        {
            if (id != dailySale.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var expenseBusinessModel = new Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen.DailySale
                {
                    Date = dailySale.Date,
                    Amount = dailySale.Amount,
                    Id = dailySale.Id,
                    BillNumber = dailySale.BillNumber,
                    Type = dailySale.SaleType
                };

                _dailySaleService.Update(id, expenseBusinessModel);

                return RedirectToAction("Index");
            }
            return View(dailySale);
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = _dailySaleService.Get((int)id);

            if (sale == null)
            {
                return NotFound();
            }

            var saleUIModel = new Models.DailySale
            {
                Date = sale.Date,
                Amount = sale.Amount,
                Id = sale.Id,
                BillNumber = sale.BillNumber,
                SaleType = sale.Type
            };

            return View(saleUIModel);
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var sale = _dailySaleService.Get((int)id);


            if (sale == null)
            {
                return NotFound();
            }


            var saleUIModel = new Models.DailySale
            {
                Date = sale.Date,
                Amount = sale.Amount,
                Id = sale.Id,
                BillNumber = sale.BillNumber,
                SaleType = sale.Type
            };
            return View(saleUIModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _dailySaleService.Delete((int)id);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin")]
        public IActionResult ExportToExcel()
        {
            List<DailySaleExcelDTO> resultsDTO = new List<DailySaleExcelDTO>();
            var results = _dailySaleService.Get();
            foreach (var item in results)
            {
                resultsDTO.Add(new DailySaleExcelDTO
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    Date = item.Date.ToString("MM/dd/yyyy hh:mm tt"),
                    SaleType = item.Type
                });
            }

            var stream = new MemoryStream();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.LoadFromCollection(resultsDTO, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"TastyKitchenSale-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            return File(stream, "application/vdn.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpPost]
        [Authorize(Roles = "Sysadmin,Admin")]
        public IActionResult ImportFromExcel(IFormFile file)
        {
            
            if (file == null || file.Length == 0)
                return Content("File Not Selected");

            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
                return Content("File Not Selected");

            var rootFolder = _hostingEnvironment.WebRootPath;
            var fileName = file.FileName;
            var filePath = Path.Combine(rootFolder, "uploadedFiles", fileName);
            var fileLocation = new FileInfo(filePath);

            if (file.Length <= 0 && file.Length < (1000000 / 2)) // not more than 0.5 MB excel file)
                return BadRequest("File not found or size is more than specified limit");

            try
            {

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(fileLocation))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
                    //var workSheet = package.Workbook.Worksheets.First();
                    int totalRows = workSheet.Dimension.Rows;

                    var dailySale = new List<Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen.DailySale>();

                    for (int i = 2; i <= totalRows; i++)
                    {
                        if (workSheet.Cells[i, 3].Value != null
                            && workSheet.Cells[i, 4].Value != null
                            && workSheet.Cells[i, 5].Value != null)
                        {
                            dailySale.Add(new Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen.DailySale
                            {
                                Type = workSheet.Cells[i, 3].Value.ToString(),
                                Amount = Double.Parse(workSheet.Cells[i, 4].Value.ToString()),
                                Date = DateTime.Parse(workSheet.Cells[i, 5].Value.ToString())
                            });
                        }
                    }
                    _dailySaleService.AddCollection(dailySale);
                }
            }
            finally
            {
                System.IO.File.Delete(filePath);
            }
            return RedirectToAction("Index", "DailySale");
        }

        [HttpPost]
        [Authorize(Roles = "Sysadmin,Admin")]
        public IActionResult ImportDailySale(List<IFormFile> files)
        {
            foreach (var file in files)
            {

                if (file == null || file.Length == 0)
                    return Content("File Not Selected");

                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".rtf" && fileExtension != ".RTF")
                    return Content("File Not Selected");

                var rootFolder = _hostingEnvironment.WebRootPath;
                var fileName = file.FileName;
                var filePath = Path.Combine(rootFolder, "uploadedFiles", fileName);
                var fileLocation = new FileInfo(filePath);

                if (file.Length <= 0 && file.Length < (1000000 / 2)) // not more than 0.5 MB excel file)
                    return BadRequest("File not found or size is more than specified limit");

                try
                {

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    string content = Read(filePath);
                    string[] lines = content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                    string reportName = string.Empty;
                    if (!string.IsNullOrEmpty(lines[2].Trim()))
                        reportName = lines[2].Trim();

                    DateTime? reportDate = null; ;
                    if (!string.IsNullOrEmpty(lines[3].Trim()))
                    {
                        string reportDateStr = lines[3].Trim().Split(" ")[0];

                        DateTime dt;
                        if (DateTime.TryParseExact(reportDateStr,
                                                    "d-M-yyyy",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None,
                            out dt))
                        {
                            reportDate = dt;
                        }
                    }

                    if (reportName == "BILL SALE  REPORT (DAILY - Z)")
                    {
                        string[] splitValues = new string[3];
                        splitValues[0] = " ";
                        splitValues[1] = "CSH";
                        splitValues[2] = "CRD";

                        BillWiseSaleReport billWiseSaleReport = new BillWiseSaleReport { Date = reportDate.Value };

                        for (int i = 7; i < lines.Length; i++)
                        {
                            BillWiseSaleData billWiseSaleData = new BillWiseSaleData();
                            if (lines[i].Trim() == "GRAND TOTAL")
                            {
                                var grandTotalRow = lines[i + 4].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                                billWiseSaleReport.Amount = double.Parse(grandTotalRow[grandTotalRow.Length - 1]);
                                break;
                            }

                            var row = lines[i].Split(splitValues, StringSplitOptions.RemoveEmptyEntries);
                            billWiseSaleData.BillNumber = row[0];
                            billWiseSaleData.Amount = double.Parse(row[row.Length - 1]);
                            billWiseSaleReport.BillWiseSales.Add(billWiseSaleData);
                        }
                        _dailySaleService.AddBillWiseSaleReport(billWiseSaleReport);
                    }

                    else if (reportName == "PLU  SALE REPORT (DAILY - Z)")
                    {
                        MenuItemWiseSaleReport billDetails = new MenuItemWiseSaleReport { Date = reportDate.Value };
                        string grandTotal = string.Empty;
                        string totalQuantity = string.Empty;
                        for (int i = 7; i < lines.Length; i++)
                        {
                            if (lines[i].Trim().StartsWith("GRAND TOTAL"))
                            {
                                var grandTotalRow = lines[i].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                                billDetails.Amount = double.Parse(grandTotalRow[grandTotalRow.Length - 1]);
                                totalQuantity = grandTotalRow[grandTotalRow.Length - 2];
                                break;
                            }

                            var rowCoumns = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                            MenuItemWiseSaleData menuItem = new MenuItemWiseSaleData();
                            bool isQuantityColumnDone = false; ;
                            for (int j = 1; j < rowCoumns.Length; j++)
                            {
                                double result = 0;
                                if (!string.IsNullOrEmpty(rowCoumns[j]) && !double.TryParse(rowCoumns[j], out result))
                                {
                                    menuItem.Name += string.IsNullOrEmpty(menuItem.Name) ? rowCoumns[j] : " " + rowCoumns[j];
                                }
                                else if (isQuantityColumnDone)
                                {
                                    menuItem.Amount = result;
                                }
                                else
                                {
                                    menuItem.Quantity = result;
                                    isQuantityColumnDone = true;
                                }
                            }
                            billDetails.MenuItemWiseSales.Add(menuItem);
                        }
                        _dailySaleService.AddMenuItemWiseSaleReport(billDetails);
                    }

                    else if (reportName == "FINANCIAL REPORT (DAILY - Z)")
                    {
                        MenuCategoryWiseSaleReport billDetails = new MenuCategoryWiseSaleReport { Date = reportDate.Value };
                        string grandTotal = string.Empty;
                        string totalQuantity = string.Empty;
                        for (int i = 7; i < lines.Length; i++)
                        {
                            if (lines[i].Trim().StartsWith("TOTAL SALE"))
                            {
                                var grandTotalRow = lines[i].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                                billDetails.Amount = double.Parse(grandTotalRow[grandTotalRow.Length - 1]);
                                totalQuantity = grandTotalRow[grandTotalRow.Length - 2];
                                break;
                            }

                            var rowCoumns = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                            MenuCategoryWiseSaleData menuItem = new MenuCategoryWiseSaleData();
                            bool isQuantityColumnDone = false;
                            for (int j = 1; j < rowCoumns.Length; j++)
                            {
                                double result = 0;
                                if (!string.IsNullOrEmpty(rowCoumns[j]) && !double.TryParse(rowCoumns[j], out result))
                                {
                                    menuItem.Name += string.IsNullOrEmpty(menuItem.Name) ? rowCoumns[j] : " " + rowCoumns[j];
                                }
                                else if (isQuantityColumnDone)
                                {
                                    menuItem.Amount = result;
                                }
                                else
                                {
                                    menuItem.Quantity = result;
                                    isQuantityColumnDone = true;
                                }
                            }
                            if (rowCoumns.Length > 1)
                                billDetails.MenuCategoryWiseSales.Add(menuItem);
                        }
                        _dailySaleService.AddMenuCategoryWiseSaleReport(billDetails);
                    }
                }
                finally
                {
                    System.IO.File.Delete(filePath);
                }
            }
            return RedirectToAction("Index", "DailySale");
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin")]
        public IActionResult GetMonthSaleByDate()
        {
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var firstDayOfPreviousMonth = firstDayOfMonth.AddMonths(-1);
            var lastDayOfPreviousMonth = firstDayOfPreviousMonth.AddMonths(1).AddDays(-1);
            List<MenuItemWiseSaleReport> previousMonthSalesData = _dailySaleService.GetMenuItemWiseSaleReport(firstDayOfPreviousMonth, lastDayOfPreviousMonth);

            //Mapping to UI Model
            List<SaleByDate> saleByDates = new List<SaleByDate>();
            foreach (var item in previousMonthSalesData)
            {
                saleByDates.Add(new SaleByDate { Date = item.Date, Amount = item.Amount });
            }
            return Json(saleByDates);
        }

        private string Read(string file)
        {
            try
            {
                var reader = new StreamReader(file);
                var content = reader.ReadToEnd();
                reader.Close();
                return content;
            }
            catch
            {
                return null;
            }
        }
    }
}
