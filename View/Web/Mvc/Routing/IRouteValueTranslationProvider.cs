using System.Globalization;

namespace System.Web.Routing
{
    public interface IRouteValueTranslationProvider
    {
        RouteValueTranslation TranslateToRouteValue(string translatedValue, CultureInfo culture);
        RouteValueTranslation TranslateToTranslatedValue(string routeValue, CultureInfo culture);
    }
}
