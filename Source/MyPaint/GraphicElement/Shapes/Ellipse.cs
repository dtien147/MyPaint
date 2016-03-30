using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Serialization;

namespace MyPaint.GraphicElement.Shapes
{
    [DataContract]
    class Ellipse : ShapeWithBackground
    {
        [DataMember(Name = "Center")] private Point m_center;

        [DataMember(Name = "rx")] private double radiusX;

        [DataMember(Name = "ry")] private double radiusY;

        public Ellipse() { }

        public Ellipse(Point center, Brush backgroundType, Color outLineColor, int outlineWidth, System.Windows.Media.DashStyle outLineType)
            : base(outLineColor, outlineWidth, outLineType)
        {
            this.m_center = center;
            this.radiusX = 0;
            this.radiusY = 0;
            this.Background = backgroundType;
   
        }

        public override void Draw(DrawingContext drawingContext)
        {
            Pen pen = this.CreatePen();
  
            Trans = new RotateTransform(Degree,Center.X,Center.Y);
            drawingContext.PushTransform(Trans);
            drawingContext.DrawEllipse(this.Background, pen, Center, radiusX, radiusY);
            drawingContext.Pop();
        }
        public override void UpdateNextPoint(Point nextPoint)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                radiusX = Math.Sqrt(Math.Pow((m_center.X - nextPoint.X), 2.0) + Math.Pow((m_center.Y - nextPoint.Y), 2.0));
                radiusY = Math.Sqrt(Math.Pow((m_center.X - nextPoint.X), 2.0) + Math.Pow((m_center.Y - nextPoint.Y), 2.0));
            }
            else
            {
                radiusX = Math.Abs(nextPoint.X - m_center.X);
                radiusY = Math.Abs(nextPoint.Y - m_center.Y);
            }
        }
        public override bool Intersect(Point point)
        {
            point = Rotate(point);

            double rx2 = (radiusX + OutlineWidth / 2) * (radiusX + OutlineWidth / 2);
            double ry2 = (radiusY + OutlineWidth / 2) * (radiusY + OutlineWidth / 2);
            double x2 = (point.X - m_center.X) * (point.X - m_center.X);
            double y2 = (point.Y - m_center.Y) * (point.Y - m_center.Y);
            //(x - center.X)^2/a^2 + (y - center.Y)^2/b^2 = 1
            //or (x - center.Y)^2/a^2 + (y - center.X)^2/b^2 = 1
            //a is radiusX (a is half of length); b is radiusY (b is halft of heigth)
            if (x2 / rx2 + y2 / ry2 <= 1)
                return true;
            return false;

        }
        public override void Move(double x, double y)
        {
            m_center.X += x;
            m_center.Y += y;
        }
        public override void DrawRect(DrawingContext drawingContext)
        {
            Point topLeftPoint = new Point(m_center.X - Math.Abs(radiusX) - Distance, m_center.Y - Math.Abs(radiusY) - Distance);
            Point bottomRightPoint = new Point(m_center.X + Math.Abs(radiusX) + Distance,m_center.Y + Math.Abs(radiusY) + Distance);
            Center.X = m_center.X;
            Center.Y = m_center.Y;
            drawingContext.PushTransform(Trans);
            DrawRect(drawingContext, new Rect(topLeftPoint, bottomRightPoint));
            drawingContext.Pop();
        }

        public override AnchorType IntersectWithAnchor(Point point)
        {
            point = Rotate(point);

            Point topLeftPoint = new Point(m_center.X - Math.Abs(radiusX) - Distance, m_center.Y - Math.Abs(radiusY) - Distance);
            Point bottomRightPoint = new Point(m_center.X + Math.Abs(radiusX) + Distance, m_center.Y + Math.Abs(radiusY) + Distance);

            Rect rect = new Rect(topLeftPoint, bottomRightPoint);

            return IntersectWithAnchor(rect, point);
        }

        public override void Resize(AnchorType anchorType, double x, double y)
        {
            switch (anchorType)
            {
                case AnchorType.TopLeft:
                    radiusX -= x;
                    radiusY -= y;
                    break;
                case AnchorType.TopRight:
                    radiusX += x;
                    radiusY -= y;
                    break;
                case AnchorType.BottomRight:
                    radiusX += x;
                    radiusY += y;
                    break;
                case AnchorType.BottomLeft:
                    radiusX -= x;
                    radiusY += y;
                    break;
                case AnchorType.Top:
                    radiusY -= y;
                    break;
                case AnchorType.Bottom:
                    radiusY += y;
                    break;
                case AnchorType.Right:
                    radiusX += x;
                    break;
                case AnchorType.Left:
                    radiusX -= x;
                    break;
            }
        }
    }
}


