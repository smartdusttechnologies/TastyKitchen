using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neubel.Wow.Win.Authentication.Common;
using Neubel.Wow.Win.Authentication.Core.Interfaces.TastyKitchen;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TastyKitchen.Web.UI.Models;

namespace TastyKitchen.Web.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDailyExpenseService _dailyExpenseService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IDailyExpenseService dailyExpenseService, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _dailyExpenseService = dailyExpenseService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Index()
        {

            var expenses = _dailyExpenseService.GetPages(1);

            List<DailyExpense> dailyExpenses = new List<DailyExpense>();

            foreach (var item in expenses)
            {
                dailyExpenses.Add(new DailyExpense { Id = item.Id, Amount = item.Amount, Date = item.Date, Name = item.Name, Unit = item.Unit, Quantity = item.Quantity });
            }
            ViewBag.nextPage = 2;
            ViewBag.PreviousPage = 0;
            return View(dailyExpenses.AsEnumerable());
        }

        [HttpPost]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Index(int pageIndex)
        {
            
            var expenses = _dailyExpenseService.GetPages(pageIndex);

            List<DailyExpense> dailyExpenses = new List<DailyExpense>();

            foreach (var item in expenses)
            {
                dailyExpenses.Add(new DailyExpense { Id = item.Id, Amount = item.Amount, Date = item.Date, Name = item.Name, Unit = item.Unit, Quantity = item.Quantity });
            }
            ViewBag.nextPage = pageIndex + 1;
            ViewBag.PreviousPage = pageIndex == 1 ? 1 : pageIndex - 1;
            return View(dailyExpenses.AsEnumerable());
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Create()
        {
            return View(new DailyExpense { Date = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Create([Bind] DailyExpense dailyExpense)
        {
            if (ModelState.IsValid)
            {
                var dailyExpenseBusinessModel = new Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen.DailyExpense
                {
                    Date = dailyExpense.Date,
                    Unit = dailyExpense.Unit,
                    Amount = dailyExpense.Amount,
                    Id = dailyExpense.Id,
                    Name = dailyExpense.Name,
                    Quantity = dailyExpense.Quantity
                };

                _dailyExpenseService.Add(dailyExpenseBusinessModel);

                return RedirectToAction("Index");
            }
            return View(dailyExpense);
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var expense = _dailyExpenseService.Get((int)id);


            if (expense == null)
            {
                return NotFound();
            }


            var expenseUIModel = new DailyExpense
            {
                Date = expense.Date,
                Unit = expense.Unit,
                Amount = expense.Amount,
                Id = expense.Id,
                Name = expense.Name,
                Quantity = expense.Quantity
            };

            return View(expenseUIModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Edit(int id, [Bind] DailyExpense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var expenseBusinessModel = new Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen.DailyExpense
                {
                    Date = expense.Date,
                    Unit = expense.Unit,
                    Amount = expense.Amount,
                    Id = expense.Id,
                    Name = expense.Name,
                    Quantity = expense.Quantity
                };

                _dailyExpenseService.Update(id, expenseBusinessModel);

                return RedirectToAction("Index");
            }
            return View(expense);
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            var expense = _dailyExpenseService.Get((int)id);

            if (expense == null)
            {
                return NotFound();
            }

            var expenseUIModel = new DailyExpense
            {
                Date = expense.Date,
                Unit = expense.Unit,
                Amount = expense.Amount,
                Id = expense.Id,
                Name = expense.Name,
                Quantity = expense.Quantity
            };

            return View(expenseUIModel);
        }

        [HttpGet]
        [Authorize(Roles = "Sysadmin,Admin,Manager")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var expense = _dailyExpenseService.Get((int)id);


            if (expense == null)
            {
                return NotFound();
            }


            var expenseUIModel = new DailyExpense
            {
                Date = expense.Date,
                Unit = expense.Unit,
                Amount = expense.Amount,
                Id = expense.Id,
                Name = expense.Name,
                Quantity = expense.Quantity
            };
            return View(expenseUIModel);
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
            _dailyExpenseService.Delete((int)id);
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
            List<DailyExpenseExcelDTO> resultsDTO = new List<DailyExpenseExcelDTO>();
            var results = _dailyExpenseService.Get();
            foreach (var item in results)
            {
                resultsDTO.Add(new DailyExpenseExcelDTO
                { 
                    Id = item.Id, 
                    Amount = item.Amount,
                    Date = item.Date.ToString("MM/dd/yyyy hh:mm tt"), 
                    Name = item.Name, 
                    Quantity = item.Quantity, 
                    Unit = item.Unit 
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
            string excelName = $"TastyKitchenExpense-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
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

                    var dailyExpense = new List<Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen.DailyExpense>();

                    for (int i = 2; i <= totalRows; i++)
                    {
                        if (workSheet.Cells[i, 1].Value != null
                            && workSheet.Cells[i, 2].Value != null
                            && workSheet.Cells[i, 3].Value != null
                            && workSheet.Cells[i, 5].Value != null)
                        {
                            //var quantityAndUnit = workSheet.Cells[i, 4].Value.ToString().Split(" ");
                        dailyExpense.Add(new Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen.DailyExpense
                        {
                            Name = workSheet.Cells[i, 1].Value.ToString(),
                            Amount = Convert.ToInt32(workSheet.Cells[i, 2].Value),
                            Quantity = double.Parse(workSheet.Cells[i, 3].Value.ToString()),
                            Unit = workSheet.Cells[i, 4].Value != null ? workSheet.Cells[i, 4].Value.ToString() : string.Empty,
                            Date = Helpers.ConverddmmyyyyTommddyyyy(workSheet.Cells[i, 5].Value.ToString().Split(" ")[0]).Value
                        });
                    }
                    }
                    _dailyExpenseService.AddCollection(dailyExpense);
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Home", new { errorMessage = ex.Message, stackTrace = ex.StackTrace }); 

            }
            finally
            {
                System.IO.File.Delete(filePath);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
