using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentago
{
    public class ComputerRotation
    {
        public Player[,] board {  get; set; }
        public Direction Direction {  get; set; }

        public ComputerRotation(Player[,] players,Direction direction) 
        {
            board = players;
            Direction = direction;
        }

    }
}
