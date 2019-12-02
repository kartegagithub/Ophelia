using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.DataGrid
{
	public class Footer
	{
		private DataGrid oDataGrid;
		private Style oStyle;
		private Structure.Structure oControl;
		public Style Style {
			get {
				if (this.oStyle == null) {
					this.oStyle = new Style();
					this.oStyle.HorizontalAlignment = SectionAlignment.Left;
					this.oStyle.Font.Color = "black";
				}
				return this.oStyle;
			}
		}
		public Structure.Structure Control {
			get {
				if (this.oControl == null) {
					if (this.DataGrid.IsMobileGrid) {
						this.oControl = new Structure.Structure("CB_" + this.DataGrid.ID + "_Footer", 1, 1, false, true, this.DataGrid.IsMobileGrid);
					} else {
						this.oControl = new Structure.Structure("CB_" + this.DataGrid.ID + "_Footer");
					}
					this.oControl.Style.Dock = DockStyle.Fill;
					this.oControl.Style.Font.Color = "black";
				}
				return this.oControl;
			}
		}
		public Structure.Cells.Cell DefaultRegion {
			get { return this.Control(0, 0); }
		}
		public DataGrid DataGrid {
			get { return this.oDataGrid; }
		}
		public Footer(DataGrid DataGrid)
		{
			this.oDataGrid = DataGrid;
		}
	}
}
