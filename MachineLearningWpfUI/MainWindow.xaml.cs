using MachineLearingInterfaces;
using MachineLearning;
using MachineLearning.Biases;
using MachineLearning.Funcs;
using MachineLearning.Wages;
using MachineLearningWpfUI.Models;
using MachineLearningWpfUI.ViewModels;
using MindFusion.Diagramming.Wpf.Layout;
using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using MindFusion.Diagramming.Wpf.Fluent;
using System.Windows.Media;


namespace MachineLearningWpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ParameterViewModel Context { get; set; }

        public MainWindow()
        {
            var inputs = new MachineLearning.Layer(6, 1, LineralActivationFunc.Create());
            var hiddenfirsLayer = new MachineLearning.Layer(10, 2,  LineralActivationFunc.Create());
            var hiddenfirsLayer2 = new MachineLearning.Layer(10, 2, LineralActivationFunc.Create());
            var output = new MachineLearning.Layer(3, 3);

            var list = new List<ILayer>() { inputs, hiddenfirsLayer, hiddenfirsLayer2, output };

            var a= new Network(list, new InitXawierWages(), new InitZeroBiases());
            int thickness = 2;

            DataContext = Context = new ParameterViewModel(a,this);
            InitializeComponent();

            Rect longNodeRect = new Rect(10, 10, 140, 40);
            Rect roundNodeRect = new Rect(130, 10, 50, 50);

            string[] layerLabels = { "Input Layer", "Hidden Layers", "Output Layer" };
            //brushes for styling
            SolidColorBrush bananaBrush  = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFff"));
            SolidColorBrush skyBlueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#66bbff"));
            SolidColorBrush fourBlueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#443399dd"));
            SolidColorBrush aaBlueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#aa3399dd"));
            SolidColorBrush oneBlueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0055aaff"));

            SolidColorBrush strokeBlueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#179F77"));
            SolidColorBrush khakiBrush = new SolidColorBrush(Color.FromRgb(177, 121, 36));

            Brush[] layerBrushes = { skyBlueBrush, bananaBrush, khakiBrush };
            //default style for links
            Style linkNodeStyle = new Style();
            linkNodeStyle.Setters.Add(new Setter(DiagramLink.BaseShapeProperty,
               ArrowHeads.None));
            linkNodeStyle.Setters.Add(new Setter(DiagramLink.HeadShapeProperty,
               ArrowHeads.None));
            mfDiagram.DiagramLinkStyle = linkNodeStyle;
           
            mfDiagram.DefaultShape = Shapes.Rectangle;
            mfDiagram.FontFamily = new FontFamily("Verdana");


            // Tworzenie warstw i łączenie ich
            for (int i = 0; i < 5; i++)
            {
                ShapeNode roundNode = new ShapeNode();
                roundNode.Shape = Shapes.Ellipse;
                roundNode.Bounds = roundNodeRect;
                roundNode.Stroke = bananaBrush;
                roundNode.Brush = skyBlueBrush;
                roundNode.TextAlignment= TextAlignment.Center;
                roundNode.TextVerticalAlignment = AlignmentY.Center;
                roundNode.TextBrush = bananaBrush;
                if (i == 1)
                    roundNode.Brush = fourBlueBrush;
                if(i==3)
                    roundNode.Brush = aaBlueBrush;
                roundNode.Id = "layer1_" + i.ToString();
                roundNode.StrokeThickness = thickness;
              
                mfDiagram.Nodes.Add(roundNode);
            }

            for (int i = 1; i < 4; i++)
            {
                ShapeNode roundNode = new ShapeNode();
                roundNode.Shape = Shapes.Ellipse;
                roundNode.Bounds = roundNodeRect;
                roundNode.Brush = i!=1? skyBlueBrush : new SolidColorBrush(Color.FromArgb(0,0,0,0));
                roundNode.Brush = skyBlueBrush;
                if (i == 1)
                    roundNode.Brush = fourBlueBrush;
                if (i == 2)
                    roundNode.Brush = aaBlueBrush;
                if(i==3)
                    roundNode.Brush = oneBlueBrush;

                roundNode.Stroke = bananaBrush;
                roundNode.Id = "layer2_" + i.ToString();
               
                roundNode.StrokeThickness = thickness;
                roundNode.TextAlignment = TextAlignment.Center;
                roundNode.TextVerticalAlignment = AlignmentY.Center;
                roundNode.TextBrush = bananaBrush;
                mfDiagram.Nodes.Add(roundNode);

                if (i != 10)
                for (int j = 0; j < 5; j++)
                {
                    DiagramNode layer1Node = mfDiagram.FindNodeById("layer1_" + j.ToString());

                    if (layer1Node != null)
                    {
                        DiagramLink link = new DiagramLink(mfDiagram, layer1Node, roundNode);
                        link.HeadShape = ArrowHeads.None;
                        link.BaseShape = ArrowHeads.None;
                        link.BaseBrush = bananaBrush;
                        link.BaseBrush = Brushes.Blue;
                        link.Stroke = bananaBrush;
                        mfDiagram.Links.Add(link);
                    }
                }
            }

            //third layer of round nodes: 4 nodes
            for (int i = 0; i < 4; i++)
            {

               
                ShapeNode roundNode = new ShapeNode();
                roundNode.Shape = Shapes.Ellipse;
                roundNode.Bounds = roundNodeRect;
                roundNode.Brush = skyBlueBrush;
                roundNode.Stroke = bananaBrush;
                roundNode.Id = "layer3_" + i.ToString();
                roundNode.StrokeThickness = thickness;
                roundNode.TextAlignment = TextAlignment.Center;
                roundNode.TextVerticalAlignment = AlignmentY.Center;
                roundNode.TextBrush = bananaBrush;
                if (i == 1)
                    roundNode.Brush = fourBlueBrush;
                if (i == 3)
                    roundNode.Brush = oneBlueBrush;

                mfDiagram.Nodes.Add(roundNode);

                //create a connection between this node and all
                //nodes from the previous layer
                for (int j = 0; j < 7; j++)
                {
                    DiagramNode layer2Node = mfDiagram.FindNodeById("layer2_" + j.ToString());

                    if (layer2Node != null)
                    {
                        DiagramLink link = new DiagramLink(mfDiagram, layer2Node, roundNode);
                        link.HeadShape = ArrowHeads.None;
                        link.BaseShape = ArrowHeads.None;
                        link.Stroke = bananaBrush;
                        mfDiagram.Links.Add(link);
                    }
                }
            }

            //last layer of round nodes: one
            ShapeNode lastRoundNode = new ShapeNode();
            lastRoundNode.Shape = Shapes.Ellipse;
            lastRoundNode.Bounds = roundNodeRect;
            lastRoundNode.Stroke = bananaBrush;
            lastRoundNode.StrokeThickness = thickness;
            lastRoundNode.Brush = skyBlueBrush;
            lastRoundNode.Id = "layer4_0";
            lastRoundNode.TextAlignment = TextAlignment.Center;
            lastRoundNode.TextVerticalAlignment = AlignmentY.Center;
            lastRoundNode.TextBrush = bananaBrush;
            mfDiagram.Nodes.Add(lastRoundNode);
           
            //create a connection between the last round node and all
            //nodes from the previous layer
            for (int j = 0; j < 4; j++)
            {
                DiagramNode layer3Node = mfDiagram.FindNodeById("layer3_" + j.ToString());

                if (layer3Node != null)
                {
                    DiagramLink link = new DiagramLink(mfDiagram, layer3Node, lastRoundNode);
                    link.HeadShape = ArrowHeads.None;
                    link.Stroke = bananaBrush;
                    link.BaseShape = ArrowHeads.None;
                    mfDiagram.Links.Add(link);
                }
            }
           

            double layerLabelWidth = 500.0;
            double layerLabelHeight = 60.0;

            //apply the layout
            LayeredLayout layout = new LayeredLayout();
            layout.Orientation = MindFusion.Diagramming.Wpf.Layout.Orientation.Horizontal;
            layout.LayerDistance = 280;
            layout.Margins = new Size(10, 3 * layerLabelHeight);
            layout.Arrange(mfDiagram);


            RandomColors();
        }
        public void RandomColors()
        {
            SolidColorBrush skyBlueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#66bbff"));
            SolidColorBrush fourBlueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#443399dd"));
            SolidColorBrush aaBlueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#aa3399dd"));
            SolidColorBrush oneBlueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0055aaff"));
            Random a = new Random();
            foreach (var itemi in mfDiagram.Nodes)
            {
               
                var b =(decimal) a.NextDouble();
                var c = (byte)(b * 255);
                itemi.Brush = new SolidColorBrush(Color.FromArgb(c,85,175,255)); ;
                itemi.Text = b.ToString("0.##");

            }
        }

        public MainWindow(Network network)
        {
            DataContext = Context = new ParameterViewModel(network, this);
            InitializeComponent();
        }

        #region Validate Numbers
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        #endregion

        #region Moving Mode
        public void TurnOnMovingMode()
        {
            ListBox layersListBox = (ListBox)FindName("LayersWpf");
            layersListBox.SelectedItem = null;

            Style itemContainerStyle = new Style(typeof(ListBoxItem));
            itemContainerStyle.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(s_PreviewMouseLeftButtonDown)));
            itemContainerStyle.Setters.Add(new EventSetter(ListBoxItem.DropEvent, new DragEventHandler(listbox1_Drop)));
            itemContainerStyle.Setters.Add(new Setter(ListBoxItem.BackgroundProperty, Brushes.Transparent));
            layersListBox.ItemContainerStyle = itemContainerStyle;
        }
        public void TurnOffMovingMode()
        {
            ListBox layersListBox = (ListBox)FindName("LayersWpf");

            Style itemContainerStyle = new Style(typeof(ListBoxItem));
            layersListBox.ItemContainerStyle = itemContainerStyle;
        }

        public LayerModel GetSelectedItem()
        {
            ListBox layersListBox = (ListBox)FindName("LayersWpf");

            return layersListBox.SelectedItem as LayerModel;
        }


        private Point startPoint;

        void s_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (sender is ListBoxItem item)
            {
                ListBoxItem draggedItem = sender as ListBoxItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                item.Background = Brushes.LightBlue;
                draggedItem.IsSelected = true;
            }
        }

        void listbox1_Drop(object sender, DragEventArgs e)
        {
            ListBox layersListBox = (ListBox)FindName("LayersWpf");

            LayerModel droppedData = e.Data.GetData(typeof(LayerModel)) as LayerModel;
            LayerModel target = ((ListBoxItem)(sender)).DataContext as LayerModel;

            int removedIdx = layersListBox.Items.IndexOf(droppedData);
            int targetIdx = layersListBox.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                Context.Layers.Insert(targetIdx + 1, droppedData);
                Context.Layers.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (Context.Layers.Count + 1 > remIdx)
                {
                    Context.Layers.Insert(targetIdx, droppedData);
                    Context.Layers.RemoveAt(remIdx);
                }
            }

            for (int i = 0; i < Context.Layers.Count(); i++)
            {
                Context.Layers[i].Id = i;
            }
        }

        #endregion

        private int lastSelectedItem;

       
    }
}
