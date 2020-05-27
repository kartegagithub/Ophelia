using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Ophelia.Web.View.Mvc
{
    public static class EnumExtensions
    {
        public static SelectListItem ToSelectListItem(this Enum value, bool isSelected = false)
        {
            return new SelectListItem { Text = value.ToStringValue(), Value = value.GetValue<Int32>(), Selected = isSelected };
        }

        public static string ToClassName(this Enum style, string prefix)
        {
            var styleText = style.ToString()
                .ToDashCase()
                .ToLower(System.Globalization.CultureInfo.InvariantCulture);
            return string.Concat(prefix, styleText); ;
        }

        public static List<SelectListItem> GetEnumSelectList(this Type typeToDrawEnum, Client client)
        {
            var source = Enum.GetValues(typeToDrawEnum);

            var displayAttributeType = typeof(DisplayAttribute);

            var items = new List<SelectListItem>();
            foreach (var value in source)
            {
                FieldInfo field = value.GetType().GetField(value.ToString());

                var attrs = (DisplayAttribute)field.GetCustomAttributes(displayAttributeType, false).FirstOrDefault();
                object underlyingValue = Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()));

                items.Add(new SelectListItem() { Text = (attrs != null ? client.TranslateText(attrs.GetName()) : client.TranslateText(value.ToString())), Value = Convert.ToString(underlyingValue) });
            }
            return items;
        }

        public static string GetEnumDisplayName(this Type typeToDrawEnum, object selectedValue, Client client)
        {
            var source = Enum.GetValues(typeToDrawEnum);

            var displayAttributeType = typeof(DisplayAttribute);

            var items = new List<SelectListItem>();
            foreach (var value in source)
            {
                FieldInfo field = value.GetType().GetField(value.ToString());

                var attrs = (DisplayAttribute)field.GetCustomAttributes(displayAttributeType, false).FirstOrDefault();
                object underlyingValue = Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()));
                if(Convert.ToString(underlyingValue) == Convert.ToString(selectedValue))
                {
                    return (attrs != null ? client.TranslateText(attrs.GetName()) : client.TranslateText(value.ToString()));
                }
            }
            return "";
        }
    }
}