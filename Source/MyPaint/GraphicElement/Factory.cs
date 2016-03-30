using System.Windows.Media;
using System.Windows;
using MyPaint.GraphicElement.Shapes;

namespace MyPaint.GraphicElement
{
    internal static class Factory
    {
        public static Shape GetGraphic(ShapeType shapeType, Point start, Brush backgroundType, Color outlineColor,
            int outlineWidth, System.Windows.Media.DashStyle outlineType)
        {
            switch (shapeType)
            {
                case ShapeType.Line:
                    return new Line(start, outlineColor, outlineWidth, outlineType);
                case ShapeType.Ellipse:
                    return new Ellipse(start, backgroundType, outlineColor, outlineWidth, outlineType);
                case ShapeType.Rectangle:
                    return new Rectangle(start, backgroundType, outlineColor, outlineWidth, outlineType);
                case ShapeType.Star:
                    return new Star(start, backgroundType, outlineColor, outlineWidth, outlineType);
                case ShapeType.Triangle:
                    return new Triangle(start, backgroundType, outlineColor, outlineWidth, outlineType);
                default:
                    return null;
            }
        }

        public static Graphic GetGraphic(string graphicName)
        {
            switch (graphicName)
            {
                case "Ellipse":
                    return new Ellipse();
                case "Line":
                    return new Line();
                case "Rectangle":
                    return new Rectangle();
                case "Star":
                    return new Star();
                case "Text":
                    return new Text();
                case "Image":
                    return new Image();
                default:
                    return null;
            }
        }

        public static Brush GetBrush(BrushType brushType)
        {
            switch (brushType)
            {
                case BrushType.LinearGradient:
                    return new LinearGradientBrush();


                case BrushType.Solid:
                    return new SolidColorBrush();

                case BrushType.Pattern:
                    return new ImageBrush();
                    
                default:
                    return null;                   
            }
        }
        public static System.Windows.Media.DashStyle GetDashStyle(MyPaint.DashStyle dashStyle)
        {
            switch (dashStyle)
            {
                case MyPaint.DashStyle.Dash:
                    return DashStyles.Dash;

                case MyPaint.DashStyle.DashDot:
                    return DashStyles.DashDot;

                case MyPaint.DashStyle.DashDotDot:
                    return DashStyles.DashDotDot;

                case MyPaint.DashStyle.Dot:
                    return DashStyles.Dot;

                default:
                    return DashStyles.Solid;
            }
        }
        public static ImageSource GetImageSource_ShapeType(string typeShape)
        {
            switch (typeShape)
            {
                case "Line":
                    return (ImageSource)
                        new ImageSourceConverter().ConvertFrom(@"pack://application:,,,/Resources/" + "line-icon.png");
                case "Rectangle":
                    return (ImageSource)
                        new ImageSourceConverter().ConvertFrom(@"pack://application:,,,/Resources/" +
                                                               "rectangle-icon.png");
                case "Ellipse":
                    return (ImageSource)
                        new ImageSourceConverter().ConvertFrom(@"pack://application:,,,/Resources/" + "ellipse-icon.png");
                case "Star":
                    return (ImageSource)
                        new ImageSourceConverter().ConvertFrom(@"pack://application:,,,/Resources/" + "star-icon.png");
                case "Triangle":
                    return (ImageSource)
                        new ImageSourceConverter().ConvertFrom(@"pack://application:,,,/Resources/" +
                                                               "triangle-icon.png");

                case "None":
                    return (ImageSource)
                        new ImageSourceConverter().ConvertFrom(@"pack://application:,,,/Resources/" + "shapes-icon.png");
                default:
                    return null;
            }
        }

        public static Color GetColors(string ColorString)
        {
            switch (ColorString)
            {
                case "System.Windows.Media.Color AliceBlue":
                    return Colors.AliceBlue;

                case "System.Windows.Media.Color AntiqueWhite":
                    return Colors.AntiqueWhite;

                case "System.Windows.Media.Color Aqua":
                    return Colors.Aqua;

                case "System.Windows.Media.Color Aquamarine":
                    return Colors.Aquamarine;

                case "System.Windows.Media.Color Azure":
                    return Colors.Azure;

                case "System.Windows.Media.Color Beige":
                    return Colors.Beige;

                case "System.Windows.Media.Color Bisque":
                    return Colors.Bisque;

                case "System.Windows.Media.Color Black":
                    return Colors.Black;

                case "System.Windows.Media.Color BlanchedAlmond":
                    return Colors.BlanchedAlmond;

                case "System.Windows.Media.Color Blue":
                    return Colors.Blue;

                case "System.Windows.Media.Color BlueViolet":
                    return Colors.BlueViolet;

                case "System.Windows.Media.Color Brown":
                    return Colors.Brown;

                case "System.Windows.Media.Color BurlyWood":
                    return Colors.BurlyWood;

                case "System.Windows.Media.Color CadetBlue":
                    return Colors.CadetBlue;

                case "System.Windows.Media.Color Chartreuse":
                    return Colors.Chartreuse;

                case "System.Windows.Media.Color Chocolate":
                    return Colors.Chocolate;

                case "System.Windows.Media.Color Coral":
                    return Colors.Coral;

                case "System.Windows.Media.Color CornflowerBlue":
                    return Colors.CornflowerBlue;

                case "System.Windows.Media.Color Cornsilk":
                    return Colors.Cornsilk;

                case "System.Windows.Media.Color Crimson":
                    return Colors.Crimson;

                case "System.Windows.Media.Color Cyan":
                    return Colors.Cyan;

                case "System.Windows.Media.Color DarkBlue":
                    return Colors.DarkBlue;

                case "System.Windows.Media.Color DarkCyan":
                    return Colors.DarkCyan;

                case "System.Windows.Media.Color DarkGoldenrod":
                    return Colors.DarkGoldenrod;

                case "System.Windows.Media.Color DarkGray":
                    return Colors.DarkGray;

                case "System.Windows.Media.Color DarkGreen":
                    return Colors.DarkGreen;

                case "System.Windows.Media.Color DarkKhaki":
                    return Colors.DarkKhaki;

                case "System.Windows.Media.Color DarkMagenta":
                    return Colors.DarkMagenta;

                case "System.Windows.Media.Color DarkOliveGreen":
                    return Colors.DarkOliveGreen;

                case "System.Windows.Media.Color DarkOrange":
                    return Colors.DarkOrange;

                case "System.Windows.Media.Color DarkOrchid":
                    return Colors.DarkOrchid;

                case "System.Windows.Media.Color DarkRed":
                    return Colors.DarkRed;

                case "System.Windows.Media.Color DarkSalmon":
                    return Colors.DarkSalmon;

                case "System.Windows.Media.Color DarkSeaGreen":
                    return Colors.DarkSeaGreen;

                case "System.Windows.Media.Color DarkSlateBlue":
                    return Colors.DarkSlateBlue;

                case "System.Windows.Media.Color DarkSlateGray":
                    return Colors.DarkSlateGray;

                case "System.Windows.Media.Color DarkTurquoise":
                    return Colors.DarkTurquoise;

                case "System.Windows.Media.Color DarkViolet":
                    return Colors.DarkViolet;

                case "System.Windows.Media.Color DeepPink":
                    return Colors.DeepPink;

                case "System.Windows.Media.Color DeepSkyBlue":
                    return Colors.DeepSkyBlue;

                case "System.Windows.Media.Color DimGray":
                    return Colors.DimGray;

                case "System.Windows.Media.Color DodgerBlue":
                    return Colors.DodgerBlue;

                case "System.Windows.Media.Color Firebrick":
                    return Colors.Firebrick;

                case "System.Windows.Media.Color FloralWhite":
                    return Colors.FloralWhite;

                case "System.Windows.Media.Color ForestGreen":
                    return Colors.ForestGreen;

                case "System.Windows.Media.Color Fuchsia":
                    return Colors.Fuchsia;

                case "System.Windows.Media.Color Gainsboro":
                    return Colors.Gainsboro;

                case "System.Windows.Media.Color GhostWhite":
                    return Colors.GhostWhite;

                case "System.Windows.Media.Color Gold":
                    return Colors.Gold;

                case "System.Windows.Media.Color Goldenrod":
                    return Colors.Goldenrod;

                case "System.Windows.Media.Color Gray":
                    return Colors.Gray;

                case "System.Windows.Media.Color Green":
                    return Colors.Green;

                case "System.Windows.Media.Color GreenYellow":
                    return Colors.GreenYellow;

                case "System.Windows.Media.Color Honeydew":
                    return Colors.Honeydew;

                case "System.Windows.Media.Color HotPink":
                    return Colors.HotPink;

                case "System.Windows.Media.Color IndianRed":
                    return Colors.IndianRed;

                case "System.Windows.Media.Color Indigo":
                    return Colors.Indigo;

                case "System.Windows.Media.Color Ivory":
                    return Colors.Ivory;

                case "System.Windows.Media.Color Khaki":
                    return Colors.Khaki;

                case "System.Windows.Media.Color LavenderBlush":
                    return Colors.LavenderBlush;

                case "System.Windows.Media.Color LawnGreen":
                    return Colors.LawnGreen;

                case "System.Windows.Media.Color LemonChiffon":
                    return Colors.LemonChiffon;

                case "System.Windows.Media.Color LightBlue":
                    return Colors.LightBlue;

                case "System.Windows.Media.Color LightCoral":
                    return Colors.LightCoral;

                case "System.Windows.Media.Color LightCyan":
                    return Colors.LightCyan;

                case "System.Windows.Media.Color LightGoldenrodYellow":
                    return Colors.LightGoldenrodYellow;

                case "System.Windows.Media.Color LightGray":
                    return Colors.LightGray;

                case "System.Windows.Media.Color LightGreen":
                    return Colors.LightGreen;

                case "System.Windows.Media.Color LightPink":
                    return Colors.LightPink;

                case "System.Windows.Media.Color LightSalmon":
                    return Colors.LightSalmon;

                case "System.Windows.Media.Color LightSeaGreen":
                    return Colors.LightSeaGreen;

                case "System.Windows.Media.Color LightSkyBlue":
                    return Colors.LightSkyBlue;

                case "System.Windows.Media.Color LightSlateGray":
                    return Colors.LightSlateGray;

                case "System.Windows.Media.Color LightSteelBlue":
                    return Colors.LightSteelBlue;

                case "System.Windows.Media.Color LightYellow":
                    return Colors.LightYellow;

                case "System.Windows.Media.Color Lime":
                    return Colors.Lime;

                case "System.Windows.Media.Color LimeGreen":
                    return Colors.LimeGreen;

                case "System.Windows.Media.Color Linen":
                    return Colors.Linen;

                case "System.Windows.Media.Color Magenta":
                    return Colors.Magenta;

                case "System.Windows.Media.Color Maroon":
                    return Colors.Maroon;

                case "System.Windows.Media.Color MediumAquamarine":
                    return Colors.MediumAquamarine;

                case "System.Windows.Media.Color MediumBlue":
                    return Colors.MediumBlue;

                case "System.Windows.Media.Color MediumOrchid":
                    return Colors.MediumOrchid;

                case "System.Windows.Media.Color MediumPurple":
                    return Colors.MediumPurple;

                case "System.Windows.Media.Color MediumSeaGreen":
                    return Colors.MediumSeaGreen;

                case "System.Windows.Media.Color MediumSlateBlue":
                    return Colors.MediumSlateBlue;

                case "System.Windows.Media.Color MediumSpringGreen":
                    return Colors.MediumSpringGreen;

                case "System.Windows.Media.Color MediumTurquoise":
                    return Colors.MediumTurquoise;

                case "System.Windows.Media.Color MediumVioletRed":
                    return Colors.MediumVioletRed;

                case "System.Windows.Media.Color MidnightBlue":
                    return Colors.MidnightBlue;

                case "System.Windows.Media.Color MintCream":
                    return Colors.MintCream;

                case "System.Windows.Media.Color MistyRose":
                    return Colors.MistyRose;

                case "System.Windows.Media.Color Moccasin":
                    return Colors.Moccasin;

                case "System.Windows.Media.Color NavajoWhite":
                    return Colors.NavajoWhite;

                case "System.Windows.Media.Color Navy":
                    return Colors.Navy;

                case "System.Windows.Media.Color OldLace":
                    return Colors.OldLace;

                case "System.Windows.Media.Color Olive":
                    return Colors.Olive;

                case "System.Windows.Media.Color OliveDrab":
                    return Colors.OliveDrab;

                case "System.Windows.Media.Color Orange":
                    return Colors.Orange;

                case "System.Windows.Media.Color OrangeRed":
                    return Colors.OrangeRed;

                case "System.Windows.Media.Color Orchid":
                    return Colors.Orchid;

                case "System.Windows.Media.Color PaleGoldenrod":
                    return Colors.PaleGoldenrod;

                case "System.Windows.Media.Color PaleGreen":
                    return Colors.PaleGreen;

                case "System.Windows.Media.Color PaleTurquoise":
                    return Colors.PaleTurquoise;

                case "System.Windows.Media.Color PaleVioletRed":
                    return Colors.PaleVioletRed;

                case "System.Windows.Media.Color PapayaWhip":
                    return Colors.PapayaWhip;

                case "System.Windows.Media.Color PeachPuff":
                    return Colors.PeachPuff;

                case "System.Windows.Media.Color Peru":
                    return Colors.Peru;

                case "System.Windows.Media.Color Pink":
                    return Colors.Pink;

                case "System.Windows.Media.Color Plum":
                    return Colors.Plum;

                case "System.Windows.Media.Color PowderBlue":
                    return Colors.PowderBlue;

                case "System.Windows.Media.Color Purple":
                    return Colors.Purple;

                case "System.Windows.Media.Color Red":
                    return Colors.Red;

                case "System.Windows.Media.Color RosyBrown":
                    return Colors.RosyBrown;

                case "System.Windows.Media.Color RoyalBlue":
                    return Colors.RoyalBlue;

                case "System.Windows.Media.Color SaddleBrown":
                    return Colors.SaddleBrown;

                case "System.Windows.Media.Color Salmon":
                    return Colors.Salmon;

                case "System.Windows.Media.Color SandyBrown":
                    return Colors.SandyBrown;

                case "System.Windows.Media.Color SeaGreen":
                    return Colors.SeaGreen;

                case "System.Windows.Media.Color SeaShell":
                    return Colors.SeaShell;

                case "System.Windows.Media.Color Sienna":
                    return Colors.Sienna;

                case "System.Windows.Media.Color Silver":
                    return Colors.Silver;

                case "System.Windows.Media.Color SkyBlue":
                    return Colors.SkyBlue;

                case "System.Windows.Media.Color SlateBlue":
                    return Colors.SlateBlue;

                case "System.Windows.Media.Color SlateGray":
                    return Colors.SlateGray;

                case "System.Windows.Media.Color Snow":
                    return Colors.Snow;

                case "System.Windows.Media.Color SpringGreen":
                    return Colors.SpringGreen;

                case "System.Windows.Media.Color SteelBlue":
                    return Colors.SteelBlue;

                case "System.Windows.Media.Color Tan":
                    return Colors.Tan;

                case "System.Windows.Media.Color Teal":
                    return Colors.Teal;

                case "System.Windows.Media.Color Tomato":
                    return Colors.Tomato;

                case "System.Windows.Media.Color Turquoise":
                    return Colors.Turquoise;

                case "System.Windows.Media.Color Wheat":
                    return Colors.Wheat;

                case "System.Windows.Media.Color White":
                    return Colors.White;

                case "System.Windows.Media.Color WhiteSmoke":
                    return Colors.WhiteSmoke;

                case "System.Windows.Media.Color Yellow":
                    return Colors.Yellow;

                case "System.Windows.Media.Color YellowGreen":
                    return Colors.YellowGreen;

                default:
                    return Colors.Transparent;
            }
        }
    }
}

