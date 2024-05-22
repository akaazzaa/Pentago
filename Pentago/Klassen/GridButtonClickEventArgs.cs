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
        public Button Button { get; }

        public GridButtonClickEventArgs(int row, int column,Button button)
        {
            Row = row;
            Column = column;
            Button = button;
        }
    }
}
