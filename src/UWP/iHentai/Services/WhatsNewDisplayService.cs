using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using iHentai.Basic.Extensions;
using iHentai.Extensions;
using iHentai.Helpers;
using iHentai.Views;

namespace iHentai.Services
{
    // For instructions on testing this service see https://github.com/Microsoft/WindowsTemplateStudio/tree/master/docs/features/whats-new-prompt.md
    public static class WhatsNewDisplayService
    {
        internal static async Task ShowIfAppropriateAsync()
        {
            var currentVersion = PackageVersionToReadableString(Package.Current.Id.Version);

            var lastVersion = nameof(currentVersion).Read<string>();

            if (lastVersion == null)
            {
                currentVersion.Save(nameof(currentVersion));
            }
            else
            {
                if (currentVersion != lastVersion)
                {
                    currentVersion.Save(nameof(currentVersion));

                    var dialog = new WhatsNewDialog();
                    await dialog.ShowAsync();
                }
            }
        }

        private static string PackageVersionToReadableString(PackageVersion packageVersion)
        {
            return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}.{packageVersion.Revision}";
        }
    }
}