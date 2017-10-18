namespace iHentai.Services.EHentai.Models
{
    public class LanguageModel
    {
        public LanguageModel(int id)
        {
            ID = id;
        }
        public int ID { get; }

        public bool Original { get; set; } = false;
        
        public string OriginalID => $"{ID}";

        public bool Translated { get; set; } = false;
        
        public string TranslatedID => $"{ID + 1024}";

        public bool Rewrite { get; set; } = false;

        public string RewriteID => $"{ID + 2048}";

        public bool All
        {
            get
            {
                return Original && Translated && Rewrite;
            }
            set
            {
                Original = value;
                Translated = value;
                Rewrite = value;
            }
        }

        public string Value => $"{(Original && ID != 0 ? $"{OriginalID}x" : null)}{(Translated ? $"{TranslatedID}x" : null)}{(Rewrite ? $"{RewriteID}x" : null)}";

    }

}