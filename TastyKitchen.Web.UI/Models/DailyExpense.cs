using System;
using System.ComponentModel.DataAnnotations;

namespace TastyKitchen.Web.UI.Models
{
    public class DailyExpense
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public double Quantity { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
