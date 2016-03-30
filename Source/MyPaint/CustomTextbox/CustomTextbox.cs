using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyPaint.CustomTextbox
{
    class CustomTextbox
    {
        private Point start;

        private TextBox tb;

        private bool isSizeChange;

        public Point Start
        {
            get
            {
                return start;
            }

            set
            {
                start = value;
            }
        }

        public bool IsSizeChange
        {
            get
            {
                return isSizeChange;
            }

            set
            {
                isSizeChange = value;
            }
        }

        public TextBox Tb
        {
            get
            {
                return tb;
            }

            set
            {
                tb = value;
            }
        }

        

        public CustomTextbox(Point start)
        {
            this.start = start;
           
        }

        public void showTextbox(Grid mainGrid, Point mousePosition)
        {
            Tb = new TextBox();
            Tb.TextWrapping = TextWrapping.Wrap;
            Tb.Background = Brushes.Transparent;
            Tb.BorderThickness = new Thickness(0, 0, 0, 0);
            Tb.Width = 100;
            Tb.SizeChanged += Tb_SizeChanged;
            Tb.HorizontalAlignment = HorizontalAlignment.Left;
            Tb.VerticalAlignment = VerticalAlignment.Top;
            Tb.Margin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);
            mainGrid.Children.Add(Tb);
            Tb.UpdateLayout();
        }

        private void Tb_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            isSizeChange = true;
            Tb.UpdateLayout();
        }

        
        public bool isMouseOver()
        {
            return Tb.IsMouseOver;
        }

        public void DrawRect(DrawingContext drawingContext)
        {
            DrawRect(drawingContext, new Rect(start, new Point(start.X + Tb.ActualWidth, start.Y + Tb.ActualHeight)));
        }



        private void DrawRect(DrawingContext drawingContext, Rect rect)
        {
            Color shapeRectBackgroundColor = Colors.Transparent;
            Color shapeRectOutlineColor = Colors.DimGray;
            int shapeRectOutlineWidth = 1;
            Brush rectBrush = new SolidColorBrush(shapeRectBackgroundColor);
            Pen rectPen = new Pen(new SolidColorBrush(shapeRectOutlineColor), shapeRectOutlineWidth);

            rectPen.DashStyle = DashStyles.DashDot;

            drawingContext.DrawRectangle(rectBrush, rectPen, rect);

            DrawAnchor(drawingContext, rect);
        }

        private void DrawAnchor(DrawingContext drawingContext, Rect rect)
        {
            Color anchorColor = Colors.Blue;
            int anchorWidth = 2;
            Size anchorSize = new Size(anchorWidth, anchorWidth);

            Pen anchorPen = new Pen(new SolidColorBrush(anchorColor), anchorWidth);
            Brush anchorBrush = new SolidColorBrush(anchorColor);

            //Top Left Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen, new Rect(rect.TopLeft, anchorSize));

            //Top Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen,
                new Rect(new Point((rect.Right + rect.Left) / 2, rect.Top), anchorSize));

            //Top Right Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen, new Rect(rect.TopRight, anchorSize));

            //Right Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen,
                new Rect(new Point(rect.Right, (rect.Top + rect.Bottom) / 2), anchorSize));

            //Bottom Right Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen, new Rect(rect.BottomRight, anchorSize));

            //Bottom Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen,
                new Rect(new Point((rect.Right + rect.Left) / 2, rect.Bottom), anchorSize));

            //Bottom Left Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen, new Rect(rect.BottomLeft, anchorSize));

            //Left Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen,
                new Rect(new Point(rect.Left, (rect.Top + rect.Bottom) / 2), anchorSize));
        }

        public void changeFontSize(string size)
        {
            try
            {
                Tb.FontSize = double.Parse(size);
            }
            catch (Exception)
            { }
        }

        public void changeFont(string font)
        {
            Tb.FontFamily = new FontFamily(font);
        }

        public void changeColor(Color color)
        {
            Tb.Foreground = new SolidColorBrush(color);
        }

        public void changeBackground(bool trans, Color color)
        {
            if (trans)
                Tb.Background = Brushes.Transparent;
            else
                Tb.Background = new SolidColorBrush(color);
        }

        public string getText()
        {
            return Tb.Text;
        }

        public void destroy(Grid mainGrid)
        {
            mainGrid.Children.Remove(Tb);
        }

        public Brush getTextColor()
        {
            return tb.Foreground;
        }

        public Brush getBackgroundColor()
        {
            return tb.Background;
        }

        public double getFontSize()
        {
            return tb.FontSize;
        }

        public FontFamily getFont()
        {
            return tb.FontFamily;
        }
    }
}
