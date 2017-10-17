using System;
using System.Collections.Generic;
using System.Text;

namespace iHentai.Services.Core.Models.Interfaces
{
    public interface IGalleryModel
    {
        string Title { get; set; }
        string Thumb { get; set; }
        string Link { get; set; }
    }
}
