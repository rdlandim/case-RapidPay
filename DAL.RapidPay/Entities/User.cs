using DAL.RapidPay.Context;
using Microsoft.EntityFrameworkCore;

namespace DAL.RapidPay.Entities
{
    public class User
    {
        public User()
        {
            CreditCards = new HashSet<CreditCard>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<CreditCard> CreditCards { get; set; }

        public bool IsCreditCardOwner(string creditCardNumber)
        {
            return CreditCards.Any(cc => cc.Number == creditCardNumber);
        }
    }
}
