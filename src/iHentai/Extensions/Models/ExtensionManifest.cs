using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace iHentai.Extensions.Models
{
    public class ExtensionManifest
    {
        [JsonProperty("entry")] public string? Entry { get; set; }
        [JsonProperty("modules")] public List<string>? Modules { get; set; }
        [JsonProperty("icon")] public string? Icon { get; set; }
        [JsonProperty("manifest_version")] public int ManifestVersion { get; set; }
        [JsonProperty("version")] public int Version { get; set; }
        [JsonProperty("name")] public string? Name { get; set; }
        [JsonProperty("description")] public string? Description { get; set; }
        [JsonProperty("author")] public string? Author { get; set; }
        [JsonProperty("hosts")] public List<string>? Hosts { get; set; }
        [JsonProperty("permissions")] public List<string>? Permissions { get; set; }
        [JsonProperty("homepage_url")] public string? HomepageUrl { get; set; }

        

        public bool MatchHost(Uri uri)
        {
            if (Hosts == null)
            {
                return false;
            }

            foreach (var item in Hosts)
            {
                if (item == null)
                {
                    continue;
                }

                if (item == uri.Host)
                {
                    return true;
                }

                if (Regex.IsMatch(uri.Host, item))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class ExtensionFolderData
    {
        public string? Path { get; set; }
        public string? Token { get; set; }
    }
}