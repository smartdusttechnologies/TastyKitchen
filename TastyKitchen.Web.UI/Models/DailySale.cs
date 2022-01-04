using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using System;
using System.ComponentModel.DataAnnotations;

namespace TastyKitchen.Web.UI.Models
{
    public class DailySale
    {
        public int Id { get; set; }
                
        public string BillNumber { get; set; }

        [Required]
        public string SaleType { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }

    
}
