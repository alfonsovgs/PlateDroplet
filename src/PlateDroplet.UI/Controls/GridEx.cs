using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PlateDroplet.UI.Controls
{
    public class GridEx : Grid
    {
        private const string All = "*";
        private const string Auto = "auto";
        private const string One = "1";

        public static readonly DependencyProperty ColumnWidthsProperty = DependencyProperty.RegisterAttached("ColumnWidths", 
            typeof(string), typeof(GridEx), new PropertyMetadata("*", ColumnWidthsPropertyChanged));

        public static readonly DependencyProperty RowHeightsProperty = DependencyProperty.RegisterAttached("RowHeights",
            typeof(string), typeof(GridEx), new PropertyMetadata("*", RowHeightsPropertyChanged));

        public static string GetColumnWidths(DependencyObject obj) => (string) obj.GetValue(ColumnWidthsProperty);

        public static void SetColumnWidths(DependencyObject obj, string value) => obj.SetValue(ColumnWidthsProperty, value);

        public static string GetRowHeights(DependencyObject obj) => (string) obj.GetValue(RowHeightsProperty);

        public static void SetRowHeights(DependencyObject obj, string value) => obj.SetValue(RowHeightsProperty, value);

        private static void ColumnWidthsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Grid grid)) return;

            grid.ColumnDefinitions.Clear();

            var widhts = e.NewValue.ToString().SplitSafe();
            MapDefinition<ColumnDefinition>(widhts, grid.ColumnDefinitions.Add);
        }

        private static void RowHeightsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Grid grid)) return;

            grid.RowDefinitions.Clear();

            var heights = e.NewValue.ToString().SplitSafe();
            MapDefinition<RowDefinition>(heights, grid.RowDefinitions.Add);
        }

        private static void MapDefinition<T>(IEnumerable<string> values, Action<T> add) where T : DefinitionBase, new()
        {
            foreach (var v in values)
            {
                var with = v.Clean();
                if (string.IsNullOrWhiteSpace(with)) with = All;

                GridLength gridLength;
                if (with.EndsWith(All))
                {
                    var starWith = with.Replace(All, string.Empty);

                    if (string.IsNullOrEmpty(starWith))
                    {
                        starWith = One;
                    }

                    var stars = double.Parse(starWith);
                    gridLength = new GridLength(stars, GridUnitType.Star);
                }
                else if (with == Auto)
                {
                    gridLength = new GridLength(1, GridUnitType.Auto);
                }
                else
                {
                    var pixels = double.Parse(with);
                    gridLength = new GridLength(pixels, GridUnitType.Pixel);
                }

                if (typeof(T) == typeof(ColumnDefinition))
                {
                    var definition = (T)(object) new ColumnDefinition {Width = gridLength};
                    add(definition);
                }
                else
                {
                    var definition = (T)(object) new RowDefinition {Height = gridLength};
                    add(definition);
                }
            }
        }
    }

    public static class GridExtensions
    {
        public static IEnumerable<string> SplitSafe(this string value) => value?.Split(',') ?? Array.Empty<string>();

        public static string Clean(this string value) => value.ToLower().Trim();
    }
}