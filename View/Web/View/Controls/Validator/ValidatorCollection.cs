using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.ServerSide.ScriptManager;
namespace Ophelia.Web.View.Controls.Validator
{
	public class ValidatorCollection : Ophelia.Application.Base.CollectionBase
	{
		private DataControl oControl;
		private string sID = "";
		private Script oValidationScript;
		public bool ShowErrorsInToolTip { get; set; }
		public Script ValidationScript {
			get { return this.oValidationScript; }
			set { this.oValidationScript = value; }
		}
		public string ID {
			get { return this.sID; }
		}
		public DataControl Control {
			get { return this.oControl; }
		}
		public FileValidator AddFileValidator(string FileExtension, string Message = "")
		{
			if (this.Control.GetType.ToString == "Ophelia.Web.View.Controls.FileBox") {
				FileValidator Validator = new FileValidator(this);
				Validator.ErrorMessage = Message;
				Validator.FileExtension = FileExtension;
				return this.Add(Validator);
			}
			return null;
		}
		public EmailValidator AddEmailValidator(string Message = "")
		{
			EmailValidator Validator = new EmailValidator(this);
			Validator.ValidationType = View.Web.Controls.Validator.Validator.eValidationType.Email;
			Validator.ErrorMessage = Message;
			return this.Add(Validator);
		}
		public EmailValidator AddEmailTextValidator(string Message = "")
		{
			EmailValidator Validator = new EmailValidator(this);
			Validator.ValidationType = View.Web.Controls.Validator.Validator.eValidationType.EmailText;
			Validator.ErrorMessage = Message;
			return this.Add(Validator);
		}
		public NumericValidator AddNumericValidator(string Message = "")
		{
			NumericValidator Validator = new NumericValidator(this);
			if (this.Control.GetType.Name.IndexOf("Infinity") > -1) {
				Validator.AllowInfitiyCharacter = true;
			}
			Validator.ErrorMessage = Message;
			return this.Add(Validator);
		}
		public TextValidator AddMaxLengthValidator(string Message = "")
		{
			TextValidator Validator = new TextValidator(this);
			Validator.ValidationType = View.Web.Controls.Validator.Validator.eValidationType.MaxLength;
			Validator.ErrorMessage = Message;
			return this.Add(Validator);
		}
		public NumericValidator AddNumericRangeValidator(string Message = "", int MinValue = int.MinValue, int MaxValue = int.MaxValue)
		{
			NumericValidator Validator = new NumericValidator(this);
			Validator.ValidationType = View.Web.Controls.Validator.Validator.eValidationType.NumericRange;
			Validator.ErrorMessage = Message;
			Validator.MinValue = MinValue;
			Validator.MaxValue = MaxValue;
			if (this.Control.GetType.Name.IndexOf("Infinity") > -1) {
				Validator.AllowInfitiyCharacter = true;
			}
			return this.Add(Validator);
		}
		public TextValidator AddTextValidator(string Message = "", int MinLength = 1, int MaxLength = -1)
		{
			TextValidator Validator = new TextValidator(this);
			Validator.ErrorMessage = Message;
			Validator.MinLength = MinLength;
			Validator.MaxLength = MaxLength;
			return this.Add(Validator);
		}
		public DateValidator AddDateValidator(string Message = "")
		{
			DateValidator Validator = new DateValidator(this);
			Validator.ErrorMessage = Message;
			return this.Add(Validator);
		}
		public Validator Add(Validator Validator)
		{
			this.List.Add(Validator);
			return Validator;
		}
		public Validator Add()
		{
			return this.Add(new Validator(this));
		}
		public string DrawValidatorString(bool ShowToolTip = false)
		{
			if (!this.Control.NeedToValidate)
				return "";

			string ID = this.Control.ID;
			string sReturnString = "";
			string Element = "document.getElementById('" + ID + "')";
			string ErrorElement = "document.getElementById('" + ID + "Error')";
			if (this.ValidationScript == null)
				return "";
			Validator Validator = default(Validator);
			Function Function = this.ValidationScript.Function(this.Control.ID + "_Validate");
			if (Function != null)
				return ID + "_Validate()";
			Function = this.ValidationScript.AddFunction(this.Control.ID + "_Validate");

			InputDataControl DataControl = null;
			if (this.Control.GetType.IsSubclassOf(Type.GetType("Ophelia.Web.View.Controls.InputDataControl"))) {
				DataControl = this.Control;
			}
			string OnBlurEvent = "";
			string OnChangeEvent = "";
			for (int n = 0; n <= this.List.Count - 1; n++) {
				Validator = this.List(n);
				Function.AppendLine("if(!");
				switch (Validator.ValidationType) {
					case View.Web.Controls.Validator.Validator.eValidationType.BlankEmail:
					case View.Web.Controls.Validator.Validator.eValidationType.Email:
						if (Validator.IsMust) {
							OnBlurEvent = this.ValidationScript.IsEmail(Element, 1, Validator.ErrorMessage);
						} else {
							OnChangeEvent = this.ValidationScript.IsEmail(Element, 1, Validator.ErrorMessage);
						}
						break;
					case View.Web.Controls.Validator.Validator.eValidationType.BlankNumeric:
					case View.Web.Controls.Validator.Validator.eValidationType.Numeric:
						NumericValidator NumericValidator = Validator;
						if (Validator.IsMust) {
							OnBlurEvent = this.ValidationScript.IsNumeric(Element, 1, Validator.ErrorMessage, NumericValidator.AllowInfitiyCharacter);
						} else {
							OnChangeEvent = this.ValidationScript.IsNumeric(Element, 1, Validator.ErrorMessage, NumericValidator.AllowInfitiyCharacter);
						}
						break;
					case View.Web.Controls.Validator.Validator.eValidationType.NumericRange:
						NumericValidator NumericValidator = Validator;
						if (Validator.IsMust) {
							OnBlurEvent = this.ValidationScript.IsOnNumericRange(Element, 1, Validator.ErrorMessage, Validator.MinValue, Validator.MaxValue, NumericValidator.AllowInfitiyCharacter);
						} else {
							OnChangeEvent = this.ValidationScript.IsOnNumericRange(Element, 1, Validator.ErrorMessage, Validator.MinValue, Validator.MaxValue, NumericValidator.AllowInfitiyCharacter);
						}
						break;
					case View.Web.Controls.Validator.Validator.eValidationType.NonBlank:
						Function.AppendLine(this.ValidationScript.IsText(Element, 1, Validator.ErrorMessage, Validator.MinLength, ShowToolTip));
						break;
					//[Function].AppendLine(Me.ValidationScript.IsText(Element, 1, Validator.ErrorMessage, Validator.MinLength))
					case View.Web.Controls.Validator.Validator.eValidationType.File:
						Function.AppendLine(this.ValidationScript.IsAcceptedFile("document.getElementById('" + ID + "_fake')", 1, Validator.ErrorMessage, Validator.FileExtension));
						break;
					case View.Web.Controls.Validator.Validator.eValidationType.MaxLength:
						Function.AppendLine(this.ValidationScript.CheckMaxLength(Element, Validator.ErrorMessage));
						this.Control.OnKeyDownEvent = "CheckMaxLength(document.getElementById('" + this.ID + "'),'' ,event);" + this.Control.OnKeyDownEvent;
						break;
					case View.Web.Controls.Validator.Validator.eValidationType.BlankEmail:
					case View.Web.Controls.Validator.Validator.eValidationType.EmailText:
						Function.AppendLine(this.ValidationScript.IsEmailText(Element, 1, Validator.ErrorMessage, ErrorElement));
						break;
					case View.Web.Controls.Validator.Validator.eValidationType.Date:
						OnBlurEvent = this.ValidationScript.IsDate(Element, Validator.ErrorMessage);
						break;
				}
				if (DataControl != null) {
					if (DataControl.OnBlurEvent != null && !string.IsNullOrEmpty(DataControl.OnBlurEvent) && !DataControl.OnBlurEvent.EndsWith(";")) {
						DataControl.OnBlurEvent += ";";
					}
					if (DataControl.OnBlurEvent == null || !DataControl.OnBlurEvent.Contains(OnBlurEvent)) {
						DataControl.OnBlurEvent += OnBlurEvent;
						if (!DataControl.OnBlurEvent.EndsWith(";")) {
							DataControl.OnBlurEvent += ";";
						}
					}
				}
				if (DataControl != null) {
					if (DataControl.OnChangeEvent != null && !string.IsNullOrEmpty(DataControl.OnChangeEvent) && !DataControl.OnChangeEvent.EndsWith(";")) {
						DataControl.OnChangeEvent += ";";
					}
					if (DataControl.OnChangeEvent == null || !DataControl.OnChangeEvent.Contains(OnChangeEvent)) {
						DataControl.OnChangeEvent += OnChangeEvent;
						if (!DataControl.OnChangeEvent.EndsWith(";")) {
							DataControl.OnChangeEvent += ";";
						}
					}
				}
				Function.AppendLine(OnBlurEvent);
				Function.AppendLine(OnChangeEvent);
				Function.AppendLine("){");
				Function.AppendLine("return false;");
				Function.AppendLine("}");
			}
			return ID + "_Validate()";
		}
		public ValidatorCollection(DataControl Control)
		{
			this.oControl = Control;
			VBMath.Randomize();
			this.sID = VBMath.Rnd();
			this.sID = Strings.Left(this.sID.Replace(".", "").Replace(",", ""), 5);
		}
	}
}
