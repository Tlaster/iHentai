using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.ViewModels;
using Marduk.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace iHentai.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GalleryPage
    {
        public GalleryPage()
        {
            InitializeComponent();
        }

        public new GalleryViewModel ViewModel
        {
            get => (GalleryViewModel) base.ViewModel;
            set => base.ViewModel = value;
        }
    }
}