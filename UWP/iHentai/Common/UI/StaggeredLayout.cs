using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace iHentai.Common.UI
{
    
    [System.Diagnostics.DebuggerDisplay("Count = {Count}, Height = {Height}")]
    internal class StaggeredColumnLayout : List<StaggeredItem>
    {
        public double Height { get; private set; }

        public new void Add(StaggeredItem item)
        {
            Height = item.Top + item.Height;
            base.Add(item);
        }

        public new void Clear()
        {
            Height = 0;
            base.Clear();
        }
    }
    
    internal class StaggeredItem
    {
        public StaggeredItem(int index)
        {
            this.Index = index;
        }

        public double Top { get; internal set; }

        public double Height { get; internal set; }

        public int Index { get; }
    }
    internal class StaggeredLayoutState
    {
        private List<StaggeredItem> _items = new List<StaggeredItem>();
        private VirtualizingLayoutContext _context;
        private Dictionary<int, StaggeredColumnLayout> _columnLayout = new Dictionary<int, StaggeredColumnLayout>();
        private double _lastAverageHeight;

        public StaggeredLayoutState(VirtualizingLayoutContext context)
        {
            _context = context;
        }

        public double ColumnWidth { get; internal set; }

        public int NumberOfColumns { get { return _columnLayout.Count; } }

        public double RowSpacing { get; internal set; }

        internal void AddItemToColumn(StaggeredItem item, int columnIndex)
        {
            if (_columnLayout.TryGetValue(columnIndex, out StaggeredColumnLayout columnLayout) == false)
            {
                columnLayout = new StaggeredColumnLayout();
                _columnLayout[columnIndex] = columnLayout;
            }

            if (columnLayout.Contains(item) == false)
            {
                columnLayout.Add(item);
            }
        }

        internal StaggeredItem GetItemAt(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            if (index <= (_items.Count - 1))
            {
                return _items[index];
            }
            else
            {
                StaggeredItem item = new StaggeredItem(index);
                _items.Add(item);
                return item;
            }
        }

        internal StaggeredColumnLayout GetColumnLayout(int columnIndex)
        {
            _columnLayout.TryGetValue(columnIndex, out StaggeredColumnLayout columnLayout);
            return columnLayout;
        }

        /// <summary>
        /// Clear everything that has been calculated.
        /// </summary>
        internal void Clear()
        {
            _columnLayout.Clear();
            _items.Clear();
        }

        /// <summary>
        /// Clear the layout columns so they will be recalculated.
        /// </summary>
        internal void ClearColumns()
        {
            _columnLayout.Clear();
        }

        /// <summary>
        /// Gets the estimated height of the layout.
        /// </summary>
        /// <returns>The estimated height of the layout.</returns>
        /// <remarks>
        /// If all of the items have been calculated then the actual height will be returned.
        /// If all of the items have not been calculated then an estimated height will be calculated based on the average height of the items.
        /// </remarks>
        internal double GetHeight()
        {
            double desiredHeight = Enumerable.Max(_columnLayout.Values, c => c.Height);

            var itemCount = Enumerable.Sum(_columnLayout.Values, c => c.Count);
            if (itemCount == _context.ItemCount)
            {
                return desiredHeight;
            }

            double averageHeight = 0;
            foreach (var kvp in _columnLayout)
            {
                averageHeight += kvp.Value.Height / kvp.Value.Count;
            }

            averageHeight /= _columnLayout.Count;
            double estimatedHeight = (averageHeight * _context.ItemCount) / _columnLayout.Count;
            if (estimatedHeight > desiredHeight)
            {
                desiredHeight = estimatedHeight;
            }

            if (Math.Abs(desiredHeight - _lastAverageHeight) < 5)
            {
                return _lastAverageHeight;
            }

            _lastAverageHeight = desiredHeight;
            return desiredHeight;
        }

        internal void RecycleElementAt(int index)
        {
            UIElement element = _context.GetOrCreateElementAt(index);
            _context.RecycleElement(element);
        }

        internal void RemoveFromIndex(int index)
        {
            if (index > _items.Count)
            {
                // Item was added/removed but we haven't realized that far yet
                return;
            }

            int numToRemove = _items.Count - index;
            _items.RemoveRange(index, numToRemove);

            foreach (var kvp in _columnLayout)
            {
                StaggeredColumnLayout layout = kvp.Value;
                for (int i = 0; i < layout.Count; i++)
                {
                    if (layout[i].Index >= index)
                    {
                        numToRemove = layout.Count - i;
                        layout.RemoveRange(i, numToRemove);
                        break;
                    }
                }
            }
        }

        internal void RemoveRange(int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (i > _items.Count)
                {
                    break;
                }

                StaggeredItem item = _items[i];
                item.Height = 0;
                item.Top = 0;

                // We must recycle all elements to ensure that it gets the correct context
                RecycleElementAt(i);
            }

            foreach (var kvp in _columnLayout)
            {
                StaggeredColumnLayout layout = kvp.Value;
                for (int i = 0; i < layout.Count; i++)
                {
                    if ((startIndex <= layout[i].Index) && (layout[i].Index <= endIndex))
                    {
                        int numToRemove = layout.Count - i;
                        layout.RemoveRange(i, numToRemove);
                        break;
                    }
                }
            }
        }
    }
    /// <summary>
    /// Arranges child elements into a staggered grid pattern where items are added to the column that has used least amount of space.
    /// </summary>
    public class StaggeredLayout : VirtualizingLayout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaggeredLayout"/> class.
        /// </summary>
        public StaggeredLayout()
        {
        }


        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
            nameof(VerticalOffset), typeof(double), typeof(StaggeredLayout), new PropertyMetadata(default(double)));

        public double VerticalOffset
        {
            get { return (double) GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the desired width for each column.
        /// </summary>
        /// <remarks>
        /// The width of columns can exceed the DesiredColumnWidth if the HorizontalAlignment is set to Stretch.
        /// </remarks>
        public double DesiredColumnWidth
        {
            get { return (double)GetValue(DesiredColumnWidthProperty); }
            set { SetValue(DesiredColumnWidthProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DesiredColumnWidth"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="DesiredColumnWidth"/> dependency property.</returns>
        public static readonly DependencyProperty DesiredColumnWidthProperty = DependencyProperty.Register(
            nameof(DesiredColumnWidth),
            typeof(double),
            typeof(StaggeredLayout),
            new PropertyMetadata(250d, OnDesiredColumnWidthChanged));

        /// <summary>
        /// Gets or sets the spacing between columns of items.
        /// </summary>
        public double ColumnSpacing
        {
            get { return (double)GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ColumnSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register(
            nameof(ColumnSpacing),
            typeof(double),
            typeof(StaggeredLayout),
            new PropertyMetadata(0d, OnSpacingChanged));

        /// <summary>
        /// Gets or sets the spacing between rows of items.
        /// </summary>
        public double RowSpacing
        {
            get { return (double)GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RowSpacing"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register(
            nameof(RowSpacing),
            typeof(double),
            typeof(StaggeredLayout),
            new PropertyMetadata(0d, OnSpacingChanged));

        /// <inheritdoc/>
        protected override void InitializeForContextCore(VirtualizingLayoutContext context)
        {
            context.LayoutState = new StaggeredLayoutState(context);
            base.InitializeForContextCore(context);
        }

        /// <inheritdoc/>
        protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
        {
            context.LayoutState = null;
            base.UninitializeForContextCore(context);
        }

        /// <inheritdoc/>
        protected override void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
        {
            var state = (StaggeredLayoutState)context.LayoutState;

            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    state.RemoveFromIndex(args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    state.RemoveFromIndex(args.NewStartingIndex);

                    // We must recycle the element to ensure that it gets the correct context
                    state.RecycleElementAt(args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    int minIndex = Math.Min(args.NewStartingIndex, args.OldStartingIndex);
                    int maxIndex = Math.Max(args.NewStartingIndex, args.OldStartingIndex);
                    state.RemoveRange(minIndex, maxIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    state.RemoveFromIndex(args.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    state.Clear();
                    break;
            }

            base.OnItemsChangedCore(context, source, args);
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            if (context.ItemCount == 0)
            {
                return new Size(availableSize.Width, VerticalOffset);
            }

            if ((context.RealizationRect.Width == 0) && (context.RealizationRect.Height == 0))
            {
                return new Size(availableSize.Width, VerticalOffset);
            }

            var state = (StaggeredLayoutState)context.LayoutState;

            double availableWidth = availableSize.Width;
            double availableHeight = availableSize.Height;
            var numColumns = (int) Math.Floor(availableSize.Width / Math.Min(DesiredColumnWidth, availableSize.Width));
            var columnWidth = availableSize.Width / numColumns;

            if (columnWidth != state.ColumnWidth)
            {
                // The items will need to be remeasured
                state.Clear();
            }

            state.ColumnWidth = columnWidth;

            // adjust for column spacing on all columns expect the first
            double totalWidth = state.ColumnWidth + ((numColumns - 1) * (state.ColumnWidth + ColumnSpacing));
            if (totalWidth > availableWidth)
            {
                numColumns--;
            }
            else if (double.IsInfinity(availableWidth))
            {
                availableWidth = totalWidth;
            }

            if (numColumns != state.NumberOfColumns)
            {
                // The items will not need to be remeasured, but they will need to go into new columns
                state.ClearColumns();
            }

            if (RowSpacing != state.RowSpacing)
            {
                // If the RowSpacing changes the height of the rows will be different.
                // The columns stores the height so we'll want to clear them out to
                // get the proper height
                state.ClearColumns();
                state.RowSpacing = RowSpacing;
            }

            var columnHeights = Enumerable.Repeat(VerticalOffset, numColumns).ToArray();
            var itemsPerColumn = new int[numColumns];
            var deadColumns = new HashSet<int>();

            for (int i = 0; i < context.ItemCount; i++)
            {
                var columnIndex = GetColumnIndex(columnHeights);

                UIElement element = null;
                StaggeredItem item = state.GetItemAt(i);
                if (item.Height == 0)
                {
                    // Item has not been measured yet. GetValueAttribute the element and store the values
                    element = context.GetOrCreateElementAt(i);
                    element.Measure(new Size(state.ColumnWidth, availableHeight));
                    item.Height = element.DesiredSize.Height;
                }

                double spacing = itemsPerColumn[columnIndex] > 0 ? RowSpacing : 0;
                item.Top = columnHeights[columnIndex] + spacing;
                double bottom = item.Top + item.Height;
                columnHeights[columnIndex] = bottom;
                itemsPerColumn[columnIndex]++;
                state.AddItemToColumn(item, columnIndex);

                if (bottom < context.RealizationRect.Top)
                {
                    // The bottom of the element is above the realization area
                    if (element != null)
                    {
                        context.RecycleElement(element);
                    }
                }
                else if (item.Top > context.RealizationRect.Bottom)
                {
                    // The top of the element is below the realization area
                    if (element != null)
                    {
                        context.RecycleElement(element);
                    }

                    deadColumns.Add(columnIndex);
                }
                else
                {
                    // We ALWAYS want to measure an item that will be in the bounds
                    context.GetOrCreateElementAt(i).Measure(new Size(state.ColumnWidth, availableHeight));
                }

                if (deadColumns.Count == numColumns)
                {
                    break;
                }
            }

            double desiredHeight = state.GetHeight() + VerticalOffset;

            return new Size(availableWidth, desiredHeight);
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            if ((context.RealizationRect.Width == 0) && (context.RealizationRect.Height == 0))
            {
                return finalSize;
            }

            var state = (StaggeredLayoutState)context.LayoutState;

            // Cycle through each column and arrange the items that are within the realization bounds
            for (int columnIndex = 0; columnIndex < state.NumberOfColumns; columnIndex++)
            {
                StaggeredColumnLayout layout = state.GetColumnLayout(columnIndex);
                for (int i = 0; i < layout.Count; i++)
                {
                    StaggeredItem item = layout[i];

                    double bottom = item.Top + item.Height;
                    if (bottom < context.RealizationRect.Top)
                    {
                        // element is above the realization bounds
                        continue;
                    }

                    if (item.Top <= context.RealizationRect.Bottom)
                    {
                        double itemHorizontalOffset = (state.ColumnWidth * columnIndex) + (ColumnSpacing * columnIndex);

                        Rect bounds = new Rect(itemHorizontalOffset, item.Top, state.ColumnWidth, item.Height);
                        UIElement element = context.GetOrCreateElementAt(item.Index);
                        element.Arrange(bounds);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return finalSize;
        }

        private static void OnDesiredColumnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (StaggeredLayout)d;
            panel.InvalidateMeasure();
        }

        private static void OnSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (StaggeredLayout)d;
            panel.InvalidateMeasure();
        }

        private static int GetColumnIndex(double[] columnHeights)
        {
            int columnIndex = 0;
            double height = columnHeights[0];
            for (int j = 1; j < columnHeights.Length; j++)
            {
                if (columnHeights[j] < height)
                {
                    columnIndex = j;
                    height = columnHeights[j];
                }
            }

            return columnIndex;
        }
    }
}