namespace DAL.RapidPay.DTO.CreditCards
{
    public class CreditCardPaymentRequest
    {
        public int UserId { get; set; }
        public string CreditCardNumber { get; set; }
        public string ValidUntil { get; set; }
        public int CVC { get; set; }
        public decimal Value { get; set; }
    }
}
