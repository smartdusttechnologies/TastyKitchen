using System;

namespace Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen
{
    public class DailyExpense
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public DateTime Date { get; set; }
    }
}
