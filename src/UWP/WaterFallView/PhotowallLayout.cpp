#include "pch.h"
#include "PhotowallLayout.h"

using namespace WaterFallView;

PhotowallLayout::PhotowallLayout(double spacing, double width, double unitSize)
{
    _spacing = spacing;
    _width = width;
    _unitSize = unitSize;
    _units = new std::vector<PhotowallUnit^>();
    _itemUnitMap = new std::unordered_map<Platform::Object^, PhotowallUnit^, HashObject>();
}

PhotowallLayout::~PhotowallLayout()
{
    delete(_units);
    delete(_itemUnitMap);
}

void PhotowallLayout::AddItem(int index, Platform::Object^ item, Size size)
{
    size.Height = (float)_unitSize;
    auto unit = ref new PhotowallUnit(item, size);

    (*_itemUnitMap)[item] = unit;

    if (index >= 0 && index < (LONGLONG)_units->size())
    {
        _units->insert(_units->begin() + index, unit);
        SetRelayoutIndex(index);
    }
    else
    {
        if (!(_requestRelayoutIndex < 0 || _requestRelayoutIndex >= (LONGLONG)_units->size()))
        {
            Relayout();
        }

        _units->push_back(unit);

        bool isNeedNext = abs(_width - _offset) > abs(_width - (_offset + size.Width + (_offset == 0 ? 0 : _spacing)));

        _lastRowLocked = false;
        if (isNeedNext || _offset * 2 < _width)
        {
            unit->Offset = _offset + (_offset == 0 ? 0 : _spacing);
            unit->RowIndex = _rowIndex;
            _offset += size.Width + (_offset == 0 ? 0 : _spacing);
        }
        else
        {
            _rowIndex++;
            _offset = 0;

            unit->Offset = 0;
            unit->RowIndex = _rowIndex;
            _offset += size.Width;

            RelayoutRow((int)_units->size() - 2);
        }


        if (_offset >= _width)
        {
            RelayoutRow((int)_units->size() - 1);
            _rowIndex++;
            _offset = 0;
            _lastRowLocked = true;
        }
    }
}

void PhotowallLayout::RelayoutRow(int itemIndex)
{
    int rowFirstItemIndex = -1, rowLastItemIndex = -1;

    for (int i = itemIndex; i >= 0; i--)
    {
        if (_units->at(i)->RowIndex == _units->at(itemIndex)->RowIndex - 1)
        {
            rowFirstItemIndex = i + 1;
            break;
        }
    }

    if (rowFirstItemIndex < 0)
    {
        rowFirstItemIndex = 0;
    }

    for (int i = itemIndex; i < (LONGLONG)_units->size(); i++)
    {
        if (_units->at(i)->RowIndex == _units->at(itemIndex)->RowIndex + 1)
        {
            rowLastItemIndex = i - 1;
            break;
        }
    }

    if (rowLastItemIndex < 0)
    {
        rowLastItemIndex = (int)_units->size() - 1;
    }

    double newOffset = 0;
    double rowLength = _units->at(rowLastItemIndex)->Offset + _units->at(rowLastItemIndex)->DesiredSize.Width;

    for (int i = rowFirstItemIndex; i <= rowLastItemIndex; i++)
    {
        auto unit = _units->at(i);

        double overloadLength = rowLength - (Spacing * (rowLastItemIndex - rowFirstItemIndex));
        double itemLength = _width - (Spacing * (rowLastItemIndex - rowFirstItemIndex));
        double actualWidth = unit->DesiredSize.Width / overloadLength * itemLength;
        actualWidth = (int)(actualWidth + 0.5);

        unit->ActualSize = Size((float)actualWidth, (float)unit->DesiredSize.Height);
        unit->ActualOffset = newOffset;
        newOffset += actualWidth + _spacing;
    }

    newOffset -= _spacing;
    if (newOffset != _width)
    {
        _units->at(rowLastItemIndex)->ActualSize = Size((float)(_width - newOffset + _units->at(rowLastItemIndex)->ActualSize.Width),(float)( _units->at(rowLastItemIndex)->ActualSize.Height));
    }
}

LONGLONG PhotowallLayout::GetVisableItems(VisualWindow window, int* firstIndex, int * lastIndex)
{
    if (!(_requestRelayoutIndex < 0 || _requestRelayoutIndex >= (LONGLONG)_units->size()))
    {
        Relayout();
    }

    window.Offset -= _headerSize.Height;
    std::vector<Platform::Object^>* result = new std::vector<Platform::Object^>();

    if (_units->size() == 0)
    {
        *firstIndex = -1;
        *lastIndex = -1;
        return (LONGLONG)result;
    }

    int firstRowIndex, lastRowIndex, visableRowCount, newFirstIndex = -1, newLastIndex = -1;

    firstRowIndex = (int)floor((window.Offset + _spacing) / (_unitSize + _spacing));
    visableRowCount = (int)floor((window.Length + _spacing) / (_unitSize + _spacing));
    lastRowIndex = (int)floor((VisualWindowExtension::GetEndOffset(window) + _spacing) / (_unitSize + _spacing));

    int firstRow = 0;
    int lastRow = _units->at(_units->size() - 1)->RowIndex;

    if (lastRow - firstRow + 1 < visableRowCount)
    {
        firstRowIndex = 0;
        lastRowIndex = lastRow;
    }
    else
    {
        if (firstRowIndex > lastRow - visableRowCount + 1)
        {
            firstRowIndex = lastRow - visableRowCount + 1;
        }

        if (firstRowIndex < 0)
        {
            firstRowIndex = 0;
        }
    }

    lastRowIndex = firstRowIndex + visableRowCount - 1;

    if (firstRowIndex)

        if (*firstIndex < 0)
        {
            for (int i = 0; i < (LONGLONG)_units->size(); i++)
            {
                if (_units->at(i)->RowIndex == firstRowIndex)
                {
                    newFirstIndex = i;
                    break;
                }
            }
        }
        else
        {
            if (_units->at(*firstIndex)->RowIndex < firstRowIndex)
            {
                for (int i = *firstIndex; i < (LONGLONG)_units->size(); i++)
                {
                    if (_units->at(i)->RowIndex == firstRowIndex)
                    {
                        newFirstIndex = i;
                        break;
                    }
                }
            }
            else
            {
                for (int i = *firstIndex; i >= 0; i--)
                {
                    if (_units->at(i)->RowIndex == firstRowIndex - 1)
                    {
                        newFirstIndex = i + 1;
                        break;
                    }
                }
            }
        }

    if (newFirstIndex < 0)
    {
        newFirstIndex = 0;
    }

    if (*lastIndex < 0)
    {
        for (int i = 0; i < (LONGLONG)_units->size(); i++)
        {
            if (_units->at(i)->RowIndex == lastRowIndex + 1)
            {
                newLastIndex = i - 1;
                break;
            }
        }

    }
    else
    {
        if (*lastIndex >= (LONGLONG)_units->size())
        {
            *lastIndex = (int)_units->size() - 1;
        }

        if (_units->at(*lastIndex)->RowIndex < lastRowIndex)
        {
            for (int i = *lastIndex; i < (LONGLONG)_units->size(); i++)
            {
                if (_units->at(i)->RowIndex == lastRowIndex + 1)
                {
                    newLastIndex = i - 1;
                    break;
                }
            }
        }
        else
        {
            for (int i = *lastIndex; i >= 0; i--)
            {
                if (_units->at(i)->RowIndex == lastRowIndex)
                {
                    newLastIndex = i;
                    break;
                }
            }
        }
    }

    if (newLastIndex < 0)
    {
        newLastIndex = (int)_units->size() - 1;
    }

    if (newLastIndex - newFirstIndex > 200)
    {

    }

    *firstIndex = newFirstIndex;
    *lastIndex = newLastIndex;

    for (int i = *firstIndex; i <= *lastIndex; i++)
    {
        result->push_back(_units->at(i)->Item);
    }

    return (LONGLONG)result;
}

Rect PhotowallLayout::GetItemLayoutRect(int index)
{
    if (!(_requestRelayoutIndex < 0 || _requestRelayoutIndex >= (LONGLONG)_units->size()))
    {
        Relayout();
    }

    Rect result = Rect();

    auto unit = _units->at(index);

    if (unit->RowIndex == _rowIndex && !_lastRowLocked)
    {
        result.Height = (float)unit->DesiredSize.Height;
        result.Width = (float)unit->DesiredSize.Width;
        result.X = (float)unit->Offset;
        result.Y = (float)(unit->RowIndex * (_unitSize + _spacing) + _headerSize.Height);
    }
    else
    {
        result.Height = isinf(unit->ActualSize.Height) ? unit->DesiredSize.Height : unit->ActualSize.Height;
        result.Width = isinf(unit->ActualSize.Width) ? unit->DesiredSize.Width : unit->ActualSize.Width;
        result.X = (float)((unit->ActualOffset < 0) ? unit->Offset : unit->ActualOffset);
        result.Y = (float)(unit->RowIndex * (_unitSize + _spacing) + _headerSize.Height);
    }

    return result;
}

bool PhotowallLayout::FillWindow(VisualWindow window)
{
    auto lastRowIndex = floor((VisualWindowExtension::GetEndOffset(window) + _spacing) / (_unitSize + _spacing));

    return _rowIndex > lastRowIndex;
}

bool PhotowallLayout::IsItemInWindow(VisualWindow window, int index)
{
    auto rect = GetItemLayoutRect(index);
    return VisualWindowExtension::Contain(window, VisualWindow{ rect.Top,rect.Height });
}

void PhotowallLayout::ChangeItem(int index, Platform::Object^ item, Size size)
{
    if (item != nullptr)
    {
        _itemUnitMap->erase(_units->at(index)->Item);
        (*_itemUnitMap)[item] = _units->at(index);
        _units->at(index)->Item = item;
    }

    if (size.Width != _units->at(index)->DesiredSize.Width)
    {
        if (_units->at(index)->RowIndex < _rowIndex)
        {
            int thisRowStartIndex = -1, nextRowStartIndex = -1, newNextRowStartIndex = -1;

            for (int i = index; i >= 0; i--)
            {
                if (_units->at(i)->RowIndex == _units->at(index)->RowIndex - 1)
                {
                    thisRowStartIndex = i + 1;
                    break;
                }
            }

            if (thisRowStartIndex < 0)
            {
                thisRowStartIndex = 0;
            }

            for (int i = index; i < (LONGLONG)_units->size(); i++)
            {
                if (_units->at(i)->RowIndex == _units->at(index)->RowIndex + 1)
                {
                    nextRowStartIndex = i;
                    break;
                }
            }

            if (nextRowStartIndex < 0)
            {
                throw Platform::Exception::CreateException(-1, "A catastrophic error occurred.");
            }

            _units->at(index)->DesiredSize = Size(size.Width, _units->at(index)->DesiredSize.Height);

            double offset = 0;
            for (int i = thisRowStartIndex; i <= nextRowStartIndex; i++)
            {
                bool isNeedNext = abs(_width - offset) > abs(_width - (offset + size.Width + (offset == 0 ? 0 : _spacing)));

                if (isNeedNext || offset * 2 < _width)
                {
                    offset += _units->at(i)->DesiredSize.Width + (_offset == 0 ? 0 : _spacing);
                }
                else
                {
                    newNextRowStartIndex = i;
                    break;
                }
            }

            if (newNextRowStartIndex != nextRowStartIndex)
            {
                SetRelayoutIndex(thisRowStartIndex);
            }
            else
            {
                RelayoutRow(thisRowStartIndex);
            }
        }
        else
        {
            _units->at(index)->DesiredSize = Size(size.Width, _units->at(index)->DesiredSize.Height);
            SetRelayoutIndex(index);
        }
    }
}

void PhotowallLayout::ChangePanelSize(double width)
{
    if (width != _width)
    {
        _width = width;
        SetRelayoutIndex(0);
    }
}

void PhotowallLayout::ChangeSpacing(double spacing)
{
    if (spacing != _spacing)
    {
        _spacing = spacing;
        SetRelayoutIndex(0);
    }
}

void PhotowallLayout::RemoveItem(int index)
{
    SetRelayoutIndex(max(0, index - 1));
    _itemUnitMap->erase(_units->at(index)->Item);
    _units->erase(_units->begin() + index);
}

void PhotowallLayout::RemoveAll()
{
    _itemUnitMap->clear();
    _units->clear();
    _lastRowLocked = false;
    _rowIndex = 0;
    _offset = 0;
    SetRelayoutIndex(0);
}

Size PhotowallLayout::GetItemSize(int index)
{
    Size result;
    if (_units->at(index)->RowIndex == _rowIndex && !_lastRowLocked)
    {
        result.Height = _units->at(index)->DesiredSize.Height;
        result.Width = _units->at(index)->DesiredSize.Width;
    }
    else
    {
        result.Height = isinf(_units->at(index)->ActualSize.Height) ? _units->at(index)->DesiredSize.Height : _units->at(index)->ActualSize.Height;
        result.Width = isinf(_units->at(index)->ActualSize.Width) ? _units->at(index)->DesiredSize.Width : _units->at(index)->ActualSize.Width;
    }

    return result;
}

Size PhotowallLayout::GetItemSize(Platform::Object^ item)
{
    Size result;
    if ((*_itemUnitMap)[item]->RowIndex == _rowIndex && !_lastRowLocked)
    {
        result.Height = (*_itemUnitMap)[item]->DesiredSize.Height;
        result.Width = (*_itemUnitMap)[item]->DesiredSize.Width;
    }
    else
    {
        result.Height = isinf((*_itemUnitMap)[item]->ActualSize.Height) ? (*_itemUnitMap)[item]->DesiredSize.Height : (*_itemUnitMap)[item]->ActualSize.Height;
        result.Width = isinf((*_itemUnitMap)[item]->ActualSize.Width) ? (*_itemUnitMap)[item]->DesiredSize.Width : (*_itemUnitMap)[item]->ActualSize.Width;
    }
    return result;
}

void PhotowallLayout::SetRelayoutIndex(int index)
{
    if (index >= 0 && _requestRelayoutIndex >= 0)
    {
        _requestRelayoutIndex = min(_requestRelayoutIndex, index);
    }
    else
    {
        _requestRelayoutIndex = max(_requestRelayoutIndex, index);
    }
}

void PhotowallLayout::ChangeUnitSize(double unitSize)
{
    if (unitSize != _unitSize)
    {
        _unitSize = unitSize;
        SetRelayoutIndex(0);
    }
}

void PhotowallLayout::Relayout()
{
    int thisRowStartIndex = -1;

    for (int i = _requestRelayoutIndex; i >= 0; i--)
    {
        if (_units->at(i)->RowIndex == _units->at(_requestRelayoutIndex)->RowIndex - 1)
        {
            thisRowStartIndex = i + 1;
            break;
        }
    }

    if (thisRowStartIndex < 0)
    {
        thisRowStartIndex = 0;
    }

    _rowIndex = _units->at(_requestRelayoutIndex)->RowIndex;
    _offset = 0;
    std::vector<int>* relayoutRows = new std::vector<int>();

    for (int i = thisRowStartIndex; i < (LONGLONG)_units->size(); i++)
    {
        auto unit = _units->at(i);
        double length = unit->DesiredSize.Width;

        bool isNeedNext = abs(_width - _offset) > abs(_width - (_offset + length + (_offset == 0 ? 0 : _spacing)));

        _lastRowLocked = false;
        if (isNeedNext || _offset * 2 < _width)
        {
            unit->Offset = _offset + (_offset == 0 ? 0 : _spacing);
            unit->RowIndex = _rowIndex;
            _offset += length + (_offset == 0 ? 0 : _spacing);
        }
        else
        {
            _rowIndex++;
            _offset = 0;

            unit->Offset = 0;
            unit->RowIndex = _rowIndex;
            _offset += length;

            relayoutRows->push_back(i - 1);
        }

        if (_offset >= _width)
        {
            relayoutRows->push_back(i);
            _rowIndex++;
            _offset = 0;
            _lastRowLocked = true;
        }
    }

    for (auto row : *relayoutRows)
    {
        RelayoutRow(row);
    }

    delete(relayoutRows);
    _requestRelayoutIndex = -1;
}

Size PhotowallLayout::GetHeaderAvailableSize()
{
    return Size((float)Width, INFINITY);
}

Size PhotowallLayout::GetFooterAvailableSize()
{
    return Size((float)(Width - _offset + Spacing), (float)UnitSize);
}

bool PhotowallLayout::SetHeaderSize(Size size)
{
    if (size.Width != _headerSize.Width || size.Height != _headerSize.Height)
    {
        _headerSize = size;
        return true;
    }
    return false;
}

bool PhotowallLayout::SetFooterSize(Size size)
{
    if (size.Width != _footerSize.Width || size.Height != _footerSize.Height)
    {
        _footerSize = size;
        return true;
    }
    return false;
}

Rect PhotowallLayout::GetHeaderLayoutRect()
{
    return Rect(0, 0, _headerSize.Width, _headerSize.Height);
}

Rect PhotowallLayout::GetFooterLayoutRect()
{
    return Rect(0, 0, 0, 0);
}