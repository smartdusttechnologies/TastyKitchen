using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen
{
    public class BillWiseSaleData : Entity
    {
        public string BillNumber { get; set; }
        public double Amount { get; set; }
        public int BillWiseSaleReportId { get; set; }
    }
}
