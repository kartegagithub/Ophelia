using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.Extensions
{
    public static class StringExtensions
    {
        public static string CheckForInjection(string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("/*", "").Replace("*/", "").Replace("\"", "&quot;");
            return value;
        }
        public static object ArrangeStringAgainstRiskyChar(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Replace("<", "").Replace(">", "").Replace("(", "").Replace(")", "").Replace(";", "").Replace("&", "").Replace("+", "").Replace("%", "").Replace("#", "").Replace("$", "").Replace("\\", "").Replace("*", "").Replace("|", "").Replace("'", "").Replace("script", "");
            return value;
        }
        public static string FormatForWebUrl(this string url)
        {
            url = url.Trim();
            url = url.Replace("ğ", "g").Replace("Ğ", "G").Replace("ü", "u").Replace("Ü", "U").Replace("ş", "s").Replace("Ş", "S").Replace("ç", "c").Replace("Ç", "C").Replace("ö", "o").Replace("Ö", "O").Replace("ı", "i").Replace("İ", "I");
            url = url.Replace("$", "").Replace("&", "").Replace("+", "").Replace(",", "").Replace("/", "").Replace(":", "").Replace(";", "").Replace("=", "").Replace("?", "").Replace("@", "").Replace("(", "").Replace(")", "").Replace("*", "").Replace("_", "").Replace("!", "");
            url = url.Replace("'", "").Replace("<", "").Replace(">", "").Replace("#", "").Replace("%", "").Replace("{", "").Replace("}", "").Replace("|", "").Replace("\\\\", "").Replace("^", "").Replace("~", "").Replace("[", "").Replace("]", "").Replace("`", "");
            url = url.Replace(".", "").Replace("\\'", "").Replace("‘", "").Replace("’", "").Replace("\"", "").Replace("ˆ", "").Replace("‚", "");
            url = url.Replace("  ", "-").Replace(" ", "-");
            return url;
        }
        public static string GetAvailableIDValue(this string value)
        {
            value = value.ToUpper();
            string result = string.Empty;
            for (int i = 0; i <= value.Length - 1; i++)
            {
                int temp = char.ConvertToUtf32(value, i);
                if ((temp >= 33 && temp < 47)
                    || (temp == 64)
                    || (temp >= 91 && temp <= 96)
                    || (temp >= 123 && temp <= 137)
                    || (temp >= 139 && temp <= 140)
                    || (temp >= 145 && temp <= 148)
                    || (temp >= 150 && temp <= 156)
                    || (temp >= 162 && temp <= 191)
                    || (temp == 240)
                    || (temp == 247))
                {
                    continue;
                }
                else if (temp == 138)
                {
                    result += "S";
                }
                else if (temp == 142)
                {
                    result += "Z";
                }
                else if (temp == 149)
                {
                    result += "O";
                }
                else if (temp == 158)
                {
                    result += "Z";
                }
                else if (temp == 159)
                {
                    result += "Y";
                }
                else if (temp == 161)
                {
                    result += "I";
                }
                else if (temp >= 192 && temp <= 198)
                {
                    result += "A";
                }
                else if (temp == 199)
                {
                    result += "C";
                }
                else if (temp >= 200 && temp <= 203)
                {
                    result += "E";
                }
                else if (temp >= 204 && temp <= 207)
                {
                    result += "I";
                }
                else if (temp == 208)
                {
                    result += "D";
                }
                else if (temp == 209)
                {
                    result += "N";
                }
                else if (temp >= 210 && temp <= 214)
                {
                    result += "O";
                }
                else if (temp == 215)
                {
                    result += "X";
                }
                else if (temp == 216)
                {
                    result += "Q";
                }
                else if (temp >= 217 && temp <= 221)
                {
                    result += "U";
                }
                else if (temp == 216)
                {
                    result += "Y";
                }
                else if (temp >= 222 && temp <= 223)
                {
                    result += "B";
                }
                else if (temp >= 224 && temp <= 230)
                {
                    result += "A";
                }
                else if (temp == 231)
                {
                    result += "C";
                }
                else if (temp >= 232 && temp <= 235)
                {
                    result += "E";
                }
                else if (temp >= 232 && temp <= 235)
                {
                    result += "I";
                }
                else if (temp >= 242 && temp <= 246)
                {
                    result += "O";
                }
                else if (temp == 241)
                {
                    result += "N";
                }
                else if (temp == 248)
                {
                    result += "Q";
                }
                else if (temp >= 249 && temp <= 252)
                {
                    result += "U";
                }
                else if (temp == 253)
                {
                    result += "Y";
                }
                else if (temp == 254)
                {
                    result += "B";
                }
                else if (temp == 255)
                {
                    result += "Y";
                }
                else
                {
                    result += value[i];
                }
            }
            result = result.Replace("Ý", "I").Replace("Þ", "S").Replace("Ç", "C").Replace("Ð", "G").Replace("Ö", "O").Replace("Ü", "U");
            return result;
        }
    }
}
