using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia
{
    public class Guard
    { 
        /// <summary>
        /// Değeri null olan değişkenin ismini fırlatır.
        /// </summary>
        public static void ArgumentNullException(object value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name); 
        }

        /// <summary>
        /// Değeri null ya da boş olan değişkenin ismini fırlatır.
        /// </summary>
        public static void StringNullOrEmptyException(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(name);
        }

        /// <summary>
        /// Değeri 0 ya da 0'dan küçük olan değişkenin ismini fırlatır.
        /// </summary>
        public static void ArgumentInvalidNumberException(int value, string name)
        {
            if (value <= 0)
                throw new ArgumentException(name);
        }

        /// <summary>
        /// Nesnenin o anki değeri geçersiz olduğunda değişkenin ismini fırlatır.
        /// </summary>
        public static void InvalidOperation(object value, string name)
        {
            if (value == null)
                throw new InvalidOperationException(name);
        }

        /// <summary>
        /// Değişkenin aldığı değer, çağrılan metot için izin verilen değer aralığında değilse değişkenin ismini fırlatır
        /// </summary>
        public static void ArgumentOutOfRange(int value, string name, int minimum, int maximum)
        {
            if (minimum > value || value > maximum)
                throw new ArgumentOutOfRangeException(name);
        }

        /// <summary>
        /// Değer bulunamadığında uyarı metni fırlatır.
        /// </summary>
        public static void NotFoundException(object value, object key, string name)
        {
            if (value == null)
                throw new Exception(string.Format(" '{0}' key of '{1}' named entity could not be found. ", key, name));
        }

        /// <summary>
        /// Guid null ya da boş ise değişkenin ismini fırlatır.
        /// </summary>
        public static void EmptyGuidException(Guid value, string name)
        {
            if (value.IsNullOrEmpty())
                throw new ArgumentException(name);
        }

    }
}
