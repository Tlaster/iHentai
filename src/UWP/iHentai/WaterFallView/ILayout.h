#pragma once
#include "VisualWindow.h"

using namespace Windows::Foundation;

namespace WaterFallView
{
	[Windows::Foundation::Metadata::WebHostHidden()]
	public interface class ILayout
	{
	public:
		property double Width {double get(); };
		property Size LayoutSize {Size get(); };
		property int UnitCount {int get(); };
		property Size HeaderSize {Size get(); };
		property Size FooterSize {Size get(); };

		void AddItem(int index, Platform::Object^ item, Size size);
		void ChangeItem(int index, Platform::Object^ item, Size size);
		void RemoveItem(int index);
		void RemoveAll();

		LONGLONG GetVisableItems(VisualWindow window, int* firstIndex, int* lastIndex);
		Rect GetItemLayoutRect(int index);
		bool FillWindow(VisualWindow window);
		void ChangePanelSize(double width);
		Size GetItemSize(int index);

		Size GetHeaderAvailableSize();
		Size GetFooterAvailableSize();
		bool SetHeaderSize(Size size);
		bool SetFooterSize(Size size);
		Rect GetHeaderLayoutRect();
		Rect GetFooterLayoutRect();
	};
}
