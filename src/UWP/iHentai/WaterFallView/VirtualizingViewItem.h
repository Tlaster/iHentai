//
// VirtualizingViewItem.h
// Declaration of the VirtualizingViewItem class.
//

#pragma once
#include "pch.h"

namespace WinCon = Windows::UI::Xaml::Controls;

using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Input;

namespace WaterFallView
{
	[Windows::Foundation::Metadata::WebHostHidden()]
	public ref class VirtualizingViewItem sealed :
		public WinCon::ContentControl
	{
		RegisterDependencyProperty(bool, _isSelectedProperty, IsSelectedProperty, IsSelected);

	public:
		VirtualizingViewItem();
		event DependencyPropertyChangedEventHandler^ IsSelectedChanged;

	protected:
		virtual void OnPointerPressed(PointerRoutedEventArgs^ e) override;
		virtual void OnPointerReleased(PointerRoutedEventArgs^ e) override;
		virtual void OnPointerExited(PointerRoutedEventArgs^ e) override;
		virtual void OnPointerCaptureLost(PointerRoutedEventArgs^ e) override;
		virtual void OnIsSelectedChanged(DependencyPropertyChangedEventArgs^ e);

	private:
		static void OnIsSelectedChangedStatic(DependencyObject^ sender, DependencyPropertyChangedEventArgs^ e);
		Windows::Foundation::EventRegistrationToken _sizeChangedToken;

	internal:
		RegisterProperty(Windows::Foundation::EventRegistrationToken, _sizeChangedToken, SizeChangedToken)
	};
}
