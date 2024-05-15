namespace tEST
{
    public class GameState
    {
        public int[,] GameGridTL {  get; set; }
        public int[,] GameGridTR { get; set; }
        public int[,] GameGridBL { get; set; }
        public int[,] GameGridBR { get; set; }
        public Player CurrentPayer { get; set; }
        public bool GameOver {  get; set; }

        public GameState() 
        {
            this.GameGridTL = new int[3,3];
            this.GameGridTR = new int[3,3];
            this.GameGridBL = new int[3,3];
            this.GameGridBR = new int[3,3];
            this.GameOver = false;
            this.CurrentPayer = Player.Blue;
        }

        private bool CanMakeMove(int x, int y)
        {
            return !GameOver && GameGridBR[x, y] == 0;
        }

        private void SwitchPlayer()
        {
            if (CurrentPayer == Player.Blue)
            {
                CurrentPayer = Player.Red;
            }
            else
            {
                CurrentPayer = Player.Blue;
            }
        }

        private bool Marked((int, int)[] grid)
        {
            foreach ((int row ,int col) in grid )
            {
                if (GameGridTL[row, col] != 1)
                {
                    return false;
                }
                if (GameGridTR[row, col] != 1)
                {
                    return false ;
                } 
                if (GameGridBL[row,col] != 1)
                {
                    return false;
                }
                if (GameGridBR[row, col] != 1)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckGrid(int row , int col )
        {
            

            






            
        }
    }
}
