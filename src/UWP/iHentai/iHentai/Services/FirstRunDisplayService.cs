using System;
using System.Threading.Tasks;
using iHentai.Basic.Extensions;
using iHentai.Extensions;
using iHentai.Helpers;
using iHentai.Views;

namespace iHentai.Services
{
    public static class FirstRunDisplayService
    {
        internal static async Task ShowIfAppropriateAsync()
        {
            var hasShownFirstRun = false;
            hasShownFirstRun = nameof(hasShownFirstRun).Read<bool>();

            if (!hasShownFirstRun)
            {
                true.Save(nameof(hasShownFirstRun));
                var dialog = new FirstRunDialog();
                await dialog.ShowAsync();
            }
        }
    }
}