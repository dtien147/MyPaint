using System.Windows.Media;
using MyPaint.GraphicElement;

namespace MyPaint.Actions
{
    class BackgroundChanging : Action
    {
        private Brush curBackground;
        private Brush prevBackground;

        public BackgroundChanging(Graphic graphic, Brush newBackground) : base(graphic)
        {
            prevBackground = Graphic.GetBackground();
            curBackground = newBackground;
            Graphic.ChangeBackground(newBackground);
        }

        public override void Undo()
        {
            Graphic.ChangeBackground(prevBackground);
        }

        public override void Redo()
        {
            Graphic.ChangeBackground(curBackground);
        }
    }
}
