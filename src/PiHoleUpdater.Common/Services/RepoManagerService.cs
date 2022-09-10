using LibGit2Sharp;
using PiHoleUpdater.Common.Extensions;
using PiHoleUpdater.Common.Logging;
using PiHoleUpdater.Common.Models;
using PiHoleUpdater.Common.Models.Config;
using PiHoleUpdater.Common.Providers;
using Branch = LibGit2Sharp.Branch;
using Repository = LibGit2Sharp.Repository;
using Signature = LibGit2Sharp.Signature;

namespace PiHoleUpdater.Common.Services;

public interface IRepoManagerService
{
  void UpdateLocalRepo();
  void CommitChanges();
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

  public void UpdateLocalRepo()
  {
    var githubCreds = _credsProvider.GetCredentials();
    var repoPath = _config.Repo.CheckoutDir;
    var projectName = githubCreds.AppName;

    if (!Directory.Exists(repoPath))
      throw new Exception("Unable to find local repository!");

    var localRepo = new Repository(repoPath);
    var success = Pull(localRepo, projectName);

    if (!success)
      throw new Exception("Failed to update local repo!");
  }

  public void CommitChanges()
  {
    var repoPath = _config.Repo.CheckoutDir;
    var branchName = "master";

    if (!Directory.Exists(repoPath))
      throw new Exception("Unable to find local repository!");

    var localRepo = new Repository(repoPath);
    CommitChanges(localRepo);

    Branch localRepoBranch = localRepo.Branches[branchName];
    PushChanges(localRepo, localRepoBranch);

    _logger.LogInformation("Pushed latest changes to GitHub");
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

  private void CommitChanges(IRepository repo)
  {
    _logger.LogInformation("Staging changes");
    var creds = _credsProvider.GetCredentials();
    LibGit2Sharp.Commands.Stage(repo, repo.Info.WorkingDirectory);

    var commitMessage = $"Automated list update: {DateTime.Now:u}";
    _logger.LogInformation("Committing and pushing: {message}", commitMessage);
    var author = new LibGit2Sharp.Signature(creds.CommitAuthorName, creds.CommitAuthorEmail, DateTime.Now);
    repo.Commit(commitMessage, author, author);
  }

  private void PushChanges(IRepository repository, LibGit2Sharp.Branch branch)
  {
    var remote = repository.Network.Remotes["origin"];

    repository.Branches.Update(branch,
      b => b.Remote = remote.Name,
      b => b.UpstreamBranch = branch.CanonicalName);

    repository.Network.Push(branch, new PushOptions
    {
      CredentialsProvider = (_, _, _) => CreateGithubAuth()
    });
  }

  private UsernamePasswordCredentials CreateGithubAuth()
  {
    var creds = _credsProvider.GetCredentials();

    return new UsernamePasswordCredentials()
    {
      Username = creds.Username,
      Password = creds.AccessToken
    };
  }
}
