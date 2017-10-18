using System;

namespace iHentai.Services.EHentai.Models
{
    public class TagNamespaceModel
    {
        public bool Enable { get; set; } = false;
        public int Index { get; }
        public int Value => Enable ? Convert.ToInt32(Math.Pow(2, Index - 1)) : 0;

        public TagNamespaceModel(int index)
        {
            Index = index;
        }
    }
}