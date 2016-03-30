using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using MyPaint.GraphicElement.Shapes;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace MyPaint
{
    /// <summary>
    /// Interaction logic for GradientBrush.xaml
    /// </summary>
    
    public partial class GradientBrush : Window
    {
        public List<string> colorListString = new List<string>();
        public GradientStopCollection colors = new GradientStopCollection();
       
        public GradientBrush()
        {
            InitializeComponent();
        }

        

        private void BtnAdd_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                colorListString.Add(ListView_ColorBoard.SelectedValue.ToString());

                System.Drawing.Color selectedColor =
                    System.Drawing.Color.FromName(ListView_ColorBoard.SelectedValue.ToString());

                Color color = MyPaint.GraphicElement.Factory.GetColors(selectedColor.Name);
                Rectangle colorItem = new Rectangle();
                colorItem.Stroke = new SolidColorBrush(Colors.Black);
                colorItem.Width = 10;
                colorItem.Height = 10;

                colorItem.Fill = new SolidColorBrush(color);

                ListBox_ColorList.Items.Add(colorItem);
            }

            catch (Exception)
            {

            }
        }

        private void Btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            CurrentBrush.CurBrushType = BrushType.LinearGradient;
            colors = new GradientStopCollection();
            for (int i = 0; i < ListBox_ColorList.Items.Count; i++)
            {
                System.Drawing.Color selectedColor =
                    System.Drawing.Color.FromName(colorListString[i]);

                Color color = MyPaint.GraphicElement.Factory.GetColors(selectedColor.Name);
                GradientStop temp = new GradientStop();
                temp.Color = color;
                temp.Offset = ((double)(i) + 1) / (double)(ListBox_ColorList.Items.Count);
                colors.Add(temp);
            }

            CurrentBrush.Brush =  new LinearGradientBrush(colors);
            CurrentBrush.Brush.Opacity = MainWindow.mainWindow.GetSlider().Value/100;
            CurrentBrush.CurBrushType = BrushType.LinearGradient;


            this.Close();


        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ListBox_ColorList.SelectedIndex;
                colorListString.RemoveAt(index);
                ListBox_ColorList.Items.RemoveAt(index);
            }

            catch (Exception)
            {

            }
        }

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;

        }

        private void BtnRemoveAll_Clicked(object sender, RoutedEventArgs e)
        {
            
            colorListString.Clear();
            colors.Clear();
            ListBox_ColorList.Items.Clear();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            CurrentBrush.ChangeBrush = false;
            MainWindow.mainWindow.GetRadioButton(BrushType.LinearGradient).IsChecked = false;
            MainWindow.mainWindow.GetRadioButton(CurrentBrush.CurBrushType).IsChecked = true;
            
            this.Close();
            
        }
    }
}
