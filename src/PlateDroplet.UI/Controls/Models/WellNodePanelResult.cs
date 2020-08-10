using System;

namespace PlateDroplet.UI.Controls.Models
{
    public class WellNodePanelResult
    {
        private readonly WellNodePanel[,] _wells;

        public WellNodePanelResult()
        {
            
        }

        public WellNodePanelResult(WellNodePanel[,] wells) => _wells = wells;

        public WellNodePanel this[int row, int col] => _wells[row, col];

        public bool WellNotIsNull() => _wells != null;

        public int GetRows() => WellNotIsNull() ? _wells.GetLength(0) : throw new ArgumentNullException("Wells cannot be null");

        public int GetCols() => WellNotIsNull() ? _wells.GetLength(1) : throw new ArgumentNullException("Wells cannot be null");
    }
}