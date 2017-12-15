#pragma once

using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;

namespace WaterFallView
{
	ref class PhotowallUnit sealed
	{
	public:
		PhotowallUnit(Platform::Object^ item, Size desiredSize);

		RegisterProperty(Platform::Object^, _item, Item);
		RegisterProperty(int, _rowIndex, RowIndex);
		RegisterProperty(Size, _desiredSize, DesiredSize);
		RegisterProperty(Size, _actualSize, ActualSize);
		RegisterProperty(double, _offset, Offset);
		RegisterProperty(double, _actualOffset, ActualOffset);
	private:
		Object^ _item;
		int  _rowIndex;
		double _offset;
		double _actualOffset = -1;
		Size _desiredSize = Size::Empty;
		Size _actualSize = Size::Empty;
	};
}
