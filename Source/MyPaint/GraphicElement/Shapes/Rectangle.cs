using System;
using System.Runtime.Serialization;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace MyPaint.GraphicElement.Shapes
{
    [DataContract]
    [KnownType(typeof(System.Windows.Media.SolidColorBrush))]
    class Rectangle : ShapeWithBackground
    {
        [DataMember(Name = "Start")]
        private Point start;

        [DataMember(Name = "End")]
        private Point end;

        [DataMember(Name = "EdgeLength")]
        private double edgeLength;
     
        public Rectangle() { }

        public Rectangle(Point start, Brush backgroundType, Color outLineColor, int outlineWidth, System.Windows.Media.DashStyle  outLineType)
            : base(outLineColor, outlineWidth, outLineType)
        {
            this.start = start;
            this.end = start;
            Background = backgroundType;
        }

        public override void Draw(DrawingContext drawingContext)
        {
            Pen pen = this.CreatePen();
            
            Trans = new RotateTransform(Degree, Center.X, Center.Y);
            drawingContext.PushTransform(Trans);
            drawingContext.DrawRectangle(Background, pen, new Rect(start, end));
            drawingContext.Pop();
        }

        public override void UpdateNextPoint(Point nextPoint)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (Math.Abs(nextPoint.X - start.X) >= Math.Abs(nextPoint.Y - start.Y))
                {
                    edgeLength = Math.Abs(nextPoint.X - start.X);
                    if (nextPoint.Y >= start.Y)
                        end = new Point(nextPoint.X, start.Y + edgeLength);
                    else
                        end = new Point(nextPoint.X, start.Y - edgeLength);
                }
                else
                {
                    edgeLength = Math.Abs(nextPoint.Y - start.Y);
                    if (nextPoint.X >= start.X)
                        end = new Point(start.X + edgeLength, nextPoint.Y);
                    else
                        end = new Point(start.X - edgeLength, nextPoint.Y);
                }
            }
            else
                end = nextPoint;
        }

        public override bool Intersect(Point point)
        {
            //Map point (x, y) -> (x',y') by that same rotation
            Point rotatedPoint = Rotate(point);

            Rect mouseRect = new Rect(rotatedPoint.X, rotatedPoint.Y, 1, 1);
            Rect currentRect = new Rect(start, end);
            return (mouseRect.IntersectsWith(currentRect));
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
            drawingContext.PushTransform(Trans);
            DrawRect(drawingContext, new Rect(topLeftPoint, bottomRightPoint));
            Center.X = (start.X + end.X) / 2;
            Center.Y = (start.Y + end.Y) / 2;
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
                end.Y = ymin1;
            }
        }
    }
}
