using System;

namespace StoreCommon
{
    public class DiscountCode
    {
        public string Code { get; set; }
        public double Percentage { get; set; }

        public DiscountCode(string code, double percentage)
        {
            SetValues(code, percentage);
        }

        public void SetValues(string code, double percentage)
        {
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

            Code = code;
            Percentage = percentage;
        }
    }
}
