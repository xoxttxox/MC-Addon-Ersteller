using System.Windows.Forms;

namespace MCAddonErsteller;

internal static class Program
{
  [STAThread]
  private static void Main()
  {
    ApplicationConfiguration.Initialize();

    FontManager.Load();

    Application.Run(new MainForm());
  }
}