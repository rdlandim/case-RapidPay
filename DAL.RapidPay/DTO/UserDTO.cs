using DAL.RapidPay.Entities;

namespace DAL.RapidPay.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public IEnumerable<CreditCard> CreditCards { get; set; }
    }
}
