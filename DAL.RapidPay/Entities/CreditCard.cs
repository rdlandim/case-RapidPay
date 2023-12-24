namespace DAL.RapidPay.Entities
{
    public class CreditCard
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Number { get; set; }
        public string ValidUntil { get; set; }
        public string Cvc { get; set; }
        public decimal Balance { get; set; }

        public virtual User User { get; set; }
    }
}
