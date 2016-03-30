using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPaint.Shapes;
using System.Windows;
using System.Windows.Media;

namespace MyPaint.Actions
{
    class Rotate:Action
    {
        private Point prevPoint;
        private Point curPoint;

        public Rotate(Shape shape, Point mousePosition)
        {
            if (shape == null)
                return;
            this.Shape = shape;
            prevPoint = mousePosition;
            curPoint = prevPoint;
        }

        public override void Start(Point mousePosition)
        {
            int newDegree = calDegree(mousePosition);
            Shape.Rotate(newDegree);

            curPoint = mousePosition;
        }

        public override void Undo()
        {
            Shape.Rotate(calDegree(prevPoint) - calDegree(curPoint));
        }

        public override void Redo()
        {
            Shape.Rotate(calDegree(curPoint) - calDegree(prevPoint));
        }

        private int calDegree(Point mousePosition)
        {
            Point shapeCenter = Shape.GetCenter();
            double rad = Math.Atan2(mousePosition.Y - shapeCenter.Y, mousePosition.X - shapeCenter.X);

            return (int)(rad * 180 / Math.PI);
        }
    }
}
