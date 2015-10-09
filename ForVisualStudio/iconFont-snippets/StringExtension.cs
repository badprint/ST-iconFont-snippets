using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iconFont_snippets.StringExtension
{
    public static class StringExtension
    {
        public static string GetStrBetweenTags(this string value,
                               string startTag,
                               string endTag)
        {
            if (value.Contains(startTag) && value.Contains(endTag))
            {
                int index = value.IndexOf(startTag) + startTag.Length;
                return value.Substring(index, value.IndexOf(endTag) - index);
            }
            else
                return null;
        }
    }
}
