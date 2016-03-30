using System.IO;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MyPaint.Layer
{
    [DataContract]
    public class LayerDisplay
    {
        [IgnoreDataMember] private const int height = 60;              //Height of Layer
        [IgnoreDataMember] private const int layerImageWidth = 120;
        [IgnoreDataMember] public static int distance = 80;            //Distance between two LayerDisplay
        [IgnoreDataMember] private DockPanel dockPanel;
        [IgnoreDataMember] private Image image;
        [IgnoreDataMember] private CheckBox checkBox;
        [DataMember] private string name;
        [DataMember]public bool IsSelected { get; set; }

        public LayerDisplay(Layer layer, string name)
        {
            this.name = name;
            
            CreateDockPanel();
            this.Select();
        }

        [OnDeserialized]
        private void Load(StreamingContext sc)
        {
            CreateDockPanel();
        }

        private Label CreateLabel()
        {
            Label label = new Label();
            label.Content = name;
            label.VerticalAlignment = VerticalAlignment.Center;
            return label;
        }

        private Image CreateImage()
        {
            this.image = new Image();
            this.image.Width = layerImageWidth;
            RenderOptions.SetBitmapScalingMode(this.image, BitmapScalingMode.HighQuality);
            return this.image;
        }

        private CheckBox CreateCheckBox()
        {
            checkBox = new CheckBox();
            checkBox.VerticalAlignment = VerticalAlignment.Center;
            checkBox.IsChecked = true;
            checkBox.Checked += Check;
            checkBox.Unchecked += Uncheck;
            return checkBox;
        }

        private DockPanel CreateDockPanel()
        {
            CreateImage();
            dockPanel = new DockPanel();
            dockPanel.Height = height;
            dockPanel.Children.Add(image);
            dockPanel.Children.Add(CreateLabel());
            dockPanel.Children.Add(CreateCheckBox());
            dockPanel.MouseDown += Select;
            dockPanel.ContextMenu = CreateContextMenu();
            dockPanel.VerticalAlignment = VerticalAlignment.Top;
            return dockPanel;
        }

        private ContextMenu CreateContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem deleteLayer = new MenuItem();
            deleteLayer.Header = "Delete Layer";
            deleteLayer.Click += DeleteLayer_Click;
            contextMenu.Items.Add(deleteLayer);

            contextMenu.Items.Add(new Separator());

            MenuItem moveUp = new MenuItem();
            moveUp.Header = "Move Up";
            moveUp.Click += MoveUp_Click;
            contextMenu.Items.Add(moveUp);

            MenuItem moveDown = new MenuItem();
            moveDown.Header = "Move Down";
            moveDown.Click += MoveDown_Click;
            contextMenu.Items.Add(moveDown);

            contextMenu.Items.Add(new Separator());

            MenuItem mergeUp = new MenuItem();
            mergeUp.Header = "Merge Up";
            mergeUp.Click += MergeUp_Click;
            contextMenu.Items.Add(mergeUp);

            MenuItem mergeDown = new MenuItem();
            mergeDown.Header = "Merge Down";
            mergeDown.Click += MergeDown_Click;
            contextMenu.Items.Add(mergeDown);

            return contextMenu;
        }

        private void MergeDown_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.MergeDown(this);
        }

        private void MergeUp_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.MergeUp(this);
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.MoveDownLayer(this);
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.MoveUpLayer(this);
        }

        private void DeleteLayer_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.DeleteLayer(this);
        }

        public void Select()
        {
            if (!IsSelected)
            {
                IsSelected = true;
                dockPanel.Background = new SolidColorBrush(Colors.LightSlateGray);
                MainWindow.mainWindow.UnselectLayer();
                MainWindow.mainWindow.SelectLayer(this);
            }
        }

        private void Uncheck(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.HideLayer(this);
        }

        private void Check(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.ShowLayer(this);
        }


        public DockPanel Get()
        {
            return dockPanel;
        }

        public void Change(RenderTargetBitmap newImage)
        {
            var bitmapImage = new BitmapImage();
            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(newImage));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }

            this.image.Source = bitmapImage;
        }

        private void Select(object sender, MouseButtonEventArgs e)
        {
            Select();
        }

        public void Unselect()
        {
            IsSelected = false;
            dockPanel.Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
