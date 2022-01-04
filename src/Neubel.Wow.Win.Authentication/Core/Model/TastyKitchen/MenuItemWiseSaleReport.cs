using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen
{
    public class MenuItemWiseSaleReport : Entity
    {
        public List<MenuItemWiseSaleData> MenuItemWiseSales { get; set; } = new List<MenuItemWiseSaleData>();
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
