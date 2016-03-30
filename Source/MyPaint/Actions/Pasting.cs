using MyPaint.GraphicElement;

namespace MyPaint.Actions
{
    class Pasting : ActionWithLayer
    {
        public Pasting(Graphic graphic, Layer.Layer layer) : base(layer, graphic)
        {
            Layer.AddGraphic(graphic);
        }

        public override void Undo()
        {
            Layer.DeleteGraphics(Graphic);
        }

        public override void Redo()
        {
            Layer.DeleteGraphics(Graphic);
        }
    }
}
