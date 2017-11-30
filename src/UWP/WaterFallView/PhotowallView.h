#pragma once
#include "OrientedVirtualizingPanel.h"
#include "IItemResizer.h"
#include "PhotowallLayout.h"

namespace WaterFallView
{
	[Windows::Foundation::Metadata::WebHostHidden()]
	public ref class PhotowallView sealed :
		public OrientedVirtualizingPanel
	{
		RegisterDependencyProperty(double, _spacingProperty, SpacingProperty, Spacing);
		RegisterDependencyProperty(double, _unitSizeProperty, UnitSizeProperty, UnitSize);

	public:
		PhotowallView();

	protected:
		virtual void RegisterDependencyProperties() override;
		virtual Size MeasureOverride(Size availableSize) override;
		virtual Size GetItemAvailableSize(Size availableSize) override;
		virtual bool NeedRelayout(Size availableSize) override;
		virtual void Relayout(Size availableSize) override;
		virtual ILayout^ GetLayout(Size availableSize) override;

	private:
		RegisterReadOnlyPropertyWithExpression(PhotowallLayout^, if (_photowallLayout == nullptr) { _photowallLayout = dynamic_cast<PhotowallLayout^>(Layout); } return _photowallLayout; , Photowall)
			PhotowallLayout^ _photowallLayout;
		static void OnSpacingChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);
		static void OnUnitSizeChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);
	};

}
