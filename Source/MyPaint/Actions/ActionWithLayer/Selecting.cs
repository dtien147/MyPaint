using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MyPaint.Layer;
using MyPaint.GraphicElement.Shapes;
namespace MyPaint.Actions
{
    class Selecting : ActionWithLayer
    {
        public Selecting(Layer.Layer layer, Point mousePosition)
            : base(layer)
        {
            bool found = false;
            int i = layer.CountGraphics() - 1;
            while (!found && i >= 0)
            {
                if (layer.GetGraphic(i).Intersect(mousePosition))
                {
                    found = true;
                }
                else i--;
            }
            if (found)
            {
                Graphic = layer.GetGraphic(i);
            }
        }
    }
}
