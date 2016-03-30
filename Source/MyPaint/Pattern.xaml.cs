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
using System.Windows.Shapes;
using MyPaint.GraphicElement.Shapes;

namespace MyPaint
{
    /// <summary>
    /// Interaction logic for Pattern.xaml
    /// </summary>
    public partial class Pattern : Window
    {
        public Brush tempBrush;
        public Color foreGround;

        public Color backGround;
        public Pattern()
        {
            InitializeComponent();
        }
      

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            CurrentBrush.CurBrushType = BrushType.Pattern;
            CurrentBrush.Brush = tempBrush.Clone();
            CurrentBrush.Brush.Opacity = MainWindow.mainWindow.GetSlider().Value/100;
            this.Close();
        }

        private void chessBoardFill_MouseLeftBtnUp(object sender, MouseButtonEventArgs e)
        {

            tempBrush = chessBoardFill.Fill;
        }

        private void horizontalFill_LeftBtnUp(object sender, MouseButtonEventArgs e)
        {
     
            tempBrush = horizontalFill.Fill;
        }

        private void verticalFill_LeftBtnUp(object sender, MouseButtonEventArgs e)
        {

            tempBrush = verticalFill.Fill;
        }       

        private void diagonalFill1_LeftBtnUp(object sender, MouseButtonEventArgs e)
        {

            tempBrush = diagonalFill1.Fill;
        }

        private void diagonalFill2_LeftBtnUp(object sender, MouseButtonEventArgs e)
        {

            tempBrush = diagonalFill2.Fill;
        }

        private void PatternWindows_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

       

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CurrentBrush.ChangeBrush = false;
            MainWindow.mainWindow.GetRadioButton(BrushType.Pattern).IsChecked = false;
            MainWindow.mainWindow.GetRadioButton(CurrentBrush.CurBrushType).IsChecked = true;

            this.Close();
        }

        private void PatternWindows_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                chessBoard_Foreground.Brush = new SolidColorBrush(foreGround);
                chessBoard_Background.Brush = new SolidColorBrush(backGround);
                chessBoard.Opacity = 1;

                horizontal_Foreground.Color = foreGround;
                horizontal_Background.Color = backGround;
                horizontalFill.Opacity = 1;

                vertical_Foreground.Color = foreGround;
                vertical_Background.Color = backGround;
                verticalFill.Opacity = 1;

                diagonal1_Foreground.Color = foreGround;
                diagonal1_Background.Color = backGround;
                diagonalFill1.Opacity = 1;

                diagonal2_Foreground.Color = foreGround;
                diagonal2_Background.Color = backGround;
                diagonalFill2.Opacity = 1;
            }
        }

        

       
    }
}
