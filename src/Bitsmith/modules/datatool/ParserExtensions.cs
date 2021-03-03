using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Parsing
{
    public static class ParserExtensions
    {
        
        public static bool TryParse(this TabularData tabularData)
        {
            bool b = false;

            return b;
        }

        public static List<string> AllLines(this string text)
        {
            return new List<string>(text.Split(new char[] { '\r','\n' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static List<string> AllLines(this FileInfo fileInfo)
        {
            return new List<string>(File.ReadAllLines(fileInfo.FullName));
        }

    }
}
