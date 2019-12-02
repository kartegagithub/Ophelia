using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Text;
using System.Net;
using System.IO;
namespace Ophelia.Web.View.Controls.ServerSide
{
	public class ImageDrawer
	{
		public static void DrawCaptchaImage(System.Web.UI.Page Form)
		{
			DrawCaptchaImage(Form.Request, Form.Response, Form.Session);
		}
		public static void DrawCaptchaImage(HttpRequest HttpRequest, HttpResponse HttpResponse, System.Web.SessionState.HttpSessionState HttpSession)
		{
			string sText = null;
			int nWidth = 0;
			int nHeight = 0;
			string sFamilyName = "";
			Random oRandom = new Random();

			string DisplayImage = HttpRequest["DisplayCaptchaImage"];

			DisplayImage = DisplayImage.Replace(",,", "=");
			DisplayImage = DisplayImage.Replace("$$$", "&");
			string[] Parameters = DisplayImage.Split("&");
			string Parameter = "";
			string[] ParsedParameter = null;
			for (int i = 0; i <= Parameters.Length - 1; i++) {
				Parameter = Parameters[i];
				ParsedParameter = Parameter.Split("=");
				if (ParsedParameter.Length == 2) {
					switch (ParsedParameter[0]) {
						case "width":
							nWidth = ParsedParameter[1];
							break;
						case "height":
							nHeight = ParsedParameter[1];
							break;
						case "familyname":
							sFamilyName = SetFamilyName(ParsedParameter[1].Replace("_", " "));
							break;
					}
				}
			}
			if (nWidth <= 0) {
				throw new ArgumentOutOfRangeException("width", nWidth, "Argument out of range, must be greater than zero.");
			}
			if (nHeight <= 0) {
				throw new ArgumentOutOfRangeException("height", nHeight, "Argument out of range, must be greater than zero.");
			}
			sText = GenerateRandomCode(HttpSession);
			Bitmap Bitmap = new Bitmap(nWidth, nHeight, PixelFormat.Format32bppArgb);
			Graphics Graphics = Graphics.FromImage(Bitmap);
			Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle Rect = new Rectangle(0, 0, nWidth, nHeight);
			HatchBrush HatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
			Graphics.FillRectangle(HatchBrush, Rect);

			SizeF Size = default(SizeF);
			float FontSize = Rect.Height + 1;
			Font Font = null;
			do {
				FontSize -= 1;
				Font = new Font(sFamilyName, FontSize, FontStyle.Bold);
				Size = Graphics.MeasureString(sText, Font);
			} while (Size.Width > Rect.Width);

			StringFormat Format = new StringFormat();
			Format.Alignment = StringAlignment.Center;
			Format.LineAlignment = StringAlignment.Center;

			GraphicsPath Path = new GraphicsPath();
			Path.AddString(sText, Font.FontFamily, Convert.ToInt32(Font.Style), Font.Size, Rect, Format);
			float V = 4f;
			PointF[] Points = {
				new PointF(oRandom.Next(Rect.Width) / V, oRandom.Next(Rect.Height) / V),
				new PointF(Rect.Width - oRandom.Next(Rect.Width) / V, oRandom.Next(Rect.Height) / V),
				new PointF(oRandom.Next(Rect.Width) / V, Rect.Height - oRandom.Next(Rect.Height) / V),
				new PointF(Rect.Width - oRandom.Next(Rect.Width) / V, Rect.Height - oRandom.Next(Rect.Height) / V)
			};
			Matrix Matrix = new Matrix();
			Matrix.Translate(0f, 0f);
			Path.Warp(Points, Rect, Matrix, WarpMode.Perspective, 0f);

			HatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
			Graphics.FillPath(HatchBrush, Path);

			int M = Math.Max(Rect.Width, Rect.Height);
			for (int i = 0; i <= Convert.ToInt32(Math.Truncate(Rect.Width * Rect.Height / 30f)) - 1; i++) {
				int x = oRandom.Next(Rect.Width);
				int y = oRandom.Next(Rect.Height);
				int w = oRandom.Next(M / 50);
				int h = oRandom.Next(M / 50);
				Graphics.FillEllipse(HatchBrush, x, y, w, h);
			}

			Font.Dispose();
			HatchBrush.Dispose();
			Graphics.Dispose();
			HttpResponse.Clear();
			HttpResponse.ContentType = "image/jpeg";
			Bitmap.Save(HttpResponse.OutputStream, ImageFormat.Jpeg);
		}
		public static string GetImageUrl(string ResourceName, int Width = -1, int Height = -1, string Extension = "", string Namespace = "Ophelia")
		{
			string Url = string.Format("{0}EFiles/{1}/{2}.{3}", System.Web.UI.Current.Client.ApplicationBase, Namespace, ResourceName, (string.IsNullOrEmpty(Extension) ? "png" : Extension));
			if (Height > -1 || Width > -1)
				return string.Format("{0}?width={1}&height={2}", Url, Width, Height);
			return Url;
		}
		public static void DrawPieChart(System.Web.UI.Page Form)
		{
			string sColors = "";
			string sData = "";
			Color oBgColor = default(Color);
			int i3dDepth = 0;
			int iSliceWidth = 0;
			int iSliceHeight = 0;
			int x = 0;
			int y = 0;
			int iImageWidth = 0;
			int iImageHeight = 0;
			string DisplayImage = Form.Request["DisplayChartImage"];
			DisplayImage = DisplayImage.Replace(",,", "=");
			DisplayImage = DisplayImage.Replace("$$$", "&");
			string[] Parameters = DisplayImage.Split("&");
			string Parameter = "";
			string[] ParsedParameter = null;
			for (int i = 0; i <= Parameters.Length - 1; i++) {
				Parameter = Parameters[i];
				ParsedParameter = Parameter.Split("=");
				if (ParsedParameter.Length == 2) {
					switch (ParsedParameter[0]) {
						case "colors":
							sColors = ParsedParameter[1];
							break;
						case "data":
							sData = ParsedParameter[1];
							break;
						case "bgcolor":
							oBgColor = GetColor(ParsedParameter[1]);
							break;
						case "width":
							iSliceWidth = ParsedParameter[1];
							break;
						case "height":
							iSliceHeight = ParsedParameter[1];
							break;
						case "depth":
							i3dDepth = ParsedParameter[1];
							break;
					}
				}
			}

			char[] sDelimiter = ",".ToCharArray();
			string[] oColors = sColors.Split(sDelimiter);
			string[] oRawData = sData.Split(sDelimiter);
			iImageWidth = iSliceWidth + 4;
			iImageHeight = iSliceHeight + i3dDepth + 2;
			Bitmap oBitmap = new Bitmap(iImageWidth, iImageHeight, PixelFormat.Format24bppRgb);

			Graphics oGraphics = Graphics.FromImage(oBitmap);
			oGraphics.SmoothingMode = SmoothingMode.AntiAlias;
			oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			oGraphics.CompositingMode = CompositingMode.SourceOver;
			oGraphics.CompositingQuality = CompositingQuality.HighQuality;
			oGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			ColorPalette Pal = oBitmap.Palette;
			SolidBrush Solid = new SolidBrush(null);
			Solid.Color = oBgColor;
			oGraphics.FillRectangle(Solid, 0, 0, iImageWidth, iImageHeight);

			Draw3DPieChart(ref oGraphics, oRawData, oColors, x, y, i3dDepth, iSliceWidth, iSliceHeight);

			Form.Response.ContentType = "image/bmp";

			System.IO.MemoryStream Stream = new System.IO.MemoryStream();
			oBitmap.Save(Stream, ImageFormat.Bmp);

			oGraphics.Dispose();
			Solid.Dispose();
			oBitmap.Dispose();
			Form.Response.BinaryWrite(Stream.ToArray());
		}
		public static void ResizeImage(System.Web.UI.Page Form)
		{
			ResizeImage(Form.Request, Form.Response);
		}
		public static void ResizeImage(HttpRequest HttpRequest, HttpResponse HttpResponse)
		{
			string Url = "";

			int MaxHeight = 180;
			int MaxWidth = 180;
			int Width = 0;
			int Height = 0;

			string ScaleFactor = "Width";
			byte Type = 1;
			//1 jpeg, 2 png 

			string DisplayImage = HttpRequest["ResizeImage"];
			DisplayImage = DisplayImage.Replace(",,", "=");
			DisplayImage = DisplayImage.Replace("$$$", "&");
			string[] Parameters = DisplayImage.Split("&");
			string Parameter = "";
			string[] ParsedParameter = null;
			for (int i = 0; i <= Parameters.Length - 1; i++) {
				Parameter = Parameters[i];
				ParsedParameter = Parameter.Split("=");
				if (ParsedParameter.Length == 2) {
					switch (ParsedParameter[0].ToLower()) {
						case "width":
							int.TryParse(ParsedParameter[1], out MaxWidth);
							break;
						case "height":
							int.TryParse(ParsedParameter[1], out MaxHeight);
							break;
						case "url":
							Url = ParsedParameter[1];
							break;
						case "scalefactor":
							ScaleFactor = ParsedParameter[1].ToLower();
							break;
						case "type":
							int.TryParse(ParsedParameter[1].ToLower(), out Type);
							if (Type > 2 || Type < 0)
								Type = 1;
							break;
					}
				}
			}

			if (!string.IsNullOrEmpty(Url)) {
				System.Drawing.Image CurrentImage = null;
				try {
					HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);
					HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
					Stream ResponseStream = Response.GetResponseStream();
					CurrentImage = System.Drawing.Image.FromStream(ResponseStream);
				} catch {
					return;
				}

				ResizeImage(CurrentImage, MaxWidth, MaxHeight, ScaleFactor, HttpResponse);
			}
		}
		public static void ResizeImage(System.Drawing.Image CurrentImage, int Width, int Height, string ScaleFactor, HttpResponse HttpResponse)
		{
			double ImageHeight = Convert.ToInt32(CurrentImage.Height);
			double ImageWidth = Convert.ToInt32(CurrentImage.Width);
			int Jc = 85;
			if (ImageWidth > Width || ImageHeight > Height) {
				double DeltaWidth = ImageWidth - Width;
				double DeltaHeight = ImageHeight - Height;
				if (Height == 0) {
					DeltaHeight = 0;
				}
				double Scale = 0;

				if (ScaleFactor == "height") {
					if (DeltaHeight > DeltaWidth) {
						Scale = Height / ImageHeight;
					} else {
						Scale = Width / ImageWidth;
					}
				} else {
					if (DeltaHeight < DeltaWidth) {
						Scale = Height / ImageHeight;
					} else {
						Scale = Width / ImageWidth;
					}
				}

				Width = Convert.ToInt32(ImageWidth * Scale);
				Height = Convert.ToInt32(ImageHeight * Scale);
			} else {
				Width = Convert.ToInt32(ImageWidth);
				Height = Convert.ToInt32(ImageHeight);
			}

			//If False Then
			//    If Width < 200 Then
			//        Width = 200
			//    End If
			//    If Height < 200 Then
			//        Height = 200
			//    End If
			//    'Flag at. 
			//End If


			Bitmap Thumb = new Bitmap(Width, Height, PixelFormat.Format32bppRgb);
			Graphics Graphics = Graphics.FromImage(Thumb);
			Graphics.Clear(Color.FromArgb(0, 255, 255, 255));
			Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
			Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			System.Drawing.PointF PointF = new PointF(0, 0);
			//PointF modifiye et. 
			//Graphics.FillRectangle(Brushes.White,New Rectangle(Width,Height)
			Graphics.DrawImage(CurrentImage, PointF.X, PointF.Y, Width, Height);
			HttpResponse.ContentType = "image/png";
			SavePNGWithCompressionSetting(HttpResponse, Thumb, Jc);
			Thumb.Dispose();
			CurrentImage.Dispose();
		}
		private static string GenerateRandomCode(System.Web.SessionState.HttpSessionState HttpSession)
		{
			HttpSession["CaptchaImageCode"] = GenerateRandomCaptchaCode();
			return HttpSession["CaptchaImageCode"];
		}
		private static string GenerateRandomCaptchaCode()
		{
			Random oRandom = new Random();
			string s = "";
			for (int i = 0; i <= 5; i++) {
				s = String.Concat(s, oRandom.Next(10).ToString());
			}
			return s;
		}
		private static void Draw3DPieChart(ref Graphics oGraphics, string[] oRawData, string[] oColors, int x, int y, int i3dDepth, int iSliceWidth, int iSliceHeight)
		{
			int iStartAngle = 0;
			SolidBrush oBrush = new SolidBrush(null);
			oBrush.Color = Color.Red;
			oGraphics.SmoothingMode = SmoothingMode.HighQuality;
			decimal iTotal = GetTotal(oRawData);
			string sColor = "";
			for (int i = 0; i <= oRawData.Length - 1; i++) {
				if (Information.IsNumeric(oRawData[i])) {
					decimal iPercent = Convert.ToDecimal(oRawData[i]);
					decimal iSweepAngle = Convert.ToDecimal((iPercent / iTotal) * 360);

					if (oColors.GetUpperBound(0) >= i) {
						sColor = oColors[i];
					} else {
						sColor = Color.WhiteSmoke.Name;
					}
					oBrush.Color = GetColor(sColor);

					int iRealStartAngle = (iStartAngle + 180) % 360;

					//3D Effect 
					if (iStartAngle + iSweepAngle > 180) {
						for (int j = 0; j <= i3dDepth - 1; j++) {
							oGraphics.FillPie(new HatchBrush(HatchStyle.Percent50, oBrush.Color), x, y + j, iSliceWidth, iSliceHeight, iRealStartAngle, iSweepAngle);
						}
					}

					oGraphics.FillPie(oBrush, x, y, iSliceWidth, iSliceHeight, iRealStartAngle, iSweepAngle);

					iStartAngle += iSweepAngle;
				}
			}
			oBrush.Dispose();
		}
		private static decimal GetTotal(string[] oRawData)
		{
			decimal iRet = 0;
			for (int i = 0; i <= oRawData.Length - 1; i++) {
				if (Information.IsNumeric(oRawData[i])) {
					decimal iPercent = Convert.ToDecimal(oRawData[i]);
					iRet += iPercent;
				}
			}
			return iRet;
		}
		private static System.Drawing.Color GetColor(string sColor)
		{
			sColor = sColor.Replace("#", "");

			Color oColor = Color.FromName(sColor);
			bool bColorEmpty = oColor.R == 0 && oColor.G == 0 && oColor.B == 0;
			if (!bColorEmpty) {
				return oColor;
			}

			if (sColor.Length != 6) {
				//On Error Return White 
				return Color.White;
			}

			string sRed = sColor.Substring(0, 2);
			string sGreen = sColor.Substring(2, 2);
			string sBlue = sColor.Substring(4, 2);

			oColor = System.Drawing.Color.FromArgb(HexToInt(sRed), HexToInt(sGreen), HexToInt(sBlue));
			return oColor;
		}
		private static int HexToInt(string hexString)
		{
			return int.Parse(hexString, System.Globalization.NumberStyles.HexNumber, null);
		}
		private static string SetFamilyName(string FamilyName)
		{
			try {
				Font Font = new Font(FamilyName, 12f);
				return FamilyName;
				Font.Dispose();
			} catch (Exception ex) {
				return System.Drawing.FontFamily.GenericSerif.Name;
			}
		}
		private static void SaveJPGWithCompressionSetting(HttpResponse Response, System.Drawing.Image Image, int Cl)
		{
			EncoderParameters EncoderParameters = new EncoderParameters();
			EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Cl);
			ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
			Image.Save(Response.OutputStream, ici, EncoderParameters);
			Image.Dispose();
		}
		private static void SavePNGWithCompressionSetting(HttpResponse Response, System.Drawing.Image Image, int Cl)
		{
			EncoderParameters EncoderParameters = new EncoderParameters();
			EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Cl);
			ImageCodecInfo ici = GetEncoderInfo("image/png");
			Image.Save(Response.OutputStream, ici, EncoderParameters);
			Image.Dispose();
		}
		private static ImageCodecInfo GetEncoderInfo(string MimeType)
		{
			ImageCodecInfo[] Encoders = ImageCodecInfo.GetImageEncoders();
			for (int i = 0; i <= Encoders.Length - 1; i++) {
				if (Encoders[i].MimeType == MimeType) {
					return Encoders[i];
				}
			}
			return null;
		}
	}
}

