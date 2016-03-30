using System.Windows;
using MyPaint.GraphicElement;
namespace MyPaint.Actions
{
    class Moving : Action
    {
        private Point prevPoint;
        private Point curPoint;
        public Moving(Graphic graphic, Point mousePosition)
        {
            if (graphic == null)
                return;
            this.Graphic = graphic;
            prevPoint = mousePosition;
            curPoint = prevPoint;
        }

        public override void Start(Point mousePosition)
        {
            Graphic.Move(mousePosition.X - curPoint.X, mousePosition.Y - curPoint.Y);
            curPoint = mousePosition;
        }

        public override void Undo()
        {
            Graphic.Move(prevPoint.X - curPoint.X, prevPoint.Y - curPoint.Y);
        }

        public override void Redo()
        {
            Graphic.Move(curPoint.X - prevPoint.X, curPoint.Y - prevPoint.Y);
        }
    }
}
