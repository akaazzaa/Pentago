using System;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices.WindowsRuntime;

namespace tEST
{
    public class GameState
    {
        public Player[,] GameGrid { get; set; }
        public int WinCondition = 0;

        public Player currentPlayer;

        public GameState() 
        {
            currentPlayer = Player.Blue;
            GameGrid = new Player[6, 6]
            {
                {Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue },
                {Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue },
                {Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue  },
                {Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue },
                {Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue  },
                {Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue  }

            };
        }

        //public void Ausgabe()
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        for(int j = 0; j < 3; j++)
        //        {
        //            Console.Write(arrayTopLeft[i,j].ToString());
        //            Console.Write(' ');
        //        }
        //        Console.Write(' ');
        //        Console.Write(" ");
        //        for (int j = 0; j < 3; j++)
        //        {
        //            Console.Write(arrayTopRight[i, j].ToString());
        //            Console.Write(' ');
        //        }

        //        Console.WriteLine(" ");
        //    }
        //}
        public void Ausgaben()
        {
        }

        private bool IsMarked(int r, int c ,Player player)
        {
    
                if (GameGrid[r, c] != player)
                {
                    return false;
                }
                
            
            return true;

        }

        

       

        internal bool IsMacked((int, int)[] values ,Player player) 
        {
            foreach((int r,int c) in values)
            {
                if (GameGrid[r, c] != player)
                {

                    return false;
                }
            }
            return true;

        }

        public bool CkeckWin(int c, int r)
        {
            (int, int)[] row = new[] { (r, c), (r + 1, c), (r + 2, c), (r + 3, c), (r + 4, c) };

            if (IsMacked(row, currentPlayer))
            {
                return true;
            }
            return false;
        }

        private void Gewonnen()
        {
            Console.WriteLine("Gewonnen");
        }
    }
}
