using System.Drawing.Text;

namespace MCAddonErsteller;

public static class FontManager
{
  private static readonly PrivateFontCollection Fonts = new();

  public static FontFamily Metropolis = null!;
  public static FontFamily Noto = null!;
  public static FontFamily Minecraft = null!;

  public static void Load()
  {
    Fonts.AddFontFile(Path.Combine(AppContext.BaseDirectory, "assets", "fonts", "metropolis-regular-webfont.ttf"));
    Fonts.AddFontFile(Path.Combine(AppContext.BaseDirectory, "assets", "fonts", "noto-sans-v9-latin-regular.ttf"));
    Fonts.AddFontFile(Path.Combine(AppContext.BaseDirectory, "assets", "fonts", "minecraft_five_bold-webfont.ttf"));

    Metropolis = Fonts.Families[0];
    Noto = Fonts.Families[1];
    Minecraft = Fonts.Families[2];
  }
}