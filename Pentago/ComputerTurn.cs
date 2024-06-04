using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentago
{
    public class ComputerTurn
    {
        public (ComputerMark, ComputerRotation) move { get; set; }

        public ComputerTurn((ComputerMark, ComputerRotation) move)
        {
            this.move = move;
        }
    }
   
}
