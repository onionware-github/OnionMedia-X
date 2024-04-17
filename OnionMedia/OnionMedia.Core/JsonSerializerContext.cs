using System.Text.Json.Serialization;
using OnionMedia.Core.Models;

namespace OnionMedia.Core;

[JsonSerializable(typeof(ConversionPreset))]
[JsonSerializable(typeof(IEnumerable<ConversionPreset>))]
[JsonSerializable(typeof(FFmpegCodecConfig))]
public partial class JsonContext : JsonSerializerContext{}