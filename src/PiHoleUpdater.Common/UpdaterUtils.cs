using System.Reflection;

namespace PiHoleUpdater.Common;

public static class UpdaterUtils
{
  public static string ExeRelative(string path) =>
    Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
}
