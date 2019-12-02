using System;
using System.Web.Mvc;

namespace System.Web.Routing
{
    public static class TranslatedRouteCollectionExtensions
    {
        public static TranslatedRoute MapTranslatedRoute(this RouteCollection routes, string name, string url, object defaults, object routeValueTranslationProviders, bool setDetectedCulture)
        {
            TranslatedRoute route = new TranslatedRoute(
                url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(routeValueTranslationProviders),
                setDetectedCulture,
                new MvcRouteHandler());
            routes.Add(name, route);
            return route;
        }

        public static TranslatedRoute MapTranslatedRoute(this RouteCollection routes, string name, string url, object defaults, object routeValueTranslationProviders, object constraints, bool setDetectedCulture)
        {
            TranslatedRoute route = new TranslatedRoute(
                url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(routeValueTranslationProviders),
                new RouteValueDictionary(constraints),
                setDetectedCulture,
                new MvcRouteHandler());
            routes.Add(name, route);
            return route;
        }
    }
}
