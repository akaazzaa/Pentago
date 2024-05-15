using System.Windows.Controls;

namespace Pentago
{
    /// <summary>
    /// Interaktionslogik für Board.xaml
    /// </summary>
    public partial class Board : UserControl
    {
       Klassen.Game game;
        GameGrid gridTL ;
        GameGrid gridTR ;
        GameGrid gridBL ;
        GameGrid gridBr ;
        public Board()
        {
            InitializeComponent();
           
            game = new Klassen.Game();
             gridTL = new GameGrid(GridTL);
             gridTR = new GameGrid(GridTR);
             gridBL = new GameGrid(GridBL);
             gridBr = new GameGrid(GridBR);

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            gridTL.RotateGrid(-90);
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
