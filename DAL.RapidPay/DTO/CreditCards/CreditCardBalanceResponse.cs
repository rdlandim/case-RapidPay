namespace DAL.RapidPay.DTO.CreditCards
{
    public class CreditCardBalanceResponse
    {
        public virtual decimal PreviousBalance { get; set; }
        public virtual decimal Balance { get; set; }
    }
}
