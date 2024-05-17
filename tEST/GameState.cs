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
                {Player.Red,Player.Red,Player.Blue,Player.Blue,Player.Blue,Player.Blue },
                {Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue },
                {Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue  },
                {Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue },
                {Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue  },
                {Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue,Player.Blue  }

            };
        }

       
        

        

       
    }
}
