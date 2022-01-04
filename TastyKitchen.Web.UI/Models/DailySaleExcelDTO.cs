using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace TastyKitchen.Web.UI.Models
{
    public class DailySaleExcelDTO
    {
        public int Id { get; set; }

        public string BillNumber { get; set; }

        public string SaleType { get; set; }

        public double Amount { get; set; }

        public string Date { get; set; }
    }
}
