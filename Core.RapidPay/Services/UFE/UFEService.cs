using Interfaces.RapidPay.UFE;
using Microsoft.Extensions.Caching.Memory;
using Shared.RapidPay.Extensions;

namespace Core.RapidPay.Services.UFE
{
    public class UFEService : IUFEService
    {
        private TimeSpan _now => DateTime.Now.TimeOfDay;
        private readonly IMemoryCache _memoryCache;
        private readonly string _feeCacheKey = "fee";
        private readonly string _lastUpdateCacheKey = "lastUpdate";

        public UFEService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            CalculateNewFee();
        }

        public decimal GetFee(bool forceUpdate = false)
        {
            if (IsNextUpdate(forceUpdate))
                CalculateNewFee();

            return _memoryCache.Get<decimal>(_feeCacheKey);
        }

        private void CalculateNewFee()
        {
            var rnd = new Random();

            var nextFeeDecimal = (decimal)rnd.NextDouble() * 2;

            var cachedFee = _memoryCache.GetOrCreate(_feeCacheKey, value => { return new decimal(0); });

            if (cachedFee == 0)
                _memoryCache.Set(_feeCacheKey, nextFeeDecimal);
            else
                _memoryCache.Set(_feeCacheKey, cachedFee * nextFeeDecimal);

            UpdateLastFeeTime();
        }

        private void UpdateLastFeeTime()
        {
            _memoryCache.Set(_lastUpdateCacheKey, new TimeSpan(_now.Hours, 0, 0));
        }

        private bool IsNextUpdate(bool forceUpdate = false)
        {
            if (forceUpdate)
                return true;

            var lastUpdate = _memoryCache.GetOrCreate(_lastUpdateCacheKey, value => { return new TimeSpan(_now.Hours, 0, 0); });

            return (_now - lastUpdate).TotalHours >= 1;
        }
    }
}
