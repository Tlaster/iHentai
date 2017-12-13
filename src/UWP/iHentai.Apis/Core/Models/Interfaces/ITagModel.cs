using System;
using System.Collections.Generic;
using System.Text;

namespace iHentai.Apis.Core.Models.Interfaces
{
    public interface ITagModel
    {
        string Title { get; set; }
        string[] Tags { get; set; }
    }
}
