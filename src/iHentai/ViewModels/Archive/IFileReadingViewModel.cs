using Windows.ApplicationModel.Email;
using Windows.Storage;

namespace iHentai.ViewModels.Archive
{
    interface IFileReadingViewModel
    {
        StorageFile File { get; }
    }
}