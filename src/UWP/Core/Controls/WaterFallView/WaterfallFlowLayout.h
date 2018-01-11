#pragma once
#include "VisualWindow.h"
#include "WaterfallFlowUnit.h"
#include "ILayout.h"

using namespace Windows::Foundation;

namespace WaterFallView
{
	ref class WaterfallFlowLayout sealed
		: ILayout
	{
	public:
		virtual RegisterReadOnlyProperty(double, _width, Width);
		virtual RegisterReadOnlyProperty(Size, Size((float)Width, ((float)*std::max_element(_stacks->begin(), _stacks->end()) + _footerSize.Height + _headerSize.Height)), LayoutSize);
		virtual RegisterReadOnlyProperty(int, (int)_units->size(), UnitCount);
		RegisterReadOnlyProperty(double, _spacing, Spacing);
		RegisterReadOnlyProperty(int, (int)_stacks->size(), StackCount);
		virtual RegisterReadOnlyProperty(Size, _headerSize, HeaderSize);
		virtual RegisterReadOnlyProperty(Size, _footerSize, FooterSize);

		WaterfallFlowLayout(double spacing, double width, int stackCount);

		virtual void AddItem(int index, Platform::Object^ item, Size size);
		virtual void ChangeItem(int index, Platform::Object^ item, Size size);
		virtual void RemoveItem(int index);
		virtual void RemoveAll();

		virtual LONGLONG GetVisableItems(VisualWindow window, int* firstIndex, int * lastIndex);
		virtual Rect GetItemLayoutRect(int index);
		virtual bool FillWindow(VisualWindow window);
		virtual void ChangePanelSize(double width);
		virtual Size GetItemSize(int index);

		virtual Size GetHeaderAvailableSize();
		virtual Size GetFooterAvailableSize();
		virtual bool SetHeaderSize(Size size);
		virtual bool SetFooterSize(Size size);
		virtual Rect GetHeaderLayoutRect();
		virtual Rect GetFooterLayoutRect();

		void ChangeSpacing(double width);
		void ChangeStackCount(int stackCount);

	private:
		~WaterfallFlowLayout();
		void Relayout();
		std::vector<WaterfallFlowUnit^>* _units;
		double _spacing;
		double _width;

		std::vector<double>* _stacks;

		int _requestRelayoutIndex = -1;
		void SetRelayoutIndex(int index);

		Size _headerSize = Size(0, 0);
		Size _footerSize = Size(0, 0);
	};
}
