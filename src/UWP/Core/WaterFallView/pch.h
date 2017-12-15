//
// pch.h
// Header for standard system include files.
//

#pragma once

#include <collection.h>
#include <ppltasks.h>

struct CompareObject
{
public:
    bool operator() (Platform::Object^ a, Platform::Object^b) const
    {
        auto i1 = a->GetHashCode();
        auto i2 = b->GetHashCode();
        return i1 - i2 < 0;
    }
};

struct HashObject
{
    size_t operator()(Platform::Object^ obj) const
    {
        return obj->GetHashCode();
    }
};

#define typeof(x) Windows::UI::Xaml::Interop::TypeName(x::typeid)
#define nameof(x) #x
#define RegisterProperty(type, field, propertyName) \
property type propertyName \
{ \
    type get() \
    { \
        return field; \
    } \
    void set(type value) \
    { \
        field = value; \
    } \
} 

#define RegisterReadOnlyPropertyWithExpression(type, exp, propertyName) \
property type propertyName \
{ \
    type get() \
    { \
       exp \
    } \
} 

#define RegisterReadOnlyProperty(type, field, propertyName) RegisterReadOnlyPropertyWithExpression(type, return field;, propertyName)


#define RegisterDependencyProperty(type, fieldName, dependencyPropertyName, propertyName) \
public: \
    static RegisterReadOnlyProperty(Windows::UI::Xaml::DependencyProperty^, fieldName, dependencyPropertyName) \
    property type propertyName \
    { \
        type get() \
        { \
            return safe_cast<type>(this->GetValue(dependencyPropertyName)); \
        } \
        void set(type value) \
        { \
            this->SetValue(dependencyPropertyName, value); \
        } \
    } \
private: \
    static Windows::UI::Xaml::DependencyProperty^ fieldName


#include "VirtualizingViewItem.h"
#include "VirtualizingPanel.h"