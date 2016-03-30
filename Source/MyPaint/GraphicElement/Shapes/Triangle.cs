using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MyPaint.GraphicElement.Shapes
{
    [DataContract]
    class Triangle : ShapeWithBackground
    {
        [DataMember(Name = "Center")] private Point m_center;

        [DataMember(Name = "rx")] private double radiusX;

        [DataMember(Name = "ry")] private double radiusY;

        [DataMember(Name = "IsRight")] private bool isRight;

        public Triangle() { }

        public Triangle(Point center, Brush backgroundType, Color outLineColor, int outlineWidth, System.Windows.Media.DashStyle outLineType)
            : base(outLineColor, outlineWidth, outLineType)
        {
            this.m_center = center;
            this.radiusX = 0;
            this.radiusY = 0;
            this.Background = backgroundType;
            this.isRight = false;
        }

        public override void UpdateNextPoint(Point nextPoint)
        {
            radiusX = nextPoint.X - m_center.X;
            radiusY = nextPoint.Y - m_center.Y;

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                isRight = true;
            }
        }

        private List<Point> PointsOfTriangle(double r, Point center)
        {
            //Find points of star
            List<Point> points = new List<Point>();

            Point top = new Point(center.X, center.Y);
            Point left, right;

            if (isRight)
            {
                left = new Point(center.X, center.Y + radiusY);
                right = new Point(center.X + radiusX, center.Y + radiusY);
            }
            else
            {
                left = new Point(center.X - radiusX, center.Y + radiusY);
                right = new Point(center.X + radiusX, center.Y + radiusY);
            }
            

            points.Add(top);
            points.Add(left);
            points.Add(right);

            return points;
        }

        public override void Draw(DrawingContext drawingContext)
        {
            List<Point> points = PointsOfTriangle(radiusX, m_center);

            var geometry = new StreamGeometry();
            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(points[0], true /* is filled */, true /* is closed */);
                ctx.PolyLineTo(points, true, false);
            }
            geometry.Freeze();

            Pen pen = this.CreatePen();

            Trans = new RotateTransform(Degree, Center.X, Center.Y);
            drawingContext.PushTransform(Trans);
            drawingContext.DrawGeometry(this.Background, pen, geometry);
            drawingContext.Pop();
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

        public override void DrawRect(DrawingContext drawingContext)
        {
            List<Point> points = PointsOfTriangle(radiusX, m_center);
            double minX = m_center.X + Math.Abs(radiusX);
            double minY = m_center.Y + Math.Abs(radiusY);
            double maxX = m_center.X - Math.Abs(radiusX);
            double maxY = m_center.Y - Math.Abs(radiusY);
            foreach (Point point in points)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }

            Center.X = m_center.X;
            Center.Y = m_center.Y;
            drawingContext.PushTransform(Trans);
            DrawRect(drawingContext, new Rect(new Point(minX - Distance, minY - Distance), new Point(maxX + Distance, maxY + Distance)));
            drawingContext.Pop();
        }

        public override bool Intersect(Point point)
        {
            point = Rotate(point);

            //Source: http://stackoverflow.com/questions/11716268/point-in-polygon-algorithm
            List<Point> points = PointsOfTriangle(radiusX, m_center);
            int i, j, nvert = points.Count;
            bool c = false;

            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((points[i].Y >= point.Y) != (points[j].Y >= point.Y)) &&
                    (point.X <= (points[j].X - points[i].X) * (point.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X)
                  )
                    c = !c;
            }

            return c;
        }

        public override AnchorType IntersectWithAnchor(Point point)
        {
            point = Rotate(point);

            List<Point> points = PointsOfTriangle(radiusX, m_center);
            double minX = m_center.X + radiusX;
            double minY = m_center.Y + radiusY;
            double maxX = 0;
            double maxY = 0;
            foreach (Point p in points)
            {
                minX = Math.Min(minX, p.X);
                minY = Math.Min(minY, p.Y);
                maxX = Math.Max(maxX, p.X);
                maxY = Math.Max(maxY, p.Y);
            }

            Rect rect = new Rect(new Point(minX - Distance, minY - Distance), new Point(maxX + Distance, maxY + Distance));

            return IntersectWithAnchor(rect, point);
        }

        public override void Move(double x, double y)
        {
            m_center.X += x;
            m_center.Y += y;
        }
    }
}
