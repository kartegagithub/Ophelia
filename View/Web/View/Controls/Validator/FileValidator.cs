using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Validator
{
	public class FileValidator : Validator
	{
		public FileValidator(ValidatorCollection Collection) : base(Collection)
		{
			this.ValidationType = eValidationType.File;
		}
	}
}
