using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace MyPaint.GraphicElement.Shapes
{
    [DataContract]
    internal abstract class Shape : Graphic
    {
        [DataMember(Name = "OutlineColor")] protected Color OutLineColor;

        [DataMember(Name = "OutlineWidth")] protected int OutlineWidth;

        [DataMember(Name = "DashStyle")] protected System.Windows.Media.DashStyle OutLineType;

        public override Color GetOutlineColor()
        {
            return OutLineColor;
        }

        protected Shape()
        {

            OutLineColor = Colors.Black;
            OutlineWidth = 1;
            OutLineType = DashStyles.Solid;
            Degree = 0;
            Trans = new RotateTransform(Degree);
        }

        protected Pen CreatePen()
        {
            Pen pen = new Pen(new SolidColorBrush(this.OutLineColor), this.OutlineWidth);
            pen.DashStyle = this.OutLineType;

            return pen;
        }

        protected Shape(Color outLineColor, int outlineWidth, System.Windows.Media.DashStyle outLineType)
        {

            this.OutLineColor = outLineColor;
            this.OutlineWidth = outlineWidth;
            this.OutLineType = outLineType;
            Degree = 0;
            Trans = new RotateTransform(Degree);

        
        }

        public override bool ChangeOutlineColor(Color newOutlineColor)
        {
            if (!OutLineColor.Equals(newOutlineColor) && newOutlineColor != null)
            {
                OutLineColor = newOutlineColor;
                return true;
            }

            return false;
        }

        public override bool ChangeOutlineWidth(int newOutlineWidth)
        {
            if (!newOutlineWidth.Equals(OutlineWidth))
            {
                OutlineWidth = newOutlineWidth;
                return true;
            }

            return false;
        }

        public override bool ChangeOutlineType(System.Windows.Media.DashStyle newOutlineType)
        {
            if (newOutlineType != null && !newOutlineType.Equals(OutLineType))
            {
                OutLineType = newOutlineType;
                return true;
            }

            return false;
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
                new Rect(new Point((rect.Right + rect.Left)/2, rect.Top), anchorSize));

            //Top Right Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen, new Rect(rect.TopRight, anchorSize));

            //Right Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen,
                new Rect(new Point(rect.Right, (rect.Top + rect.Bottom)/2), anchorSize));

            //Bottom Right Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen, new Rect(rect.BottomRight, anchorSize));

            //Bottom Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen,
                new Rect(new Point((rect.Right + rect.Left)/2, rect.Bottom), anchorSize));

            //Bottom Left Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen, new Rect(rect.BottomLeft, anchorSize));

            //Left Point
            drawingContext.DrawRectangle(anchorBrush, anchorPen,
                new Rect(new Point(rect.Left, (rect.Top + rect.Bottom)/2), anchorSize));
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
            if (new Rect(new Point((rect.Right + rect.Left)/2, rect.Top), anchorSize).IntersectsWith(mouseRect))
            {
                return AnchorType.Top;
            }

            //Top Right Point
            if (new Rect(rect.TopRight, anchorSize).IntersectsWith(mouseRect))
            {
                return AnchorType.TopRight;
            }

            //Right Point
            if (new Rect(new Point(rect.Right, (rect.Top + rect.Bottom)/2), anchorSize).IntersectsWith(mouseRect))
            {
                return AnchorType.Right;
            }

            //Bottom Right Point
            if (new Rect(rect.BottomRight, anchorSize).IntersectsWith(mouseRect))
            {
                return AnchorType.BottomRight;
            }

            //Bottom Point
            if (new Rect(new Point((rect.Right + rect.Left)/2, rect.Bottom), anchorSize).IntersectsWith(
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
            if (new Rect(new Point(rect.Left, (rect.Top + rect.Bottom)/2), anchorSize)
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
    }
}
