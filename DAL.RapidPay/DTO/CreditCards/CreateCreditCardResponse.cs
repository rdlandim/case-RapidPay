namespace DAL.RapidPay.DTO.CreditCards
{
    public class CreateCreditCardResponse
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int CVC { get; set; }
        public string ValidUntil { get; set; }
        public decimal Balance { get; set; }
    }
}
