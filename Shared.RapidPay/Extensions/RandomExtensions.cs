namespace Shared.RapidPay.Extensions
{
    public static class RandomExtensions
    {
        public static decimal NextNonNegativeDecimal(this Random rng, int min, int max)
        {
            byte scale = (byte)rng.Next(29);

            var lo = rng.Next(2);
            var mid = rng.Next(1, 2);
            var hi = rng.Next(0, 1);

            return decimal.Round(new decimal(lo, mid, hi, false, scale), 2, MidpointRounding.AwayFromZero);
        }
    }
}
