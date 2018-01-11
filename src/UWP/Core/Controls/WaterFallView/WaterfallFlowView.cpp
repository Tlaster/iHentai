#include "pch.h"
#include "WaterfallFlowView.h"

using namespace WaterFallView;

DependencyProperty^ WaterfallFlowView::_spacingProperty = nullptr;
DependencyProperty^ WaterfallFlowView::_stackCountProperty = nullptr;
DependencyProperty^ WaterfallFlowView::_adaptiveModeProperty = nullptr;
DependencyProperty^ WaterfallFlowView::_maxItemWidthProperty = nullptr;
DependencyProperty^ WaterfallFlowView::_minItemWidthProperty = nullptr;



WaterfallFlowView::WaterfallFlowView()
{
    RegisterDependencyProperties();
}

void WaterfallFlowView::RegisterDependencyProperties()
{
    VirtualizingPanel::RegisterDependencyProperties();
    if (_spacingProperty == nullptr)
    {
        _spacingProperty = DependencyProperty::Register(
            nameof(Spacing),
            typeof(double),
            typeof(WaterfallFlowView),
            ref new PropertyMetadata(5.0,
                ref new PropertyChangedCallback(
                    &WaterfallFlowView::OnSpacingChangedStatic)));
    }

    if (_stackCountProperty == nullptr)
    {
        _stackCountProperty = DependencyProperty::Register(
            nameof(StackCount),
            typeof(int),
            typeof(WaterfallFlowView),
            ref new PropertyMetadata(2,
                ref new PropertyChangedCallback(
                    &WaterfallFlowView::OnStackCountChangedStatic)));
    }

    if (_adaptiveModeProperty == nullptr)
    {
        _adaptiveModeProperty = DependencyProperty::Register(
            nameof(AdaptiveMode),
            typeof(WaterFallView::AdaptiveMode),
            typeof(WaterfallFlowView),
            ref new PropertyMetadata(WaterFallView::AdaptiveMode::Disable,
                ref new PropertyChangedCallback(
                    &WaterfallFlowView::OnAdaptiveModeChangedStatic)));
    }

    if (_maxItemWidthProperty == nullptr)
    {
        _maxItemWidthProperty = DependencyProperty::Register(
            nameof(MaxItemWidth),
            typeof(int),
            typeof(WaterfallFlowView),
            ref new PropertyMetadata(300,
                ref new PropertyChangedCallback(
                    &WaterfallFlowView::OnMaxItemWidthChangedStatic)));
    }

    if (_minItemWidthProperty == nullptr)
    {
        _minItemWidthProperty = DependencyProperty::Register(
            nameof(MinItemWidth),
            typeof(int),
            typeof(WaterfallFlowView),
            ref new PropertyMetadata(200,
                ref new PropertyChangedCallback(
                    &WaterfallFlowView::OnMinItemWidthChangedStatic)));
    }
}

Size WaterfallFlowView::GetItemAvailableSize(Size availableSize)
{
    availableSize.Width = (float)((availableSize.Width - ((StackCount - 1) * Spacing)) / StackCount);
    return availableSize;
}

bool WaterfallFlowView::NeedRelayout(Size availableSize)
{
    ResetStackCount(availableSize);
    return OrientedVirtualizingPanel::NeedRelayout(availableSize) || WaterfallFlow->Spacing != Spacing || WaterfallFlow->StackCount != StackCount;
}

void WaterfallFlowView::Relayout(Size availableSize)
{
    WaterfallFlow->ChangeSpacing(Spacing);
    WaterfallFlow->ChangeStackCount(StackCount);
    OrientedVirtualizingPanel::Relayout(availableSize);
}

ILayout^ WaterfallFlowView::GetLayout(Size availableSize)
{
    return ref new WaterfallFlowLayout(Spacing, availableSize.Width, StackCount);
}

void WaterfallFlowView::OnSpacingChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<WaterfallFlowView^>(sender);

    if (panel == nullptr || panel->WaterfallFlow == nullptr)
    {
        return;
    }

    panel->InvalidateMeasure();
    panel->InvalidateArrange();
}

void WaterfallFlowView::OnStackCountChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<WaterfallFlowView^>(sender);

    if (panel == nullptr || panel->WaterfallFlow == nullptr)
    {
        return;
    }

    panel->InvalidateMeasure();
    panel->InvalidateArrange();
}

void WaterfallFlowView::OnAdaptiveModeChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<WaterfallFlowView^>(sender);

    if (panel == nullptr || panel->WaterfallFlow == nullptr)
    {
        return;
    }

    switch (panel->AdaptiveMode)
    {
    default:
    case WaterFallView::AdaptiveMode::Disable:
        break;
    case WaterFallView::AdaptiveMode::MaxBased:
    case WaterFallView::AdaptiveMode::MinBased:
        panel->ResetStackCount();
        break;
    }
}

void WaterfallFlowView::OnMaxItemWidthChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<WaterfallFlowView^>(sender);

    if (panel == nullptr || panel->WaterfallFlow == nullptr)
    {
        return;
    }

    switch (panel->AdaptiveMode)
    {
    default:
    case WaterFallView::AdaptiveMode::Disable:
    case WaterFallView::AdaptiveMode::MinBased:
        break;
    case WaterFallView::AdaptiveMode::MaxBased:
        panel->ResetStackCount();
        break;
    }
}

void WaterfallFlowView::OnMinItemWidthChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<WaterfallFlowView^>(sender);

    if (panel == nullptr || panel->WaterfallFlow == nullptr)
    {
        return;
    }

    switch (panel->AdaptiveMode)
    {
    default:
    case WaterFallView::AdaptiveMode::Disable:
    case WaterFallView::AdaptiveMode::MaxBased:
        break;
    case WaterFallView::AdaptiveMode::MinBased:
        panel->ResetStackCount();
        break;
    }
}

void WaterfallFlowView::ResetStackCount()
{
    ResetStackCount(Size((float)ActualWidth, (float)ActualWidth));
}


void WaterfallFlowView::ResetStackCount(Size availableSize)
{
    float width = availableSize.Width;
    switch (this->AdaptiveMode)
    {
    default:
    case WaterFallView::AdaptiveMode::Disable:
        break;
    case WaterFallView::AdaptiveMode::MinBased:
    {
        int maxStackCount = (int)((width + Spacing) / (MinItemWidth + Spacing));
        StackCount = max(maxStackCount, 1);
    }
    break;
    case WaterFallView::AdaptiveMode::MaxBased:
    {
        int minStackCount = (int)((width + Spacing) / (MaxItemWidth + Spacing));
        StackCount = max(minStackCount, 1);
    }
    break;
    }
}
