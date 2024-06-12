using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentago.Klassen
{
    public class ComputerGegner
    {
        private Player[,] GameBoard {  get; set; }
        private Player[,] GameBoardCopy { get; set; }
        public Player CurrentPlayer { get; set; }

        public ComputerGegner()
        {
            GameBoard = new Player[6,6];
            GameBoardCopy = new Player[6,6];
            CurrentPlayer = Player.Blue;
        }
  
     
        public List<Tuple<int, int, Corner, Direction>> GetAllMoves()
        {
            List<Tuple<int, int, Corner, Direction>> moves = new List<Tuple<int, int, Corner, Direction>>();

            for (int r = 0; r < GameBoard.GetLength(0); r++)
            {
                for (int c = 0; c < GameBoard.GetLength(1); c++)
                {
                    if (GameBoard[r, c] == Player.None)
                    {
                        for (int quadrant = 0; quadrant < 4; quadrant++)
                        {
                            moves.Add(Tuple.Create(r, c, (Corner)quadrant, Direction.Right));
                            moves.Add(Tuple.Create(r, c, (Corner)quadrant, Direction.Left));
                        }
                    }
                }
            }
            return moves;
        }
   
        
    }
}
