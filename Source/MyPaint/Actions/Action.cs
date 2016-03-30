using System.Windows;
using MyPaint.GraphicElement;

namespace MyPaint.Actions
{
    //Actions can be undo and redo
    abstract class Action   
    {
        protected Graphic Graphic;

        protected Action() { }

        protected Action(Graphic graphic)
        {
            this.Graphic = graphic;
        }
        public virtual void Start(Point mousePosition) {}

        public virtual void Undo() { }

        public virtual void Redo() { }

        public Graphic GetGraphic()
        {
            return Graphic;
        }
    }
}
