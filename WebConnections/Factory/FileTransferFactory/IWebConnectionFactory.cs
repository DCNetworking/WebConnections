namespace WebConnections.Factory.FileTransferFactory;

/// <summary>
/// Represents an interface for creating instances of web connection services.
/// </summary>
public interface IWebConnectionFactory
{
    /// <summary>
    /// Creates an instance of a web connection service.
    /// </summary>
    void CreateInstance();
}