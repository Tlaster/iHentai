using iHentai.Apis.Core.Models.Interfaces;
using iHentai.Mvvm;

namespace iHentai.ViewModels
{
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