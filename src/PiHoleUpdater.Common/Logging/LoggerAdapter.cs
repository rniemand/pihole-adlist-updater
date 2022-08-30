using Microsoft.Extensions.Logging;

namespace PiHoleUpdater.Common.Logging;

public class LoggerAdapter<T> : ILoggerAdapter<T>
{
  private readonly ILogger<T> _logger;

  public LoggerAdapter(ILogger<T> logger)
  {
    this._logger = logger;
  }

  // Trace
  public void LogTrace(string? message, params object?[] args)
  {
    if (!_logger.IsEnabled(LogLevel.Trace))
      return;

    _logger.LogTrace(message, args);
  }

  public void LogTrace(Exception? exception, string? message, params object?[] args)
  {
    if (!_logger.IsEnabled(LogLevel.Trace))
      return;

    _logger.LogTrace(exception, message, args);
  }

  // Debug
  public void LogDebug(string? message, params object?[] args)
  {
    if (!_logger.IsEnabled(LogLevel.Debug))
      return;

    _logger.LogDebug(message, args);
  }

  public void LogDebug(Exception? exception, string? message, params object?[] args)
  {
    if (!_logger.IsEnabled(LogLevel.Debug))
      return;

    _logger.LogDebug(exception, message, args);
  }

  // Information
  public void LogInformation(string? message, params object?[] args)
  {
    if (!_logger.IsEnabled(LogLevel.Information))
      return;

    _logger.LogInformation(message, args);
  }

  public void LogInformation(Exception? exception, string? message, params object?[] args)
  {
    if (!_logger.IsEnabled(LogLevel.Information))
      return;

    _logger.LogInformation(exception, message, args);
  }

  // Warning
  public void LogWarning(string? message, params object?[] args)
  {
    if (!_logger.IsEnabled(LogLevel.Warning))
      return;

    _logger.LogWarning(message, args);
  }

  public void LogWarning(Exception? exception, string? message, params object?[] args)
  {
    if (!_logger.IsEnabled(LogLevel.Warning))
      return;

    _logger.LogWarning(exception, message, args);
  }

  // Error
  public void LogError(string? message, params object?[] args)
  {
    if (!_logger.IsEnabled(LogLevel.Error))
      return;

    _logger.LogError(message, args);
  }

  public void LogError(Exception? exception, string? message, params object?[] args)
  {
    if (!_logger.IsEnabled(LogLevel.Error))
      return;

    _logger.LogError(exception, message, args);
  }
}
