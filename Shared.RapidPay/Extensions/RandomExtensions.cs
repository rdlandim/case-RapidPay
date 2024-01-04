namespace Shared.RapidPay.Extensions
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Generates a pseudo-random decimal between 0 and 2
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static decimal NextNonNegativeDecimal(this Random rng)
        {
            byte scale = (byte)rng.Next(29);

            var lo = rng.Next(2);
            var mid = rng.Next(1, 2);
            var hi = rng.Next(0, 1);

            return decimal.Round(new decimal(lo, mid, hi, false, scale), 2, MidpointRounding.AwayFromZero);
        }
    }
}
