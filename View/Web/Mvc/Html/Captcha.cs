using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web;
using System.IO;
using Ophelia.Web.View.Mvc.Models;

namespace Ophelia.Web.View.Mvc.Html
{
    public class Captcha : IDisposable
    {
        private CaptchaModel mCaptchaModel;
        private string sCaptchaSessionKey;
        public string CaptchaSessionKey { get { return this.sCaptchaSessionKey; } }

        public CaptchaModel CaptchaModel
        {
            get {
                return this.mCaptchaModel;
            }
            set {
                this.mCaptchaModel = value;
            }
        }
        public Captcha(CaptchaModel Model)
        {
            this.CaptchaModel = Model;
        }
        public Captcha()
        {

            this.CaptchaModel = new CaptchaModel();
        }
        private void GenerateCaptchaValue()
        {
            this.sCaptchaSessionKey = this.GetRandomCode(10, true, false, false);
            HttpContext.Current.Session["Captcha_" + this.CaptchaSessionKey] = this.GetRandomCode(this.CaptchaModel.CaptchaValueLength, this.CaptchaModel.UseNumerics, this.CaptchaModel.UseLowerChars, this.CaptchaModel.UseUpperChars);
        }

        private string GetRandomCode(int Length, bool UseNumerics, bool UseLowerChars, bool UseUpperChars)
        {
            Random Rand = new Random(DateTime.Now.Millisecond);
            char[] CharacterSet = new char[62];
            string RandomValue = string.Empty;
            int CopiedCharacterLength = 0;
            if (UseNumerics)
            {
                "123456789".ToCharArray().CopyTo(CharacterSet, 0);
                CopiedCharacterLength = 9;
            }
            if (UseLowerChars)
            {
                "abcdefghjkmnpqrstuvwxyz".ToCharArray().CopyTo(CharacterSet, CopiedCharacterLength);
                CopiedCharacterLength += 23;
            }
            if (UseUpperChars)
            {
                "ABCDEFGHJKLMNPQRSTUVWXYZ".ToCharArray().CopyTo(CharacterSet, CopiedCharacterLength);
                CopiedCharacterLength += 24;
            }

            for (int i = 1; i <= Length; i++)
                RandomValue += CharacterSet[Rand.Next(0, CopiedCharacterLength - 1)].ToString();
            return RandomValue;
        }

        public string GetCaptchaImageString()
        {
            this.GenerateCaptchaValue();
            Bitmap CaptchaImage = new Bitmap(this.CaptchaModel.Width, this.CaptchaModel.Height);
            Graphics Graphic = Graphics.FromImage(CaptchaImage);
            Graphic.SmoothingMode = SmoothingMode.HighQuality;
            Rectangle CaptchaContainer = new Rectangle(0, 0, this.CaptchaModel.Width, this.CaptchaModel.Height);
            HatchBrush CaptchaBackground = new HatchBrush(HatchStyle.SmallConfetti, Color.LightSlateGray, Color.White);
            Graphic.FillRectangle(CaptchaBackground, CaptchaContainer);

            Font Font = new Font(this.CaptchaModel.FontFamily, this.CaptchaModel.FontSize, FontStyle.Bold);
            SizeF Size = Graphic.MeasureString(HttpContext.Current.Session["Captcha_" + this.CaptchaSessionKey].ToString(), Font);
            StringFormat Format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
            GraphicsPath Path = new GraphicsPath();
            Path.AddString(HttpContext.Current.Session["Captcha_" + this.CaptchaSessionKey].ToString(), Font.FontFamily, (int)Font.Style, this.CaptchaModel.FontSize, CaptchaContainer, Format);
            
            Font.Dispose();
            Font = null;

            float v = 3.5F;
            Random Rand = new Random(DateTime.Now.Millisecond);
            int RandomWidth = Rand.Next(CaptchaContainer.Width);
            int RandomHeight = Rand.Next(CaptchaContainer.Height);
            PointF[] Points =
			{
				new PointF(RandomWidth / v, RandomHeight / v),
				new PointF(CaptchaContainer.Width - RandomWidth / v, RandomHeight / v),
				new PointF(RandomWidth / v, CaptchaContainer.Height - RandomHeight / v),
				new PointF(CaptchaContainer.Width - RandomWidth / v, CaptchaContainer.Height - RandomHeight / v)
			};

            Matrix Matrix = new Matrix();
            Matrix.Translate(0F, 0F);
            Path.Warp(Points, CaptchaContainer, Matrix, WarpMode.Perspective, 0F);

            CaptchaBackground = new HatchBrush(HatchStyle.LargeConfetti, Color.FromArgb(130, 130, 130), Color.FromArgb(130, 130, 130));
            Graphic.FillPath(CaptchaBackground, Path);

            int m = Math.Max(CaptchaContainer.Width, CaptchaContainer.Height);
            for (int i = 0; i < (int)(CaptchaContainer.Width * CaptchaContainer.Height / 30F); i++)
            {
                int x = Rand.Next(CaptchaContainer.Width);
                int y = Rand.Next(CaptchaContainer.Height);
                int w = Rand.Next(m / 50);
                int h = Rand.Next(m / 50);
                Graphic.FillEllipse(CaptchaBackground, x, y, w, h);
            }
            CaptchaBackground.Dispose();
            CaptchaBackground = null;
            Graphic.Dispose();
            Graphic = null;
            
            string CaptchaImageString = string.Empty;
            using (MemoryStream MemoryStream = new MemoryStream())
            {
                CaptchaImage.Save(MemoryStream, System.Drawing.Imaging.ImageFormat.Png);
                CaptchaImageString = "data:image/png;base64," + Convert.ToBase64String(MemoryStream.ToArray());
            }
            return CaptchaImageString;
        }

        public bool CheckCaptcha(string CaptchaCode, string CaptchaSessionKey)
        {
            if (!string.IsNullOrEmpty(CaptchaCode) && HttpContext.Current.Session["Captcha_" + CaptchaSessionKey] != null)
            {
                if (HttpContext.Current.Session["Captcha_" + CaptchaSessionKey].ToString().Trim().Equals(CaptchaCode))
                    return true;
            }
            int ErrorCount = HttpContext.Current.Session["WrongEntryCount"] != null ? Int32.Parse(HttpContext.Current.Session["WrongEntryCount"].ToString()) : 0;
            HttpContext.Current.Session["WrongEntryCount"] = ErrorCount + 1;
            return false;
        }

        private bool IsWrongEntryCountExceed()
        {
            if (HttpContext.Current.Session["WrongEntryCount"] == null)
                HttpContext.Current.Session["WrongEntryCount"] = 0;
            else if (Int32.Parse(HttpContext.Current.Session["WrongEntryCount"].ToString()) >= this.CaptchaModel.ErrorCount)
                return true;
            return false;
        }

        public bool Enabled()
        {
            return this.CaptchaModel.AlwaysShow || this.IsWrongEntryCountExceed(); 
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void DeleteSessionKey(string KeyName)
        {
            if(HttpContext.Current.Session["Captcha_" + KeyName] != null)
                HttpContext.Current.Session["Captcha_" + KeyName] = null;
        }
        public void IncreaseCaptchaErrorCount()
        {
            int ErrorCount = HttpContext.Current.Session["WrongEntryCount"] != null ? Int32.Parse(HttpContext.Current.Session["WrongEntryCount"].ToString()) : 0;
            HttpContext.Current.Session["WrongEntryCount"] = ErrorCount + 1;
        }
        public void ClearErrorCount()
        {
            if (HttpContext.Current.Session["WrongEntryCount"] != null)
                HttpContext.Current.Session["WrongEntryCount"] = 0;
        }
    }
}
