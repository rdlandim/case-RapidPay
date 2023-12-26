using Interfaces.RapidPay.UFE;
using Shared.RapidPay.Extensions;

namespace Core.RapidPay.Services.UFE
{
    public class UFEService : IUFEService
    {
        private TimeSpan _now => DateTime.Now.TimeOfDay;
        private TimeSpan _lastFeeUpdate;
        private decimal _fee = new(0);

        public UFEService()
        {
            CalculateNewFee();
        }

        public decimal GetFee()
        {
            if (IsNextUpdate())
                CalculateNewFee();

            return _fee;
        }

        private void CalculateNewFee()
        {
            var rnd = new Random();

            var nextFeeDecimal = rnd.NextNonNegativeDecimal(0, 2);

            if (_fee == 0)
                _fee = nextFeeDecimal;
            else
                _fee *= nextFeeDecimal;

            UpdateLastFeeTime();
        }

        private void UpdateLastFeeTime()
        {
            _lastFeeUpdate = new TimeSpan(_now.Hours, 0, 0);
        }

        private bool IsNextUpdate()
        {
            return (_now - _lastFeeUpdate).TotalHours >= 1;
        }
    }
}
