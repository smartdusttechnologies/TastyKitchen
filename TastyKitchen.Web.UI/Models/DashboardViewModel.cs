using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TastyKitchen.Web.UI.Models
{
    public class DashboardViewModel
    {
        public double TotalSaleAmount { get; set; }
        public int TotalNoOfOrders { get; set; }
        public double AverageOrderAmountPerOrder { get; set; }
        public double TotalExpenseAmount { get; set; }
        public MenuItemWiseTotalSaleByAmountDTO[] MenuItemWiseTotalSaleByAmountDTO { get; set; }
        public MenuItemWiseTotalSaleByQuantityDTO[] MenuItemWiseTotalSaleByQuantityDTO { get; set; }
    }
}
