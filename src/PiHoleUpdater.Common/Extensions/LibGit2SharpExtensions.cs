using LibGit2Sharp;

namespace PiHoleUpdater.Common.Extensions;

public static class LibGit2SharpExtensions
{
  public static bool IsSuccessStatus(this MergeStatus status)
  {
    switch (status)
    {
      case MergeStatus.FastForward:
      case MergeStatus.UpToDate:
      case MergeStatus.NonFastForward:
        return true;

      case MergeStatus.Conflicts:
        return false;

      default:
        throw new Exception($"Unsupported value: {status:G}");
    }
  }
}
