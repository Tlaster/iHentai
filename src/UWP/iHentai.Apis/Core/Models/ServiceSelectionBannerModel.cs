using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media;

namespace iHentai.Apis.Core.Models
{
    public class ServiceSelectionBannerModel
    {
        public ServiceSelectionBannerModel(string name)
        {
            ServiceType = Enum.Parse<ServiceTypes>(name);
             Image = $"ms-appx:///Assets/ApiBanners/{name}.png";
        }

        public ServiceTypes ServiceType { get; }

        public string Image { get; }
    }
}