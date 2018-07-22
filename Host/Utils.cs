using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersatileFileComparerHost
{
    public static class Utils
    {
        /// <summary>
        /// Helper that replaces a string at the beginning of a string
        /// </summary>
        /// <param name="fulltext"></param>
        /// <param name="front"></param>
        /// <param name="withWhat"></param>
        /// <returns></returns>
        public static string ReplaceFront(this string fulltext, string front, string withWhat)
        {
            var index = fulltext.IndexOf(front,StringComparison.InvariantCulture);

            if (index != 0)
            {
                return fulltext;
            }

            // Use string builder to avoid creating string instances
            var sb = new StringBuilder();
            sb.Append(withWhat);
            sb.Append(fulltext.Substring(front.Length));

            return sb.ToString();
        }
    }
}
