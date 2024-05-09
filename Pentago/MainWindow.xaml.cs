using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pentago
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Button_1.Background = Brushes.Yellow;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GridTopR.RenderTransform = new RotateTransform(90);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            GridTopR.RenderTransform = new TranslateTransform(GridTopR.get + 90)
        }
    }
}
