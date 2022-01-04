using System.ComponentModel.DataAnnotations;

namespace TastyKitchen.Web.UI.Models
{
    public class LoginDTO
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
