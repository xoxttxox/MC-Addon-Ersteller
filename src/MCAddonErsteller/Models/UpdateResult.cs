namespace MCAddonErsteller.Models
{
  public sealed class UpdateResult
  {
    public string CurrentVersion { get; set; } = "";
    public string LatestVersion { get; set; } = "";
    public bool IsUpdateAvailable { get; set; }
    public string ReleaseUrl { get; set; } = "";
  }
}
