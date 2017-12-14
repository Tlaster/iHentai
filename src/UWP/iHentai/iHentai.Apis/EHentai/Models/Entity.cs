using iHentai.Apis.Core.Common.Attributes;

namespace iHentai.Apis.EHentai.Models
{
    public enum ImageSize
    {
        /// <summary>
        ///     Image Size Auto
        /// </summary>
        [StringValue("a")] Auto,

        /// <summary>
        ///     Image Size 780x
        /// </summary>
        [StringValue("780")] X780,

        /// <summary>
        ///     Image Size 980x
        /// </summary>
        [StringValue("980")] X980,

        /// <summary>
        ///     Image Size 1280x
        /// </summary>
        [StringValue("1280")] X1280,

        /// <summary>
        ///     Image Size 1600x
        /// </summary>
        [StringValue("1600")] X1600,

        /// <summary>
        ///     Image Size 2400x
        /// </summary>
        [StringValue("2400")] X2400
    }

    public enum GalleryTitle
    {
        /// <summary>
        ///     Default gallery title
        /// </summary>
        [StringValue("r")] Default,

        /// <summary>
        ///     Japanese gallery title
        /// </summary>
        [StringValue("j")] Japanese
    }

    public enum ArchiverDownload
    {
        /// <summary>
        ///     Manual Accept, Manual Start
        /// </summary>
        MAMS = 0,

        /// <summary>
        ///     Auto Accept, Manual Start
        /// </summary>
        AAMS,

        /// <summary>
        ///     Manual Accept, Auto Start
        /// </summary>
        MAAS,

        /// <summary>
        ///     Auto Accept, Auto Start
        /// </summary>
        AAAS = 3
    }

    public enum LayoutMode
    {
        /// <summary>
        ///     List View on the front and search pages
        /// </summary>
        [StringValue("l")] List,

        /// <summary>
        ///     Thumbnail View on the front and search pages
        /// </summary>
        [StringValue("t")] Thumb
    }

    public enum FavoritesSort
    {
        /// <summary>
        ///     Sort favorites by last gallery update time
        /// </summary>
        [StringValue("p")] GalleryUpdateTime,

        /// <summary>
        ///     Sort favorites by favorited time
        /// </summary>
        [StringValue("f")] FavoritedTime
    }

    public enum ResultCount
    {
        /// <summary>
        ///     25 results per page for the index/search page and torrent search pages
        /// </summary>
        C25 = 0,

        /// <summary>
        ///     50 results per page for the index/search page and torrent search pages
        /// </summary>
        C50,

        /// <summary>
        ///     100 results per page for the index/search page and torrent search pages
        /// </summary>
        C100,

        /// <summary>
        ///     200 results per page for the index/search page and torrent search pages
        /// </summary>
        C200 = 3
    }

    public enum MouseOver
    {
        /// <summary>
        ///     On mouse-over
        /// </summary>
        [StringValue("m")] Yes,

        /// <summary>
        ///     On page load
        /// </summary>
        [StringValue("p")] NO
    }

    public enum PreviewSize
    {
        /// <summary>
        ///     Preview normal size
        /// </summary>
        [StringValue("m")] Normal,

        /// <summary>
        ///     Preview large size
        /// </summary>
        [StringValue("l")] Large
    }

    public enum PreviewRowCount
    {
        /// <summary>
        ///     4 row preview per page
        /// </summary>
        R4 = 2,

        /// <summary>
        ///     10 row preview per page
        /// </summary>
        R10 = 5,

        /// <summary>
        ///     20 row preview per page
        /// </summary>
        R20 = 10,

        /// <summary>
        ///     40 row preview per page
        /// </summary>
        R40 = 20
    }

    public enum CommentsSort
    {
        /// <summary>
        ///     Oldest comments first
        /// </summary>
        [StringValue("a")] OldestFirst,

        /// <summary>
        ///     Recent comments first
        /// </summary>
        [StringValue("d")] RecentFirst,

        /// <summary>
        ///     By highest score
        /// </summary>
        [StringValue("s")] HighestScoreFirst
    }

    public enum CommentVotes
    {
        /// <summary>
        ///     Show gallery comment votes On score hover or click
        /// </summary>
        Pop = 0,

        /// <summary>
        ///     Always show gallery comment votes
        /// </summary>
        Always = 1
    }

    public enum TagSort
    {
        /// <summary>
        ///     Sort order for gallery tags alphabetically
        /// </summary>
        [StringValue("a")] Alphabetically,

        /// <summary>
        ///     Sort order for gallery tags by tag power
        /// </summary>
        [StringValue("p")] Power
    }

    public enum MultiPageStyle
    {
        /// <summary>
        ///     Align left, only scale if image is larger than browser width
        /// </summary>
        [StringValue("n")] N,

        /// <summary>
        ///     Align center, only scale if image is larger than browser width
        /// </summary>
        [StringValue("c")] C,

        /// <summary>
        ///     Align center, Always scale images to fit browser width
        /// </summary>
        [StringValue("y")] Y
    }
}