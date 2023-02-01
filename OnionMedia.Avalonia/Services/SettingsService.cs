/*
 * Copyright (C) 2022 Jaden Phil Nebel (Onionware)
 *
 * This file is part of OnionMedia.
 * OnionMedia is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, version 3.

 * OnionMedia is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.

 * You should have received a copy of the GNU Affero General Public License along with OnionMedia. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using OnionMedia.Core.Services;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.Json;
using AngleSharp.Dom;

namespace OnionMedia.Services
{
    sealed class SettingsService : ISettingsService
    {
        private Dictionary<string, object?> settings;
        private string settingsFilePath;
        
        public SettingsService(IPathProvider pathProvider)
        {
            settingsFilePath = Path.Combine(pathProvider.InstallPath, "appsettings.json");
            DeserializeSettings();
        }
        
        public object? GetSetting(string key)
        {
            var result = this.settings.ContainsKey(key) ? settings[key] : null;
            return result is JsonElement je ? ConvertToPrimitive(je) : result;
        }

        public void SetSetting(string key, object value)
        {
            if (settings.ContainsKey(key))
            {
                settings[key] = value;
            }
            else
            {
                settings.Add(key, value);
            }
            SerializeSettings();
        }

        private void SerializeSettings()
        {
            File.WriteAllText(settingsFilePath, JsonSerializer.Serialize(settings), Encoding.UTF8);
        }
        
        private void DeserializeSettings()
        {
            if (!File.Exists(settingsFilePath))
            {
                settings = new();
                File.WriteAllText(settingsFilePath, JsonSerializer.Serialize(settings));
                return;
            }
            try
            {
                settings = JsonSerializer.Deserialize<Dictionary<string, object?>>(File.ReadAllText(settingsFilePath));
            }
            catch
            {
                settings = new();
            }
        }

        //TODO: Implement a way to support all types/save types and return the right type
        private object? ConvertToPrimitive(JsonElement obj)
        {
            switch (obj.ValueKind)
            {
                case JsonValueKind.Number:
                    if (obj.TryGetInt32(out int intVal))
                        return intVal;
                    if (obj.TryGetDouble(out double doubleVal))
                        return doubleVal;
                    return null;
                
                case JsonValueKind.String:
                    return obj.GetString();
                
                case JsonValueKind.True or JsonValueKind.False:
                    return obj.GetBoolean();
                default: return null;
            }
        }
    }
}
