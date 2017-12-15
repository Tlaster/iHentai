//
// VirtualizingViewItem.cpp
// Implementation of the VirtualizingViewItem class.
//

#include "pch.h"
#include "VirtualizingViewItem.h"

using namespace WaterFallView;

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Documents;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Interop;
using namespace Windows::UI::Xaml::Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

DependencyProperty^ VirtualizingViewItem::_isSelectedProperty = DependencyProperty::Register(
    nameof(IsSelected),
    typeof(bool),
    typeof(VirtualizingViewItem),
    ref new PropertyMetadata(nullptr,
        ref new PropertyChangedCallback(
            &VirtualizingViewItem::OnIsSelectedChangedStatic)));

VirtualizingViewItem::VirtualizingViewItem()
{
    DefaultStyleKey = "WaterFallView.VirtualizingViewItem";
}

void VirtualizingViewItem::OnPointerPressed(PointerRoutedEventArgs^ e)
{
    VisualStateManager::GoToState(this, "PointerDown", true);
}

void VirtualizingViewItem::OnPointerReleased(PointerRoutedEventArgs^ e)
{
    VisualStateManager::GoToState(this, "PointerUp", true);
}

void VirtualizingViewItem::OnPointerExited(PointerRoutedEventArgs^ e)
{
    VisualStateManager::GoToState(this, "PointerUp", true);
}

void VirtualizingViewItem::OnPointerCaptureLost(PointerRoutedEventArgs^ e)
{
    VisualStateManager::GoToState(this, "PointerUp", true);
}

void VirtualizingViewItem::OnIsSelectedChangedStatic(DependencyObject^ sender, DependencyPropertyChangedEventArgs^ e)
{
    auto item = dynamic_cast<VirtualizingViewItem^>(sender);

    if (item != nullptr)
    {
        item->OnIsSelectedChanged(e);
    }
}

void VirtualizingViewItem::OnIsSelectedChanged(DependencyPropertyChangedEventArgs^ e)
{
    if ((bool)e->NewValue)
    {
        VisualStateManager::GoToState(this, "IsSelected", true);
    }
    else
    {
        VisualStateManager::GoToState(this, "NotSelected", true);
    }
    IsSelectedChanged(this, e);
}