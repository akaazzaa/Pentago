using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentago
{
    public class ComputerMark
    {
        public int fromRow {  get; set; }
        public int toRow { get; set; }

        public int fromCol { get; set; }
        public int toCol { get; set; }
        public ComputerMark(int fromRow, int toRow, int fromCol, int toCol)
        {
            this.fromRow = fromRow;
            this.toRow = toRow;
            this.fromCol = fromCol;
            this.toCol = toCol;
        }
    }
}
