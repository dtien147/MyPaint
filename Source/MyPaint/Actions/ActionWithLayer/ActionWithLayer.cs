using MyPaint.GraphicElement;

namespace MyPaint.Actions
{
    abstract class ActionWithLayer : Action
    {
        protected Layer.Layer Layer;

        protected ActionWithLayer(Layer.Layer layer)
        {
            this.Layer = layer;
        }

        protected ActionWithLayer(Layer.Layer layer, Graphic graphic)
            : base(graphic)
        {
            this.Layer = layer;
        }

        public Layer.Layer GetLayer()
        {
            return Layer;
        }
    }
}
