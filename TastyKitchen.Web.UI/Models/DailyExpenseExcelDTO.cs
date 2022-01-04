using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TastyKitchen.Web.UI.Models
{
    public class DailyExpenseExcelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public string Date { get; set; }
    }
}
