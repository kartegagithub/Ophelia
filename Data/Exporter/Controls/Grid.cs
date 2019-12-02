using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Data.Exporter.Controls
{
    public class Grid : IDisposable
    {
        public string ID { get; set; }
        public string Text { get; set; }
        public List<Row> Rows { get; set; }
        public List<Column> Columns { get; set; }
        public Grid(string ID): this()
        {
            this.ID = ID;
            this.Text = ID;
        }
        public Grid()
        {
            this.Rows = new List<Row>();
            this.Columns = new List<Column>();
        }
        public Column AddColumn(string ID = "", string Text = "", bool IsNumeric = false)
        {
            return this.AddColumn(new Column(this) { ID = ID, Text = Text, IsNumeric = IsNumeric });
        }
        public Column AddColumn(Column column)
        {
            this.Columns.Add(column);
            return column;
        }
        public Row AddRow()
        {
            return this.AddRow(new Row(this));
        }
        public Row AddRow(Row row)
        {
            this.Rows.Add(row);
            return row;
        }
        public void Dispose()
        {
            this.Rows.Clear();
            this.Columns.Clear();
            this.Text = "";
        }
    }
}
