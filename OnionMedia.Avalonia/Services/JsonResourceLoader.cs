using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using OnionMedia.Core.Services;

namespace OnionMedia.Services;

sealed class JsonResourceLoader : IStringResourceService
{
    string workingDirectory;
    bool onlyTopLevelDirectory;
    Dictionary<string, Dictionary<string, string>> resources = new();
    readonly string defaultResourceFileName = "Resources";

    public JsonResourceLoader(string workingDirectory, bool onlyTopLevelDirectory = false)
    {
        this.workingDirectory = workingDirectory;
        this.onlyTopLevelDirectory = onlyTopLevelDirectory;
        GetResources();
    }
    
    public string GetLocalized(string resourceName, string sectionName = null)
    {
        if (resourceName == null) throw new ArgumentNullException(nameof(resourceName));
        string resourceFileName = sectionName ?? defaultResourceFileName;
        return resources[resourceFileName][resourceName];
    }

    private void GetResources()
    {
        foreach (var file in Directory.GetFiles(workingDirectory, "*.json", onlyTopLevelDirectory ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories))
        {
            string json = "";
            using (var sr = File.OpenText(file))
                json = sr.ReadToEnd();

            var fileResources = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            string resourceFileName = Path.GetFileNameWithoutExtension(file);
            resources.Add(resourceFileName, fileResources);
        }
    }
}