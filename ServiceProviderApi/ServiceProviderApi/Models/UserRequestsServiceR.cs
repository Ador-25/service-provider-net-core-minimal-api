using System.ComponentModel.DataAnnotations;

namespace ServiceProviderApi.Models
{
    public class UserRequestsServiceR
    {
        [Key]
        public Guid ServiceId { get; set; } = Guid.NewGuid();
        public int UserID { get; set; }

        public int ServiceProviderID { get; set; }

    }
}
