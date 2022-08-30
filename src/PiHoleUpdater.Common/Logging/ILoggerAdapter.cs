namespace PiHoleUpdater.Common.Logging;

public interface ILoggerAdapter<out T>
{
  void LogTrace(string? message, params object?[] args);
  void LogTrace(Exception? exception, string? message, params object?[] args);

  void LogDebug(string? message, params object?[] args);
  void LogDebug(Exception? exception, string? message, params object?[] args);

  void LogInformation(string? message, params object?[] args);
  void LogInformation(Exception? exception, string? message, params object?[] args);

  void LogWarning(string? message, params object?[] args);
  void LogWarning(Exception? exception, string? message, params object?[] args);

  void LogError(string? message, params object?[] args);
  void LogError(Exception? exception, string? message, params object?[] args);
}
