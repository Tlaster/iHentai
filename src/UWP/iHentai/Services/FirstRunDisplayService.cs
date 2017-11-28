using System;
using System.Threading.Tasks;
using Windows.Storage;
using iHentai.Helpers;
using iHentai.Views;

namespace iHentai.Services
{
    public static class FirstRunDisplayService
    {
        internal static async Task ShowIfAppropriateAsync()
        {
            var hasShownFirstRun = false;
            hasShownFirstRun = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(hasShownFirstRun));

            if (!hasShownFirstRun)
            {
                await ApplicationData.Current.LocalSettings.SaveAsync(nameof(hasShownFirstRun), true);
                var dialog = new FirstRunDialog();
                await dialog.ShowAsync();
            }
        }
    }
}