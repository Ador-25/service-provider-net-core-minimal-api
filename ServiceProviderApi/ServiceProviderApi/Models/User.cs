using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServiceProviderApi.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Key]
        public int UserID { get; set; }

        public string Email { get; set; }
    
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

    }
}
