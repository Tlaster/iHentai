#pragma once

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
namespace WinCon = ::Windows::UI::Xaml::Controls;

namespace WaterFallView
{
	public enum class ItemSelectMode : int
	{
		None,
		Single,
		Multiple,
	};

	public enum class ItemTapMode : int
	{
		Left,
		LeftDouble,
		Right
	};

	[Windows::Foundation::Metadata::WebHostHidden()]
	public ref class ItemTappedEventArgs sealed
	{
	public:
		RegisterReadOnlyProperty(Platform::Object^, _item, Item);
		RegisterReadOnlyProperty(VirtualizingViewItem^, _container, Container);
		RegisterReadOnlyProperty(ItemTapMode, _tapMode, TapMode);

	internal:
		ItemTappedEventArgs(VirtualizingViewItem^ container, Platform::Object^ item, ItemTapMode mode);

	private:
		Platform::Object^ _item;
		VirtualizingViewItem^ _container;
		ItemTapMode _tapMode;
	};

	[Windows::Foundation::Metadata::WebHostHidden()]
	public ref class SeletionChangedEventArgs sealed
	{
	internal:
		SeletionChangedEventArgs();

	private:
	};

	[Windows::Foundation::Metadata::WebHostHidden()]
	public delegate void ItemTappedEventHandler(Platform::Object^ sender, ItemTappedEventArgs^ e);

	[Windows::Foundation::Metadata::WebHostHidden()]
	public delegate void SeletionChangedEventHandler(Platform::Object^ sender, SeletionChangedEventArgs^ e);

	[Windows::UI::Xaml::Markup::ContentPropertyAttribute(Name = "Items")]
	[Windows::Foundation::Metadata::WebHostHidden()]
	public ref class VirtualizingPanel :
		public ::WinCon::Panel
	{
		RegisterDependencyProperty(Windows::UI::Xaml::Style^, _itemContainerStyleProperty, ItemContainerStyleProperty, ItemContainerStyle);
		RegisterDependencyProperty(WinCon::StyleSelector^, _itemContainerStyleSelectorProperty, ItemContainerStyleSelectorProperty, ItemContainerStyleSelector);
		RegisterDependencyProperty(DataTemplate^, _itemTemplateProperty, ItemTemplateProperty, ItemTemplate);
		RegisterDependencyProperty(WinCon::DataTemplateSelector^, _itemTemplateSelectorProperty, ItemTemplateSelectorProperty, ItemTemplateSelector);
		RegisterDependencyProperty(Platform::Object^, _itemsSourceProperty, ItemsSourceProperty, ItemsSource);

		RegisterDependencyProperty(Windows::UI::Xaml::Style^, _headerContainerStyleProperty, HeaderContainerStyleProperty, HeaderContainerStyle);
		RegisterDependencyProperty(DataTemplate^, _headerTemplateProperty, HeaderTemplateProperty, HeaderTemplate);
		RegisterDependencyProperty(Platform::Object^, _headerProperty, HeaderProperty, Header);

		RegisterDependencyProperty(Windows::UI::Xaml::Style^, _footerContainerStyleProperty, FooterContainerStyleProperty, FooterContainerStyle);
		RegisterDependencyProperty(DataTemplate^, _footerTemplateProperty, FooterTemplateProperty, FooterTemplate);
		RegisterDependencyProperty(Platform::Object^, _footerProperty, FooterProperty, Footer);
	public:
		RegisterReadOnlyProperty(IVectorView<Platform::Object^>^, _selectedItems->GetView(), SelectedItems);
		RegisterProperty(ItemSelectMode, _selectMode, SelectMode);
		RegisterProperty(bool, _isShiftSelectEnable, IsShiftSelectEnable);
		RegisterProperty(bool, _isRightTapSelectEnable, IsRightTapSelectEnable);

		void BeginSelect();
		void EndSelect();
		VirtualizingViewItem^ GetContainerFormItem(Platform::Object^ item);
		VirtualizingViewItem^ GetContainerFormIndex(int index);
		Platform::Object^ GetItemFormContainer(VirtualizingViewItem^ container);
		Platform::Object^ GetItemFormIndex(int index);
		int GetIndexFormItem(Platform::Object^ item);

		event ItemTappedEventHandler^ ItemTapped;
		event SeletionChangedEventHandler^ SeletionChanged;

	internal:
		VirtualizingPanel();

	protected:
		RegisterReadOnlyProperty(IVector<VirtualizingViewItem^>^, _recycledContainers, RecycledContainers);
		RegisterReadOnlyProperty(IVector<Platform::Object^>^, _items, Items);
		RegisterReadOnlyProperty(WinCon::ContentControl^, _headerContainer, HeaderContainer);
		RegisterReadOnlyProperty(WinCon::ContentControl^, _footerContainer, FooterContainer);

		virtual void RegisterDependencyProperties();
		virtual bool IsItemItsOwnContainerOverride(Platform::Object^ obj);
		virtual VirtualizingViewItem^ GetContainerForItemOverride();
		virtual void ClearContainerForItemOverride(VirtualizingViewItem^ container, Object^ item);
		virtual void PrepareContainerForItemOverride(VirtualizingViewItem^ container, Object^ item);

		virtual void OnItemContainerStyleChanged(Windows::UI::Xaml::Style^ newStyle, Windows::UI::Xaml::Style^ oldStyle);
		virtual void OnItemContainerStyleSelectorChanged(WinCon::StyleSelector^ newStyleSelector, WinCon::StyleSelector^ oldStyleSelector);
		virtual void OnItemTemplateChanged(DataTemplate^ newTemplate, DataTemplate^ oldTemplate);
		virtual void OnItemTemplateSelectorChanged(WinCon::DataTemplateSelector^ newTemplateSelector, WinCon::DataTemplateSelector^ oldTemplateSelector);
		virtual void OnItemsChanged(IObservableVector<Platform::Object^>^ source, IVectorChangedEventArgs^ e);
		virtual void OnItemsSourceChanged(Platform::Object^ newItems, Platform::Object^ oldItems);
		virtual void OnSeletionChanged(IObservableVector<Platform::Object^>^ source, IVectorChangedEventArgs^ e);

		virtual void OnHeaderContainerStyleChanged(Windows::UI::Xaml::Style^ newStyle, Windows::UI::Xaml::Style^ oldStyle);
		virtual void OnHeaderTemplateChanged(DataTemplate^ newTemplate, DataTemplate^ oldTemplate);
		virtual void OnHeaderChanged(Platform::Object^ newHeader, Platform::Object^ oldHeader);

		virtual void OnFooterContainerStyleChanged(Windows::UI::Xaml::Style^ newStyle, Windows::UI::Xaml::Style^ oldStyle);
		virtual void OnFooterTemplateChanged(DataTemplate^ newTemplate, DataTemplate^ oldTemplate);
		virtual void OnFooterChanged(Platform::Object^ newFooter, Platform::Object^ oldFooter);

		virtual void OnHeaderMeasureOverride(Size availableSize);
		virtual void OnHeaderArrangeOverride(Size finalSize);

		virtual void OnFooterMeasureOverride(Size availableSize);
		virtual void OnFooterArrangeOverride(Size finalSize);

		virtual void OnItemContainerSizeChanged(Platform::Object^ item, VirtualizingViewItem^ itemContainer, Size newSize);

		void OnItemTapped(Object^ sender, Input::TappedRoutedEventArgs^ e);
		void OnItemDoubleTapped(Object^ sender, Input::DoubleTappedRoutedEventArgs^ e);
		void OnItemRightTapped(Object^ sender, Input::RightTappedRoutedEventArgs^ e);
		void OnKeyDown(Object^ sender, Input::KeyRoutedEventArgs^ e);
		void OnKeyUp(Object^ sender, Input::KeyRoutedEventArgs^ e);

		void RecycleItem(Platform::Object^ item);
		VirtualizingViewItem^ RealizeItem(Platform::Object^ item);
		void RecycleAllItem();

		void LoadMoreItems(int count);
		void LoadMoreItems();

	private:
		static void OnItemsSourceChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);
		static void OnItemTemplateChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);
		static void OnItemTemplateSelectorChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);
		static void OnItemContainerStyleChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);
		static void OnItemContainerStyleSelectorChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);

		static void OnHeaderContainerStyleChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);
		static void OnHeaderTemplateChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);
		static void OnHeaderChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);

		static void OnFooterContainerStyleChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);
		static void OnFooterTemplateChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);
		static void OnFooterChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);

		void ApplyItemContainerStyle(VirtualizingViewItem^ container, Platform::Object^ item);
		void ApplyItemTemplate(VirtualizingViewItem^ container, Platform::Object^ item);
		void HandleTapped(Platform::Object^ item, ItemTapMode tapMode);

		void CreateHeaderContainer();
		void CreateFooterContainer();

		EventRegistrationToken _collectionEventToken;
		void OnCollectionChanged(Platform::Object^ sender, Windows::UI::Xaml::Interop::NotifyCollectionChangedEventArgs^ e);

		Vector<VirtualizingViewItem^>^ _recycledContainers;
		UnorderedMap<Platform::Object^, VirtualizingViewItem^, HashObject>^ _itemContainerMap;
		Vector<Platform::Object^>^ _items;
		Windows::UI::Xaml::Data::ISupportIncrementalLoading^ _sil;
		int _loadCount = 0;
		bool _moreItemsLoading = false;

		RegisterReadOnlyProperty(bool, _shiftSelecting || _rightTapSelecting || _userSelecting, Selecting);

		Vector<Platform::Object^>^ _selectedItems;
		ItemSelectMode _selectMode;
		bool _shiftSelecting = false;
		bool _rightTapSelecting = false;
		bool _userSelecting = false;
		bool _isShiftSelectEnable = true;
		bool _isRightTapSelectEnable = true;

		WinCon::ContentControl^ _headerContainer;
		WinCon::ContentControl^ _footerContainer;
		void OnItemSizeChanged(Platform::Object ^sender, Windows::UI::Xaml::SizeChangedEventArgs ^e);
	};
}
