using Pentago.Klassen;
using System;
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

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Game game = new Game();
            game.isSinglePlayer = true;
            MainModel.SetNewContent(new Board(game));
        }

        private void TextBlock_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Game game = new Game();
            MainModel.SetNewContent(new Board(game));
        }
    }
}
