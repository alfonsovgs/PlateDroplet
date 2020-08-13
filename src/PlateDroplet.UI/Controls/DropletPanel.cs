using PlateDroplet.UI.Controls.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlateDroplet.UI.Controls
{
    public class DropletPanel : Canvas
    {
        private const int Size = 50;
        private const int InitialTopPosition = 1;
        private int _rows;
        private int _cols;

        private readonly Color _base = Color.FromRgb(242, 248, 253);
        private readonly Color _gray = Color.FromRgb(193, 187, 183);
        private readonly Color _red = Color.FromRgb(238, 61, 72);

        public static readonly DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(WellNodePanelResult),
            typeof(DropletPanel), new PropertyMetadata(null, OnWellsChanged));

        public WellNodePanelResult Result
        {
            get => (WellNodePanelResult) GetValue(ResultProperty);
            set => SetValue(ResultProperty, value);
        }

        private static void OnWellsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InvalidateAll(d);
            if (!(d is DropletPanel dropletPanel)) return;
            if(dropletPanel.Result == null) return;

            dropletPanel._rows = dropletPanel.Result.GetRows();
            dropletPanel._cols = dropletPanel.Result.GetCols();
            dropletPanel?.Draw();
        }

        private static void InvalidateAll(DependencyObject d)
        {
            if (!(d is UIElement element)) return;
            element.InvalidateVisual();
        }

        private void Draw()
        {
            Children.Clear();
            DrawEmptySpace();
            DrawColumns();
            DrawRows();
            DrawWells();
        }

        private  void DrawEmptySpace()
        {
            var well = BuildItem(_base);
            Children.Add(well);
        }

        private void DrawColumns()
        {
            for(var col = 1; col <= _cols; col++)
            {
                var well = BuildItem(_base, null, col.ToString());
                SetLeft(well, (col) * Size);
                Children.Add(well);
            }
        }

        private void DrawRows()
        {
            var lettersRow = Enumerable.Range('A', _rows)
            .Select(char.ConvertFromUtf32)
            .ToArray();

            for (var row = 0; row < _rows; row++)
            {
                var well = BuildItem(_base, null, lettersRow[row]);
                SetTop(well, (row + InitialTopPosition) * Size);
                Children.Add(well); 
            }
        }

        private void DrawWells()
        {
            for (var row = 0; row < _rows; row++)
            {
                for (var col = 0; col < _cols; col++)
                {
                    var well = Result[row, col];
                    var rectangle = BuildWell(well);

                    Children.Add(rectangle);
                    SetLeft(rectangle, (col + InitialTopPosition) * Size);
                    SetTop(rectangle, (row + InitialTopPosition) * Size);
                }
            }
        }

        private Color GetColor(EColor color)
        {
            return color switch
            {
                EColor.Red => _red,
                EColor.Gray => _gray,
                _ => _base
            };
        }

        private static Border BuildItem(Color color, ToolTip tooltip = null, string legend = "") => new Border
        {
            BorderBrush = Brushes.DimGray,
            BorderThickness = new Thickness(1),
            Background = new SolidColorBrush(color),
            Height = Size,
            Width = Size,
            ToolTip = tooltip,
            Child = new Label
            {
                Content = legend,
                FontSize = 14.0d,
                FontWeight = FontWeights.SemiBold,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            },
        };

        private Border BuildWell(WellNodePanel well)
        {
            var tooltip = new ToolTip
            {
                Content = $"Index: {well.Index} \n" +
                          $"DropletCount: {well.DropletCount} \n" +
                          $"Group: {well.GetGroup()}",
            };

            return BuildItem(GetColor(well.Color), tooltip, well.Legend);
        }
    }
}
