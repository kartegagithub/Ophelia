using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.Web.Routing
{
    public class DictionaryRouteValueTranslationProvider
        : IRouteValueTranslationProvider
    {
        public IList<RouteValueTranslation> Translations { get; private set; }

        public DictionaryRouteValueTranslationProvider(IList<RouteValueTranslation> translations)
        {
            this.Translations = translations;
        }

        public RouteValueTranslation TranslateToRouteValue(string translatedValue, CultureInfo culture)
        {
            RouteValueTranslation translation = null;

            translation = this.Translations.Where(
                t => t.TranslatedValue.Equals(translatedValue, StringComparison.InvariantCultureIgnoreCase)
                    && (t.Culture.ToString() == culture.ToString() || t.Culture.ToString().Substring(0, 2) == culture.ToString().Substring(0, 2)))
                .OrderByDescending(t => t.Culture)
                .FirstOrDefault();
            if (translation != null)
            {
                return translation;
            }

            translation = this.Translations.Where(t => t.TranslatedValue.Equals(translatedValue, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if (translation != null)
            {
                return translation;
            }

            return new RouteValueTranslation
            {
                Culture = culture,
                RouteValue = translatedValue,
                TranslatedValue = translatedValue
            };
        }

        public RouteValueTranslation TranslateToTranslatedValue(string routeValue, CultureInfo culture)
        {
            RouteValueTranslation translation = null;

            translation = this.Translations.Where(
                t => t.RouteValue.Equals(routeValue, StringComparison.InvariantCultureIgnoreCase)
                    && (t.Culture.ToString() == culture.ToString() || t.Culture.ToString().Substring(0, 2) == culture.ToString().Substring(0, 2)))
                .OrderByDescending(t => t.Culture)
                .FirstOrDefault();
            if (translation != null)
            {
                return translation;
            }

            translation = this.Translations.Where(t => t.RouteValue.Equals(routeValue,StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            if (translation != null)
            {
                return translation;
            }

            return new RouteValueTranslation
            {
                Culture = culture,
                RouteValue = routeValue,
                TranslatedValue = routeValue
            };
        }
    }
}
