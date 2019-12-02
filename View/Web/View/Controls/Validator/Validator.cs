using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Validator
{
	public class Validator
	{
		private ValidatorCollection oCollection;
		private eValidationType peValidationType = eValidationType.None;
		private string sErrorMessage = "";
		private string sFileExtension = "";
		private int nMaxValue = int.MaxValue;
		private int nMinValue = int.MinValue;
		private int nMaxLength = -1;
		private int nMinLength = 1;
		private bool bIsMust;
		public string FileExtension {
			get { return this.sFileExtension; }
			set { this.sFileExtension = value; }
		}
		public bool IsMust {
			get { return this.bIsMust; }
			set { this.bIsMust = value; }
		}
		public int MinLength {
			get { return this.nMinLength; }
			set { this.nMinLength = value; }
		}
		public int MaxLength {
			get { return this.nMaxLength; }
			set { this.nMaxLength = value; }
		}
		public int MaxValue {
			get { return this.nMaxValue; }
			set { this.nMaxValue = value; }
		}
		public int MinValue {
			get { return this.nMinValue; }
			set { this.nMinValue = value; }
		}
		public string ErrorMessage {
			get { return this.sErrorMessage; }
			set { this.sErrorMessage = value; }
		}
		public eValidationType ValidationType {
			get { return this.peValidationType; }
			set {
				this.peValidationType = value;
				if (this.peValidationType == eValidationType.Numeric || this.peValidationType == eValidationType.NumericRange) {
					this.IsMust = true;
				}
			}
		}
		public ValidatorCollection Collection {
			get { return this.oCollection; }
		}
		public bool Validate()
		{
			if (!Information.IsNumeric(this.Collection.Control.Value) && string.IsNullOrEmpty(this.Collection.Control.Value)) {
				switch (this.ValidationType) {
					case eValidationType.None:
					case eValidationType.BlankDate:
					case eValidationType.BlankEmail:
					case eValidationType.BlankNumeric:
						return true;
					default:
						return false;
				}
			} else {
				switch (this.ValidationType) {
					case eValidationType.BlankDate:
					case eValidationType.Date:
						return Information.IsDate(this.Collection.Control.Value);
					case eValidationType.BlankEmail:
					case eValidationType.Email:
						return this.Collection.Control.Value.IndexOf("@") > -1 && this.Collection.Control.Value.IndexOf(".") != this.Collection.Control.Value.LastIndexOf(".");
					case eValidationType.BlankNumeric:
					case eValidationType.Numeric:
						return Information.IsNumeric(this.Collection.Control.Value);
					case eValidationType.NonBlank:
						return true;
					case eValidationType.File:
						if (this.Collection.Control.GetType.ToString == "Ophelia.Web.View.Controls.FileBox") {
							return this.Collection.Control.Value.ToString.Substring(this.Collection.Control.Value.ToString.LastIndexOf(".")).ToLower == FileExtension.ToLower();
						} else {
							return true;
						}
						break;
				}
			}
			return false;
		}
		public Validator(ValidatorCollection Collection)
		{
			this.oCollection = Collection;
		}
		public enum eValidationType : int
		{
			None = 0,
			Email = 1,
			Numeric = 2,
			NonBlank = 3,
			Date = 4,
			BlankNumeric = 5,
			BlankEmail = 6,
			BlankDate = 7,
			NumericRange = 8,
			File = 9,
			MaxLength = 10,
			EmailText = 11,
			InfinityNumeric = 12
		}
	}
}
