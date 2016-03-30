using System.Windows.Media;
using MyPaint.GraphicElement;

namespace MyPaint.Actions
{
    class OutlineChanging : Action
    {
        private Color curColor;
        private Color prevColor;

        public OutlineChanging(Graphic graphic, Color newColor) : base(graphic)
        {
            prevColor = graphic.GetOutlineColor();
            curColor = newColor;
            Graphic.ChangeOutlineColor(newColor);
        }

        public override void Undo()
        {
            Graphic.ChangeOutlineColor(prevColor);
        }

        public override void Redo()
        {
            Graphic.ChangeOutlineColor(curColor);
        }
    }
}
