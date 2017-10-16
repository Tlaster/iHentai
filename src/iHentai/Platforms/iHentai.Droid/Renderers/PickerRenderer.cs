using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PickerRenderer = iHentai.Droid.Renderers.PickerRenderer;

[assembly: ExportRenderer(typeof(Picker), typeof(PickerRenderer))]

namespace iHentai.Droid.Renderers
{
    public class PickerRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<Picker, Spinner>
    {
        private ArrayAdapter _adapter;
        private bool _disposed;

        public PickerRenderer()
        {
            AutoPackage = false;
        }

        protected override Spinner CreateNativeControl()
        {
            return new Spinner(Context);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;

                ((INotifyCollectionChanged) Element.Items).CollectionChanged -= RowsCollectionChanged;

                Control.ItemSelected -= Spinner_ItemSelected;
            }

            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            if (e.OldElement != null)
            {
                ((INotifyCollectionChanged) e.OldElement.Items).CollectionChanged -= RowsCollectionChanged;
                Control.ItemSelected -= Spinner_ItemSelected;
            }

            if (e.NewElement != null)
            {
                ((INotifyCollectionChanged) e.NewElement.Items).CollectionChanged += RowsCollectionChanged;
                if (Control == null)
                {
                    var spinner = CreateNativeControl();
                    _adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleSpinnerItem,
                        Element.Items.ToArray());
                    _adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    spinner.Adapter = _adapter;
                    spinner.ItemSelected += Spinner_ItemSelected;
                    SetNativeControl(spinner);
                }
                UpdatePicker();
                UpdateTextColor();
            }

            base.OnElementChanged(e);
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ((IElementController) Element).SetValueFromRenderer(Picker.SelectedIndexProperty, e.Position);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Picker.TitleProperty.PropertyName)
                UpdatePicker();
            if (e.PropertyName == Picker.SelectedIndexProperty.PropertyName)
                UpdatePicker();
            if (e.PropertyName == Picker.TextColorProperty.PropertyName)
                UpdateTextColor();
        }


        private void RowsCollectionChanged(object sender, EventArgs e)
        {
            UpdatePicker();
        }

        private void UpdatePicker()
        {
            _adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleSpinnerItem, Element.Items.ToArray());
            _adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            Control.Adapter = _adapter;
            Control.Prompt = Element.Title;
            if (Element.SelectedIndex != -1 && Element.Items != null)
                Control.SetSelection(Element.SelectedIndex);
        }

        private void UpdateTextColor()
        {
        }
    }
}