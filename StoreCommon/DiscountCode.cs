using System;
using System.Text.RegularExpressions;

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
                throw new ArgumentNullException("DiscountCode string cannot be null.");
            }

            code = code.Trim();

            if (code.Length < 3 || code.Length > 20)
            {
                throw new ArgumentException("The length of the DiscountCode-string is too short/long.");
            }
            // Check if 'code' is a valid English alphanumeric string.
            // ^ = start of string, [] = contains the valid characters.
            // 0-9 = range of valid digits,
            // a-z = range of valid lowercase letters,
            // and A-Z = range of valid uppercaser letters
            // * match 0 or more of the characters in the []
            // $ = end of the string
            else if (!Regex.IsMatch(code, "^[0-9a-zA-Z]*$"))
            {
                throw new Exception("The discount code is not valid. Only English alphanumerics are allowed.");
            }
            
            if (percentage <= 0.0 || percentage > 1.0)
            {
                throw new ArgumentException("The percentage value must be between 0 and 1 (inclusive).");
            }

            Code = code;
            Percentage = percentage;
        }
    }
}
