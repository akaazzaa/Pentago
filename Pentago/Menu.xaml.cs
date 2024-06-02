using Pentago.Klassen;
using System.Windows;
using System.Windows.Controls;

namespace Pentago
{
    /// <summary>
    /// Interaktionslogik für Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        
        public Menu()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game();
            MainModel.SetNewContent(new Board(game));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Game game = new Game();
            game.IsComputer = true;
            MainModel.SetNewContent(new Board(game));
        }
    }
}
