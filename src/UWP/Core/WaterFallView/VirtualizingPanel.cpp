#include "pch.h"
#include "VirtualizingPanel.h"
#include <ppltasks.h>

using namespace WaterFallView;
using namespace concurrency;

DependencyProperty^ VirtualizingPanel::_itemContainerStyleProperty = nullptr;
DependencyProperty^ VirtualizingPanel::_itemContainerStyleSelectorProperty = nullptr;
DependencyProperty^ VirtualizingPanel::_itemsSourceProperty = nullptr;
DependencyProperty^ VirtualizingPanel::_itemTemplateProperty = nullptr;
DependencyProperty^ VirtualizingPanel::_itemTemplateSelectorProperty = nullptr;

DependencyProperty^ VirtualizingPanel::_headerContainerStyleProperty = nullptr;
DependencyProperty^ VirtualizingPanel::_headerTemplateProperty = nullptr;
DependencyProperty^ VirtualizingPanel::_headerProperty = nullptr;

DependencyProperty^ VirtualizingPanel::_footerContainerStyleProperty = nullptr;
DependencyProperty^ VirtualizingPanel::_footerTemplateProperty = nullptr;
DependencyProperty^ VirtualizingPanel::_footerProperty = nullptr;

VirtualizingPanel::VirtualizingPanel()
{
    //RegisterDependencyProperties();
    _selectMode = ItemSelectMode::Multiple;
    _items = ref new Vector<Object^>();
    _selectedItems = ref new Vector<Object^>();
    _recycledContainers = ref new Vector<VirtualizingViewItem^>();
    _itemContainerMap = ref new UnorderedMap<Object^, VirtualizingViewItem^, HashObject>();

    _items->VectorChanged += ref new Windows::Foundation::Collections::VectorChangedEventHandler<Object ^>(this, &WaterFallView::VirtualizingPanel::OnItemsChanged);
    _selectedItems->VectorChanged += ref new Windows::Foundation::Collections::VectorChangedEventHandler<Platform::Object ^>(this, &WaterFallView::VirtualizingPanel::OnSeletionChanged);

    UIElement::AddHandler(UIElement::TappedEvent, ref new Input::TappedEventHandler(this, &VirtualizingPanel::OnItemTapped), true);
    UIElement::AddHandler(UIElement::DoubleTappedEvent, ref new Input::DoubleTappedEventHandler(this, &VirtualizingPanel::OnItemDoubleTapped), true);
    UIElement::AddHandler(UIElement::RightTappedEvent, ref new Input::RightTappedEventHandler(this, &VirtualizingPanel::OnItemRightTapped), true);
    UIElement::AddHandler(UIElement::KeyDownEvent, ref new Input::KeyEventHandler(this, &VirtualizingPanel::OnKeyDown), true);
    UIElement::AddHandler(UIElement::KeyUpEvent, ref new Input::KeyEventHandler(this, &VirtualizingPanel::OnKeyUp), true);

    ChildrenTransitions = ref new Media::Animation::TransitionCollection();
    ChildrenTransitions->Append(ref new Media::Animation::RepositionThemeTransition());
    ChildrenTransitions->Append(ref new Media::Animation::AddDeleteThemeTransition());
    ChildrenTransitions->Append(ref new Media::Animation::ReorderThemeTransition());
    ChildrenTransitions->Append(ref new Media::Animation::PaneThemeTransition());
    ChildrenTransitions->Append(ref new Media::Animation::EdgeUIThemeTransition());
}

void VirtualizingPanel::RegisterDependencyProperties()
{
    if (_itemContainerStyleProperty == nullptr)
    {
        _itemContainerStyleProperty = DependencyProperty::Register(
            nameof(ItemContainerStyle),
            typeof(Windows::UI::Xaml::Style),
            typeof(VirtualizingPanel),
            ref new PropertyMetadata(nullptr,
                ref new PropertyChangedCallback(
                    &VirtualizingPanel::OnItemContainerStyleChangedStatic)));
    }
    if (_itemContainerStyleSelectorProperty == nullptr)
    {
        _itemContainerStyleSelectorProperty = DependencyProperty::Register(
            nameof(ItemContainerStyleSelector),
            typeof(WinCon::StyleSelector),
            typeof(VirtualizingPanel),
            ref new PropertyMetadata(nullptr,
                ref new PropertyChangedCallback(
                    &VirtualizingPanel::OnItemContainerStyleChangedStatic)));
    }
    if (_itemsSourceProperty == nullptr)
    {
        _itemsSourceProperty = DependencyProperty::Register(
            nameof(ItemsSource),
            typeof(Object),
            typeof(VirtualizingPanel),
            ref new PropertyMetadata(nullptr,
                ref new PropertyChangedCallback(
                    &VirtualizingPanel::OnItemsSourceChangedStatic)));
    }
    if (_itemTemplateProperty == nullptr)
    {
        _itemTemplateProperty = DependencyProperty::Register(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(VirtualizingPanel),
            ref new PropertyMetadata(nullptr,
                ref new PropertyChangedCallback(
                    &VirtualizingPanel::OnItemTemplateChangedStatic)));
    }
    if (_itemTemplateSelectorProperty == nullptr)
    {
        _itemTemplateSelectorProperty = DependencyProperty::Register(
            nameof(ItemTemplateSelector),
            typeof(WinCon::DataTemplateSelector),
            typeof(VirtualizingPanel),
            ref new PropertyMetadata(nullptr,
                ref new PropertyChangedCallback(
                    &VirtualizingPanel::OnItemTemplateSelectorChangedStatic)));
    }
    if (_headerContainerStyleProperty == nullptr)
    {
        _headerContainerStyleProperty = DependencyProperty::Register(
            nameof(HeaderContainerStyle),
            typeof(Windows::UI::Xaml::Style),
            typeof(VirtualizingPanel),
            ref new PropertyMetadata(nullptr,
                ref new PropertyChangedCallback(
                    &VirtualizingPanel::OnHeaderContainerStyleChangedStatic)));
    }
    if (_headerTemplateProperty == nullptr)
    {
        _headerTemplateProperty = DependencyProperty::Register(
            nameof(HeaderTemplate),
            typeof(DataTemplate),
            typeof(VirtualizingPanel),
            ref new PropertyMetadata(nullptr,
                ref new PropertyChangedCallback(
                    &VirtualizingPanel::OnHeaderTemplateChangedStatic)));
    }
    if (_headerProperty == nullptr)
    {
        _headerProperty = DependencyProperty::Register(
            nameof(Header),
            typeof(Object),
            typeof(VirtualizingPanel),
            ref new PropertyMetadata(nullptr,
                ref new PropertyChangedCallback(
                    &VirtualizingPanel::OnHeaderChangedStatic)));
    }
    if (_footerContainerStyleProperty == nullptr)
    {
        _footerContainerStyleProperty = DependencyProperty::Register(
            nameof(FooterContainerStyle),
            typeof(Windows::UI::Xaml::Style),
            typeof(VirtualizingPanel),
            ref new PropertyMetadata(nullptr,
                ref new PropertyChangedCallback(
                    &VirtualizingPanel::OnFooterContainerStyleChangedStatic)));
    }
    if (_footerTemplateProperty == nullptr)
    {
        _footerTemplateProperty = DependencyProperty::Register(
            nameof(FooterTemplate),
            typeof(DataTemplate),
            typeof(VirtualizingPanel),
            ref new PropertyMetadata(nullptr,
                ref new PropertyChangedCallback(
                    &VirtualizingPanel::OnFooterTemplateChangedStatic)));
    }
    if (_footerProperty == nullptr)
    {
        _footerProperty = DependencyProperty::Register(
            nameof(Footer),
            typeof(Object),
            typeof(VirtualizingPanel),
            ref new PropertyMetadata(nullptr,
                ref new PropertyChangedCallback(
                    &VirtualizingPanel::OnFooterChangedStatic)));
    }
}

void VirtualizingPanel::OnItemTapped(Object^ sender, Input::TappedRoutedEventArgs^ e)
{
    if (e->OriginalSource == this || e->OriginalSource == HeaderContainer || e->OriginalSource == FooterContainer)
    {
        return;
    }

    auto original = dynamic_cast<FrameworkElement^>(e->OriginalSource);
    auto item = original->DataContext;

    if (item == nullptr)
    {
        return;
    }

    HandleTapped(item, ItemTapMode::Left);
}

void VirtualizingPanel::OnItemDoubleTapped(Object^ sender, Input::DoubleTappedRoutedEventArgs^ e)
{
    if (e->OriginalSource == this || e->OriginalSource == HeaderContainer || e->OriginalSource == FooterContainer)
    {
        return;
    }

    auto original = dynamic_cast<FrameworkElement^>(e->OriginalSource);
    auto item = original->DataContext;

    if (item == nullptr)
    {
        return;
    }

    HandleTapped(item, ItemTapMode::LeftDouble);
}


void VirtualizingPanel::OnItemRightTapped(Object^ sender, Input::RightTappedRoutedEventArgs^ e)
{
    if (e->OriginalSource == this || e->OriginalSource == HeaderContainer || e->OriginalSource == FooterContainer)
    {
        return;
    }

    auto original = dynamic_cast<FrameworkElement^>(e->OriginalSource);
    auto item = original->DataContext;

    if (item == nullptr)
    {
        return;
    }

    if (IsRightTapSelectEnable)
    {
        _rightTapSelecting = true;
    }
    HandleTapped(item, ItemTapMode::Right);
    if (IsRightTapSelectEnable)
    {
        _rightTapSelecting = false;
    }
}

void VirtualizingPanel::HandleTapped(Platform::Object^ item, ItemTapMode tapMode)
{
    unsigned int index = 0;
    if (!Items->IndexOf(item, &index))
    {
        return;
    }

    auto container = GetContainerFormItem(item);

    switch (_selectMode)
    {
    case ItemSelectMode::Single:
        if (Selecting)
        {
            if (_selectedItems->Size == 1 && _selectedItems->IndexOf(item, &index))
            {
                if (container != nullptr)
                {
                    container->IsSelected = false;
                }
                _selectedItems->Clear();
            }
            else
            {
                for each (auto i in _selectedItems)
                {
                    auto c = GetContainerFormItem(i);
                    if (c != nullptr)
                    {
                        c->IsSelected = false;
                    }
                }
                _selectedItems->Clear();
                _selectedItems->Append(item);
            }
            break;
        }
    case ItemSelectMode::Multiple:
        if (Selecting)
        {
            if (_selectedItems->IndexOf(item, &index))
            {
                if (container != nullptr)
                {
                    container->IsSelected = false;
                }
                _selectedItems->RemoveAt(index);
            }
            else
            {
                if (container != nullptr)
                {
                    container->IsSelected = true;
                }
                _selectedItems->Append(item);
            }
            break;
        }
    case ItemSelectMode::None:
    default:
        ItemTapped(this, ref new ItemTappedEventArgs(container, item, tapMode));
        break;
    }

}

void VirtualizingPanel::OnKeyDown(Object^ sender, Input::KeyRoutedEventArgs^ e)
{
    if (IsShiftSelectEnable && (e->Key | Windows::System::VirtualKey::Shift) == Windows::System::VirtualKey::Shift)
    {
        _shiftSelecting = true;
    }
}

void VirtualizingPanel::OnKeyUp(Object^ sender, Input::KeyRoutedEventArgs^ e)
{
    if (IsShiftSelectEnable && (e->Key | Windows::System::VirtualKey::Shift) == Windows::System::VirtualKey::Shift)
    {
        _shiftSelecting = false;
    }
}

bool VirtualizingPanel::IsItemItsOwnContainerOverride(Object^ obj)
{
    auto container = dynamic_cast<VirtualizingViewItem^>(obj);
    return container != nullptr;
}

VirtualizingViewItem^ VirtualizingPanel::GetContainerForItemOverride()
{
    return ref new VirtualizingViewItem();
}

void VirtualizingPanel::ClearContainerForItemOverride(VirtualizingViewItem^ container, Object^ item)
{
    if (IsItemItsOwnContainerOverride(item))
    {
        return;
    }

    container->Content = nullptr;
    container->ContentTemplate = nullptr;
    container->Style = nullptr;
    container->IsSelected = false;
}

void VirtualizingPanel::PrepareContainerForItemOverride(VirtualizingViewItem^ container, Object^ item)
{
    if (IsItemItsOwnContainerOverride(item))
    {
        return;
    }

    container->Content = item;

    ApplyItemContainerStyle(container, item);
    ApplyItemTemplate(container, item);

    unsigned int index = 0;
    container->IsSelected = SelectedItems->IndexOf(item, &index);
}

VirtualizingViewItem^ VirtualizingPanel::GetContainerFormItem(Object^ item)
{
    if (_itemContainerMap->HasKey(item))
    {
        return _itemContainerMap->Lookup(item);
    }
    else
    {
        return nullptr;
    }
}

VirtualizingViewItem^ VirtualizingPanel::GetContainerFormIndex(int index)
{
    if (index < 0 || (UINT)index >= _items->Size)
    {
        throw ref new OutOfBoundsException("Index out of bounds.");
    }

    auto item = _items->GetAt(index);
    return GetContainerFormItem(item);
}

Object^ VirtualizingPanel::GetItemFormContainer(VirtualizingViewItem^ container)
{
    if (container == nullptr)
    {
        return nullptr;
    }

    auto item = container->Content;

    if (_itemContainerMap->HasKey(item))
    {
        return item;
    }
    else
    {
        return nullptr;
    }
}

Object^ VirtualizingPanel::GetItemFormIndex(int index)
{
    if (index < 0 || (UINT)index >= _items->Size)
    {
        throw ref new OutOfBoundsException("Index out of bounds.");
    }
	return _items->GetAt(index);
}

int VirtualizingPanel::GetIndexFormItem(Platform::Object^ item) 
{
	if (item == nullptr)
	{
		throw ref new InvalidArgumentException();
	}
	unsigned int index;
	_items->IndexOf(item, &index);
	return index;
}

void VirtualizingPanel::OnItemsSourceChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<VirtualizingPanel^>(sender);

    if (panel == nullptr)
    {
        return;
    }

    panel->OnItemsSourceChanged(e->NewValue, e->OldValue);
}

void VirtualizingPanel::OnItemTemplateChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<VirtualizingPanel^>(sender);

    if (panel == nullptr)
    {
        return;
    }

    panel->OnItemTemplateChanged(dynamic_cast<DataTemplate^>(e->NewValue), dynamic_cast<DataTemplate^>(e->OldValue));
}

void VirtualizingPanel::OnItemTemplateSelectorChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<VirtualizingPanel^>(sender);

    if (panel == nullptr)
    {
        return;
    }

    panel->OnItemTemplateSelectorChanged(dynamic_cast<WinCon::DataTemplateSelector^>(e->NewValue), dynamic_cast<WinCon::DataTemplateSelector^>(e->OldValue));
}

void VirtualizingPanel::OnItemContainerStyleChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<VirtualizingPanel^>(sender);

    if (panel == nullptr)
    {
        return;
    }

    panel->OnItemContainerStyleChanged(dynamic_cast<Windows::UI::Xaml::Style^>(e->NewValue), dynamic_cast<Windows::UI::Xaml::Style^>(e->OldValue));
}

void VirtualizingPanel::OnItemContainerStyleSelectorChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<VirtualizingPanel^>(sender);

    if (panel == nullptr)
    {
        return;
    }

    panel->OnItemContainerStyleSelectorChanged(dynamic_cast<WinCon::StyleSelector^>(e->NewValue), dynamic_cast<WinCon::StyleSelector^>(e->OldValue));
}

void VirtualizingPanel::OnHeaderContainerStyleChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<VirtualizingPanel^>(sender);

    if (panel == nullptr)
    {
        return;
    }

    panel->OnHeaderContainerStyleChanged(dynamic_cast<Windows::UI::Xaml::Style^>(e->NewValue), dynamic_cast<Windows::UI::Xaml::Style^>(e->OldValue));
}

void VirtualizingPanel::OnHeaderTemplateChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<VirtualizingPanel^>(sender);

    if (panel == nullptr)
    {
        return;
    }

    panel->OnHeaderTemplateChanged(dynamic_cast<Windows::UI::Xaml::DataTemplate^>(e->NewValue), dynamic_cast<Windows::UI::Xaml::DataTemplate^>(e->OldValue));
}

void VirtualizingPanel::OnHeaderChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<VirtualizingPanel^>(sender);

    if (panel == nullptr)
    {
        return;
    }

    panel->OnHeaderChanged(e->NewValue, e->OldValue);
}

void VirtualizingPanel::OnFooterContainerStyleChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<VirtualizingPanel^>(sender);

    if (panel == nullptr)
    {
        return;
    }

    panel->OnFooterContainerStyleChanged(dynamic_cast<Windows::UI::Xaml::Style^>(e->NewValue), dynamic_cast<Windows::UI::Xaml::Style^>(e->OldValue));
}

void VirtualizingPanel::OnFooterTemplateChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<VirtualizingPanel^>(sender);

    if (panel == nullptr)
    {
        return;
    }

    panel->OnFooterTemplateChanged(dynamic_cast<Windows::UI::Xaml::DataTemplate^>(e->NewValue), dynamic_cast<Windows::UI::Xaml::DataTemplate^>(e->OldValue));
}

void VirtualizingPanel::OnFooterChangedStatic(DependencyObject^ sender, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e)
{
    auto panel = dynamic_cast<VirtualizingPanel^>(sender);

    if (panel == nullptr)
    {
        return;
    }

    panel->OnFooterChanged(e->NewValue, e->OldValue);
}

void VirtualizingPanel::OnItemContainerStyleChanged(Windows::UI::Xaml::Style^ newStyle, Windows::UI::Xaml::Style^ oldStyle)
{
    for each (auto var in _itemContainerMap)
    {
        ApplyItemContainerStyle(var->Value, var->Key);
    }
}

void VirtualizingPanel::OnItemContainerStyleSelectorChanged(WinCon::StyleSelector^ newStyleSelector, WinCon::StyleSelector^ oldStyleSelector)
{
    for each (auto var in _itemContainerMap)
    {
        ApplyItemContainerStyle(var->Value, var->Key);
    }
}

void VirtualizingPanel::OnItemTemplateChanged(DataTemplate^ newTemplate, DataTemplate^ oldTemplate)
{
    for each (auto var in _itemContainerMap)
    {
        ApplyItemTemplate(var->Value, var->Key);
    }
}

void VirtualizingPanel::OnItemTemplateSelectorChanged(WinCon::DataTemplateSelector^ newTemplateSelector, WinCon::DataTemplateSelector^ oldTemplateSelector)
{
    for each (auto var in _itemContainerMap)
    {
        ApplyItemTemplate(var->Value, var->Key);
    }
}

void VirtualizingPanel::OnItemsSourceChanged(Object^ newItems, Object^ oldItems)
{
    this->RecycleAllItem();
    _itemContainerMap->Clear();
    _items->Clear();

    auto items = dynamic_cast<Windows::UI::Xaml::Interop::IBindableIterable^>(newItems);

    if (items != nullptr)
    {
        auto itertor = items->First();

        while (itertor->HasCurrent)
        {
            _items->Append(itertor->Current);
            itertor->MoveNext();
        }
    }
    else
    {
        _items->Append(newItems);
    }

    auto nc = dynamic_cast<Windows::UI::Xaml::Interop::INotifyCollectionChanged^>(oldItems);
    if (nc != nullptr)
    {
        nc->CollectionChanged -= _collectionEventToken;
    }

    nc = dynamic_cast<Windows::UI::Xaml::Interop::INotifyCollectionChanged^>(newItems);
    if (nc != nullptr)
    {
        _collectionEventToken = nc->CollectionChanged += ref new Windows::UI::Xaml::Interop::NotifyCollectionChangedEventHandler(this, &WaterFallView::VirtualizingPanel::OnCollectionChanged);
    }

    _sil = nullptr;
    _loadCount = 0;
    _sil = dynamic_cast<Windows::UI::Xaml::Data::ISupportIncrementalLoading^>(newItems);
}

void VirtualizingPanel::OnCollectionChanged(Object^ sender, Windows::UI::Xaml::Interop::NotifyCollectionChangedEventArgs^ e)
{
    Windows::UI::Xaml::Interop::IBindableIterator^ newItertor = nullptr;
    int newIndex = -1;

    if (e->NewItems != nullptr)
    {
        newItertor = e->NewItems->First();
        newIndex = e->NewStartingIndex;
    }

    switch (e->Action)
    {
    case Windows::UI::Xaml::Interop::NotifyCollectionChangedAction::Add:
        while (newItertor->HasCurrent)
        {
            _items->InsertAt(newIndex++, newItertor->Current);
            newItertor->MoveNext();
        }
        break;
    case Windows::UI::Xaml::Interop::NotifyCollectionChangedAction::Move:
        throw Exception::CreateException(-1, "Unexpected collection operation.");
        break;
    case Windows::UI::Xaml::Interop::NotifyCollectionChangedAction::Remove:
        for (int i = 0; i < (LONGLONG)e->OldItems->Size; i++)
        {
            RecycleItem(e->OldItems->GetAt(i));
            _items->RemoveAt(e->OldStartingIndex + i);
        }
        break;
    case Windows::UI::Xaml::Interop::NotifyCollectionChangedAction::Replace:
        while (newItertor->HasCurrent)
        {
            _items->SetAt(newIndex++, newItertor->Current);
            newItertor->MoveNext();
        }
        break;
    case Windows::UI::Xaml::Interop::NotifyCollectionChangedAction::Reset:
        _items->Clear();
        break;
    default:
        throw Exception::CreateException(-1, "Unexpected collection operation.");
        break;
    }
}

void VirtualizingPanel::OnItemsChanged(IObservableVector<Object^>^ source, IVectorChangedEventArgs^ e)
{
    return;
}

void  VirtualizingPanel::OnSeletionChanged(IObservableVector<Object^>^ source, IVectorChangedEventArgs^ e)
{
    SeletionChanged(this, ref new SeletionChangedEventArgs());
}

void VirtualizingPanel::ApplyItemContainerStyle(VirtualizingViewItem^ container, Object^ item)
{
    if (ItemContainerStyleSelector != nullptr)
    {
        container->Style = ItemContainerStyleSelector->SelectStyle(item, container);
    }

    if (container->Style == nullptr)
    {
        container->Style = ItemContainerStyle;
    }
}

void VirtualizingPanel::ApplyItemTemplate(VirtualizingViewItem^ container, Object^ item)
{
    if (ItemTemplateSelector != nullptr)
    {
        container->ContentTemplate = ItemTemplateSelector->SelectTemplate(item);
    }

    if (container->ContentTemplate == nullptr)
    {
        container->ContentTemplate = ItemTemplate;
    }
}

void VirtualizingPanel::RecycleItem(Object^ item)
{
    auto container = GetContainerFormItem(item);

    if (container == nullptr)
    {
        return;
    }

    unsigned int index = 0;
    if (Children->IndexOf(container, &index))
    {
        Children->RemoveAt(index);
        _itemContainerMap->Remove(item);
        ClearContainerForItemOverride(container, item);

        container->SizeChanged -= container->SizeChangedToken;

        if (!IsItemItsOwnContainerOverride(item))
        {
            RecycledContainers->Append(container);
        }
    }
    else
    {
        throw Exception::CreateException(-1, "Can't found container in panel.");
    }
}

VirtualizingViewItem^  VirtualizingPanel::RealizeItem(Object^ item)
{
    VirtualizingViewItem^ container = nullptr;

    if (_itemContainerMap->HasKey(item))
    {
        return _itemContainerMap->Lookup(item);
    }

    if (!IsItemItsOwnContainerOverride(item))
    {
        if (RecycledContainers->Size > 0)
        {
            container = RecycledContainers->GetAt(RecycledContainers->Size - 1);
            RecycledContainers->RemoveAtEnd();
        }
        else
        {
            container = GetContainerForItemOverride();
        }
    }
    else
    {
        container = dynamic_cast<VirtualizingViewItem^>(item);
    }

    PrepareContainerForItemOverride(container, item);
    _itemContainerMap->Insert(item, container);
    container->SizeChangedToken = container->SizeChanged += ref new Windows::UI::Xaml::SizeChangedEventHandler(this, &WaterFallView::VirtualizingPanel::OnItemSizeChanged);
    Children->Append(container);

    return container;
}

void VirtualizingPanel::RecycleAllItem()
{
    Vector<Platform::Object^>^ items = ref new Vector<Platform::Object^>();

    for each (auto item in _itemContainerMap)
    {
        items->Append(item->Key);
    }

    for each (auto item in items)
    {
        RecycleItem(item);
    }
}

void VirtualizingPanel::LoadMoreItems(int count)
{
    if (_sil != nullptr)
    {
        if (_sil->HasMoreItems && !_moreItemsLoading)
        {
            _moreItemsLoading = true;
            concurrency::create_task(_sil->LoadMoreItemsAsync(count)).then([this](Windows::UI::Xaml::Data::LoadMoreItemsResult result)
            {
                _moreItemsLoading = false;
            });
        }
    }
}

void VirtualizingPanel::LoadMoreItems()
{
    LoadMoreItems(_loadCount++);
}

void VirtualizingPanel::BeginSelect()
{
    _userSelecting = true;
}

void VirtualizingPanel::EndSelect()
{
    _userSelecting = false;
}

void VirtualizingPanel::CreateHeaderContainer()
{
    if (_headerContainer == nullptr && Header != nullptr)
    {
        _headerContainer = ref new WinCon::ContentControl();
        _headerContainer->Style = HeaderContainerStyle;
        _headerContainer->ContentTemplate = HeaderTemplate;
        _headerContainer->Content = Header;
        _headerContainer->HorizontalContentAlignment = Windows::UI::Xaml::HorizontalAlignment::Stretch;
        Children->InsertAt(0, _headerContainer);
    }
}

void VirtualizingPanel::CreateFooterContainer()
{
    if (_footerContainer == nullptr && Footer != nullptr)
    {
        _footerContainer = ref new WinCon::ContentControl();
        _footerContainer->Style = FooterContainerStyle;
        _footerContainer->ContentTemplate = FooterTemplate;
        _footerContainer->Content = Footer;
        _footerContainer->HorizontalContentAlignment = Windows::UI::Xaml::HorizontalAlignment::Stretch;
        Children->InsertAt(0, _footerContainer);
    }
}

void VirtualizingPanel::OnHeaderContainerStyleChanged(Windows::UI::Xaml::Style^ newStyle, Windows::UI::Xaml::Style^ oldStyle)
{
    if (newStyle == oldStyle) return;

    if (_headerContainer == nullptr)
    {
        CreateHeaderContainer();
    }
    else
    {
        _headerContainer->Style = newStyle;
    }

    InvalidateMeasure();
    InvalidateArrange();
}

void VirtualizingPanel::OnHeaderTemplateChanged(DataTemplate^ newTemplate, DataTemplate^ oldTemplate)
{
    if (newTemplate == oldTemplate) return;

    if (_headerContainer == nullptr)
    {
        CreateHeaderContainer();
    }
    else
    {
        _headerContainer->ContentTemplate = newTemplate;
    }

    InvalidateMeasure();
    InvalidateArrange();
}

void VirtualizingPanel::OnHeaderChanged(Platform::Object^ newHeader, Platform::Object^ oldHeader)
{
    if (newHeader == oldHeader) return;

    if (newHeader == nullptr)
    {
        unsigned int index;
        Children->IndexOf(_headerContainer, &index);
        Children->RemoveAt(index);
        _headerContainer = nullptr;
        return;
    }

    if (_headerContainer == nullptr)
    {
        CreateHeaderContainer();
    }
    else
    {
        _headerContainer->Content = newHeader;
    }

    InvalidateMeasure();
    InvalidateArrange();
}

void VirtualizingPanel::OnFooterContainerStyleChanged(Windows::UI::Xaml::Style^ newStyle, Windows::UI::Xaml::Style^ oldStyle)
{
    if (newStyle == oldStyle) return;

    if (_footerContainer == nullptr)
    {
        CreateFooterContainer();
    }
    else
    {
        _footerContainer->Style = newStyle;
    }

    InvalidateMeasure();
    InvalidateArrange();
}

void VirtualizingPanel::OnFooterTemplateChanged(DataTemplate^ newTemplate, DataTemplate^ oldTemplate)
{
    if (newTemplate == oldTemplate) return;

    if (_footerContainer == nullptr)
    {
        CreateFooterContainer();
    }
    else
    {
        _footerContainer->ContentTemplate = newTemplate;
    }

    InvalidateMeasure();
    InvalidateArrange();
}

void VirtualizingPanel::OnFooterChanged(Platform::Object^ newFooter, Platform::Object^ oldFooter)
{
    if (newFooter == oldFooter) return;

    if (newFooter == nullptr)
    {
        unsigned int index;
        Children->IndexOf(_footerContainer, &index);
        Children->RemoveAt(index);
        _footerContainer = nullptr;
        return;
    }

    if (_footerContainer == nullptr)
    {
        CreateFooterContainer();
    }
    else
    {
        _footerContainer->Content = newFooter;
    }

    InvalidateMeasure();
    InvalidateArrange();
}

void VirtualizingPanel::OnHeaderMeasureOverride(Size availableSize)
{

}

void VirtualizingPanel::OnHeaderArrangeOverride(Size finalSize)
{

}

void VirtualizingPanel::OnFooterMeasureOverride(Size availableSize)
{

}

void VirtualizingPanel::OnFooterArrangeOverride(Size finalSize)
{

}

void VirtualizingPanel::OnItemContainerSizeChanged(Platform::Object^ item, VirtualizingViewItem^ itemContainer, Size newSize)
{

}

ItemTappedEventArgs::ItemTappedEventArgs(VirtualizingViewItem^ container, Platform::Object^ item, ItemTapMode mode)
{
    _container = container;
    _item = item;
    _tapMode = mode;
}

SeletionChangedEventArgs::SeletionChangedEventArgs()
{

}

void VirtualizingPanel::OnItemSizeChanged(Platform::Object ^sender, Windows::UI::Xaml::SizeChangedEventArgs ^e)
{
    auto container = dynamic_cast<VirtualizingViewItem^> (sender);
    OnItemContainerSizeChanged(GetItemFormContainer(container), container, e->NewSize);
}
