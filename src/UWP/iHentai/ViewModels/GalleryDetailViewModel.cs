using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Mvvm;
using iHentai.Pages;

namespace iHentai.ViewModels
{
    [Page(typeof(GalleryDetailPage))]
    public class GalleryDetailViewModel : ViewModel
    {
        public GalleryDetailViewModel(IGalleryModel model)
        {
            Model = model;
        }

        public IGalleryModel Model { get; set; }

        public void ShowGallery()
        {
            Navigate<GalleryViewModel>();
        }

    }
}
