using System.Windows;
using MyPaint.GraphicElement;
using MyPaint.GraphicElement.Shapes;

namespace MyPaint.Actions
{
    class Resizing : Action
    {
        private Point curAnchorPoint;
        private Point prevAnchorPoint;
        private AnchorType curAnchorType;

        public Resizing(Graphic graphic, Point mousePosition)
            : base(graphic)
        {
            curAnchorType = graphic.IntersectWithAnchor(mousePosition);
            if (curAnchorType != AnchorType.None)
            {
                curAnchorPoint = mousePosition;
                prevAnchorPoint = curAnchorPoint;
            }
        }

        public override void Start(Point mousePosition)
        {
            double x = mousePosition.X - curAnchorPoint.X;
            double y = mousePosition.Y - curAnchorPoint.Y;

            curAnchorPoint = mousePosition;

            Graphic.Resize(curAnchorType, x, y);
        }

        public override void Undo()
        {
            double x = prevAnchorPoint.X - curAnchorPoint.X;
            double y = prevAnchorPoint.Y - curAnchorPoint.Y;

            Graphic.Resize(curAnchorType, x, y);
        }

        public override void Redo()
        {
            double x = curAnchorPoint.X - prevAnchorPoint.X;
            double y = curAnchorPoint.Y - prevAnchorPoint.Y;

            Graphic.Resize(curAnchorType, x, y);
        }
    }
}
