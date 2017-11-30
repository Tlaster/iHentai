#include "pch.h"
#include "WaterfallFlowUnit.h"

using namespace WaterFallView;

WaterfallFlowUnit::WaterfallFlowUnit(Platform::Object^ item, Size desiredSize)
{
    _item = item;
    _desiredSize = desiredSize;
}
