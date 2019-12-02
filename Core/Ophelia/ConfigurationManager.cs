using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ophelia
{
    public static class ConfigurationManager
    {
        public static T GetParameter<T>(string key, T defaultValue = default(T)) where T : struct
        {
            try
            {
                object value = GetParameter(key);
                return value as T? ?? defaultValue;
            }
            catch { return defaultValue; }
        }
        public static string GetParameter(string key, string defaultValue = "")
        {
            try
            {
                string returnValue = defaultValue;
                if (!string.IsNullOrEmpty(key) && System.Configuration.ConfigurationManager.AppSettings[key] != null)
                    returnValue = System.Configuration.ConfigurationManager.AppSettings[key];
                return returnValue;
            }
            catch { return defaultValue; }
        }
        public static void SetParameter(string key, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                    if (config.AppSettings.Settings[key] == null)
                        config.AppSettings.Settings.Add(key, value);
                    else
                        config.AppSettings.Settings[key].Value = value;
                    config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                    System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch { }
        }
        public static void AddKey(string key, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                    if (config.AppSettings.Settings[key] == null)
                        config.AppSettings.Settings.Add(key, value);
                    else
                        config.AppSettings.Settings[key].Value = value;
                    config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                    System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch (Exception ex)
            {
                var s = ex.StackTrace;
            }
        }
        public static void RemoveKey(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                    if (config.AppSettings.Settings[key] != null)
                    {
                        config.AppSettings.Settings.Remove(key);
                        config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                        System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                    }
                }
            }
            catch { }
        }
        public static Uri DomainUrl
        {
            get
            {
                string domainUrl = GetParameter("DomainUrl").ToString();
                if (Uri.IsWellFormedUriString(domainUrl, UriKind.Absolute)) return new Uri(domainUrl);
                return null;
            }
        }
        public static string CdnUrl
        {
            get { return GetParameter("CdnUrl").ToString(); }
        }
        public static ApplicationEnvironment ApplicationEnvironment
        {
            get
            {
                if (HttpContext.Current.Request.Url.Authority.IndexOf("localhost", StringComparison.InvariantCultureIgnoreCase) > -1)
                    return ApplicationEnvironment.Local;
                else
                {
                    ApplicationEnvironment environment = ApplicationEnvironment.Live;
                    string value = GetParameter("ApplicationEnvironment", "Live");
                    if (!string.IsNullOrEmpty(value) && Enum.IsDefined(typeof(ApplicationEnvironment), value))
                        environment = (ApplicationEnvironment)Enum.Parse(typeof(ApplicationEnvironment), value);
                    return environment;
                }
            }
        }
        public static string ApplicationBase
        {
            get { return GetParameter("ApplicationBase", "/"); }
        }
        public static string ApplicationVersion
        {
            get { return GetParameter("ApplicationVersion"); }
        }
        public static string CompanyInfo
        {
            get { return GetParameter("CompanyInfo"); }
        }
        public static string Culture
        {
            get { return GetParameter("Culture", "tr-TR"); }
        }
        public static string Language
        {
            get { return GetParameter("Language", "TR"); }
        }
        public static string EncryptKey
        {
            get { return GetParameter("EncryptKey"); }
        }
        public static bool IgnoreSecurity
        {
            get { return GetParameter("IgnoreSecurity", "false").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static bool RemoveW3Prefix
        {
            get { return GetParameter("RemoveW3Prefix", "true").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static bool IsTestMode
        {
            get { return GetParameter("TestMode", "off").ToString().Equals("on", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static string SuperUserPassword
        {
            get { return GetParameter("SuperUserPassword").ToString(); }
        }
        public static string SmtpServer
        {
            get { return GetParameter("SmtpServer"); }
        }
        public static int SmtpPort
        {
            get { return int.Parse(GetParameter("SmtpPort", "25")); }
        }
        public static string SmtpUser
        {
            get { return GetParameter("SmtpUser"); }
        }
        public static string SmtpPassword
        {
            get { return GetParameter("SmtpPassword"); }
        }
        public static string MailFrom
        {
            get { return GetParameter("MailFrom"); }
        }
        public static string MailRecipients
        {
            get { return GetParameter("MailRecipients"); }
        }
        public static bool MailingEnabled
        {
            get { return GetParameter("MailingEnabled", "true").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static bool GoogleRemarketingEnabled
        {
            get { return !ConfigurationManager.IsTestMode && ConfigurationManager.GetParameter("GoogleRemarketingEnabled", "true").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static bool GoogleAnalyticsEnabled
        {
            get { return !ConfigurationManager.IsTestMode && ConfigurationManager.GetParameter("GoogleAnalyticsEnabled", "true").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static bool TagManagerEnabled
        {
            get { return !ConfigurationManager.IsTestMode && ConfigurationManager.GetParameter("TagManagerEnabled", "true").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public static string GoogleAnalyticsAccountID
        {
            get { return ConfigurationManager.GetParameter("GoogleAnalyticsAccountID").ToString(); }
        }
        public static string TagManagerAccountID
        {
            get { return ConfigurationManager.GetParameter("TagManagerAccountID").ToString(); }
        }
        public static string GoogleConversionID
        {
            get { return ConfigurationManager.GetParameter("GoogleConversionID").ToString(); }
        }
        public static string GoogleSiteVerificationKey
        {
            get { return ConfigurationManager.GetParameter("GoogleSiteVerificationKey").ToString(); }
        }
    }
}
