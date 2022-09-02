using LibGit2Sharp;
using PiHoleUpdater.Common.Extensions;
using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;
using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Providers;

namespace PiHoleUpdater.Common.Services;

public interface IRepoManagerService
{
  Task UpdateLocalRepoAsync();
  Task CommitChangesAsync();
}

public class RepoManagerService : IRepoManagerService
{
  private readonly ILoggerAdapter<RepoManagerService> _logger;
  private readonly IGithubCredsProvider _credsProvider;
  private readonly PiHoleUpdaterConfig _config;

  public RepoManagerService(ILoggerAdapter<RepoManagerService> logger,
    IGithubCredsProvider credsProvider,
    PiHoleUpdaterConfig config)
  {
    _logger = logger;
    _config = config;
    _credsProvider = credsProvider;
  }

  public async Task UpdateLocalRepoAsync()
  {
    GithubCreds githubCreds = _credsProvider.GetCredentials();
    var repoPath = _config.LocalRepo;
    var projectName = githubCreds.AppName;

    if (!Directory.Exists(repoPath))
      throw new Exception("Unable to find local repository!");

    var localRepo = new Repository(repoPath);
    var success = Pull(localRepo, projectName);

    if (!success)
      throw new Exception("Failed to update local repo!");
  }

  public async Task CommitChangesAsync()
  {
    GithubCreds githubCreds = _credsProvider.GetCredentials();
    var repoPath = _config.LocalRepo;
    var projectName = githubCreds.AppName;

    if (!Directory.Exists(repoPath))
      throw new Exception("Unable to find local repository!");

    var localRepo = new Repository(repoPath);
    var success = Pull(localRepo, projectName);



    await Task.CompletedTask;
  }


  // Internal methods
  private bool Pull(Repository repository, string project)
  {
    try
    {
      _logger.LogInformation("Running pull on {repo}", project);
      GithubCreds creds = _credsProvider.GetCredentials();

      var options = new PullOptions
      {
        FetchOptions = new FetchOptions
        {
          CredentialsProvider = (_, _, _) => CreateGithubAuth(creds)
        }
      };

      var identity = new Identity(creds.CommitAuthorName, creds.CommitAuthorEmail);
      var signature = new Signature(identity, DateTimeOffset.Now);
      MergeResult? mergeResult = Commands.Pull(repository, signature, options);

      _logger.LogInformation("Project '{name}' pull request resulted in a {status} response.",
        project,
        mergeResult.Status.ToString("G"));

      return mergeResult.Status.IsSuccessStatus();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "{exType} updating '{project}': {message}",
        ex.GetType().Name,
        project,
        ex.Message);
    }

    return false;
  }

  private static UsernamePasswordCredentials CreateGithubAuth(GithubCreds creds) =>
    new()
    {
      Username = creds.Username,
      Password = creds.AccessToken
    };
}
