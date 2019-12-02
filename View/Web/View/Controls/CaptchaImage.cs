using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public class CaptchaImage
	{
		private int nWidth;
		private int nHeight;

		private string sFamilyName;
		public string FamilyName {
			get { return this.sFamilyName; }
			set { this.sFamilyName = value; }
		}
		public int Width {
			get { return this.nWidth; }
			set { this.nWidth = value; }
		}
		public int Height {
			get { return this.nHeight; }
			set { this.nHeight = value; }
		}
		public string Draw()
		{
			FamilyName.Replace(" ", "_");
			Random RandomGenerator = new Random();
			Image Image = new Image("", "?DisplayCaptchaImage=width,," + Width + "$$$height,," + Height + "$$$familyname,," + FamilyName + "$$$requester,," + RandomGenerator.Next(1, 99999));

			return Image.Draw;
		}
		public CaptchaImage(int Width, int Height, string FamilyName = "Arial")
		{
			this.nWidth = Width;
			this.nHeight = Height;
			this.sFamilyName = FamilyName;

		}

		public CaptchaImage()
		{
		}
		public static bool CheckCode(string Code, System.Web.UI.Page Form)
		{
			return Form.Session["CaptchaImageCode"] == Code;
		}
		public static void ClearCode(System.Web.UI.Page Form)
		{
			Form.Session.Remove("CaptchaImageCode");
		}
		public static bool CheckCode(string Code, System.Web.SessionState.HttpSessionState HttpSession)
		{
			return HttpSession["CaptchaImageCode"] == Code;
		}
		public static void ClearCode(System.Web.SessionState.HttpSessionState HttpSession)
		{
			HttpSession.Remove("CaptchaImageCode");
		}
	}
}

