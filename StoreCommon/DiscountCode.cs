using System;

namespace StoreCommon
{
    public class DiscountCode
    {
        public string Code { get; set; }
        public double Percentage { get; set; }
        public DateTime? Expires { get; set; }

        /// <summary>
        /// A DiscountCode which can be added to the store's active ShoppingCart.
        /// The final sum of a ShoppingCart will equal the TotalSum * (1.0 - Percentage).
        /// </summary>
        /// <param name="code">
        /// The string that represents the DiscountCode.
        /// Cannot be an empty string.
        /// Must be under 100 characters long.
        /// </param>
        /// <param name="percentage">
        /// The percentage of the TotalSum of an ShoppingCart that will be deducted from the TotalSum.
        /// Must be > 0.0 and <= 100.0.
        /// </param>
        /// <param  name="expires">
        /// The DateTime when the DiscountCode expires.
        /// </param>
        public DiscountCode(string code, double percentage, DateTime? expires = null)
        {
            var now = DateTime.Now;
            if (code == null)
            {
                throw new ArgumentNullException("DiscountCode string cannot be null.", nameof(code));
            }
            else if (code.Length == 0 || code.Length > 100)
            {
                throw new ArgumentException("The length of the DiscountCode-string is too short/long.", nameof(code));
            }
            else if (percentage <= 0.0 || percentage > 1.0)
            {
                throw new ArgumentException("The percentage is under 0.0 or above 1.0.", nameof(percentage));
            }
            else if (expires != null && expires <= DateTime.Now)
            {
                throw new ArgumentException("The expiry-DateTime has to be greater than DateTime.Now.", nameof(expires));
            }

            Code = code;
            Percentage = percentage;
            Expires = expires;
        }
    }
}
