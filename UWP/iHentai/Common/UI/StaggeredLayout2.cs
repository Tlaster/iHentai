using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Microsoft.UI.Xaml.Controls;

namespace iHentai.Common.UI
{
    public class StaggeredLayout2 : VirtualizingLayout
    {
        private readonly List<Rect> m_cachedBounds = new List<Rect>();
        private readonly List<double> m_columnOffsets = new List<double>();

        private int m_firstIndex;
        private double m_lastAvailableWidth;
        private int m_lastIndex;

        //public StaggeredLayout()
        //{
        //    DesiredColumnWidth = 150.0;
        //}

        public double VerticalOffset { get; set; } = 0D;
        public double DesiredColumnWidth { get; set; } = 250D;

        protected override void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
        {
            base.OnItemsChangedCore(context, source, args);
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    m_cachedBounds.Clear();
                    m_columnOffsets.Clear();
                    InvalidateMeasure();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            var viewport = context.RealizationRect;

            if (availableSize.Width != m_lastAvailableWidth)
            {
                UpdateCachedBounds(availableSize);
                m_lastAvailableWidth = availableSize.Width;
            }

            // Initialize column offsets
            var numColumns = (int) Math.Floor(availableSize.Width / Math.Min(DesiredColumnWidth, availableSize.Width));
            var columnWidth = availableSize.Width / numColumns;
            if (m_columnOffsets.Count == 0)
            {
                for (var i = 0; i < numColumns; i++)
                {
                    m_columnOffsets.Add(VerticalOffset);
                }
            }

            m_firstIndex = GetStartIndex(viewport);
            var currentIndex = m_firstIndex;
            var nextOffset = -1.0;

            // Measure items from start index to when we hit the end of the viewport.
            while (currentIndex < context.ItemCount && nextOffset < viewport.Bottom)
            {
                var child = context.GetOrCreateElementAt(currentIndex);
                child.Measure(new Size(columnWidth, availableSize.Height));

                if (currentIndex >= m_cachedBounds.Count)
                {
                    // We do not have bounds for this index. Lay it out and cache it.
                    var columnIndex = GetIndexOfLowestColumn(m_columnOffsets, out nextOffset);
                    m_cachedBounds.Add(new Rect(columnIndex * columnWidth, nextOffset, columnWidth,
                        child.DesiredSize.Height));
                    m_columnOffsets[columnIndex] += child.DesiredSize.Height;
                }
                else if (child.DesiredSize.Height != m_cachedBounds[currentIndex].Height) // Item height has changed
                {
                    m_cachedBounds.RemoveRange(currentIndex, m_cachedBounds.Count - currentIndex);
                    UpdateCachedBounds(availableSize);
                    var columnIndex = GetIndexOfLowestColumn(m_columnOffsets, out nextOffset);
                    m_cachedBounds.Add(new Rect(columnIndex * columnWidth, nextOffset, columnWidth,
                        child.DesiredSize.Height));
                    m_columnOffsets[columnIndex] += child.DesiredSize.Height;
                }
                else if (currentIndex + 1 == m_cachedBounds.Count)
                {
                    // Last element. Use the next offset.
                    GetIndexOfLowestColumn(m_columnOffsets, out nextOffset);
                }
                else
                {
                    nextOffset = m_cachedBounds[currentIndex + 1].Top;
                }

                //child.Arrange(m_cachedBounds[currentIndex]);

                m_lastIndex = currentIndex;
                currentIndex++;
            }

            var extent = GetExtentSize(availableSize);
            return extent;
        }

        private Size GetExtentSize(Size availableSize)
        {
            var largestColumnOffset = m_columnOffsets[0];
            largestColumnOffset = m_columnOffsets.Concat(new[] {largestColumnOffset}).Max();

            return new Size(availableSize.Width, largestColumnOffset);
        }

        private int GetIndexOfLowestColumn(IReadOnlyList<double> columnOffsets, out double lowestOffset)
        {
            var lowestIndex = 0;
            lowestOffset = columnOffsets[lowestIndex];
            for (var index = 0; index < columnOffsets.Count; index++)
            {
                var currentOffset = columnOffsets[index];
                if (lowestOffset > currentOffset)
                {
                    lowestOffset = currentOffset;
                    lowestIndex = index;
                }
            }

            return lowestIndex;
        }

        private int GetStartIndex(Rect viewport)
        {
            var startIndex = 0;
            if (m_cachedBounds.Count == 0)
            {
                startIndex = 0;
            }
            else
            {
                // find first index that intersects the viewport
                // perhaps this can be done more efficiently than walking
                // from the start of the list.
                for (var i = 0; i < m_cachedBounds.Count; i++)
                {
                    var currentBounds = m_cachedBounds[i];
                    if (currentBounds.Y < viewport.Bottom &&
                        currentBounds.Bottom > viewport.Top)
                    {
                        startIndex = i;
                        break;
                    }
                }
            }

            return startIndex;
        }

        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            if (context.RealizationRect != default && context.ItemCount > 0)
            {
                for (var index = m_firstIndex; index <= m_lastIndex; index++)
                {
                    var child = context.GetOrCreateElementAt(index);
                    child.Arrange(m_cachedBounds[index]);
                }
            }
            return finalSize;
        }

        private void UpdateCachedBounds(Size availableSize)
        {
            var numColumns = (int) Math.Floor(availableSize.Width / Math.Min(DesiredColumnWidth, availableSize.Width));
            var columnWidth = availableSize.Width / numColumns;
            m_columnOffsets.Clear();
            for (var i = 0; i < numColumns; i++)
            {
                m_columnOffsets.Add(VerticalOffset);
            }

            for (var index = 0; index < m_cachedBounds.Count; index++)
            {
                var columnIndex = GetIndexOfLowestColumn(m_columnOffsets, out var nextOffset);
                var oldHeight = m_cachedBounds[index].Height;
                m_cachedBounds[index] = new Rect(columnIndex * columnWidth, nextOffset, columnWidth,
                    oldHeight);
                m_columnOffsets[columnIndex] += oldHeight;
            }
        }
    }
}
