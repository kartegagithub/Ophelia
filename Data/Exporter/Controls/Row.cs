﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Data.Exporter.Controls
{
    public class Row : IDisposable
    {
        public Grid Grid { get; private set; }
        public List<Cell> Cells { get; set; }
        public Row(Grid grid)
        {
            this.Grid = grid;
            this.Cells = new List<Cell>();
        }
        public Cell AddCell(Column column, object value)
        {
            var cell = new Cell(this.Grid, column, this) { Value = value };
            this.Cells.Add(cell);
            return cell;
        }
        public void Dispose()
        {
            this.Cells.Clear();
            this.Grid = null;
        }
    }
}
