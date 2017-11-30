#pragma once
#include "VisualWindow.h"
#include "PhotowallUnit.h"
#include "ILayout.h"

using namespace Windows::Foundation;

namespace WaterFallView
{
	ref class PhotowallLayout sealed
		: ILayout
	{
	public:
		virtual RegisterReadOnlyProperty(double, _width, Width);
		virtual RegisterReadOnlyProperty(Size, Size((float)Width, (float)(UnitCount == 0 ? 0 : RowCount * (_unitSize + _spacing) - _spacing)), LayoutSize);
		virtual RegisterReadOnlyProperty(int, (int)_units->size(), UnitCount);
		virtual RegisterReadOnlyProperty(Size, _headerSize, HeaderSize);
		virtual RegisterReadOnlyProperty(Size, _footerSize, FooterSize);

		RegisterReadOnlyProperty(double, _spacing, Spacing);
		RegisterReadOnlyProperty(double, _unitSize, UnitSize);
		RegisterReadOnlyProperty(int, _rowIndex + 1 - (_lastRowLocked ? 1 : 0), RowCount);

		PhotowallLayout(double spacing, double width, double unitSize);

		virtual void AddItem(int index, Platform::Object^ item, Size size);
		virtual void ChangeItem(int index, Platform::Object^ item, Size size);
		virtual void RemoveItem(int index);
		virtual void RemoveAll();

		virtual LONGLONG GetVisableItems(VisualWindow window, int* firstIndex, int * lastIndex);
		virtual Rect GetItemLayoutRect(int index);
		virtual bool FillWindow(VisualWindow window);
		virtual bool IsItemInWindow(VisualWindow window, int index);
		virtual void ChangePanelSize(double width);
		virtual Size GetItemSize(int index);

		Size GetItemSize(Platform::Object^ item);

		virtual Size GetHeaderAvailableSize();
		virtual Size GetFooterAvailableSize();
		virtual bool SetHeaderSize(Size size);
		virtual bool SetFooterSize(Size size);
		virtual Rect GetHeaderLayoutRect();
		virtual Rect GetFooterLayoutRect();

		void ChangeSpacing(double spacing);
		void ChangeUnitSize(double unitSize);
	private:
		~PhotowallLayout();
		double _spacing;
		double _width;
		double _offset;
		int _rowIndex;
		double _unitSize;
		bool _lastRowLocked = false;
		std::vector<PhotowallUnit^>* _units;
		std::unordered_map<Platform::Object^, PhotowallUnit^, HashObject>* _itemUnitMap;
		int _requestRelayoutIndex = -1;

		Size _headerSize = Size(0, 0);
		Size _footerSize = Size(0, 0);

		void SetRelayoutIndex(int index);
		void Relayout();
		void RelayoutRow(int itemIndex);
	};
}
