using System;

namespace iHentai.Apis.EHentai.Models
{
    public class TagNamespaceModel
    {
        public TagNamespaceModel(int index)
        {
            Index = index;
        }

        public bool Enable { get; set; } = false;
        private int Index { get; }
        internal int Value => Enable ? Convert.ToInt32(Math.Pow(2, Index - 1)) : 0;
    }

    public enum TagNamespaceFlags
    {
        Reclass,
        Language,
        Parody,
        Character,
        Group,
        Artist,
        Male,
        Female
    }
}