using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHentai.Common.UI
{
    internal class FixedColumnLayout : Panel
    {
        public static readonly DependencyProperty DesiredColumnWidthProperty = DependencyProperty.Register(
            nameof(DesiredColumnWidth), typeof(double), typeof(FixedColumnLayout), new PropertyMetadata(default(double)));

        public double DesiredColumnWidth
        {
            get => (double) GetValue(DesiredColumnWidthProperty);
            set => SetValue(DesiredColumnWidthProperty, value);
        }
        
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            nameof(Padding), typeof(double), typeof(FixedColumnLayout), new PropertyMetadata(8d));

        public double Padding
        {
            get => (double) GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
        private double _availableWidth;
        private double _columnWidth;
        private int _numColumns;
        private double[] _columnHeight;

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_availableWidth != availableSize.Width)
            {
                _availableWidth = availableSize.Width;
                _numColumns = (int) Math.Floor(availableSize.Width / Math.Min(DesiredColumnWidth, availableSize.Width));
                _columnWidth = Math.Max((availableSize.Width - Padding * (_numColumns - 1)) / _numColumns, 0);
                var rowCount = Math.Ceiling(Convert.ToDouble(Children.Count) / _numColumns) ;
                _columnHeight = new double[Convert.ToInt32(rowCount)];
            }
            for (var i = 0; i < Children.Count; i++)
            {
                var item = Children[i];
                item.Measure(new Size(_columnWidth, availableSize.Height));
                var rowIndex = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(i + 1) / _numColumns)) - 1;
                _columnHeight[rowIndex] = Math.Max(_columnHeight[rowIndex], item.DesiredSize.Height);
            }
            return new Size(_numColumns * _columnWidth + (_numColumns - 1) * Padding, _columnHeight.Sum() + (_columnHeight.Length - 1) * Padding);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            for (var i = 0; i < Children.Count; i++)
            {
                var item = Children[i];
                var columnIndex = (i + 1) % _numColumns - 1;
                if (columnIndex == -1)
                {
                    columnIndex = _numColumns - 1;
                }
                var rowIndex = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(i + 1) / _numColumns)) - 1;
                item.Arrange(new Rect(columnIndex * _columnWidth + Padding * columnIndex, rowIndex * _columnHeight[rowIndex] + Padding * rowIndex, _columnWidth, _columnHeight[rowIndex]));
            }

            return finalSize;
        }
    }
}