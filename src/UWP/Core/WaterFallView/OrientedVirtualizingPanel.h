#pragma once
#include "VisualWindow.h"
#include "VirtualizingPanel.h"
#include "IItemResizer.h"
#include "ILayout.h"

namespace WaterFallView
{
	[Windows::Foundation::Metadata::WebHostHidden()]
	public ref class OrientedVirtualizingPanel :
		public VirtualizingPanel
	{
	public:
		RegisterProperty(IItemResizer^, _resizer, Resizer);
		RegisterProperty(bool, _delayMeasure, DelayMeasure);

		void ScrollIntoView(Platform::Object^ item);
		[Windows::Foundation::Metadata::DefaultOverload()]
		void ScrollIntoView(unsigned int index);
		void ScrollIntoView(Platform::Object^ item, bool disableAnimation);
		void ScrollIntoView(unsigned int index, bool disableAnimation);

	internal:
		OrientedVirtualizingPanel();

	protected:
		RegisterReadOnlyProperty(WinCon::ScrollViewer^, _parentScrollView, ParentScrollView);
		RegisterReadOnlyPropertyWithExpression(VirtualizingViewItem^, if (_measureControl == nullptr) { _measureControl = GetContainerForItemOverride(); Children->Append(_measureControl); } return _measureControl; , MeasureControl);
		RegisterReadOnlyProperty(ILayout^, _layout, Layout);
		RegisterReadOnlyProperty(LONGLONG, (LONGLONG)_visableItems, VisableItems);

		virtual Size MeasureOverride(Size availableSize) override;
		virtual Size ArrangeOverride(Size finalSize) override;
		virtual void OnItemsChanged(IObservableVector<Platform::Object^>^ source, IVectorChangedEventArgs^ e) override;
		virtual Size MeasureItem(Platform::Object^ item, Size oldSize);
		virtual VisualWindow GetVisibleWindow(Point offset, Size viewportSize);
		virtual Size GetItemAvailableSize(Size availableSize);
		virtual bool NeedRelayout(Size availableSize);
		virtual void Relayout(Size availableSize);
		virtual ILayout^ GetLayout(Size availableSize);
		virtual Point MakeItemVisable(int index);

		virtual void OnHeaderMeasureOverride(Size availableSize) override;
		virtual void OnHeaderArrangeOverride(Size finalSize) override;

		virtual void OnFooterMeasureOverride(Size availableSize) override;
		virtual void OnFooterArrangeOverride(Size finalSize) override;

		virtual void OnItemContainerSizeChanged(Platform::Object^ item, VirtualizingViewItem^ itemContainer, Size newSize) override;
	private:
		WinCon::ScrollViewer^ _parentScrollView;
		int _viewIndex = -1;
		int _firstRealizationItemIndex = -1;
		int _lastRealizationItemIndex = -1;
		double _width = 0;
		int _requestShowItemIndex = -1;
		Size _itemAvailableSize;
		Size _itemAvailableSizeCache;
		std::vector<Platform::Object^>* _visableItems = new std::vector<Platform::Object^>();
		VisualWindow _requestWindow = VisualWindow{ 0, 0 };
		VirtualizingViewItem^ _measureControl;
		Windows::UI::Xaml::DispatcherTimer^ _timer;
		bool _isSkip = false;
		bool _requestRelayout = false;
		bool _delayMeasure = true;
		IItemResizer^ _resizer;
		ILayout^ _layout;
		EventRegistrationToken _sizeChangedToken;
		Point _scrollViewOffset = Point(-1, -1);
		Point _scrollViewOffsetCache = Point(-1, -1);

		Size _headerSize = Size(0, 0);
		Size _footerSize = Size(0, 0);

		void OnViewChanging(Platform::Object^ sender, WinCon::ScrollViewerViewChangingEventArgs ^ e);
		void OnSizeChanged(Platform::Object^ sender, Windows::UI::Xaml::SizeChangedEventArgs^ e);
		void OnTick(Object^ sender, Object^e);
	};
}
