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
       
        public GameGrid() 
        {
            _grid = SetGrid();
            
        }
        public GameGrid(Grid grid)
        {


            
            _grid = grid;
            SetButtons();
          
        
        }
        private Grid SetGrid()
        {
            // Erstellen des Grid-Objekts
            Grid gridBotR = new Grid();

            // Setzen des Namens des Grids
            gridBotR.Name = "GridBotR";
            gridBotR.Height = 250;
            gridBotR.Width = 250;
            // Setzen des RenderTransformOrigin
            gridBotR.RenderTransformOrigin = new System.Windows.Point(1, 0);

            // Setzen des Margins
            //gridBotR.Margin = new System.Windows.Thickness(1468, 341, 202, 489);

            // Hinzufügen des linearen Farbverlaufs zur Hintergrundfarbe des Grids
            LinearGradientBrush linearGradient = new LinearGradientBrush();
            linearGradient.StartPoint = new System.Windows.Point(0.5, 0);
            linearGradient.EndPoint = new System.Windows.Point(0.5, 1);
            linearGradient.GradientStops.Add(new GradientStop(Colors.Blue, 0));
            linearGradient.GradientStops.Add(new GradientStop(Colors.Red, 1));
            gridBotR.Background = linearGradient;

            // Hinzufügen des DropShadowEffects zum Grid
            gridBotR.Effect = new DropShadowEffect();

            // Erstellen und Hinzufügen des TransformGroup für LayoutTransform
            TransformGroup layoutTransformGroup = new TransformGroup();
            layoutTransformGroup.Children.Add(new ScaleTransform());
            layoutTransformGroup.Children.Add(new SkewTransform());
            layoutTransformGroup.Children.Add(new RotateTransform());
            layoutTransformGroup.Children.Add(new TranslateTransform());
            gridBotR.LayoutTransform = layoutTransformGroup;

            // Erstellen und Hinzufügen des TransformGroup für RenderTransform
            TransformGroup renderTransformGroup = new TransformGroup();
            renderTransformGroup.Children.Add(new ScaleTransform());
            renderTransformGroup.Children.Add(new SkewTransform() { AngleX = 0 });
            renderTransformGroup.Children.Add(new RotateTransform() { Angle = 0 });
            renderTransformGroup.Children.Add(new TranslateTransform() { X = 0, Y = 0 });
            gridBotR.RenderTransform = renderTransformGroup;

            // Hinzufügen der Zeilendefinitionen zum Grid
            gridBotR.RowDefinitions.Add(new RowDefinition());
            gridBotR.RowDefinitions.Add(new RowDefinition());
            gridBotR.RowDefinitions.Add(new RowDefinition());

            // Hinzufügen der Spaltendefinitionen zum Grid
            gridBotR.ColumnDefinitions.Add(new ColumnDefinition());
            gridBotR.ColumnDefinitions.Add(new ColumnDefinition());
            gridBotR.ColumnDefinitions.Add(new ColumnDefinition());

            return gridBotR;
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
            rotationAnimation.Duration = new Duration(TimeSpan.FromSeconds(10));

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
