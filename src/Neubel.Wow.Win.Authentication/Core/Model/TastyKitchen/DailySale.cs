using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen
{
    public class DailySale
    {
        public int Id { get; set; }

        public string BillNumber { get; set; }

        public string Type { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }
    }

    //public enum SaleType
    //{
    //    Zomato, Swiggy, DirectDelivery, TableService, Other
    //}
}
