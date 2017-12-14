#pragma once

using namespace Windows::Foundation;

namespace WaterFallView
{
	ref class WaterfallFlowUnit sealed
	{
	public:
		WaterfallFlowUnit(Object^ item, Size desiredSize);

		RegisterProperty(Object^, _item, Item);
		RegisterProperty(int, _stackIndex, StackIndex);
		RegisterProperty(double, _offset, Offset);
		RegisterProperty(Size, _desiredSize, DesiredSize);

	private:
		Object^ _item;
		int _stackIndex = -1;
		double _offset = -1;
		Size _desiredSize = Size::Empty;
	};
}
