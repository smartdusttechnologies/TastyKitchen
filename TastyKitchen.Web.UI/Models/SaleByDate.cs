using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TastyKitchen.Web.UI.Models
{
    public class SaleByDate
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
