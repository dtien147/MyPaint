using MyPaint.GraphicElement;

namespace MyPaint.Actions
{
    class Deleting : ActionWithLayer
    {
        public Deleting(Graphic graphic, Layer.Layer layer)
            : base(layer, graphic)
        {
            layer.DeleteGraphics(Graphic);
        }

        public override void Redo()
        {
            Layer.DeleteGraphics(Graphic);
        }

        public override void Undo()
        {
            Layer.AddGraphic(Graphic);
        }
    }
}
