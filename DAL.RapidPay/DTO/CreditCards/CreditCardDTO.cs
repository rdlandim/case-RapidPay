using DAL.RapidPay.Entities;

namespace DAL.RapidPay.DTO.CreditCards
{
    public class CreditCardDTO
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string ValidUntil { get; set; }
        public decimal Balance { get; set; }

        public User User { get; set; }
    }
}
