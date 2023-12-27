namespace DAL.RapidPay.DTO.CreditCards
{
    public class CreditCardPaymentRequest
    {
        public virtual int UserId { get; set; }
        public virtual string CreditCardNumber { get; set; }
        public virtual string ValidUntil { get; set; }
        public virtual int CVC { get; set; }
        public virtual decimal Value { get; set; }
    }
}
