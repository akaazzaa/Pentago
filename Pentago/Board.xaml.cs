using Pentago.Klassen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Remoting.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Pentago
{
    /// <summary>
    /// Interaktionslogik für Board.xaml
    /// </summary>
    public partial class Board : UserControl
    {
        Game game;
        
        public Board()
        {
            InitializeComponent();

            var paath = new System.Drawing.Drawing2D.GraphicsPath();
            paath.AddEllipse(0,0,100,100);
            this.Background = new SolidColorBrush(Colors.BlueViolet);
        }

        private void Button_Click_Rotation(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }

   
}
