using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentago
{
    public class Modifyer
    {
       public static int halfBoardSize = 3;
       public static int fullBoardSize = 6;

        public static Player[,] GetBoard(Player[,] topLeft, Player[,] topRight, Player[,] botLeft, Player[,] botRight)
        {
            Player[,] board = new Player[6, 6];

            for (int row = 0; row < topLeft.GetLength(0) + topRight.GetLength(0); row++)
            {
                for (int col = 0; col < botLeft.GetLength(0) + botRight.GetLength(0); col++)
                {
                    if (row < halfBoardSize && col < halfBoardSize)
                    {
                        board[row, col] = topLeft[row, col];
                    }
                    else if (row < halfBoardSize && col > halfBoardSize - 1 && col < fullBoardSize)
                    {
                        board[row, col] = topRight[row, col - halfBoardSize];
                    }
                    else if (row >= halfBoardSize && col < halfBoardSize)
                    {
                        board[row, col] = botLeft[row - halfBoardSize, col];
                    }
                    else if (row >= halfBoardSize && row < fullBoardSize && col >= halfBoardSize && col < fullBoardSize)
                    {
                        board[row, col] = botRight[row - halfBoardSize, col - halfBoardSize];
                    }
                }
            }
            return board;
        }
    }
}
