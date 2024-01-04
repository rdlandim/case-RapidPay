namespace Interfaces.RapidPay.UFE
{
    public interface IUFEService
    {
        decimal GetFee(bool forceUpdate = false);
    }
}
