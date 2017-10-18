using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using iHentai.Services.Core;
using iHentai.Services.EHentai.Models;

namespace iHentai.Services.EHentai
{
    public class UConfig : IApiConfig
    {
                #region KEY

        /// <summary>
        /// The Cookie key of uconfig
        /// </summary>
        public const string KEY_UCONFIG = "uconfig";

        /// <summary>
        /// The key of load images through the Hentai@Home Network
        /// <see cref="YesOrNo"/>
        /// </summary>
        private const string KEY_LOAD_FROM_HAH = "uh";

        /// <summary>
        /// The key of Image Size Settings
        /// <see cref="Model.Entity.ImageSize"/>
        /// </summary>
        private const string KEY_IMAGE_SIZE = "xr";

        /// <summary>
        /// The key of scale images width
        /// <see cref="ScaleWidth"/>
        /// </summary>
        private const string KEY_SCALE_WIDTH = "rx";

        /// <summary>
        /// The key of scale images height
        /// <see cref="ScaleHeight"/>
        /// </summary>
        private const string KEY_SCALE_HEIGHT = "ry";

        /// <summary>
        /// The key of Gallery Name Display
        /// <see cref="Model.Entity.GalleryTitle"/>
        /// </summary>
        private const string KEY_GALLERY_TITLE = "tl";

        /// <summary>
        /// The key of the behavior for downloading archiver
        /// <see cref="Model.Entity.ArchiverDownload"/>
        /// </summary>
        private const string KEY_ARCHIVER_DOWNLOAD = "ar";

        /// <summary>
        /// The key of display mode would you like to use on the front and search pages
        /// <see cref="Model.Entity.LayoutMode"/>
        /// </summary>
        private const string KEY_LAYOUT_MODE = "dm";

        /// <summary>
        /// The key for show popular
        /// <see cref="YesOrNo"/>
        /// </summary>
        private const string KEY_POPULAR = "prn";

        /// <summary>
        /// The key of categories would you like to view as default on the front page
        /// </summary>
        private const string KEY_DEFAULT_CATEGORIES = "cats";

        /// <summary>
        /// The key for favorites sort
        /// <see cref="Model.Entity.FavoritesSort"/>
        /// </summary>
        private const string KEY_FAVORITES_SORT = "fs";

        /// <summary>
        /// The key of exclude certain namespaces from a default tag search
        /// </summary>
        private const string KEY_EXCLUDED_NAMESPACES = "xns";

        /// <summary>
        /// The key of hide galleries in certain languages from the gallery list and searches
        /// </summary>
        private const string KEY_EXCLUDED_LANGUAGES = "xl";

        /// <summary>
        /// The key of how many results would you like per page for the index/search page and torrent search pages
        /// <see cref="Model.Entity.ResultCount"/>
        /// </summary>
        private const string KEY_RESULT_COUNT = "rc";

        /// <summary>
        /// The key of mouse-over thumb
        /// <see cref="Model.Entity.MouseOver"/>
        /// </summary>
        private const string KEY_MOUSE_OVER = "lt";

        /// <summary>
        /// The key of preview size
        /// <see cref="Model.Entity.PreviewSize"/>
        /// </summary>
        private const string KEY_PREVIEW_SIZE = "ts";

        /// <summary>
        /// The key of preview row per page
        /// <see cref="PreviewRowCount"/>
        /// </summary>
        private const string KEY_PREVIEW_ROW = "tr";

        /// <summary>
        /// The key of sort order for gallery comments
        /// <see cref="Model.Entity.CommentsSort"/>
        /// </summary>
        private const string KEY_COMMENTS_SORT = "cs";

        /// <summary>
        /// The key of show gallery comment votes
        /// <see cref="Model.Entity.CommentVotes"/>
        /// </summary>
        private const string KEY_COMMENTS_VOTES = "sc";

        /// <summary>
        /// The key of sort order for gallery tags
        /// <see cref="Model.Entity.TagSort"/>
        /// </summary>
        private const string KEY_TAGS_SORT = "to";

        /// <summary>
        /// The key of show gallery page numbers
        /// <see cref="Model.Entity.ShowGalleryIndex"/>
        /// </summary>
        private const string KEY_SHOW_GALLERY_INDEX = "pn";

        /// <summary>
        ///  The key of the IP:Port of a proxy-enabled Hentai@Home Client
        ///  to load all images
        ///  </summary>
        private const string KEY_HAH_CLIENT_IP_PORT = "hp";

        /// <summary>
        /// The key of the passkey of a proxy-enabled Hentai@Home Client
        /// to load all images
        /// </summary>
        private const string KEY_HAH_CLIENT_PASSKEY = "hk";

        /// <summary>
        /// The key of enable Tag Flagging
        /// <see cref="YesOrNo"/>
        /// </summary>
        private const string KEY_ENABLE_TAG_FLAGGING = "tf";

        /// <summary>
        /// The key of always display the original images instead of the resampled versions
        /// <see cref="YesOrNo"/>
        /// </summary>
        private const string KEY_ALWAYS_ORIGINAL = "oi";

        /// <summary>
        /// The key of enable the multi-Page Viewer
        /// <see cref="YesOrNo"/>
        /// </summary>
        private const string KEY_MULTI_PAGE = "qb";

        /// <summary>
        /// key of multi-Page Viewer Display Style
        /// <see cref="Model.Entity.MultiPageStyle"/>
        /// </summary>
        private const string KEY_MULTI_PAGE_STYLE = "ms";

        /// <summary>
        /// The key of multi-Page Viewer Thumbnail Pane
        /// <see cref="YesOrNo"/>
        /// Yes to Hide, no to show
        /// </summary>
        private const string KEY_MULTI_PAGE_THUMB = "mt";

        /// <summary>
        /// The Cookie key of lofi resolution
        /// <see cref="Model.Entity.LofiResolution"/>
        /// </summary>
        private const string KEY_LOFI_RESOLUTION = "xres";

        /// <summary>
        /// The Cookie key of show warning
        /// <see cref="Model.Entity.ContentWarning"/>
        /// </summary>
        private const string KEY_CONTENT_WARNING = "nw";

        /// <summary>
        /// browse with advertisements enabled? (Bronze Star or Hath Perk: Ads-Be-Gone Required)
        /// <see cref="Model.Entity.Advertisement"/>
        /// </summary>
        private const string KEY_ADVERTISEMENTS = "sa";

        /// <summary>
        /// By default, galleries that you have rated will appear with red stars for ratings of 2 stars and below, green for ratings between 2.5 and 4 stars, and blue for ratings of 4.5 or 5 stars. You can customize this by entering your desired color combination below.
        /// 
        /// </summary>
        private const string KEY_RATING_COLOR = "ru";

        #endregion

        public static Dictionary<string, LanguageModel> Language { get; } = new Dictionary<string, LanguageModel>
        {
            {"Japanese", new LanguageModel(0)},
            {"English", new LanguageModel(1)},
            {"Chinese", new LanguageModel(10)},
            {"Dutch", new LanguageModel(20)},
            {"French", new LanguageModel(30)},
            {"German", new LanguageModel(40)},
            {"Hungarian", new LanguageModel(50)},
            {"Italian", new LanguageModel(60)},
            {"Korean", new LanguageModel(70)},
            {"Polish", new LanguageModel(80)},
            {"Portuguese", new LanguageModel(90)},
            {"Russian", new LanguageModel(100)},
            {"Spanish", new LanguageModel(110)},
            {"Thai", new LanguageModel(120)},
            {"Vietnamese", new LanguageModel(130)},
            {"N/A", new LanguageModel(254)},
            {"Other", new LanguageModel(255)},
        };

        public Dictionary<string, TagNamespaceModel> TagNamespace { get; } = new Dictionary<string, TagNamespaceModel>
        {
            {"reclass", new TagNamespaceModel(1)},
            {"language", new TagNamespaceModel(2)},
            {"parody", new TagNamespaceModel(3)},
            {"character", new TagNamespaceModel(4)},
            {"group", new TagNamespaceModel(5)},
            {"artist", new TagNamespaceModel(6)},
            {"male", new TagNamespaceModel(7)},
            {"female", new TagNamespaceModel(8)},
        };

        public Dictionary<CategoryFlags,UconfigCategoryModel> Category { get; } = new Dictionary<CategoryFlags, UconfigCategoryModel>
        {
            {CategoryFlags.Misc, new UconfigCategoryModel(0x1)},
            {CategoryFlags.Doujinshi, new UconfigCategoryModel(0x2)},
            {CategoryFlags.Manga, new UconfigCategoryModel(0x4)},
            {CategoryFlags.ArtistCG, new UconfigCategoryModel(0x8)},
            {CategoryFlags.GameCG, new UconfigCategoryModel(0x10)},
            {CategoryFlags.ImageSet, new UconfigCategoryModel(0x20)},
            {CategoryFlags.Cosplay, new UconfigCategoryModel(0x40)},
            {CategoryFlags.AsianPorn, new UconfigCategoryModel(0x80)},
            {CategoryFlags.Nonh, new UconfigCategoryModel(0x100)},
            {CategoryFlags.Western, new UconfigCategoryModel(0x200)},
        };
        #region KeyValue


        /// <summary>
        /// Load images through the Hentai@Home Network
        /// </summary>
        public UconfigModel<YesOrNo> LoadFromHentaiAtHome { get; } = new UconfigModel<YesOrNo>(KEY_LOAD_FROM_HAH, YesOrNo.Yes);

        /// <summary>
        /// Scale width
        /// </summary>
        public UconfigModel<int> ScaleWidth { get; } = new UconfigModel<int>(KEY_SCALE_WIDTH, 0);

        /// <summary>
        /// Scale height
        /// </summary>
        public UconfigModel<int> ScaleHeight { get; } = new UconfigModel<int>(KEY_SCALE_HEIGHT, 0);
        /// <summary>
        /// Gallery title
        /// </summary>
        public UconfigModel<GalleryTitle> GalleryTitle { get; } = new UconfigModel<GalleryTitle>(KEY_GALLERY_TITLE, Model.Entity.GalleryTitle.Default);

        /// <summary>
        /// The default behavior for downloading an archiver
        /// </summary>
        public UconfigModel<ArchiverDownload> ArchiverDownload { get; } = new UconfigModel<ArchiverDownload>(KEY_ARCHIVER_DOWNLOAD, Model.Entity.ArchiverDownload.MAMS);

        /// <summary>
        /// Display mode used on the front and search pages
        /// </summary>
        public UconfigModel<LayoutMode> LayoutMode { get; } = new UconfigModel<LayoutMode>(KEY_LAYOUT_MODE, Model.Entity.LayoutMode.List);

        /// <summary>
        /// Show popular or not
        /// </summary>
        public UconfigModel<YesOrNo> Popular { get; } = new UconfigModel<YesOrNo>(KEY_POPULAR, YesOrNo.Yes);

        /// <summary>
        /// Default categories on the front page
        /// </summary>
        public UconfigModel<int> DefaultCategories => new UconfigModel<int>(KEY_DEFAULT_CATEGORIES, Category.Sum(item => item.Value));


        public UconfigModel<FavoritesSort> FavoritesSort { get; } = new UconfigModel<FavoritesSort>(KEY_FAVORITES_SORT, Model.Entity.FavoritesSort.FavoritedTime);

        /// <summary>
        /// Certain namespaces excluded from a default tag search
        /// </summary>
        public UconfigModel<int> ExcludedNamespaces => new UconfigModel<int>(KEY_EXCLUDED_NAMESPACES, TagNamespace.Sum(item => item.Value));


        /// <summary>
        ///  How many results would you like per page for the index/search page
        ///  and torrent search pages
        ///  </summary>
        public UconfigModel<ResultCount> ResultCount { get; } = new UconfigModel<ResultCount>(KEY_RESULT_COUNT, Model.Entity.ResultCount.C25);

        /// <summary>
        /// mouse-over thumb
        /// </summary>
        public UconfigModel<MouseOver> MouseOver { get; } = new UconfigModel<MouseOver>(KEY_MOUSE_OVER, Model.Entity.MouseOver.Yes);

        /// <summary>
        /// Default preview mode
        /// </summary>
        public UconfigModel<PreviewSize> PreviewSize { get; } = new UconfigModel<PreviewSize>(KEY_PREVIEW_SIZE, Model.Entity.PreviewSize.Large);

        /// <summary>
        /// Preview row
        /// </summary>
        public UconfigModel<PreviewRowCount> PreviewRow { get; } = new UconfigModel<PreviewRowCount>(KEY_PREVIEW_ROW, PreviewRowCount.R4);

        /// <summary>
        /// Sort order for gallery comments
        /// </summary>
        public UconfigModel<CommentsSort> CommentsSort { get; } = new UconfigModel<CommentsSort>(KEY_COMMENTS_SORT, Model.Entity.CommentsSort.OldestFirst);

        /// <summary>
        /// Sort order for gallery tags
        /// </summary>
        public UconfigModel<TagSort> TagSort { get; } = new UconfigModel<TagSort>(KEY_TAGS_SORT, Model.Entity.TagSort.Alphabetically);

        /// <summary>
        /// Multi-Page Viewer Display Style
        /// </summary>
        public UconfigModel<MultiPageStyle> MultiPageStyle { get; } = new UconfigModel<MultiPageStyle>(KEY_MULTI_PAGE_STYLE, Model.Entity.MultiPageStyle.N);

        /// <summary>
        /// Multi-Page Viewer Thumbnail Pane
        /// </summary>
        public UconfigModel<YesOrNo> MultiPageThumb { get; } = new UconfigModel<YesOrNo>(KEY_MULTI_PAGE_THUMB, YesOrNo.No);

        /// <summary>
        /// Lofi resolution
        /// </summary>
        //public UconfigModel<LofiResolution> LofiResolution { get; } = new UconfigModel<LofiResolution>(KEY_LOFI_RESOLUTION, Model.Entity.LofiResolution.X980);

        /// <summary>
        /// Show content warning
        /// </summary>
        //public UconfigModel<YesOrNo> ContentWarning { get; } = new UconfigModel<YesOrNo>(KEY_CONTENT_WARNING, YesOrNo.No);

        /// <summary>
        /// Show gallery page numbers
        /// </summary>
        public UconfigModel<ShowGalleryIndex> ShowGalleryIndex { get; } = new UconfigModel<ShowGalleryIndex>(KEY_SHOW_GALLERY_INDEX, Model.Entity.ShowGalleryIndex.No);

        /// <summary>
        /// Show gallery comment votes mode
        /// </summary>
        public UconfigModel<CommentVotes> CommentVotes { get; } = new UconfigModel<CommentVotes>(KEY_COMMENTS_VOTES, Model.Entity.CommentVotes.Pop);

        /// <summary>
        /// Each letter represents one star. The default RRGGB means R(ed) for the first and second star, G(reen) for the third and fourth, and B(lue) for the fifth. You can also use (Y)ellow for the normal stars. Any five-letter combination of R, G, B and Y will work.
        /// </summary>
        public UconfigModel<string> RatingColor { get; } = new UconfigModel<string>(KEY_RATING_COLOR, "rrggb");

        /// <summary>
        /// Image Size
        /// </summary>
        public UconfigModel<ImageSize> ImageSize { get; } = new UconfigModel<ImageSize>(KEY_IMAGE_SIZE, Model.Entity.ImageSize.Auto);

        /// <summary>
        /// Would you like to browse with advertisements enabled? (Bronze Star or Hath Perk: Ads-Be-Gone Required)
        /// </summary>
        public UconfigModel<Advertisement> Advertisement { get; } = new UconfigModel<Advertisement>(KEY_ADVERTISEMENTS, Model.Entity.Advertisement.OfCourse);

        /// <summary>
        /// Always display the original images instead of the resampled versions
        /// </summary>
        public UconfigModel<YesOrNo> AlwaysOriginal { get; } = new UconfigModel<YesOrNo>(KEY_ALWAYS_ORIGINAL, YesOrNo.No);

        /// <summary>
        /// Enable the multi-Page Viewer
        /// </summary>
        public UconfigModel<YesOrNo> MultiPage { get; } = new UconfigModel<YesOrNo>(KEY_MULTI_PAGE, YesOrNo.No);

        /// <summary>
        /// Enable tag flagging
        /// </summary>
        public UconfigModel<YesOrNo> EnableTagFlagging { get; } = new UconfigModel<YesOrNo>(KEY_ENABLE_TAG_FLAGGING, YesOrNo.No);

        /// <summary>
        /// The IP of a proxy-enabled Hentai@Home Client
        /// to load all images
        /// </summary>
        public UconfigModel<string> HentaiAtHomeClientIpPort { get; } = new UconfigModel<string>(KEY_HAH_CLIENT_IP_PORT, "");

        /// <summary>
        /// The passkey of a proxy-enabled Hentai@Home Client
        /// to load all images
        /// </summary>
        public UconfigModel<string> HentaiAtHomeClientPasskey { get; } = new UconfigModel<string>(KEY_HAH_CLIENT_PASSKEY, "");

        /// <summary>
        /// Certain languages excluded from list and searches
        /// </summary>
        public UconfigModel<string> ExcludedLanguages => new UconfigModel<string>(KEY_EXCLUDED_LANGUAGES, string.Join("", Language.Select(item => item.Value)).TrimEnd('x'));

        #endregion

        public string Uconfig
            => $"{KEY_UCONFIG}={string.Join("-", typeof(UconfigHelper).GetRuntimeProperties().Where(item => item.PropertyType.GetTypeInfo().IsGenericType).Select(item=>item.GetValue(null,null)))}";

    }
}