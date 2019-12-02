using System;
namespace Ophelia.Tools.QRCode.Codec.Util
{
    public class ContentConverter
    {
        internal static char newLine = '\n';

        public static String Convert(String targetString)
        {
            if (targetString == null)
                return targetString;
            if (targetString.IndexOf("MEBKM:") > -1)
                targetString = ConvertDocomoBookmark(targetString);
            if (targetString.IndexOf("MECARD:") > -1)
                targetString = ConvertDocomoAddressBook(targetString);
            if (targetString.IndexOf("MATMSG:") > -1)
                targetString = ConvertDocomoMailto(targetString);
            if (targetString.IndexOf("http\\://") > -1)
                targetString = ReplaceString(targetString, "http\\://", "\nhttp://");
            return targetString;
        }

        private static String ConvertDocomoBookmark(String targetString)
        {
            targetString = RemoveString(targetString, "MEBKM:");
            targetString = RemoveString(targetString, "TITLE:");
            targetString = RemoveString(targetString, ";");
            targetString = RemoveString(targetString, "URL:");
            return targetString;
        }

        private static String ConvertDocomoAddressBook(String targetString)
        {

            targetString = RemoveString(targetString, "MECARD:");
            targetString = RemoveString(targetString, ";");
            targetString = ReplaceString(targetString, "N:", "NAME1:");
            targetString = ReplaceString(targetString, "SOUND:", newLine + "NAME2:");
            targetString = ReplaceString(targetString, "TEL:", newLine + "TEL1:");
            targetString = ReplaceString(targetString, "EMAIL:", newLine + "MAIL1:");
            targetString = targetString + newLine;
            return targetString;
        }

        private static String ConvertDocomoMailto(String sIn)
        {
            String sOut = sIn;
            char c = '\n';
            sOut = RemoveString(sOut, "MATMSG:");
            sOut = RemoveString(sOut, ";");
            sOut = ReplaceString(sOut, "TO:", "MAILTO:");
            sOut = ReplaceString(sOut, "SUB:", c + "SUBJECT:");
            sOut = ReplaceString(sOut, "BODY:", c + "BODY:");
            sOut = sOut + c;
            return sOut;
        }

        private static String ReplaceString(String source, String oldText, String newText)
        {
            String s3 = source;
            for (int i = s3.IndexOf(oldText, 0); i > -1; i = s3.IndexOf(oldText, i + newText.Length))
                s3 = s3.Substring(0, (i) - (0)) + newText + s3.Substring(i + oldText.Length);

            return s3;
        }

        private static String RemoveString(String source, String replacement)
        {
            return ReplaceString(source, replacement, "");
        }
    }
}