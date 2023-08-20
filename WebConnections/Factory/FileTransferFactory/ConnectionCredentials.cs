namespace WebConnections.Factory.FileTransferFactory;

/// <summary>
/// Represents connection credentials for web connections used in file transfers.
/// </summary>
public class ConnectionCredentials
{
    /// <summary>
    /// Gets the server address.
    /// </summary>
    public string Server { get; private set; }

    /// <summary>
    /// Gets the username used for authentication.
    /// </summary>
    public string Username { get; private set; }

    /// <summary>
    /// Gets the password used for authentication.
    /// </summary>
    public string Password { get; private set; }

    /// <summary>
    /// Gets the buffer size for file transfer operations (optional).
    /// </summary>
    public int BufferSize { get; private set; }

    /// <summary>
    /// Gets the port number for the connection (default is 22 for SSH/SFTP).
    /// </summary>
    public int Port { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionCredentials"/> class with the specified parameters.
    /// </summary>
    /// <param name="username">The username used for authentication.</param>
    /// <param name="password">The password used for authentication.</param>
    /// <param name="server">The server address.</param>
    /// <param name="port">The port number for the connection (default is 22).</param>
    /// <param name="bufferSize">The buffer size for file transfer operations (optional).</param>
    public ConnectionCredentials(string username, string password, string server, int port = 22, int bufferSize = 1024)
    {
        ArgumentException.ThrowIfNullOrEmpty(username);
        ArgumentException.ThrowIfNullOrEmpty(password);
        ArgumentException.ThrowIfNullOrEmpty(server);
        
        Username = username;
        Password = password;
        Server = server;
        Port = port;
        BufferSize = bufferSize;
    }
}