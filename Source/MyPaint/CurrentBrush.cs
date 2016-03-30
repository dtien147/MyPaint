
using System.Windows.Controls;
using System.Windows.Media;
using MyPaint.GraphicElement.Shapes;

namespace MyPaint
{
    struct CurrentBrush
    {
        public static Brush Brush;
        public static bool ChangeBrush = true;
        public static BrushType CurBrushType;
    }
}
