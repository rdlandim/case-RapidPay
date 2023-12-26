using DAL.RapidPay.DTO.Users;

namespace DAL.RapidPay.DTO.Identity
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
