using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Data.Exporter.Controls;
using iTextSharp.text.pdf;

namespace Ophelia.Data.Exporter
{
    public class PDFExporter : IExporter
    {
        public byte[] Export(List<Grid> grids)
        {
            var grid = grids.FirstOrDefault();
            using (var ms = new System.IO.MemoryStream())
            {
                var bffont = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                var fontNormal = new iTextSharp.text.Font(bffont, 12, iTextSharp.text.Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 0));

                var doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 10f);
                doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                
                var writer = PdfWriter.GetInstance(doc, ms);

                doc.Open();
                doc.NewPage();

                var table = new PdfPTable(grid.Columns.Count);
                table.WidthPercentage = 100;
                //table.SetTotalWidth(new float[] { 25, iTextSharp.text.PageSize.A4.Rotate().Width - 25 });

                foreach (var item in grid.Columns)
                {
                    table.AddCell(new iTextSharp.text.Phrase(item.Text, fontNormal));
                }
                foreach (var item in grid.Rows)
                {
                    for (int i = 0; i < grid.Columns.Count; i++)
                    {
                        table.AddCell(new iTextSharp.text.Phrase(item.Cells[i].Value.ToString(), fontNormal));
                    }
                }
                doc.Add(table);
                doc.Close();
                return ms.ToArray();
            }
        }

        public byte[] Export(Grid grid)
        {
            var grids = new List<Grid>();
            grids.Add(grid);
            return this.Export(grids);
        }
    }
}
