using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OnionMedia.Avalonia;

[JsonSerializable(typeof(Dictionary<string, object?>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
sealed partial class OnionMediaAvaloniaJsonContext : JsonSerializerContext {}