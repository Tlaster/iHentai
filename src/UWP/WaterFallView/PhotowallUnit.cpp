#include "pch.h"
#include "PhotowallUnit.h"

using namespace WaterFallView;

PhotowallUnit::PhotowallUnit(Platform::Object^ item, Size desiredSize)
{
    Item = item;
    DesiredSize = desiredSize;
}