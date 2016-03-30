using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MyPaint.Actions;
using MyPaint.GraphicElement.Shapes;
using MyPaint.Layer;
using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Win32;
using Action = MyPaint.Actions.Action;
using MyPaint.GraphicElement;


namespace MyPaint
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindow;
        private Pattern window2 = new Pattern();
        private GradientBrush window = new GradientBrush();
        private Graphic newGraphic;
     
        private int curLayerIndex;
        private LayerManager layerSystem;
        private Graphic curGraphic;
        private RenderTargetBitmap buffer;
        private DrawingVisual drawingVisual = new DrawingVisual();
        private Color curOutLineColor;
        private ActionType curActionType;
        private Action curAction;
        private System.Windows.Media.DashStyle curDashStyle; //không xóa cái này
        private List<Action> actionList;
        private List<Action> undoActionList;
        private int curOutlineWidth = 0; //không xóa cái này
        private string filePath;
        private bool isChange;
        private CustomTextbox.CustomTextbox cTextbox;
        private LayerDisplay layerDisplay;

        public Slider GetSlider()
        {
            return SliderOpacity;
        }

        public RadioButton GetRadioButton(BrushType brushType)
        {
            switch (brushType)
            {
                case BrushType.LinearGradient:
                    return RadioBtn_Gradient;
                case BrushType.Solid:
                    return RadioBtn_Solid;
                case BrushType.Pattern:
                    return radioBtnPattern;

            }

            return null;
        }
        public void UnselectLayer()
        {
            if (layerDisplay != null)
            {
                layerDisplay.Unselect();
            }
        }

        public void Refresh()
        {
            DrawStuff();
        }

        public void ShowLayer(LayerDisplay layerDisplay)
        {
            int index = FindLayerIndex(layerDisplay);

            layerSystem.GetLayer(index).Visibility = true;
            Refresh();
        }

        public void HideLayer(LayerDisplay layerDisplay)
        {
            int index = FindLayerIndex(layerDisplay);

            layerSystem.GetLayer(index).Visibility = false;
            Refresh();
        }

        public void MergeUp(LayerDisplay layerDisplay)
        {
            curGraphic = null;

            int index = FindLayerIndex(layerDisplay);

            if (index != 0)
            {
                layerSystem.MergeLayer(index - 1, index);
                if (curLayerIndex == index)
                {
                    curLayerIndex = index - 1;
                    layerSystem.GetLayerDisplay(curLayerIndex).Select();
                }
                DeleteLayer(layerDisplay);
            }

            UpdateLayerImage();
        }

        public void MergeDown(LayerDisplay layerDisplay)
        {
            curGraphic = null;

            int index = FindLayerIndex(layerDisplay);

            if (index != layerSystem.LayerDisplays.Count)
            {
                layerSystem.MergeLayer(index, index + 1);
                if (curLayerIndex == index + 1)
                {
                    curLayerIndex = index;
                    layerSystem.GetLayerDisplay(curLayerIndex).Select();
                }
                DeleteLayer(Find(index + 1));
            }

            UpdateLayerImage();
        }

        private int FindLayerIndex(LayerDisplay layerDisplay)
        {
            return Grid.GetRow(layerDisplay.Get());
        }

        public void SelectLayer(LayerDisplay newLayerDisplay)
        {
            layerDisplay = newLayerDisplay;

            curLayerIndex = FindLayerIndex(newLayerDisplay);

            curGraphic = null;
            Refresh();
        }

        public void DeleteLayer(LayerDisplay layerDisplay)
        {
            int index = FindLayerIndex(layerDisplay);
            layerSystem.DeleteLayer(index);
            grdLayer.Children.RemoveAt(index);
            AdjustLayerPosition(index);

            if (grdLayer.Children.Count == 0)
            {
                NewLayer();
            }
            SelectFirstLayer();

            Refresh();
        }

        private LayerDisplay Find(int index)
        {
            foreach (LayerDisplay layerDisplay in layerSystem.LayerDisplays)
                if (Grid.GetRow(layerDisplay.Get()) == index)
                {
                    return layerDisplay;
                }
            return null;
        }

        public void SelectFirstLayer()
        {
            layerDisplay = layerSystem.GetLayerDisplay(0);

            layerDisplay.Select();
        }

        public void MoveUpLayer(LayerDisplay layerDisplay)
        {
            int index = FindLayerIndex(layerDisplay);
            if (layerSystem.MoveUp(index))
            {
                UIElement uiElement1 = Find(index).Get();
                UIElement uiElement2 = Find(index - 1).Get();
                Grid.SetRow(uiElement1, index - 1);
                Grid.SetRow(uiElement2, index);
            }


            if (curLayerIndex == index)
            {
                curLayerIndex -= 1;
            }

            Refresh();
        }

        public void MoveDownLayer(LayerDisplay layerDisplay)
        {
            int index = FindLayerIndex(layerDisplay);
            if (layerSystem.MoveDown(index))
            {
                UIElement uiElement1 = Find(index).Get();
                UIElement uiElement2 = Find(index + 1).Get();
                Grid.SetRow(uiElement1, index + 1);
                Grid.SetRow(uiElement2, index);
            }


            if (curLayerIndex == index)
            {
                curLayerIndex += 1;
            }

            Refresh();
        }

        private void AdjustLayerPosition(int layerIndex)
        {
            for (int i = layerIndex; i < grdLayer.Children.Count; i++)
            {
                DockPanel dockPanel = (DockPanel)grdLayer.Children[i];

                Grid.SetRow(dockPanel, i);
            }
        }

        private void AddNewAction(Action newAction)
        {
            actionList.Add(newAction);
            UpdateLayerImage();
            undoButton.IsEnabled = true;
            redoButton.IsEnabled = false;
            
        }

        private void UpdateLayerImage()
        {
            ClearStuff();
            if (buffer == null)
                return;

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                Layer.Layer layer = layerSystem.GetLayer(curLayerIndex);
                for (int i = 0; i < layer.CountGraphics(); i++)
                {
                    Graphic gp = layer.GetGraphic(i);
                    gp.Draw(drawingContext);
                }
            }
            buffer.Render(drawingVisual);
            layerDisplay.Change(buffer);

            DrawStuff();
        }

        private List<string> CreateFontSizeList()
        {
            List<string> FontSizeList = new List<string>();
            int i = 0;

            for (i = 8; i <= 12; i++)
                FontSizeList.Add(i.ToString());

            i = 12;
            while (i < 28)
            {
                i += 2;
                FontSizeList.Add(i.ToString());


            }

            FontSizeList.Add("36");

            i = 36;
            int j = 1;
            while (i < 72)
            {

                FontSizeList.Add((i + (12 * j)).ToString());
                i = i + (12 * j);
                j++;

            }

            return FontSizeList;


        }
        public MainWindow()
        {
            mainWindow = this;
            Init();
        }

        private void New()
        {
            layerSystem = new LayerManager();
            actionList = new List<Action>();
            undoActionList = new List<Action>();
            grdLayer.Children.Clear();
            curLayerIndex = 0;
            redoButton.IsEnabled = false;
            selectRibbonButton.IsChecked = false;
            ButtonRotate.IsChecked = false;
            curGraphic = null;
            RadioBtn_Solid.IsChecked = true;
            RadioBtn_SolidPenIcon.IsChecked = true;
            curAction = null;
            filePath = null;
            isChange = false;
            curOutLineColor = Colors.Black;
            ClearStuff();
            undoButton.IsEnabled = false;
            CbBox_Shapes.SelectedItem = ShapeType.None;
            curActionType = ActionType.None;
        }

        private void Init()
        {
            InitializeComponent();
            New();
            NewLayer();

            
            UpdateLayerImage();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            buffer = new RenderTargetBitmap((int)Background.Width, (int)Background.Height, 96, 96, PixelFormats.Pbgra32);
            Background.Source = buffer;
            UpdateLayerImage();

            DrawStuff();
            
        }

        private void ClearStuff()
        {
            if (buffer == null)
                return;

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(new SolidColorBrush(Colors.White), null, new Rect(0, 0, Background.Width, Background.Height));
            }

            buffer.Render(drawingVisual);
        }

        private void DrawStuff()
        {
            ClearStuff();
            if (buffer == null)
                return;

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                if (layerSystem.CountLayer() > 0)
                {
                    foreach (Layer.Layer layer in layerSystem.ListOfLayer)
                        if (layer.Visibility == true)
                        {
                            for (int i = 0; i < layer.CountGraphics(); i++)
                            {
                                Graphic gp = layer.GetGraphic(i);
                                if (gp == curGraphic)
                                {
                                    gp.DrawRect(drawingContext);
                                }
                                gp.Draw(drawingContext);
                            }
                        }
                }
            }

            buffer.Render(drawingVisual);
        }

        private void DisableMenuItem()
        {
            deleteMenuItem.IsEnabled = false;
            copyMenuItem.IsEnabled = false;
            cutMenuItem.IsEnabled = false;
        }

        private void EnableMenuItem()
        {
            deleteMenuItem.IsEnabled = true;
            copyMenuItem.IsEnabled = true;
            cutMenuItem.IsEnabled = true;
        }

        private void Background_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                switch (curActionType)
                {
                    case ActionType.Drawing:
                        curAction = new Actions.Drawing(layerSystem.GetLayer(curLayerIndex),
                            (ShapeType) CbBox_Shapes.SelectedItem, CurrentBrush.Brush, curOutLineColor,
                            curOutlineWidth, curDashStyle, e.GetPosition(Background));
                        curGraphic = curAction.GetGraphic();
                        break;
                    case ActionType.Selecting:
                        curAction = new Selecting(layerSystem.GetLayer(curLayerIndex), e.GetPosition(Background));
                        curGraphic = curAction.GetGraphic();
                        curAction = null;
                        if (curGraphic != null)
                        {
                            curActionType = ActionType.Selected;
                            EnableMenuItem();
                        }
                        break;
                    case ActionType.Selected:
                        if (curGraphic.Intersect(e.GetPosition(Background)))
                        {
                            curAction = new Moving(curGraphic, e.GetPosition(Background));
                        }
                        else
                            if (curGraphic.IntersectWithAnchor(e.GetPosition(Background)) != AnchorType.None)
                        {
                            curAction = new Resizing(curGraphic, e.GetPosition(Background));
                        }
                        else
                        {
                            curActionType = ActionType.Selecting;
                            selectRibbonButton_Checked(sender, e);
                        }
                        break;
                    case ActionType.Rotating:
                        if (curGraphic != null)
                            curAction = new Rotating(curGraphic, e.GetPosition(Background));
                        break;

                    case ActionType.DrawingText:
                        cTextbox = new CustomTextbox.CustomTextbox(e.GetPosition(Background));
                        cTextbox.showTextbox(mainGrid, e.GetPosition(mainGrid));
                        cTextbox.changeFont(cbFonts.SelectedItem.ToString());
                        cTextbox.changeFontSize(cbFontSize.SelectedItem.ToString());
                        using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                        {
                            cTextbox.DrawRect(drawingContext);
                        }
                        buffer.Render(drawingVisual);
                        curActionType = ActionType.Editting;
                        break;
                    case ActionType.Editting:
                        btnConfirm_Click(sender, e);
                        break;
                }
        }

        private void Background_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DrawStuff();
                if (curAction != null)
                {
                    curAction.Start(e.GetPosition(Background));
                }
            }
        }

        private void Background_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (curAction != null && curAction.GetGraphic() != null)
            {
                AddNewAction(curAction);
                undoActionList.Clear();
                isChange = true;
            }
            DrawStuff();
        }

        //Bùi Phạm Thiên Thư
        private void CbBoxShapes_LoadData()
        {
            List<ShapeType> shapTypeList = new List<ShapeType>();
            for (int i = 0; i < Enum.GetNames(typeof(ShapeType)).Length; i++)
            {
                ShapeType st = (ShapeType)i;
                shapTypeList.Add(st);
            }

            CbBox_Shapes.DataContext = shapTypeList;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentBrush.Brush = new SolidColorBrush();
            CbBoxShapes_LoadData();
            RibbonShapesBtn.Foreground = Brushes.Black;
            curOutlineWidth = (int)(double.Parse(SliderWidthOutline.Value.ToString()));
            CurrentBrush.Brush.Opacity = SliderOpacity.Value / 100;
        }

        private void CbBox_Shapes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                ImageSource imageSource = Factory.GetImageSource_ShapeType(CbBox_Shapes.SelectedItem.ToString());
                if (imageSource != null)
                    RibbonShapesBtn.LargeImageSource = imageSource;
                curActionType = ActionType.Drawing;
            }

            catch (Exception)
            {


            }
        }

        private void selectRibbonButton_Checked(object sender, RoutedEventArgs e)
        {
            if (ButtonRotate.IsChecked == true)
                curActionType = ActionType.Rotating;
           
            else
                curActionType = ActionType.Selecting;

            curAction = null;
            curGraphic = null;
            DisableMenuItem();
            CbBox_Shapes.IsEnabled = false;
            ButtonRotate.IsEnabled = true;
            DrawStuff();
        }

        private void selectRibbonButton_Unchecked(object sender, RoutedEventArgs e)
        {
            curGraphic = null;
            DrawStuff();
            CbBox_Shapes.IsEnabled = true;                                
            ButtonRotate.IsEnabled = false;
            buttonRotate_Unchecked(sender, e);
            curActionType = ActionType.Drawing;
        }

        private void listBox_Colors_MouseLeftBtnUp(object sender, MouseButtonEventArgs e)
        {

            RadioBtn_Solid.IsChecked = true;
            try
            {
                System.Drawing.Color selectedColor =
                    System.Drawing.Color.FromName(listBox_Colors.SelectedValue.ToString());

                Color color = Factory.GetColors(selectedColor.Name);

                if (!color.Equals(((SolidColorBrush)(ellipse_Color1.Fill)).Color))
                {
                    ellipse_Color1.Fill = new SolidColorBrush(color);
                    curOutLineColor = ((SolidColorBrush)(ellipse_Color1.Fill)).Color;
                }


                if (curGraphic != null)
                {
                    AddNewAction(new OutlineChanging(curGraphic, color));
                    DrawStuff();
                }
            }

            catch (Exception)
            {

            }
        }

        private void listBox_Colors_MouseRightBtnUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            RadioBtn_Solid.IsChecked = true;
            try
            {
                System.Drawing.Color selectedColor =
                    System.Drawing.Color.FromName(listBox_Colors.SelectedValue.ToString());

                Color color = Factory.GetColors(selectedColor.Name);

                if (!color.Equals(((SolidColorBrush)(ellipse_Color2.Fill)).Color))
                {
                    ellipse_Color2.Fill = new SolidColorBrush(color);
                    CurrentBrush.Brush = new SolidColorBrush(color);
                }

                if (CurrentBrush.Brush.Opacity != SliderOpacity.Value / 100)
                    CurrentBrush.Brush.Opacity = SliderOpacity.Value / 100;

                if (curGraphic != null)
                {
                    AddNewAction(new BackgroundChanging(curGraphic, CurrentBrush.Brush));
                    DrawStuff();
                }

                RadioBtn_Solid.IsChecked = true;
            }

            catch (Exception)
            {

            }
        }

        private void SliderOpacity_PreLeftBtnUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                CurrentBrush.Brush = Factory.GetBrush(CurrentBrush.CurBrushType);
                switch (CurrentBrush.CurBrushType)
                {
                    case BrushType.LinearGradient:
                        CurrentBrush.Brush = new LinearGradientBrush(window.colors);

                        break;

                    case BrushType.Solid:
                        CurrentBrush.Brush = new SolidColorBrush(((SolidColorBrush)(ellipse_Color2.Fill)).Color);
                        break;

                    case BrushType.Pattern:
                        CurrentBrush.Brush = window2.tempBrush;
                        break;

                }

                CurrentBrush.Brush.Opacity = SliderOpacity.Value / 100;

                if (curGraphic != null)
                {
                    AddNewAction(new BackgroundChanging(curGraphic, CurrentBrush.Brush));
                    DrawStuff();
                }
            }
            catch (Exception)
            {

            }



        }

        private void RadioBtn_SolidPenIcon_Checked(object sender, RoutedEventArgs e)
        {
            curDashStyle = Factory.GetDashStyle(MyPaint.DashStyle.Solid);
            if (curGraphic != null)
            {
                if (curGraphic.ChangeOutlineType(curDashStyle) == true)
                    DrawStuff();
            }
        }

        private void RadioBtn_DashPenIcon_Checked(object sender, RoutedEventArgs e)
        {
            curDashStyle = Factory.GetDashStyle(MyPaint.DashStyle.Dash);

            if (curGraphic != null)
            {
                if (curGraphic.ChangeOutlineType(curDashStyle))
                    DrawStuff();
            }
        }

        private void RadioBtn_DotPenIcon_Checked(object sender, RoutedEventArgs e)
        {
            curDashStyle = Factory.GetDashStyle(MyPaint.DashStyle.Dot);
            if (curGraphic != null)
            {
                if (curGraphic.ChangeOutlineType(curDashStyle) == true)
                    DrawStuff();
            }
        }

        private void RadioBtn_DashDotDotPenIcon_Checked(object sender, RoutedEventArgs e)
        {
            curDashStyle = Factory.GetDashStyle(MyPaint.DashStyle.DashDotDot);
            if (curGraphic != null)
            {
                if (curGraphic.ChangeOutlineType(curDashStyle))
                    DrawStuff();
            }
        }

        private void RadioBtn_DashtDotPenIcon_Checked(object sender, RoutedEventArgs e)
        {
            curDashStyle = Factory.GetDashStyle(MyPaint.DashStyle.DashDot);
            if (curGraphic != null)
            {
                if (curGraphic.ChangeOutlineType(curDashStyle))
                    DrawStuff();
            }
        }

        private void SliderWidthOutline_PreMouseLeftBtnUp(object sender, MouseButtonEventArgs e)
        {
            curOutlineWidth = (int)(double.Parse(SliderWidthOutline.Value.ToString()));
            if (curGraphic != null)
            {
                if (curGraphic.ChangeOutlineWidth(curOutlineWidth) == true)
                    DrawStuff();
            }
        }

        private void MainWindow_PreKeyDwn(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (curActionType == ActionType.Selecting && curGraphic == null)
                {
                    foreach (Layer.Layer layer in layerSystem.ListOfLayer)
                    {
                        while (layer.CountGraphics() != 0)
                        {
                            Graphic gp = layer.GetGraphic(0);
                            curGraphic = gp;
                            deleteMenuItem_Click(sender, e);
                        }
                    }

                    if (CbBox_Shapes.IsEnabled == true)
                    {
                        curActionType = ActionType.Drawing;
                    }
                }

                else
                {
                    deleteMenuItem_Click(sender, e);
                    CbBox_Shapes.IsEnabled = true;
                    CbBox_Shapes.IsEditable = false;
                    CbBox_Shapes.IsEditable = true;
                    ButtonRotate.IsEnabled = false;
                    curActionType = ActionType.Drawing;
                }
            }

            //Select all shortcut key
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || (Keyboard.IsKeyDown(Key.RightCtrl))) && (Keyboard.IsKeyDown(Key.A)))
            {
                curActionType = ActionType.Selecting;
                selectRibbonButton.IsChecked = true;
                curGraphic = null;
                if (layerSystem.CountLayer() > 0)
                {
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {

                        foreach (Layer.Layer layer in layerSystem.ListOfLayer)
                        {
                            for (int i = 0; i < layer.CountGraphics(); i++)
                            {
                                Graphic gp = layer.GetGraphic(i);
                                gp.DrawRect(drawingContext);
                            }
                        }


                    }
                    buffer.Render(drawingVisual);
                }
            }

            //Save shortcut key
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
            {
                saveButton_Click(sender, e);
            }

            //Open shorcut key
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.O))
            {
                openButton_Click(sender, e);
            }

            //New shorcut key
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.N))
            {
                newButton_Click(sender, e);
            }

            //Save as shorcut key
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.S))
            {
                saveAsButton_Click(sender, e);
            }

            //Undo shortcut key
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.Z) && undoButton.IsEnabled)
            {
                undoButton_Click(sender, e);
            }
            
            //Redo shortcut key
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.Y) && redoButton.IsEnabled)
            {
                redoButton_Click(sender, e);
            }
        }

        private void deleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddNewAction(new Deleting(curGraphic, layerSystem.GetLayer(curLayerIndex)));
            selectRibbonButton_Checked(sender, e);
        }

        private void buttonRotate_Unchecked(object sender, RoutedEventArgs e)
        {
            if (curGraphic != null)
                curActionType = ActionType.Selected;
            else
                curActionType = ActionType.Selecting;
        }

        private void buttonRotate_Checked(object sender, RoutedEventArgs e)
        {
            curActionType = ActionType.Rotating;
        }

        private void CbBox_Shapes_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                CbBox_Shapes.Text = CbBox_Shapes.SelectedItem.ToString();
            }
            catch (Exception)
            {


            }


        }

        private void CbBox_Shapes_PreTextInput(object sender, TextCompositionEventArgs e)
        {
            CbBox_Shapes.IsDropDownOpen = true;
        }

        private void RadioBtn_Gradient_Checked(object sender, RoutedEventArgs e)
        {
            if (CurrentBrush.ChangeBrush == true)
                window.Show();

            CurrentBrush.ChangeBrush = true;

        }

        private void RadioBtn_Solid_Checked(object sender, RoutedEventArgs e)
        {
            CurrentBrush.CurBrushType = BrushType.Solid;
            CurrentBrush.Brush = new SolidColorBrush(((SolidColorBrush)(ellipse_Color2.Fill)).Color);
            CurrentBrush.ChangeBrush = true;
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            Action lastAction = actionList[actionList.Count - 1];
            actionList.Remove(lastAction);
            undoActionList.Add(lastAction);

            lastAction.Undo();

            if (actionList.Count == 0)
            {
                undoButton.IsEnabled = false;
                curGraphic = null;
                selectRibbonButton_Unchecked(sender, e);
            }
            else
            {
                Action prevLastAction = actionList[actionList.Count - 1];
                curGraphic = prevLastAction.GetGraphic();
            }
            DrawStuff();

            redoButton.IsEnabled = true;
            isChange = true;

            UpdateLayerImage();

        }

        private void redoButton_Click(object sender, RoutedEventArgs e)
        {
            Action lastUndoAction = undoActionList[undoActionList.Count - 1];
            undoActionList.Remove(lastUndoAction);
            actionList.Add(lastUndoAction);

            lastUndoAction.Redo();
            curGraphic = lastUndoAction.GetGraphic();

            DrawStuff();

            undoButton.IsEnabled = true;

            if (undoActionList.Count == 0)
            {
                redoButton.IsEnabled = false;
            }

            isChange = true;

            UpdateLayerImage();
        }

        //Hide AuxiliaryPane of Ribbon ApplicationMenu 
        //Source: http://stackoverflow.com/questions/5578855/wpf-ribbon-can-we-hide-auxiliarypane-of-ribbon-applicationmenu
        private void RibbonApplicationMenu_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = (ribbonApplicationMenu.Template.FindName("MainPaneBorder", ribbonApplicationMenu) as Border).Parent as Grid;
            grid.ColumnDefinitions[2].Width = new GridLength(0);
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            if (isChange)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you want to save", "Open",
                    System.Windows.MessageBoxButton.YesNoCancel);
                if (messageBoxResult == MessageBoxResult.No)
                {
                    Init();
                }

                else if ((messageBoxResult == MessageBoxResult.Yes))
                {
                    saveButton_Click(sender, e);
                    Init();
                }
            }

            else
            {
                Init();
            }
        }

        private void Open()
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Xml File (*.xml)|*.xml|PNG File|*.png";

            if (dlg.ShowDialog() == true)
            {
                if (dlg.FilterIndex == 1)
                {
                    New();
                    layerSystem = LayerManager.Load(dlg.FileName);
                    for (int i = 0; i < layerSystem.CountLayer(); i++)
                    {
                        layerDisplay = layerSystem.GetLayerDisplay(i);
                        grdLayer.Children.Add(layerDisplay.Get());
                        curLayerIndex = i;

                        RowDefinition rowDefinition = new RowDefinition();
                        rowDefinition.Height = new GridLength(LayerDisplay.distance);
                        grdLayer.RowDefinitions.Add(rowDefinition);
                        Grid.SetRow(layerDisplay.Get(), i);

                        UpdateLayerImage();
                    }
                }
                else
                {
                    AddNewAction(new Actions.Drawing(layerSystem.GetLayer(curLayerIndex),
                        new GraphicElement.Image(dlg.FileName)));
                }
            }
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            if (isChange)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you want to save", "Open",
                    System.Windows.MessageBoxButton.YesNoCancel);
                if (messageBoxResult == MessageBoxResult.No)
                {
                    Open();
                }

                else if ((messageBoxResult == MessageBoxResult.Yes))
                {
                    saveButton_Click(sender, e);
                    Open();
                }
            }
            else
            {
                Open();
            }
        }

        private void Save(string fileName, int index)
        {
            if (index == 1)
            {
                System.IO.File.WriteAllText(fileName, "");
                layerSystem.Save(fileName);
            }
            else
            {
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(buffer));

                using (var fs = System.IO.File.OpenWrite(fileName))
                {
                    pngEncoder.Save(fs);
                }
            }
        }

        private void saveAsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Xml File (*.xml) | *.xml|PNG File (*.png)|*.png";

            if (dlg.ShowDialog() == true)
            {
                Save(dlg.FileName, dlg.FilterIndex);
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (filePath == null)
            {
                saveAsButton_Click(sender, e);
            }
            else
            {
                Save(filePath, 1);
            }
        }

        //Avoid program run background
        private void MainWindow1_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void textRibbonButton_Checked(object sender, RoutedEventArgs e)
        {
            curActionType = ActionType.DrawingText;
            selectRibbonButton.IsChecked = false;
            textDesign.Visibility = Visibility.Visible;
            mainRibbon.SelectedIndex = 1;

        }

        private void textRibbonButton_Unchecked(object sender, RoutedEventArgs e)
        {
            curActionType = ActionType.None;
            mainRibbon.SelectedIndex = 0;
            textDesign.Visibility = Visibility.Hidden;
        }

        private void cbFontSize_Loaded(object sender, RoutedEventArgs e)
        {

            ((ComboBox)(sender)).DataContext = CreateFontSizeList();
        }

        private void MainWindow1_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (curActionType == ActionType.Editting)
                {
                    if (cTextbox.IsSizeChange == true)
                    {
                        DrawStuff();
                        using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                        {
                            cTextbox.DrawRect(drawingContext);
                        }
                    }
                    buffer.Render(drawingVisual);
                }

            }
            catch (Exception)
            { }
        }

        private void cbFonts_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (curActionType == ActionType.Editting || curActionType == ActionType.DrawingText)
                    cTextbox.changeFont(cbFonts.SelectedItem.ToString());
            }

            catch (Exception)
            { }
        }



        private void cbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (curActionType == ActionType.Editting || curActionType == ActionType.DrawingText)
                try
                {
                    cTextbox.changeFontSize(cbFontSize.SelectedItem.ToString());
                }
                catch (Exception)
                { }
        }


        private void listBox_Text_Colors_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try {
                System.Drawing.Color selectedColor =
                    System.Drawing.Color.FromName(listBox_Text_Colors.SelectedValue.ToString());

                Color color = Factory.GetColors(selectedColor.Name);
                if (curActionType == ActionType.DrawingText || curActionType == ActionType.Editting)
                    cTextbox.changeColor(color);
            }
            catch (Exception) { }
        }

        private void listBox_Text_Colors_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            try {
                e.Handled = true;
                System.Drawing.Color selectedColor =
                       System.Drawing.Color.FromName(listBox_Text_Colors.SelectedValue.ToString());

                Color color = Factory.GetColors(selectedColor.Name);
                if (curActionType == ActionType.DrawingText || curActionType == ActionType.Editting)
                {
                    if (rBtnTransparent.IsChecked == true)
                        cTextbox.changeBackground(true, color);
                    else
                        cTextbox.changeBackground(false, color);
                }
            }
            catch (Exception) { }

        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (curActionType == ActionType.Editting)
            try
            {
                    if (cTextbox.Tb.Text != "")
                    {
                        Text text = new Text(cTextbox.Tb.Text, cTextbox.Start, cTextbox.Tb.ActualWidth, cTextbox.Tb.ActualHeight, cTextbox.getTextColor(), cTextbox.getBackgroundColor(), cTextbox.getFontSize(), cTextbox.getFont());
                        AddNewAction(new Actions.Drawing(layerSystem.GetLayer(curLayerIndex), text));
                    }

                cTextbox.destroy(mainGrid);
                DrawStuff();
                curActionType = ActionType.None;
                mainRibbon.SelectedIndex = 0;
                textDesign.Visibility = Visibility.Hidden;

                textRibbonButton.IsChecked = false;
            }
            catch (Exception)
            { }
        }

        private void pasteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            selectRibbonButton.IsChecked = true;
            if (newGraphic != null)
            {
                curActionType = ActionType.Selected;
                curGraphic = (Graphic)newGraphic.Clone();
                
                AddNewAction(new Pasting(curGraphic, layerSystem.GetLayer(curLayerIndex)));

                EnableMenuItem();

                DrawStuff();
            }
        }



        private void copyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            pasteMenuItem.IsEnabled = true;
            if (curGraphic != null)
            {
                newGraphic = curGraphic;
            }
        }



        private void cutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            pasteMenuItem.IsEnabled = true;

            if (curGraphic != null)
            {
                newGraphic = (Graphic)curGraphic.Clone();
                deleteMenuItem_Click(sender, e);
            }
        }

        private void CbBox_Shapes_GotFocus(object sender, RoutedEventArgs e)
        {
            curGraphic = null;

            DrawStuff();
        }

       

        private void radioBtnPattern_Checked(object sender, RoutedEventArgs e)
        {
            CurrentBrush.Brush.Opacity = SliderOpacity.Value / 100;

            if (CurrentBrush.ChangeBrush == true)
            {
                window2.foreGround = ((SolidColorBrush) (ellipse_Color1.Fill)).Color;
                window2.backGround = ((SolidColorBrush) (ellipse_Color2.Fill)).Color;
                window2.Show();
            }

            CurrentBrush.ChangeBrush = true;

        }

        private void NewLayer()
        {
            layerSystem.NewLayer();
            curLayerIndex = layerSystem.CurrentLayer;
            layerDisplay = layerSystem.GetLayerDisplay(curLayerIndex);
            UpdateLayerImage();
            grdLayer.Children.Add(layerDisplay.Get());

            if (curLayerIndex > grdLayer.RowDefinitions.Count - 1)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(LayerDisplay.distance);
                grdLayer.RowDefinitions.Add(rowDefinition);
            }

            Grid.SetRow(layerDisplay.Get(), curLayerIndex);
        }

        private void newLayer_Click(object sender, RoutedEventArgs e)
        {
            NewLayer();
        }
    }
}
