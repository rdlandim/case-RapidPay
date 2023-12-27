namespace DAL.RapidPay.DTO.CreditCards
{
    public class PaymentResponse
    {
        public virtual string GUID { get; set; }
        public virtual bool Success { get; set; }
        public virtual string Message { get; set; }
        public virtual CreditCardBalanceResponse Balance { get; set; }
    }
}
