using System;
using System.Drawing.Text;
using System.Runtime.Serialization;
using System.Windows.Media;
using System.Windows;

namespace MyPaint.GraphicElement.Shapes
{
    [DataContract]
    class Line : Shape
    {
        [DataMember(Name = "Start")]
        private Point start;

        [DataMember(Name = "End")]
        private Point end;

        public Point End
        {
            get
            {
                return end;
            }

            set
            {
                end = value;
            }
        }

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

        public Line() { }

        public Line(Point startPoint, Point endPoint)
        {
            Start = startPoint;
            End = endPoint;
        }

        public Line(Point start, Color outLineColor, int outlineWidth, System.Windows.Media.DashStyle outLineType)
            : base(outLineColor, outlineWidth, outLineType)
        {
            this.start = start;
            this.end = start;
        }

        public override void Draw(DrawingContext drawingContext)
        {
            Pen pen = this.CreatePen();
            Trans = new RotateTransform(Degree,Center.X,Center.Y);
            drawingContext.PushTransform(Trans);
            drawingContext.DrawLine(pen, Start, End);
            drawingContext.Pop();
        }
        public override void UpdateNextPoint(Point nextPoint)
        {
            End = nextPoint;
        }
        public override bool Intersect(Point point)
        {
            Point rotatedPoint = Rotate(point);

            double m1, m2;
            double d, d1, d2;

            //Distance from start of line to rotatedPoint
            d1 = Math.Sqrt(((Start.X - rotatedPoint.X) * (Start.X - rotatedPoint.X) + (rotatedPoint.Y - Start.Y) * (rotatedPoint.Y - Start.Y)));

            //Distance from end of line to rotatedPoint
            d2 = Math.Sqrt((float)((End.X - rotatedPoint.X) * (End.X - rotatedPoint.X) + (rotatedPoint.Y - End.Y) * (rotatedPoint.Y - End.Y)));

            //Distance from start of line to end of line
            d = Math.Sqrt(((End.X - Start.X) * (End.X - Start.X) + (Start.Y - End.Y) * (Start.Y - End.Y)));

            //If line is Vertical
            if (Math.Abs(Start.X - End.X) <= OutlineWidth)
            {
                //m1 = (Start.Y - End.Y) / (float)((Start.X - End.X));
                //m2 = Start.Y - m1 * Start.X;
                //return Math.Abs(rotatedPoint.Y - m1 * rotatedPoint.X - m2) <= OutlineWidth && Math.Abs(d - (d1 + d2)) <= OutlineWidth / 2;
                return
                    (((Math.Abs(Math.Max(End.X, Start.X) - rotatedPoint.X) + Math.Abs(rotatedPoint.X - Math.Min(End.X, Start.X)))) <=
                     OutlineWidth) &&
                    (rotatedPoint.Y >= Math.Min(Start.Y, End.Y) && rotatedPoint.Y <= Math.Max(Start.Y, End.Y));
            }
            else
                return (int)d == (int)(d1 + d2);
        }
        public override void Move(double x, double y)
        {
            start.X += x;
            end.X += x;
            start.Y += y;
            end.Y += y;
        }
        public override void DrawRect(DrawingContext drawingContext)
        {
            Point topLeftPoint = new Point(Math.Min(start.X, end.X) - Distance, Math.Min(start.Y, end.Y) - Distance);
            Point bottomRightPoint = new Point(Math.Max(start.X, end.X) + Distance, Math.Max(start.Y, end.Y) + Distance);
            Center.X = (bottomRightPoint.X + topLeftPoint.X) / 2;
            Center.Y = (bottomRightPoint.Y + topLeftPoint.Y) / 2;
            drawingContext.PushTransform(Trans);
            DrawRect(drawingContext, new Rect(topLeftPoint, bottomRightPoint));
            drawingContext.Pop();
        }

        public override AnchorType IntersectWithAnchor(Point point)
        {
            point = Rotate(point);

            Point topLeftPoint = new Point(Math.Min(start.X, end.X) - Distance, Math.Min(start.Y, end.Y) - Distance);
            Point bottomRightPoint = new Point(Math.Max(start.X, end.X) + Distance, Math.Max(start.Y, end.Y) + Distance);

            Rect rect = new Rect(topLeftPoint, bottomRightPoint);

            return IntersectWithAnchor(rect, point);
        }

        public override void Resize(AnchorType anchorType, double x, double y)
        {
            double xmin1, xmin2;
            double xmax1, xmax2;
            double ymin1, ymin2;
            double ymax1, ymax2;

            xmin1 = xmin2 = Math.Min(start.X, end.X);
            xmax1 = xmax2 = Math.Max(start.X, end.X);
            ymin1 = ymin2 = Math.Min(start.Y, end.Y);
            ymax1 = ymax2 = Math.Max(start.Y, end.Y);

            switch (anchorType)
            {
                case AnchorType.TopLeft:
                    xmin1 += x;
                    ymin1 += y;
                    break;
                case AnchorType.Top:
                    ymin1 += y;
                    break;
                case AnchorType.TopRight:
                    xmax1 += x;
                    ymin1 += y;
                    break;
                case AnchorType.Right:
                    xmax1 += x;
                    break;
                case AnchorType.BottomRight:
                    xmax1 += x;
                    ymax1 += y;
                    break;
                case AnchorType.Bottom:
                    ymax1 += y;
                    break;
                case AnchorType.BottomLeft:
                    xmin1 += x;
                    ymax1 += y;
                    break;
                case AnchorType.Left:
                    xmin1 += x;
                    break;
            }

            if (start.X == xmin2)
            {
                start.X = xmin1;
                end.X = xmax1;
            }
            else
            {
                end.X = xmin1;
                start.X = xmax1;
            }

            if (start.Y == ymin2)
            {
                start.Y = ymin1;
                end.Y = ymax1;
            }
            else
            {
                start.Y = ymax1;
                end.Y = ymax1;
            }


           
        }

        [OnSerializing]
        public void OOO(StreamingContext sc)
        {
            int b = 0;
        }
    }
}
