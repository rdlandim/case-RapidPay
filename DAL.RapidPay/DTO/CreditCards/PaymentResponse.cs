namespace DAL.RapidPay.DTO.CreditCards
{
    public class PaymentResponse
    {
        public string GUID { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public CreditCardBalanceResponse Balance { get; set; }
    }
}
