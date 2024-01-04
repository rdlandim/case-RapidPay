namespace DAL.RapidPay.Entities
{
    public class CreditCard
    {
        public CreditCard()
        {

        }

        public CreditCard(int userId, string number, string validUntil, string cvc)
        {
            UserId = userId;
            Number = number;
            ValidUntil = validUntil;
            Cvc = cvc;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Number { get; set; }
        public string ValidUntil { get; set; }
        public string Cvc { get; set; }
        public decimal Balance { get; set; }

        public virtual User User { get; set; }

        public bool IsExpired(string expiryDate)
        {
            var cardValidDate = new DateTime(Convert.ToInt32(ValidUntil[..4]), Convert.ToInt32(ValidUntil[5..]), 1);
            var providedDate = new DateTime(Convert.ToInt32(expiryDate[..4]), Convert.ToInt32(expiryDate[5..]), 1);

            return providedDate > cardValidDate;
        }
    }
}
