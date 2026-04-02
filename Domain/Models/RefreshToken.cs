namespace Domain.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime AddedDate { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}