using System;
using System.IO;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MyPaint.GraphicElement.Shapes;

namespace MyPaint.GraphicElement
{
    [DataContract]
    class Image : Graphic
    {
        [NonSerialized] private BitmapImage image;

        //Buffer strorage BitmapImage
        [DataMember(Name = "Buffer")] private byte[] buffer;

        [DataMember(Name = "Start")] private Point start;

        [DataMember(Name = "End")] private Point end;

        public Image()
        {
            int a = 0;
        }
        public Image(string filePath)
        {
            image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(filePath);
            image.EndInit();

            image.DecodePixelHeight = (int)image.Height;
            image.DecodePixelWidth = (int) image.Width;

            end.X = start.X + image.Width;
            end.Y = start.Y + image.Height;
        }

        public override void Draw(DrawingContext drawingContext)
        {
            Rect rect = new Rect(start, end);
            Trans = new RotateTransform(Degree, Center.X, Center.Y);
            drawingContext.PushTransform(Trans);
            drawingContext.DrawImage(image, rect);
            drawingContext.Pop();
        }

        [OnSerializing]
        private void StreamImage(StreamingContext sc)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                buffer = ms.ToArray();
            }

        }

        [OnDeserialized]
        void LoadImage(StreamingContext sc)
        {
            MemoryStream strmImg = new MemoryStream(buffer);
            image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = strmImg;
            //image.DecodePixelWidth = 200;
            image.EndInit();
        }

        public override void Move(double x, double y)
        {
            start.X += x;
            start.Y += y;
            end.X += x;
            end.Y += y;
        }

        public override bool Intersect(Point point)
        {
            Point rotatedPoint = Rotate(point);

            Rect rect = new Rect(start, end);

            return rect.IntersectsWith(new Rect(rotatedPoint, new Size(1, 1)));
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

        protected AnchorType IntersectWithAnchor(Rect rect, Point point)
        {
            int anchorWidth = 3;
            Size anchorSize = new Size(anchorWidth, anchorWidth);
            Rect mouseRect = new Rect(point, new Size(1, 1));

            //Top Left Point
            if (new Rect(rect.TopLeft, anchorSize).IntersectsWith(mouseRect))
            {
                return AnchorType.TopLeft;
            }

            //Top Point
            if (new Rect(new Point((rect.Right + rect.Left) / 2, rect.Top), anchorSize).IntersectsWith(mouseRect))
            {
                return AnchorType.Top;
            }

            //Top Right Point
            if (new Rect(rect.TopRight, anchorSize).IntersectsWith(mouseRect))
            {
                return AnchorType.TopRight;
            }

            //Right Point
            if (new Rect(new Point(rect.Right, (rect.Top + rect.Bottom) / 2), anchorSize).IntersectsWith(mouseRect))
            {
                return AnchorType.Right;
            }

            //Bottom Right Point
            if (new Rect(rect.BottomRight, anchorSize).IntersectsWith(mouseRect))
            {
                return AnchorType.BottomRight;
            }

            //Bottom Point
            if (new Rect(new Point((rect.Right + rect.Left) / 2, rect.Bottom), anchorSize).IntersectsWith(
                mouseRect))
            {
                return AnchorType.Bottom;
            }

            //Bottom Left Point
            if (new Rect(rect.BottomLeft, anchorSize).IntersectsWith(mouseRect))
            {
                return AnchorType.BottomLeft;
            }

            //Left Point
            if (new Rect(new Point(rect.Left, (rect.Top + rect.Bottom) / 2), anchorSize)
                .IntersectsWith(mouseRect))
            {
                return AnchorType.Left;
            }

            return AnchorType.None;
        }

        protected override void DrawRect(DrawingContext drawingContext, Rect rect)
        {
            base.DrawRect(drawingContext, rect);

            DrawAnchor(drawingContext, rect);
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

            Resize();
        }

        private void Resize()
        {
            int newWidth = (int)Math.Abs(start.X - end.X);
            int newHeight = (int)Math.Abs(start.Y - end.Y);

            image.DecodePixelHeight = newHeight;
            image.DecodePixelWidth = newWidth;
            
        }
    }
}
