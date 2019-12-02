using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Validator
{
	public class NumericValidator : Validator
	{
		private bool bAllowInfitiyCharacter;
		public bool AllowInfitiyCharacter {
			get { return this.bAllowInfitiyCharacter; }
			set { this.bAllowInfitiyCharacter = value; }
		}
		public NumericValidator(ValidatorCollection Collection) : base(Collection)
		{
			this.ValidationType = eValidationType.Numeric;
		}
	}
}
