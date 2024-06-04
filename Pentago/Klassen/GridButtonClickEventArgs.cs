using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pentago
{
    public class GridButtonClickEventArgs : EventArgs
    {
        public int Row { get; }
        public int Column { get; }
        public List<Button> Buttons { get; }

        public GridButtonClickEventArgs(int row, int column,List<Button> button)
        {
            Row = row;
            Column = column;
            Buttons = button;
        }
    }
}
