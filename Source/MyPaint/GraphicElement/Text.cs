using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using MyPaint.GraphicElement.Shapes;

namespace MyPaint.GraphicElement
{
    [DataContract]
    [KnownType(typeof(SolidColorBrush))]
    class Text : Graphic
    {
        [DataMember(Name = "Start")]
        private Point start;

        [DataMember(Name = "End")]
        private double height;

        [DataMember(Name = "Width")]
        private double width;

        [DataMember(Name = "Text")]
        private string textString;

        [DataMember(Name = "TextColor")]
        private Brush textColor;

        [DataMember(Name = "Font")]
        private string fontName;

        [NonSerialized]
        private FontFamily font;

        [DataMember(Name = "FontSize")]
        private double fontSize;

        [DataMember(Name = "Rect")]
        private Rect textRect;

        [DataMember(Name = "Background")]
        private Brush Background;

        public Text() { }
        
        public Text(string textString, Point start, double width, double height,Brush textColor, Brush Background,Double fontSize, FontFamily font)
        {
            this.textString = textString;
            this.start = start;
            this.width = width;
            this.height = height;
            this.textColor = textColor;
            this.Background = Background;
            this.fontSize = fontSize;
            this.font = font;
            Center.X = (start.X + width) / 2;
            Center.Y = (start.Y + height) / 2;

            
        }

        [OnSerializing]
        private void SaveFont(StreamingContext sc)
        {
            fontName = this.font.ToString();
        }

        [OnDeserialized]
        private void LoadFont(StreamingContext sc)
        {
            font = new FontFamily(fontName);
        }

        public override void Draw(DrawingContext drawingContext)
        {
            FormattedText formattedText = new FormattedText(
                textString,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface(font,
                FontStyles.Normal,
                FontWeights.Normal,
                FontStretches.Normal),
                fontSize,
                textColor
                );
         
            formattedText.MaxTextWidth = width;
            formattedText.MaxTextHeight = height;

            textRect = new Rect(start, new Size(width, height));
      
            Trans = new RotateTransform(Degree, Center.X, Center.Y);
            drawingContext.PushTransform(Trans);
            drawingContext.DrawRectangle(Background, null, textRect);
            drawingContext.DrawText(formattedText, start);
            drawingContext.Pop();
        }

        public override bool Intersect(Point point)
        {
            //Map point (x, y) -> (x',y') by that same rotation
            Point rotatedPoint = Rotate(point);

            Rect mouseRect = new Rect(rotatedPoint.X, rotatedPoint.Y, 1, 1);
            return (mouseRect.IntersectsWith(textRect));
        }

        public override void Move(double x, double y)
        {
            start.X += x;        
            start.Y += y;
        }

        public override void DrawRect(DrawingContext drawingContext)
        {
            Point end = textRect.BottomRight;
            Point topLeftPoint = new Point(Math.Min(start.X, end.X) - Distance, Math.Min(start.Y, end.Y) - Distance);
            Point bottomRightPoint = new Point(Math.Max(start.X, end.X) + Distance, Math.Max(start.Y, end.Y) + Distance);
            drawingContext.PushTransform(Trans);
            DrawRect(drawingContext, new Rect(topLeftPoint, bottomRightPoint));
            Center.X = (start.X + end.X) / 2;
            Center.Y = (start.Y + end.Y) / 2;
            drawingContext.Pop();

        }

        public void Rotate(int newDegree)
        {
            this.Degree = newDegree;
        }

        public Point GetCenter()
        {
            return Center;
        }

    }
}
