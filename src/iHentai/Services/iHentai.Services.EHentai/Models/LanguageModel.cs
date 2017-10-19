namespace iHentai.Services.EHentai.Models
{
    public class LanguageModel
    {
        public LanguageModel(int id)
        {
            ID = id;
        }

        private int ID { get; }

        public bool Original { get; set; }

        private string OriginalID => $"{ID}";

        public bool Translated { get; set; }

        private string TranslatedID => $"{ID + 1024}";

        public bool Rewrite { get; set; }

        private string RewriteID => $"{ID + 2048}";

        public bool All
        {
            get => Original && Translated && Rewrite;
            set
            {
                Original = value;
                Translated = value;
                Rewrite = value;
            }
        }

        internal string Value =>
            $"{(Original && ID != 0 ? $"{OriginalID}x" : null)}{(Translated ? $"{TranslatedID}x" : null)}{(Rewrite ? $"{RewriteID}x" : null)}";
    }

    public enum LanguageFlags
    {
        Japanese,
        English,
        Chinese,
        Dutch,
        French,
        German,
        Hungarian,
        Italian,
        Korean,
        Polish,
        Portuguese,
        Russian,
        Spanish,
        Thai,
        Vietnamese,
        NA,
        Other
    }
}