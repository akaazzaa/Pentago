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
            MainModel.SetNewContent(new Board());
            
        }
    }
}
