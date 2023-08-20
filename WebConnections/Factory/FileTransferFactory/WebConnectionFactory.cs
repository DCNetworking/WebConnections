using WebConnections.Factory.FileTransferFactory;
using WebConnections.Factory.FileTransferFactory.FTP;
using WebConnections.Factory.FileTransferFactory.SFTP;

namespace WebConnections.Factory.FileTransferFactory;

public class WebConnectionFactory : IWebConnectionFactory
{
    public WebConnectionService Instance { get; private set; }
    private ConnectionType _connectionType;
    private ConnectionCredentials _connectionCredentials;
    public WebConnectionFactory(ConnectionCredentials connectionCredentials,ConnectionType connectionType)
    {
        _connectionType = connectionType;
        _connectionCredentials = connectionCredentials;
    }
    public void CreateInstance()
    {
        Instance = _connectionType switch
        {
            ConnectionType.Ftp => new FtpWebConnection(_connectionCredentials),
            ConnectionType.Sftp => new SftpWebConnection(_connectionCredentials),
            _ => throw new Exception("Connection Type not implemented")
        };
    }
}



