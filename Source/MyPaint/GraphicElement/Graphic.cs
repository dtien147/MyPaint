using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Media;
using MyPaint.GraphicElement.Shapes;

namespace MyPaint.GraphicElement
{
    [DataContract]
    [KnownType(typeof(System.Windows.Media.MatrixTransform))]
    public abstract class Graphic : ICloneable
    {
        [DataMember(Name = "RotatingCenter")]
        protected Point Center;

        [DataMember(Name = "RotateTransform")]
        protected RotateTransform Trans;

        [DataMember(Name = "Degree")]
        protected int Degree;

        //Distance between graphic object and rectangle of graphic object
        protected const int Distance = 5;

        public virtual void Draw(DrawingContext drawingContext) { }

        public virtual bool Intersect(Point point)
        {
            return false;
        }

        public virtual void Move(double x, double y) { }

        public virtual void DrawRect(DrawingContext drawingContext) { }

        public virtual Color GetOutlineColor()
        {
            return Colors.Black;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        protected Point Rotate(Point point)
        {
            RotateTransform oppositeTrans = new RotateTransform(-Trans.Angle, Trans.CenterX, Trans.CenterY);

            return oppositeTrans.Transform(point);
        }

        public void Rotate(int newDegree)
        {
            this.Degree = newDegree;
        }

        public Point GetCenter()
        {
            return Center;
        }

        protected virtual void DrawRect(DrawingContext drawingContext, Rect rect)
        {
            Color shapeRectBackgroundColor = Colors.Transparent;
            Color shapeRectOutlineColor = Colors.DimGray;
            int shapeRectOutlineWidth = 1;
            Brush rectBrush = new SolidColorBrush(shapeRectBackgroundColor);
            Pen rectPen = new Pen(new SolidColorBrush(shapeRectOutlineColor), shapeRectOutlineWidth);

            rectPen.DashStyle = DashStyles.DashDot;

            drawingContext.DrawRectangle(rectBrush, rectPen, rect);

        }

        public virtual void Resize(AnchorType anchorType, double x, double y) { }

        public virtual bool ChangeBackground(Brush newBackground)
        {
            return false;
        }

        public virtual void UpdateNextPoint(Point nextPoint) { }

        public virtual AnchorType IntersectWithAnchor(Point point)
        {
            return AnchorType.None;
        }

        public virtual Brush GetBackground()
        {
            return null;
        }


        public virtual bool ChangeOutlineColor(Color newOutlineColor)
        {
            return false;
        }

        public virtual bool ChangeOutlineWidth(int newOutlineWidth)
        {
            return false;
        }

        public virtual bool ChangeOutlineType(System.Windows.Media.DashStyle newOutlineType)
        {
            return false;
        }
    }
}
