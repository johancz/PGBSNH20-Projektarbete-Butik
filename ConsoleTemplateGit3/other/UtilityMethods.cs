using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTemplateGit3.other
{
    public static class UtilityMethods
    {
        public static List<int> RandomInts(int min = 0, int max = 10, int length = 10)
        {
            var random = new Random();
            var ints = new List<int>();

            for (int i = 0; i < length; i++)
            {
                ints.Add(random.Next(min, max));
            }
            return ints;
        }
    }
}
