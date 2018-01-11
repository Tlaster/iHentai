#pragma once

using namespace Windows::Foundation;

namespace WaterFallView
{
	public interface class IItemResizer
	{
	public:
		Size Resize(Object^ item, Size oldSize, Size availableSize);
	};
}