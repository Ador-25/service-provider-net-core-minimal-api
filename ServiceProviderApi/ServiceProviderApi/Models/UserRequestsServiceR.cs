using System.ComponentModel.DataAnnotations;

namespace ServiceProviderApi.Models
{
    public class UserRequestsServiceR
    {
        [Key]
        public Guid ServiceId { get; set; } = Guid.NewGuid();
        public string? UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public string? UserAddress { get; set; }
        public string? Task { get; set; }
        public string? UserBid { get; set; }
        public int? ServiceProviderID { get; set; }

    }
}
