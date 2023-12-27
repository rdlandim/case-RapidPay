namespace DAL.RapidPay.DTO.CreditCards
{
    public class CreateCreditCardResponse
    {
        public virtual int Id { get; set; }
        public virtual string Number { get; set; }
        public virtual int CVC { get; set; }
        public virtual string ValidUntil { get; set; }
        public virtual decimal Balance { get; set; }
    }
}
