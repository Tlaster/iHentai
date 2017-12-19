using System;

namespace iHentai.Services
{
    public class ServiceSelectionBannerModel<T>
    {
        public ServiceSelectionBannerModel(string name)
        {
            ServiceType = (T) Enum.Parse(typeof(T), name);
            Image = $"ms-appx:///Assets/ApiBanners/{name}.png";
        }

        public T ServiceType { get; }

        public string Image { get; }
    }
}