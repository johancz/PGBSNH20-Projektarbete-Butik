using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTemplateGit1
{
    public class Program
    {
        public static void Main()
        {
            string s = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\TextFiles\\LoadProducts.csv";
            // We need this to make sure we can always use periods for decimal points.
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }
    }
}
