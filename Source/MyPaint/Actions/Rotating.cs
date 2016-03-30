using System;
using System.Windows;
using MyPaint.GraphicElement;

namespace MyPaint.Actions
{
    class Rotating:Action
    {
        private Point prevPoint;
        private Point curPoint;

        public Rotating(Graphic graphic, Point mousePosition)
        {
            if (graphic == null)
                return;
            this.Graphic = graphic;
            prevPoint = mousePosition;
            curPoint = prevPoint;
        }

        public override void Start(Point mousePosition)
        {
            int newDegree = calDegree(mousePosition);
            Graphic.Rotate(newDegree);

            curPoint = mousePosition;
        }

        public override void Undo()
        {
            Graphic.Rotate(calDegree(prevPoint) - calDegree(curPoint));
        }

        public override void Redo()
        {
            Graphic.Rotate(calDegree(curPoint) - calDegree(prevPoint));
        }

        private int calDegree(Point mousePosition)
        {
            Point shapeCenter = Graphic.GetCenter();
            double rad = Math.Atan2(mousePosition.Y - shapeCenter.Y, mousePosition.X - shapeCenter.X);

            return (int)(rad * 180 / Math.PI);
        }
    }
}
