using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;

namespace MyPaint.Layer
{
    [DataContract]
    class LayerManager
    {
        [DataMember] private int currentLayer;

        [DataMember] private List<Layer> listLayer;

        [DataMember] private List<LayerDisplay> layerDisplays;

        

        [DataMember] private static int index = 0;

        public List<LayerDisplay> LayerDisplays { get { return layerDisplays; } }

        internal List<Layer> ListOfLayer
        {
            get
            {
                return listLayer;
            }
        }

        public int CurrentLayer
        {
            get
            {
                return currentLayer;
            }
        }

        public LayerManager()
        {
            currentLayer = -1;
            listLayer = new List<Layer>();
            layerDisplays = new List<LayerDisplay>();
        }

        public void DeleteLayer(int position)
        {
            ListOfLayer.RemoveAt(position);
            LayerDisplays.RemoveAt(position);
        }

       

        public void MergeLayer(int layer1, int layer2)
        {
            for (int i = 0; i < ListOfLayer[layer2].CountGraphics(); i++)
            {
                ListOfLayer[layer1].AddGraphic(ListOfLayer[layer2].GetGraphic(i));
            }
        }

        public void NewLayer()
        {
            Layer layer = new Layer();
            ListOfLayer.Add(layer);
            LayerDisplays.Add(new LayerDisplay(layer, "Layer " + index.ToString()));
            index++;
            currentLayer = ListOfLayer.Count - 1;
        }

        public void ChangeCurrentLayer(int current)
        {
            currentLayer = current;
        }

        public int CountLayer()
        {
            return ListOfLayer.Count;
        }

        public Layer GetLayer(int position)
        {
            return ListOfLayer[position];
        }

        public LayerDisplay GetLayerDisplay(int position)
        {
            return LayerDisplays[position];
        }

        public void Save(string filePath)
        {
            var ser = new DataContractSerializer(this.GetType());
            string objectInfo;

            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, this);
                objectInfo = Encoding.UTF8.GetString(ms.ToArray());
            }

            File.AppendAllText(filePath, objectInfo + "\n");
        }

        public static LayerManager Load(string fileName)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(LayerManager));
            FileStream fs = new FileStream(fileName, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());

            LayerManager games = (LayerManager)dcs.ReadObject(reader);
            reader.Close();
            fs.Close();
            return games;
        }

        private void Swap(int i, int j)
        {
            LayerDisplay temp1 = LayerDisplays[i];
            LayerDisplays[i] = LayerDisplays[j];
            LayerDisplays[j] = temp1;

            Layer temp2 = ListOfLayer[i];
            ListOfLayer[i] = ListOfLayer[j];
            ListOfLayer[j] = temp2;
        }

        public bool MoveDown(int position)
        {
            if (position != CountLayer() - 1)
            {
                Swap(position, position + 1);
                return true;
            }

            return false;
        }

        public bool MoveUp(int position)
        {
            if (position != 0)
            {
                Swap(position, position - 1);
                return true;
            }

            return false;
        }
    }
}
