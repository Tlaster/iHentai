using System;

namespace iHentai.Services
{
    public class ServiceSelectionBannerModel
    {
        public ServiceSelectionBannerModel(string name)
        {
            ServiceType = name;
            Image = $"ms-appx:///Assets/ApiBanners/{name}.png";
        }

        public string ServiceType { get; }

        public string Image { get; }
    }
}