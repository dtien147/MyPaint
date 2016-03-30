using System.Windows;
using System.Windows.Media;
using MyPaint.GraphicElement;
using MyPaint.GraphicElement.Shapes;

namespace MyPaint.Actions
{
    class Drawing : ActionWithLayer
    {
        public Drawing(Layer.Layer layer, ShapeType shapeType, Brush backgroundType, Color outlineColor,
            int outlineWidth, System.Windows.Media.DashStyle outlineType, Point mousePosition)
            : base(layer)
        {
            if (shapeType != ShapeType.None)
            {
                Graphic = Factory.GetGraphic(shapeType, new Point(mousePosition.X, mousePosition.Y),
                    backgroundType, outlineColor, outlineWidth, outlineType);
                layer.AddGraphic(Graphic);
            }
        }

        public Drawing(Layer.Layer layer, Graphic shape) : base(layer, shape)
        {
            layer.AddGraphic(Graphic);
        }

        public override void Start(Point mousePosition)
        {
            if (Graphic != null)
            {
                Graphic.UpdateNextPoint(new Point(mousePosition.X, mousePosition.Y));
            }
        }

        public override void Undo()
        {
            Layer.DeleteGraphics(Graphic);
        }

        public override void Redo()
        {
            Layer.AddGraphic(Graphic);
        }
    }
}
