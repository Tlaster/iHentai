namespace iHentai.Apis.EHentai.Models
{
    public class UconfigCategoryModel
    {
        private readonly int _value;

        public UconfigCategoryModel(int value)
        {
            _value = value;
        }

        public bool Enable { get; set; } = true;
        public int Value => Enable ? _value : 0;
    }
}