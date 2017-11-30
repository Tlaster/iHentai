#include "pch.h"
#include "PhotowallView.h"

using namespace WaterFallView;

DependencyProperty^ PhotowallView::_spacingProperty = nullptr;
DependencyProperty^ PhotowallView::_unitSizeProperty = nullptr;

PhotowallView::PhotowallView()
{
    RegisterDependencyProperties();
}

void PhotowallView::RegisterDependencyProperties()
{
    VirtualizingPanel::RegisterDependencyProperties();

    if (_spacingProperty == nullptr)
    {
        _spacingProperty = DependencyProperty::Register(
            nameof(Spacing),
            typeof(double),
            typeof(PhotowallView),
            ref new PropertyMetadata(5.0,
                ref new PropertyChangedCallback(
                    &PhotowallView::OnSpacingChangedStatic)));
    }

    if (_unitSizeProperty == nullptr)
    {
        _unitSizeProperty = DependencyProperty::Register(
            nameof(UnitSize),
            typeof(int),
            typeof(PhotowallView),
            ref new PropertyMetadata(200.0,
                ref new PropertyChangedCallback(
                    &PhotowallView::OnUnitSizeChangedStatic)));
    }
}

Size PhotowallView::MeasureOverride(Size availableSize)
{
    Size result = OrientedVirtualizingPanel::MeasureOverride(availableSize);
    
    for (auto item : *(std::vector<Platform::Object^>*)(void*)VisableItems)
    {
        auto container = GetContainerFormItem(item);
        container->Measure(Photowall->GetItemSize(item));
    }

    return result;
}

Size PhotowallView::GetItemAvailableSize(Size availableSize)
{
    availableSize.Width = INFINITY;
    availableSize.Height = (float)UnitSize;
    return availableSize;
}

bool PhotowallView::NeedRelayout(Size availableSize)
{
    return OrientedVirtualizingPanel::NeedRelayout(availableSize) || Photowall->Spacing != Spacing || Photowall->UnitSize != UnitSize;
}

void PhotowallView::Relayout(Size availableSize)
{
    Photowall->ChangeSpacing(Spacing);
    Photowall->ChangeUnitSize(UnitSize);
    OrientedVirtualizingPanel::Relayout(availableSize);
}

ILayout^ PhotowallView::GetLayout(Size availableSize)
{
    return ref new PhotowallLayout(Spacing, availableSize.Width, UnitSize);
}


void PhotowallView::OnSpacingChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<PhotowallView^>(sender);

    if (panel == nullptr || panel->Photowall == nullptr)
    {
        return;
    }

    panel->InvalidateMeasure();
    panel->InvalidateArrange();
}

void PhotowallView::OnUnitSizeChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<PhotowallView^>(sender);

    if (panel == nullptr || panel->Photowall == nullptr)
    {
        return;
    }

    panel->InvalidateMeasure();
    panel->InvalidateArrange();
}