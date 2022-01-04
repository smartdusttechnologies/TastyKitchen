using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen
{
    public class BillWiseSaleReport : Entity
    {
        public List<BillWiseSaleData> BillWiseSales { get; set; } = new List<BillWiseSaleData>();
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
