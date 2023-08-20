namespace WebConnections.Factory.FileTransferFactory;

/// <summary>
/// Represents an abstract base class for web connection services used in file transfers.
/// </summary>
public abstract class WebConnectionService
{
    #region Properties

    /// <summary>
    /// Gets the connection credentials used for this web connection service.
    /// </summary>
    public abstract ConnectionCredentials ConnectionCredentials { get; }
    #endregion

    #region Methods
    /// <summary>
    /// Configures the web request connection settings.
    /// </summary>
    protected abstract void ConfigWebRequestConnection();

    /// <summary>
    /// Tests the connection to the remote server to check if it's successful.
    /// </summary>
    /// <returns>True if the connection is successful; otherwise, false.</returns>
    public abstract Task<bool> TestConnection();

    /// <summary>
    /// Uploads a file from the local path to the remote server.
    /// </summary>
    /// <param name="localPath">The local file path to upload.</param>
    /// <returns>True if the upload is successful; otherwise, false.</returns>
    public abstract Task<bool> Upload(string localPath);

    /// <summary>
    /// Downloads a file from the remote server to the local path.
    /// </summary>
    /// <param name="localPath">The local file path to save the downloaded file.</param>
    /// <returns>True if the download is successful; otherwise, false.</returns>
    public abstract Task<bool> Download(string localPath);

    /// <summary>
    /// Uploads a file from the local path to a specific remote path on the server.
    /// </summary>
    /// <param name="remotePath">The remote file path on the server.</param>
    /// <param name="localPath">The local file path to upload.</param>
    /// <returns>True if the upload is successful; otherwise, false.</returns>
    public abstract Task<bool> Upload(string remotePath, string localPath);

    /// <summary>
    /// Downloads a file from a specific remote path on the server to the local path.
    /// </summary>
    /// <param name="remotePath">The remote file path on the server.</param>
    /// <param name="localPath">The local file path to save the downloaded file.</param>
    /// <returns>True if the download is successful; otherwise, false.</returns>
    public abstract Task<bool> Download(string remotePath, string localPath);

    /// <summary>
    /// Lists the files in a remote directory on the server.
    /// </summary>
    /// <param name="remoteDirectory">The remote directory path on the server.</param>
    public abstract Task ListRemoteFiles(string remoteDirectory);

    #endregion
}