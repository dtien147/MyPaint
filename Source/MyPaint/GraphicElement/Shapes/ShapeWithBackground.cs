using System.Runtime.Serialization;
using System.Windows.Media;

namespace MyPaint.GraphicElement.Shapes
{
    [DataContract]
    [KnownType(typeof(SolidColorBrush))]
    abstract class ShapeWithBackground : Shape
    {
        [DataMember(Name = "Background")]
        protected Brush Background;

        protected ShapeWithBackground() { }

        protected ShapeWithBackground(Color outLineColor, int outlineWidth, System.Windows.Media.DashStyle outLineType) 
            : base(outLineColor, outlineWidth, outLineType)
        { }


        public override Brush GetBackground()
        {
            return Background;
        }
        public override bool ChangeBackground(Brush newBackground)
        {
            if (newBackground != null && !newBackground.Equals(Background))
            {
                Background = newBackground;
                return true;
            }

            return false;
        }
    }
}
