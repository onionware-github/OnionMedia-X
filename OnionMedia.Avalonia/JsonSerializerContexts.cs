using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OnionMedia.Avalonia;

[JsonSerializable(typeof(Dictionary<string, object?>))]
partial class StringObjectDictionaryJsonContext : JsonSerializerContext {}