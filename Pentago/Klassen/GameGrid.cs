using Pentago.Klassen;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Forms;
using System.Security.RightsManagement;
using System.Windows.Media.Effects;

namespace Pentago
{
    public class GameGrid
    {
        Grid _grid;
        DoubleAnimation rotateAnimation;
        double currentRotation = 0;
        Logik logik;
        public Grid Grid { get => _grid; set => _grid = value; }
        public GameGrid(Grid grid)
        {
            _grid = grid;
            SetButtons();
        }
       
        private void SetButtons()
        {
            for (int i = 0; i < _grid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < _grid.ColumnDefinitions.Count; j++)
                {
                    GameButton gameButton = new GameButton(i, j);
                    Grid.SetRow(gameButton.Button, i);
                    Grid.SetColumn(gameButton.Button, j);
                    _grid.Children.Add(gameButton.Button);
                }
            }
        }

        public void RotateGrid(double angle)
        {
            // Erzeuge eine Drehtransformation
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle = currentRotation;

            // Verknüpfe die Transformation mit dem Grid
            _grid.RenderTransform = rotateTransform;

            // Erzeuge eine Drehanimation
            DoubleAnimation rotationAnimation = new DoubleAnimation();
            rotationAnimation.From = currentRotation;
            rotationAnimation.To = currentRotation + angle;
            rotationAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            // Füge der Transformation die Animation hinzu
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);

            // Aktualisiere den aktuellen Drehwinkel
            currentRotation += angle;
        }

        public void ChangePosition(double toX, double toY)
        {
            DoubleAnimation xAnimation = new DoubleAnimation();
            xAnimation.To = toX;
            xAnimation.Duration = TimeSpan.FromSeconds(10);

            DoubleAnimation yAnimation = new DoubleAnimation();
            yAnimation.To = toY;
            yAnimation.Duration = TimeSpan.FromSeconds(10);

            // Wende die Animationen auf das Grid an
            _grid.BeginAnimation(Canvas.LeftProperty, xAnimation);
            _grid.BeginAnimation(Canvas.TopProperty, yAnimation);
        }
    }
}
