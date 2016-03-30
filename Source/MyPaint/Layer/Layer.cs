using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MyPaint.GraphicElement;
using MyPaint.GraphicElement.Shapes;

namespace MyPaint.Layer
{
    [DataContract]
    [KnownType(typeof(Line))]
    [KnownType(typeof(Ellipse))]
    [KnownType(typeof(Rectangle))]
    [KnownType(typeof(Star))]
    [KnownType(typeof(Triangle))]
    [KnownType(typeof(Text))]
    [KnownType(typeof(Image))]

    public class Layer
    {
        [DataMember] private List<Graphic> listOfGraphic;

        [DataMember] public bool Visibility { get; set; }

        [DataMember] private int opacity;

        public Layer()
        {
            listOfGraphic = new List<Graphic>();
            Visibility = true;
            opacity = 100;
        }

        public void AddGraphic(Graphic graphic)
        {
            listOfGraphic.Add(graphic);
        }

        public int CountGraphics()
        {
            return listOfGraphic.Count;
        }

        public void DeleteGraphics(Graphic graphic)
        {
            listOfGraphic.Remove(graphic);
        }

        public void Hide()
        {
            Visibility = true;
        }

        public void UnHide()
        {
            Visibility = false;
        }

        public Graphic GetGraphic(int postion)
        {
            return listOfGraphic[postion];
        }
    }
}