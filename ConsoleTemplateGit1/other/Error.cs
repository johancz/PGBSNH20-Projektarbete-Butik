using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleTemplateGit1
{
    class Error
    {
        public static void ToLog(Exception ex)
        {
            var lines = new List<string>
            {
                DateTime.Now.ToString(),
                $"Message: {ex.Message}",
                ex.StackTrace
            };

            File.AppendAllLines(TextfilesIO.TextfilesPath + "ErrorLog.csv", lines);
        }
    }
}
