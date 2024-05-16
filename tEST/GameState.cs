using System;

namespace tEST
{
    public class GameState
    {
        public Player[,] arrayTopLeft { get; set; }
        public Player[,] arrayTopRight { get; set; }
        public Player[,] arrayBotLeft { get; set; }
        public Player[,] arrayBotRight { get; set; }

        public GameState() 
        { 
            arrayTopLeft = new Player[3,3];
            arrayTopRight = new Player[3,3];
            arrayBotLeft = new Player[3,3];
            arrayBotRight = new Player[3,3];
        }

        public void Ausgabe()
        {
            
        }

    }
}
