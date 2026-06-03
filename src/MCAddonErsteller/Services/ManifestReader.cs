using System.Text.Json;
using MCAddonErsteller.Models;

namespace MCAddonErsteller.Services;

public static class ManifestReader
{
  public static ManifestInfo Read(string manifestPath)
  {
    if (string.IsNullOrWhiteSpace(manifestPath))
      throw new ArgumentException("Manifest-Pfad darf nicht leer sein.", nameof(manifestPath));

    if (!File.Exists(manifestPath))
      throw new FileNotFoundException("Manifest-Datei wurde nicht gefunden.", manifestPath);

    using FileStream stream = File.OpenRead(manifestPath);
    using JsonDocument document = JsonDocument.Parse(stream, new JsonDocumentOptions
    {
      AllowTrailingCommas = true,
      CommentHandling = JsonCommentHandling.Skip
    });

    JsonElement root = document.RootElement;

    if (root.ValueKind != JsonValueKind.Object)
      throw new InvalidDataException("manifest.json ist ungültig.");

    JsonElement header = root.TryGetProperty("header", out JsonElement h) && h.ValueKind == JsonValueKind.Object
      ? h
      : root;

    return new ManifestInfo
    {
      Name = ReadString(header, "name", "Unbekannt"),
      Description = ReadString(header, "description", string.Empty),
      Uuid = ReadString(header, "uuid", string.Empty),
      Version = ReadVersion(header, "version", "1.0.0"),
      Kind = ReadKind(root)
    };
  }

  private static string ReadString(JsonElement element, string propertyName, string fallback)
  {
    if (element.ValueKind != JsonValueKind.Object)
      return fallback;

    if (!element.TryGetProperty(propertyName, out JsonElement value))
      return fallback;

    return value.ValueKind == JsonValueKind.String
      ? value.GetString() ?? fallback
      : fallback;
  }

  private static string ReadVersion(JsonElement element, string propertyName, string fallback)
  {
    if (element.ValueKind != JsonValueKind.Object)
      return fallback;

    if (!element.TryGetProperty(propertyName, out JsonElement value))
      return fallback;

    if (value.ValueKind == JsonValueKind.Array)
    {
      List<int> parts = [];

      foreach (JsonElement part in value.EnumerateArray())
      {
        if (part.ValueKind == JsonValueKind.Number && part.TryGetInt32(out int number))
          parts.Add(Math.Max(0, number));
      }

      if (parts.Count > 0)
        return FileNameTools.NormalizeVersion(string.Join('.', parts));
    }

    if (value.ValueKind == JsonValueKind.String)
      return FileNameTools.NormalizeVersion(value.GetString());

    return FileNameTools.NormalizeVersion(fallback);
  }

  private static string ReadKind(JsonElement root)
  {
    if (!root.TryGetProperty("modules", out JsonElement modules) || modules.ValueKind != JsonValueKind.Array)
      return "unknown";

    bool hasResources = false;
    bool hasDataOrScript = false;

    foreach (JsonElement module in modules.EnumerateArray())
    {
      if (module.ValueKind != JsonValueKind.Object)
        continue;

      if (!module.TryGetProperty("type", out JsonElement typeElement) || typeElement.ValueKind != JsonValueKind.String)
        continue;

      string? type = typeElement.GetString()?.Trim().ToLowerInvariant();

      if (type == "resources")
        hasResources = true;
      else if (type is "data" or "script")
        hasDataOrScript = true;
    }

    if (hasResources && hasDataOrScript)
      return "mixed";

    if (hasResources)
      return "resource";

    if (hasDataOrScript)
      return "behavior";

    return "unknown";
  }
}