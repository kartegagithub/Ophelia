using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* http://www.codeproject.com/Articles/112949/Number-To-Word-Arabic-Version */

namespace Ophelia.Globalization.NumberToWords
{
    public abstract class Converter
    {
        /// <summary>Decimal Part</summary>
        public int DecimalValue { get; set; }

        /// <summary> integer part </summary>
        public long IntegerValue { get; set; }

        public string CultureCode { get; set; }
        public string CurrencyCode { get; set; }
        public string AndOperatorString { get; set; }

        public byte PartPrecision { get; set; }

        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string CurrencyName { get; set; }
        public string PluralCurrencyName { get; set; }
        public string CurrencyPartName { get; set; }
        public string PluralCurrencyPartName { get; set; }

        public string[] Ones { get; set; }
        public string[] Tens { get; set; }
        public string[] Groups { get; set; }


        public Converter()
        {
            this.Ones = new string[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            this.Tens = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            this.Groups = new string[] { "Hundred", "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion" };
            this.CurrencyName = "Dollar";
            this.PluralCurrencyName = "Dollars";
            this.Prefix = "Only";
            this.AndOperatorString = " and ";
            this.CurrencyPartName = "Penny";
            this.PluralCurrencyPartName = "Pennies";
        }

        public Converter(string CultureCode, string CurrencyCode)
            : this()
        {
            this.CultureCode = CultureCode;
            this.CurrencyCode = CurrencyCode;
        }


        public static string Get(string amount, string cultureCode, string currencyCode)
        {
            return Get(Convert.ToDecimal(amount.ToString().Replace(".", ",")), cultureCode, currencyCode);
        }

        public static string Get(decimal amount, string cultureCode, string CurrencyCode)
        {
            Converter Converter = null;
            switch (cultureCode)
            {
                case "af-ZA" /*Afrikaans - South Africa*/: break;
                case "sq-AL" /*Albanian - Albania*/: break;
                case "ar-DZ" /*Arabic - Algeria*/: break;
                case "ar-BH" /*Arabic - Bahrain*/: break;
                case "ar-EG" /*Arabic - Egypt*/: break;
                case "ar-IQ" /*Arabic - Iraq*/: break;
                case "ar-JO" /*Arabic - Jordan*/: break;
                case "ar-KW" /*Arabic - Kuwait*/: break;
                case "ar-LB" /*Arabic - Lebanon*/: break;
                case "ar-LY" /*Arabic - Libya*/: break;
                case "ar-MA" /*Arabic - Morocco*/: break;
                case "ar-OM" /*Arabic - Oman*/: break;
                case "ar-QA" /*Arabic - Qatar*/: break;

                case "ar-SA" /*Arabic - Saudi Arabia*/:
                case "ar-SY" /*Arabic - Syria*/:
                case "syr-SY" /*Syriac - Syria*/:
                case "ar-TN" /*Arabic - Tunisia*/:
                case "ar-AE" /*Arabic - United Arab Emirates*/:
                    Converter = new ArabicConverter(cultureCode, CurrencyCode);
                    break;

                case "ar-YE" /*Arabic - Yemen*/: break;
                case "hy-AM" /*Armenian - Armenia*/: break;
                case "Cy-az-AZ" /*Azeri (Cyrillic) - Azerbaijan*/: break;
                case "Lt-az-AZ" /*Azeri (Latin) - Azerbaijan*/: break;
                case "eu-ES" /*Basque - Basque*/: break;
                case "be-BY" /*Belarusian - Belarus*/: break;
                case "bg-BG" /*Bulgarian - Bulgaria*/: break;
                case "ca-ES" /*Catalan - Catalan*/: break;
                case "zh-CN" /*Chinese - China*/: break;
                case "zh-HK" /*Chinese - Hong Kong SAR*/: break;
                case "zh-MO" /*Chinese - Macau SAR*/: break;
                case "zh-SG" /*Chinese - Singapore*/: break;
                case "zh-TW" /*Chinese - Taiwan*/: break;
                case "zh-CHS" /*Chinese (Simplified)*/: break;
                case "zh-CHT" /*Chinese (Traditional)*/: break;
                case "hr-HR" /*Croatian - Croatia*/: break;
                case "cs-CZ" /*Czech - Czech Republic*/: break;
                case "da-DK" /*Danish - Denmark*/: break;
                case "div-MV" /*Dhivehi - Maldives*/: break;
                case "nl-BE" /*Dutch - Belgium*/: break;
                case "nl-NL" /*Dutch - The Netherlands*/: break;
                case "en-AU" /*English - Australia*/: break;
                case "en-BZ" /*English - Belize*/: break;
                case "en-CA" /*English - Canada*/: break;
                case "en-CB" /*English - Caribbean*/: break;
                case "en-IE" /*English - Ireland*/: break;
                case "en-JM" /*English - Jamaica*/: break;
                case "en-NZ" /*English - New Zealand*/: break;
                case "en-PH" /*English - Philippines*/: break;
                case "en-ZA" /*English - South Africa*/: break;
                case "en-TT" /*English - Trinidad and Tobago*/: break;
                case "en-GB" /*English - United Kingdom*/:
                    Converter = new BritishConverter(cultureCode, CurrencyCode);
                    break;
                case "en-ZW" /*English - Zimbabwe*/: break;
                case "et-EE" /*Estonian - Estonia*/: break;
                case "fo-FO" /*Faroese - Faroe Islands*/: break;
                case "fa-IR" /*Farsi - Iran*/: break;
                case "fi-FI" /*Finnish - Finland*/: break;
                case "fr-BE" /*French - Belgium*/: break;
                case "fr-CA" /*French - Canada*/: break;
                case "fr-FR" /*French - France*/: break;
                case "fr-LU" /*French - Luxembourg*/: break;
                case "fr-MC" /*French - Monaco*/: break;
                case "fr-CH" /*French - Switzerland*/: break;
                case "gl-ES" /*Galician - Galician*/: break;
                case "ka-GE" /*Georgian - Georgia*/: break;
                case "de-AT" /*German - Austria*/: break;
                case "de-DE" /*German - Germany*/: break;
                case "de-LI" /*German - Liechtenstein*/: break;
                case "de-LU" /*German - Luxembourg*/: break;
                case "de-CH" /*German - Switzerland*/: break;
                case "el-GR" /*Greek - Greece*/: break;
                case "gu-IN" /*Gujarati - India*/: break;
                case "he-IL" /*Hebrew - Israel*/: break;
                case "hi-IN" /*Hindi - India*/: break;
                case "hu-HU" /*Hungarian - Hungary*/: break;
                case "is-IS" /*Icelandic - Iceland*/: break;
                case "id-ID" /*Indonesian - Indonesia*/: break;
                case "it-IT" /*Italian - Italy*/: break;
                case "it-CH" /*Italian - Switzerland*/: break;
                case "ja-JP" /*Japanese - Japan*/: break;
                case "kn-IN" /*Kannada - India*/: break;
                case "kk-KZ" /*Kazakh - Kazakhstan*/: break;
                case "kok-IN" /*Konkani - India*/: break;
                case "ko-KR" /*Korean - Korea*/: break;
                case "ky-KZ" /*Kyrgyz - Kazakhstan*/: break;
                case "lv-LV" /*Latvian - Latvia*/: break;
                case "lt-LT" /*Lithuanian - Lithuania*/: break;
                case "mk-MK" /*Macedonian (FYROM)*/: break;
                case "ms-BN" /*Malay - Brunei*/: break;
                case "ms-MY" /*Malay - Malaysia*/: break;
                case "mr-IN" /*Marathi - India*/: break;
                case "mn-MN" /*Mongolian - Mongolia*/: break;
                case "nb-NO" /*Norwegian (Bokmål) - Norway*/: break;
                case "nn-NO" /*Norwegian (Nynorsk) - Norway*/: break;
                case "pl-PL" /*Polish - Poland*/: break;
                case "pt-BR" /*Portuguese - Brazil*/: break;
                case "pt-PT" /*Portuguese - Portugal*/: break;
                case "pa-IN" /*Punjabi - India*/: break;
                case "ro-RO" /*Romanian - Romania*/: break;
                case "ru-RU" /*Russian - Russia*/: break;
                case "sa-IN" /*Sanskrit - India*/: break;
                case "Cy-sr-SP" /*Serbian (Cyrillic) - Serbia*/: break;
                case "Lt-sr-SP" /*Serbian (Latin) - Serbia*/: break;
                case "sk-SK" /*Slovak - Slovakia*/: break;
                case "sl-SI" /*Slovenian - Slovenia*/: break;
                case "es-AR" /*Spanish - Argentina*/: break;
                case "es-BO" /*Spanish - Bolivia*/: break;
                case "es-CL" /*Spanish - Chile*/: break;
                case "es-CO" /*Spanish - Colombia*/: break;
                case "es-CR" /*Spanish - Costa Rica*/: break;
                case "es-DO" /*Spanish - Dominican Republic*/: break;
                case "es-EC" /*Spanish - Ecuador*/: break;
                case "es-SV" /*Spanish - El Salvador*/: break;
                case "es-GT" /*Spanish - Guatemala*/: break;
                case "es-HN" /*Spanish - Honduras*/: break;
                case "es-MX" /*Spanish - Mexico*/: break;
                case "es-NI" /*Spanish - Nicaragua*/: break;
                case "es-PA" /*Spanish - Panama*/: break;
                case "es-PY" /*Spanish - Paraguay*/: break;
                case "es-PE" /*Spanish - Peru*/: break;
                case "es-PR" /*Spanish - Puerto Rico*/: break;
                case "es-ES" /*Spanish - Spain*/: break;
                case "es-UY" /*Spanish - Uruguay*/: break;
                case "es-VE" /*Spanish - Venezuela*/: break;
                case "sw-KE" /*Swahili - Kenya*/: break;
                case "sv-FI" /*Swedish - Finland*/: break;
                case "sv-SE" /*Swedish - Sweden*/: break;
                case "ta-IN" /*Tamil - India*/: break;
                case "tt-RU" /*Tatar - Russia*/: break;
                case "te-IN" /*Telugu - India*/: break;
                case "th-TH" /*Thai - Thailand*/: break;
                case "tr-TR" /*Turkish - Turkey*/:
                    Converter = new TurkishConvertor(cultureCode, CurrencyCode);
                    break;
                case "uk-UA" /*Ukrainian - Ukraine*/: break;
                case "ur-PK" /*Urdu - Pakistan*/: break;
                case "Cy-uz-UZ" /*Uzbek (Cyrillic) - Uzbekistan*/: break;
                case "Lt-uz-UZ" /*Uzbek (Latin) - Uzbekistan*/: break;
                case "vi-VN" /*Vietnamese - Vietnam*/: break;
            }
            if (Converter == null)
            {
                Converter = new AmericanConverter(cultureCode, CurrencyCode);
                Converter.PartPrecision = 2;
            }

            Converter.ExtractIntegerAndDecimalParts(amount);
            return Converter.Get(amount);
        }

        public virtual string Get(decimal Amount)
        {
            Decimal tempNumber = Amount;

            if (tempNumber == 0)
                return "Zero";

            string decimalString = ProcessGroup(this.DecimalValue);

            string retVal = String.Empty;

            int group = 0;

            if (tempNumber < 1)
            {
                retVal = this.Ones[0];
            }
            else
            {
                while (tempNumber >= 1)
                {
                    int numberToProcess = (int)(tempNumber % 1000);

                    tempNumber = tempNumber / 1000;

                    string groupDescription = ProcessGroup(numberToProcess);

                    if (groupDescription != String.Empty)
                    {
                        if (group > 0)
                        {
                            retVal = String.Format("{0} {1}", this.Groups[group], retVal);
                        }

                        retVal = this.ValidateGroup(String.Format("{0} {1}", groupDescription, retVal), tempNumber);
                    }
                    group++;
                }
            }

            String formattedNumber = String.Empty;
            formattedNumber += (this.Prefix != String.Empty) ? String.Format("{0} ", this.Prefix) : String.Empty;
            formattedNumber += (retVal != String.Empty) ? retVal : String.Empty;
            formattedNumber += (retVal != String.Empty) ? (this.IntegerValue == 1 ? this.CurrencyName : this.PluralCurrencyName) : String.Empty;
            formattedNumber += (decimalString != String.Empty) ? " " + this.AndOperatorString + " " : String.Empty;
            formattedNumber += (decimalString != String.Empty) ? decimalString : String.Empty;
            formattedNumber += (decimalString != String.Empty) ? " " + (this.DecimalValue == 1 ? this.CurrencyPartName : this.PluralCurrencyPartName) : String.Empty;
            formattedNumber += (this.Suffix != String.Empty) ? String.Format(" {0}", this.Suffix) : String.Empty;

            return formattedNumber;
        }

        protected virtual string ValidatePart(string value, int Hundreds, int Tens, int Ones)
        {
            return value;
        }

        protected virtual string ValidateGroup(string value, decimal decValue)
        {
            return value;
        }

        internal void ExtractIntegerAndDecimalParts(decimal amount)
        {
            String[] splits = amount.ToString().Replace(",", ".").Split('.');

            this.IntegerValue = Convert.ToInt32(splits[0]);

            if (splits.Length > 1)
                this.DecimalValue = Convert.ToInt32(this.GetDecimalValue(splits[1]));
        }

        internal string GetDecimalValue(string decimalPart)
        {
            string result = String.Empty;

            if (this.PartPrecision != decimalPart.Length)
            {
                int decimalPartLength = decimalPart.Length;

                for (int i = 0; i < this.PartPrecision - decimalPartLength; i++)
                {
                    decimalPart += "0"; //Fix for 1 number after decimal ( 10.5 , 1442.2 , 375.4 ) 
                }

                result = String.Format("{0}.{1}", decimalPart.Substring(0, this.PartPrecision), decimalPart.Substring(this.PartPrecision, decimalPart.Length - this.PartPrecision));

                result = (Math.Round(Convert.ToDecimal(result))).ToString();
            }
            else
                result = decimalPart;

            for (int i = 0; i < this.PartPrecision - result.Length; i++)
            {
                result += "0";
            }

            return result;
        }

        internal string ProcessGroup(int groupNumber)
        {
            int tens = groupNumber % 100;

            int hundreds = groupNumber / 100;

            int ones = tens % 10;

            string retVal = String.Empty;

            if (hundreds > 0)
            {
                retVal = this.ValidatePart(String.Format("{0} {1}", this.Ones[hundreds], this.Groups[0]), hundreds, tens, ones);
            }
            if (tens > 0)
            {
                if (tens < 20)
                {
                    retVal += this.ValidatePart(((retVal != String.Empty) ? " " : String.Empty) + this.Ones[tens], hundreds, tens, ones);
                }
                else
                {
                    tens = (tens / 10) - 2; // 20's offset

                    retVal += this.ValidatePart(((retVal != String.Empty) ? " " : String.Empty) + this.Tens[tens], hundreds, tens, ones);

                    if (ones > 0)
                    {
                        retVal += this.ValidatePart(((retVal != String.Empty) ? " " : String.Empty) + this.Ones[ones], hundreds, tens, ones);
                    }
                }
            }

            return retVal;
        }
    }
}
