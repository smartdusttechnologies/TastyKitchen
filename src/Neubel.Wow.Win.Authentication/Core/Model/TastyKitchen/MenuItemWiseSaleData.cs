using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen
{
    public class MenuItemWiseSaleData : Entity
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }
        public int MenuItemWiseSaleReportId { get; set; }
    }
}
