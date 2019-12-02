using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.ServerSide
{
	public class CacheManager
	{
		public void Cache(string Name, object Object, int Duration)
		{
			CacheManager.AddCache(Name, Object, Duration);
		}
		public object GetObject(string Name)
		{
			return CacheManager.GetCachedObject(Name);
		}
		public int GetIntegerValue(string Name)
		{
			int ReturnValue = int.MinValue;
			if (!int.TryParse(CacheManager.GetCachedObject(Name), out ReturnValue)) {
				ReturnValue = int.MinValue;
			}
			return ReturnValue;
		}
		public string GetStringValue(string Name)
		{
			if (CacheManager.GetCachedObject(Name) != null) {
				return CacheManager.GetCachedObject(Name).ToString();
			}
			return "";
		}
		public DateTime GetDateTimeValue(string Name)
		{
			DateTime ReturnValue = DateTime.MinValue;
			DateTime.TryParse(CacheManager.GetCachedObject(Name), out ReturnValue);
			return ReturnValue;
		}
		public decimal GetDecimalValue(string Name)
		{
			return CacheManager.GetCachedObject(Name);
		}
		public bool RemoveObject(string Name)
		{
			CacheManager.RemoveCachedObject(Name);
		}
		public bool Clear()
		{
			CacheManager.ClearCachedObjects();
		}
		public static void AddCache(string Name, object Object, int Duration)
		{
			if (Object != null) {
				System.Web.HttpContext.Current.Cache.Add(Name, Object, null, DateTime.Now.AddMinutes(Duration), System.Web.Caching.Cache.NoSlidingExpiration, Caching.CacheItemPriority.Normal, null);
			}
		}
		public static object GetCachedObject(string Name)
		{
			if (System.Web.HttpContext.Current != null && !object.ReferenceEquals(System.Web.HttpContext.Current.Cache[Name], DBNull.Value)) {
				return System.Web.HttpContext.Current.Cache[Name];
			}
			return null;
		}
		public static bool RemoveCachedObject(string Name)
		{
			if (GetCachedObject(Name) != null) {
				System.Web.HttpContext.Current.Cache.Remove(Name);
			}
			return false;
		}
		public static bool ClearCachedObjects()
		{
			foreach (DictionaryEntry CachedObject in HttpContext.Current.Cache) {
				HttpContext.Current.Cache.Remove(CachedObject.Key);
			}
		}
		public static bool ClearCachedObjects(string KeyStartsWith)
		{
			foreach (DictionaryEntry CachedObject in HttpContext.Current.Cache) {
				if (CachedObject.Key.ToString().StartsWith(KeyStartsWith)) {
					HttpContext.Current.Cache.Remove(CachedObject.Key);
				}
			}
		}
	}
}
