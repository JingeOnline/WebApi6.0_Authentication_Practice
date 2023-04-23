using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi_RefreshToken.Models
{
    public class RefreshToken
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int Fk_UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public string Token { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime ExpireAt { get; set; }
        public DateTime? RevokeAt { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReasonRevoked { get; set; }

        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= ExpireAt;
        [NotMapped]
        public bool IsRevoked => RevokeAt != null;
        [NotMapped]
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
