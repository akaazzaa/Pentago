using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Pentago.Klassen
{
    public class Game
    {
        public Player[,] TopLeft { get; set; }
        public Player[,] TopRight { get; set; }
        public Player[,] BotLeft { get; set; }
        public Player[,] BotRight { get; set; }
        public Player CurrentPlayer { get; set; }
        private List<Button> Buttons { get; set; }
        public int Suchtiefe { get; set; }
        public int WinCondition { get; set; }
        public int ArrayRowLenght { get; set; }
        public int HalfArrayRowLenght { get; set; }
        public int ArrayColLenght { get; set; }
        public int HalfArrayColLenght { get; set; }
        public int TurnsPassed { get; set; }
        public bool GameOver { get; set; }
        public bool Turned { get; set; }
        public WinInfo WinInfo { get; set; }
        public GameResult GameResult { get; set; }
        public (Player[,], Player[,], Player[,], Player[,]) bestmove;
        public (int, int, bool) bestrotation;
        public bool isSinglePlayer { get; set; }

        public event Action<GameResult> GameEnded;
        public event Action GameRestarted;
        public event Action MoveMade;
        public static event Action RotateButtonsLeft;
        public static event Action RotateButtonsRight;

        public Game()
        {

            CurrentPlayer = Player.Blue;
            WinCondition = 0;
            Turned = false;
            GameOver = false;
            TopLeft = new Player[3, 3];
            TopRight = new Player[3, 3];
            BotLeft = new Player[3, 3];
            BotRight = new Player[3, 3];
            ArrayRowLenght = TopLeft.GetLength(0) + TopRight.GetLength(0);
            HalfArrayRowLenght = TopLeft.GetLength(0);
            ArrayColLenght = ArrayRowLenght;
            HalfArrayColLenght = HalfArrayRowLenght;
            isSinglePlayer = false;
            Suchtiefe = 3;


        }
        private bool IsGridFull()
        {
            return TurnsPassed == 36;
        }
        private bool CanMove(GameGrid grid, int r, int c)
        {
            switch (grid.Name)
            {
                case ("GridTopLeft"):
                    return !GameOver && Turned == false && TopLeft[r, c] == Player.None;
                case ("GridTopRight"):
                    return !GameOver && Turned == false && TopRight[r, c] == Player.None;
                case ("GridBotLeft"):
                    return !GameOver && Turned == false && BotLeft[r, c] == Player.None;
                case ("GridBotRight"):
                    return !GameOver && Turned == false && BotRight[r, c] == Player.None;

            }
            return false;
        }
        public void Changbuttoncolor(int row, int col)
        {
            Button button = GetButtonbyTag(row, col);
            if (CurrentPlayer == Player.Blue)
            {
                button.Background = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                button.Background = new SolidColorBrush(Colors.Red);
            }
        }


        private Button GetButtonbyTag(int row, int col)
        {
            foreach (var button in Buttons)
            {
                var pos = (Positions)button.Tag;

                if (pos.Row == row && pos.Column == col)
                    return button;


            }
            return null;
        }

        private bool IsMarked(int r, int c)
        {

            if (r < HalfArrayRowLenght && c < HalfArrayColLenght)
            {
                return TopLeft[r, c] == CurrentPlayer;
            }

            else if (r < HalfArrayRowLenght && c > HalfArrayRowLenght - 1 && c < ArrayColLenght)
            {
                return TopRight[r, c - HalfArrayColLenght] == CurrentPlayer;
            }


            if (r >= HalfArrayRowLenght && c < HalfArrayColLenght)
            {
                return BotLeft[r - HalfArrayRowLenght, c] == CurrentPlayer;
            }
            else
            {
                return BotRight[r - HalfArrayRowLenght, c - HalfArrayColLenght] == CurrentPlayer;
            }

        }
        private bool CheckRow()
        {
            WinCondition = 0;
            for (int r = 0; r < ArrayRowLenght; r++)
            {
                for (int c = 0; c < ArrayColLenght; c++)
                {
                    if (IsMarked(r, c))
                    {
                        WinCondition++;

                        if (WinCondition == 5)
                        {
                            WinInfo = new WinInfo { WinType = WinType.Row, r = r, c = c };
                            return true;
                        }

                    }
                    else
                    {
                        WinCondition = 0;
                    }
                }
                WinCondition = 0;
            }
            return false;
        }
        private bool CheckColumn()
        {
            WinCondition = 0;
            for (int c = 0; c < ArrayColLenght; c++)
            {
                for (int r = 0; r < ArrayRowLenght; r++)
                {
                    if (IsMarked(r, c))
                    {
                        WinCondition++;
                        if (WinCondition == 5)
                        {
                            WinInfo = new WinInfo { WinType = WinType.Column, r = r, c = c };
                            return true;
                        }
                    }
                    else
                    {
                        WinCondition = 0;

                    }
                }
                WinCondition = 0;
            }
            return false;
        }
        private bool IsDiagonalWin()
        {
            for (int row = 0; row < HalfArrayRowLenght - 1; row++)
            {
                for (int col = 0; col < HalfArrayColLenght - 1; col++)
                {
                    if (IsMarked(row, col))
                    {
                        return (CheckDiagonal(row, col));
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            for (int row = 0; row < HalfArrayRowLenght - 1; row++)
            {
                for (int col = 5; col > HalfArrayColLenght; col--)
                {
                    if (IsMarked(row, col))
                    {
                        return (CheckAntiDiagonal(row, col));
                    }
                }
            }
            return false;
        }
        private bool IsWin()
        {
            if (CheckRow())
            {
                GameResult = new GameResult { Winner = CurrentPlayer, WinInfo = this.WinInfo };
                return true;
            }
            else if (CheckColumn())
            {
                GameResult = new GameResult { Winner = CurrentPlayer, WinInfo = this.WinInfo };
                return true;
            }
            else if (IsDiagonalWin())
            {
                GameResult = new GameResult { Winner = CurrentPlayer, WinInfo = this.WinInfo };
                return true;
            }
            else if (IsGridFull())
            {
                GameResult = new GameResult { Winner = Player.None };
                return true;
            }

            GameResult = null;
            return false;

        }
        private bool CheckDiagonal(int row, int col)
        {
            WinCondition = 0;
            for (int r = row; r < ArrayRowLenght;)
            {
                for (int c = col; c < ArrayColLenght;)
                {
                    if (IsMarked(row, col))
                    {
                        WinCondition++;
                        if (WinCondition == 5)
                        {
                            WinInfo = new WinInfo { WinType = WinType.Diagonal, c = c, r = r };
                            return true;
                        }
                    }
                    else
                    {
                        WinCondition = 0;
                        return false;
                    }
                    row++;
                    col++;
                }
            }

            return false;
        }
        private bool CheckAntiDiagonal(int row, int col)
        {
            WinCondition = 0;
            for (int r = row; r < ArrayRowLenght;)
            {
                for (int c = col; c >= 0;)
                {
                    if (IsMarked(r, c))
                    {
                        WinCondition++;
                        if (WinCondition == 5)
                        {
                            WinInfo = new WinInfo { WinType = WinType.Antidiagonal, c = col, r = row };
                            return true;
                        }
                    }
                    else
                    {
                        WinCondition = 0;
                        return false;
                    }
                    r++;
                    c--;
                }
            }

            return false;
        }
        private void SwitchPlayer()
        {
            if (CurrentPlayer == Player.Blue)
            {
                CurrentPlayer = Player.Red;
            }
            else
            {
                CurrentPlayer = Player.Blue;
            }

        }
        public void MakeMove(int row, int col, GameGrid grid, List<Button> buttons)
        {
            if (!CanMove(grid, row, col))
            {
                return;
            }

            Buttons = buttons;

            Changbuttoncolor(row, col);
            switch (grid.Name)
            {
                case ("GridTopLeft"):
                    TopLeft[row, col] = CurrentPlayer;
                    break;
                case ("GridTopRight"):
                    TopRight[row, col] = CurrentPlayer;
                    break;
                case ("GridBotLeft"):
                    BotLeft[row, col] = CurrentPlayer;
                    break;
                case ("GridBotRight"):
                    BotRight[row, col] = CurrentPlayer;
                    break;
            }

            TurnsPassed++;
            if (IsWin())
            {
                GameOver = true;
                GameEnded?.Invoke(GameResult);
            }
            else
            {
                MoveMade?.Invoke();
            }

        }
        public void Reset()
        {
            TopLeft = new Player[3, 3];
            TopRight = new Player[3, 3];
            BotLeft = new Player[3, 3];
            BotRight = new Player[3, 3];
            WinCondition = 0;
            CurrentPlayer = Player.Blue;
            TurnsPassed = 0;
            GameOver = false;
            GameRestarted?.Invoke();
        }
        public void RotateArray(string buttonname, GameGrid topLeft, GameGrid topRight, GameGrid botLeft, GameGrid botRight)
        {
            switch (buttonname)
            {
                case ("BTLL"):
                    TopLeft = RotateArrayLeft(TopLeft);
                    topLeft.SetNewPositions(Direction.Left);
                    Move(topLeft, -90);
                    break;
                case ("BTLR"):
                    TopLeft = RotateArrayRight(TopLeft);
                    topLeft.SetNewPositions(Direction.Right);
                    Move(topLeft, 90);
                    break;
                case ("BTRL"):
                    TopRight = RotateArrayLeft(TopRight);
                    topRight.SetNewPositions(Direction.Left);
                    Move(topRight, -90);
                    break;
                case ("BTRR"):
                    TopRight = RotateArrayRight(TopRight);
                    topRight.SetNewPositions(Direction.Right);
                    Move(topRight, 90);
                    break;
                case ("BBLL"):
                    BotLeft = RotateArrayRight(BotLeft);
                    botLeft.SetNewPositions(Direction.Right);
                    Move(botLeft, 90);
                    break;
                case ("BBLR"):
                    BotLeft = RotateArrayLeft(BotLeft);
                    botLeft.SetNewPositions(Direction.Left);
                    Move(botLeft, -90);
                    break;
                case ("BBRL"):
                    BotRight = RotateArrayRight(BotRight);
                    botRight.SetNewPositions(Direction.Right);
                    Move(botRight, 90);
                    break;
                case ("BBRR"):
                    BotRight = RotateArrayLeft(BotRight);
                    botRight.SetNewPositions(Direction.Left);
                    Move(botRight, -90);
                    break;
            }
            Turned = false;
            SwitchPlayer();
            if (isSinglePlayer == true && CurrentPlayer == Player.Red)
            {
                Max(Suchtiefe);
                ComputerTurn();
                MoveMade?.Invoke();
            }
        }

        //private void ComputerTurn()
        //{
        //    for (int row = 0; row < bestmove.GetLength(0); row++)
        //    {
        //        for(int col = 0; col < bestmove.GetLength(1); col++) 
        //        {


        //                ComputerMakeMove(row, col);
        //                Changbuttoncolor(row, col);
        //                return;

        //        } 
        //    }
        //}

        private void ComputerMakeMove(int row, int col)
        {

            if (row < HalfArrayRowLenght && col < HalfArrayColLenght)
            {
                TopLeft[row, col] = CurrentPlayer;
            }
            else if (row < HalfArrayRowLenght && col > HalfArrayRowLenght - 1 && col < ArrayColLenght)
            {
                TopRight[row, col - HalfArrayColLenght] = CurrentPlayer;
            }
            else if (row >= HalfArrayRowLenght && col < HalfArrayColLenght)
            {
                BotLeft[row - HalfArrayRowLenght, col] = CurrentPlayer;
            }

            if (row >= HalfArrayRowLenght && col >= HalfArrayColLenght && row < ArrayRowLenght && col < ArrayColLenght)
            {
                BotRight[row - HalfArrayRowLenght, col - HalfArrayColLenght] = CurrentPlayer;
            }
        }

        private void Move(GameGrid gameGrid, double angle)
        {
            RotateTransform rotateTransformTopLeft = new RotateTransform();
            rotateTransformTopLeft.Angle = gameGrid.CurrentRotation;
            rotateTransformTopLeft.CenterX = 155;
            rotateTransformTopLeft.CenterY = 155;
            gameGrid.RenderTransform = rotateTransformTopLeft;
            DoubleAnimation rotationAnimation = new DoubleAnimation();
            rotationAnimation.From = gameGrid.CurrentRotation;
            rotationAnimation.To = gameGrid.CurrentRotation + angle;
            rotationAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            gameGrid.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);

            gameGrid.CurrentRotation += angle;

        }
        private Player[,] RotateArrayRight(Player[,] array)
        {
            if (array == null)
            {
                return null;
            }
            int size = 3;
            Player[,] tmp = new Player[size, size];

            for (int r = 0; r < size; ++r)
            {

                for (int c = 0; c < size; ++c)
                {
                    tmp[c, size - 1 - r] = array[r, c];
                }

            }
            return tmp;
        }

        private Player[,] RotateArrayLeft(Player[,] array)
        {
            if (array == null)
            {
                return null;
            }
            int size = 3;
            Player[,] tmp = new Player[size, size];

            for (int r = 0; r < size; ++r)
            {
                for (int c = 0; c < size; ++c)
                {
                    tmp[size - 1 - c, r] = array[r, c];
                }
            }
            return tmp;
        }

        public int Max(int searchrange)
        {
            int maxvalue = -100000;
            int value = 0;

            if (searchrange == 0 || GameOver)
            {
                return EvaluateBoard(Player.Red);
            }

            List<(Player[,], Player[,], Player[,], Player[,])> moves = AllPossibleMoves(Player.Red);

            foreach (var move in moves)
            {
                ApplyMove(move, Player.Red); // Apply the move
                value = Min(searchrange - 1);
                UndoMove(move, Player.Red); // Undo the move

                if (value > maxvalue)
                {
                    if (searchrange == Suchtiefe)
                    {
                        bestmove = move;
                    }
                    maxvalue = value;
                }
            }

            // Check all possible rotations
            for (int segment = 1; segment <= 4; segment++)
            {
                for (int direction = 0; direction <= 1; direction++)
                {
                    ApplyRotation(segment, direction == 1); // Apply rotation
                    value = Min(searchrange - 1);
                    UndoRotation(segment, direction == 1); // Undo rotation

                    if (value > maxvalue)
                    {
                        if (searchrange == Suchtiefe)
                        {
                            bestrotation = (segment, direction, true);
                        }
                        maxvalue = value;
                    }
                }
            }

            return maxvalue;
        }

        public int Min(int searchrange)
        {
            int minvalue = 100000;
            int value = 0;

            if (searchrange == 0 || GameOver)
            {
                return EvaluateBoard(Player.Blue);
            }

            List<(Player[,], Player[,], Player[,], Player[,])> moves = AllPossibleMoves(Player.Blue);

            foreach (var move in moves)
            {
                ApplyMove(move, Player.Blue); // Apply the move
                value = Max(searchrange - 1);
                UndoMove(move, Player.Blue); // Undo the move

                if (value < minvalue)
                {
                    minvalue = value;
                }
            }

            // Check all possible rotations
            for (int segment = 1; segment <= 4; segment++)
            {
                for (int direction = 0; direction <= 1; direction++)
                {
                    ApplyRotation(segment, direction == 1); // Apply rotation
                    value = Max(searchrange - 1);
                    UndoRotation(segment, direction == 1); // Undo rotation

                    if (value < minvalue)
                    {
                        minvalue = value;
                    }
                }
            }

            return minvalue;
        }

        public void ApplyMove((Player[,], Player[,], Player[,], Player[,]) move, Player player)
        {
            Array.Copy(move.Item1, TopLeft, move.Item1.Length);
            Array.Copy(move.Item2, TopRight, move.Item2.Length);
            Array.Copy(move.Item3, BotLeft, move.Item3.Length);
            Array.Copy(move.Item4, BotRight, move.Item4.Length);
        }

        public void UndoMove((Player[,], Player[,], Player[,], Player[,]) move, Player player)
        {
            Array.Copy(move.Item1, TopLeft, move.Item1.Length);
            Array.Copy(move.Item2, TopRight, move.Item2.Length);
            Array.Copy(move.Item3, BotLeft, move.Item3.Length);
            Array.Copy(move.Item4, BotRight, move.Item4.Length);
        }

        public void ApplyRotation(int segment, bool clockwise)
        {
            Player[,] board;
            switch (segment)
            {
                case 1: board = TopLeft; break;
                case 2: board = TopRight; break;
                case 3: board = BotLeft; break;
                case 4: board = BotRight; break;
                default: throw new ArgumentException("Invalid segment");
            }

            RotateSegment(board, clockwise);
        }

        public void UndoRotation(int segment, bool clockwise)
        {
            ApplyRotation(segment, !clockwise);
        }

        public void RotateSegment(Player[,] segment, bool clockwise)
        {
            Player[,] temp = (Player[,])segment.Clone();

            if (clockwise)
            {
                segment[0, 0] = temp[2, 0];
                segment[0, 1] = temp[1, 0];
                segment[0, 2] = temp[0, 0];
                segment[1, 0] = temp[2, 1];
                segment[1, 2] = temp[0, 1];
                segment[2, 0] = temp[2, 2];
                segment[2, 1] = temp[1, 2];
                segment[2, 2] = temp[0, 2];
            }
            else
            {
                segment[0, 0] = temp[0, 2];
                segment[0, 1] = temp[1, 2];
                segment[0, 2] = temp[2, 2];
                segment[1, 0] = temp[0, 1];
                segment[1, 2] = temp[2, 1];
                segment[2, 0] = temp[0, 0];
                segment[2, 1] = temp[1, 0];
                segment[2, 2] = temp[2, 0];
            }
        }

        public List<(Player[,], Player[,], Player[,], Player[,])> AllPossibleMoves(Player player)
        {
            List<(Player[,], Player[,], Player[,], Player[,])> moves = new List<(Player[,], Player[,], Player[,], Player[,])>();

            void AddMove(int i, int j, Player[,] board)
            {
                if (board[i, j] == Player.None)
                {
                    var newBoard1 = (Player[,])TopLeft.Clone();
                    var newBoard2 = (Player[,])TopRight.Clone();
                    var newBoard3 = (Player[,])BotLeft.Clone();
                    var newBoard4 = (Player[,])BotRight.Clone();

                    if (board == TopLeft) newBoard1[i, j] = player;
                    if (board == TopRight) newBoard2[i, j] = player;
                    if (board == BotLeft) newBoard3[i, j] = player;
                    if (board == BotRight) newBoard4[i, j] = player;

                    moves.Add((newBoard1, newBoard2, newBoard3, newBoard4));
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    AddMove(i, j, TopLeft);
                    AddMove(i, j, TopRight);
                    AddMove(i, j, BotLeft);
                    AddMove(i, j, BotRight);
                }
            }

            return moves;
        }

        public int EvaluateBoard(Player player)
        {
            if (CheckWin(Player.Red)) return 10;
            if (CheckWin(Player.Blue)) return -10;
            return 0;
        }

        public bool CheckWin(Player player)
        {
            // Flatten the 4 boards into a single 6x6 board for easier win checking
            Player[,] fullBoard = new Player[6, 6];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    fullBoard[i, j] = TopLeft[i, j];
                    fullBoard[i, j + 3] = TopRight[i, j];
                    fullBoard[i + 3, j] = BotLeft[i, j];
                    fullBoard[i + 3, j + 3] = BotRight[i, j];
                }
            }

            // Check rows, columns, and diagonals for a win
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    // Check rows
                    if (fullBoard[i, j] == player && fullBoard[i, j + 1] == player && fullBoard[i, j + 2] == player &&
                        fullBoard[i, j + 3] == player && fullBoard[i, j + 4] == player)
                        return true;

                    // Check columns
                    if (fullBoard[j, i] == player && fullBoard[j + 1, i] == player && fullBoard[j + 2, i] == player &&
                        fullBoard[j + 3, i] == player && fullBoard[j + 4, i] == player)
                        return true;
                }
            }

            // Check diagonals
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    // Top-left to bottom-right diagonals
                    if (fullBoard[i, j] == player && fullBoard[i + 1, j + 1] == player && fullBoard[i + 2, j + 2] == player &&
                        fullBoard[i + 3, j + 3] == player && fullBoard[i + 4, j + 4] == player)
                        return true;

                    // Bottom-left to top-right diagonals
                    if (fullBoard[i + 4, j] == player && fullBoard[i + 3, j + 1] == player && fullBoard[i + 2, j + 2] == player &&
                        fullBoard[i + 1, j + 3] == player && fullBoard[i, j + 4] == player)
                        return true;
                }
            }

            return false;
        }
    }
}


