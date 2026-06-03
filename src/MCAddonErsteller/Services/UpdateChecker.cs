using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using MCAddonErsteller.Models;

namespace MCAddonErsteller.Services
{
  public static class UpdateChecker
  {
    private const string CurrentVersion = "1.0.1";
    private const string LatestReleaseUrl = "https://api.github.com/repos/xoxttxox/MC-Addon-Ersteller/releases/latest";
    private const string ReleasesPageUrl = "https://github.com/xoxttxox/MC-Addon-Ersteller/releases/latest";

    public static async Task<UpdateResult> CheckForUpdateAsync()
    {
      using var client = new HttpClient();

      client.DefaultRequestHeaders.UserAgent.Add(
          new ProductInfoHeaderValue("MCAddonErsteller", CurrentVersion)
      );

      var json = await client.GetStringAsync(LatestReleaseUrl);

      using var doc = JsonDocument.Parse(json);

      string tagName = doc.RootElement.GetProperty("tag_name").GetString() ?? "";
      string latestVersionText = tagName.TrimStart('v', 'V');

      Version current = new(CurrentVersion);
      Version latest = new(latestVersionText);

      return new UpdateResult
      {
        CurrentVersion = current.ToString(),
        LatestVersion = latest.ToString(),
        IsUpdateAvailable = latest > current,
        ReleaseUrl = ReleasesPageUrl
      };
    }

    public static void OpenReleasePage()
    {
      Process.Start(new ProcessStartInfo
      {
        FileName = ReleasesPageUrl,
        UseShellExecute = true
      });
    }
  }
}