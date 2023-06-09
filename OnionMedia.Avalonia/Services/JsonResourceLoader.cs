using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using OnionMedia.Core.Services;

namespace OnionMedia.Services;

sealed class JsonResourceLoader : IStringResourceService
{
    bool onlyTopLevelDirectory;
    Dictionary<string, Dictionary<string, string>> resources = new();
    Dictionary<string, Dictionary<string, string>> fallbackResources = new();
    readonly string defaultResourceFileName = "Resources";

    public JsonResourceLoader(string workingDirectory, string? fallbackDirectory = null, bool onlyTopLevelDirectory = false)
    {
        ArgumentException.ThrowIfNullOrEmpty(workingDirectory);
        this.onlyTopLevelDirectory = onlyTopLevelDirectory;
        GetResources(workingDirectory, resources);
        if (Directory.Exists(fallbackDirectory))
            GetResources(fallbackDirectory, fallbackResources);
    }

    /// <summary>
    /// Gets the current language code and returns the right folder.
    /// </summary>
    /// <param name="resourceFolderPath">e.g. "C:\Users\Jaden\source\repos\OnionMedia\OnionMedia.Avalonia\Resources\"</param>
    /// <param name="defaultLanguageCode">e.g. "de-de"</param>
    /// <returns>e.g. "C:\Users\Jaden\source\repos\OnionMedia\OnionMedia.Avalonia\Resources\de-de\"</returns>
    public static string GetCurrentLanguagePath(string resourceFolderPath, string defaultLanguageCode = "en-us")
    {
        string[] availableCountryCodes = Directory.GetDirectories(resourceFolderPath)
            .Select(d => d.Split(Path.DirectorySeparatorChar)[^1].ToLower()).ToArray();
        string currentCountryCode = CultureInfo.CurrentCulture.Name.ToLower();

        //Try to get specific code (e.g. en-us)
        string outputPath = Path.Combine(resourceFolderPath, currentCountryCode) + Path.DirectorySeparatorChar;
        if (Directory.Exists(outputPath)) return outputPath;

        //Try to get language code (e.g. en)
        string langCode = currentCountryCode.Split('-')[0];
        outputPath = Path.Combine(resourceFolderPath, langCode) + Path.DirectorySeparatorChar;
        if (Directory.Exists(outputPath)) return outputPath;

        //If nothing found, use defaultLanguageCode
        outputPath = Path.Combine(resourceFolderPath, defaultLanguageCode) + Path.DirectorySeparatorChar;
        if (Directory.Exists(outputPath)) return outputPath;
        throw new DirectoryNotFoundException(outputPath);
    }

    public string GetLocalized(string resourceName, string? sectionName = null)
    {
        if (resourceName == null) throw new ArgumentNullException(nameof(resourceName));
        string resourceFileName = sectionName ?? defaultResourceFileName;
        if (resources.TryGetValue(resourceFileName, out var rscDict)
            && rscDict.TryGetValue(resourceName, out var rsc))
            return rsc;
        
        if (!fallbackResources.Any()) return string.Empty;
        fallbackResources.TryGetValue(resourceFileName, out var fallbackDict);
        string? fallbackRsc = null;
        fallbackDict?.TryGetValue(resourceName, out fallbackRsc);
        return fallbackRsc ?? string.Empty;
    }

    private void GetResources(string workingDirectory, IDictionary<string, Dictionary<string, string>> targetDict)
    {
        ArgumentNullException.ThrowIfNull(targetDict);
        foreach (var file in Directory.GetFiles(workingDirectory, "*.json",
                     onlyTopLevelDirectory ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories))
        {
            string json = "";
            using (var sr = File.OpenText(file))
                json = sr.ReadToEnd();

            var fileResources = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            string resourceFileName = Path.GetFileNameWithoutExtension(file);
            targetDict.Add(resourceFileName, fileResources);
        }
    }
}