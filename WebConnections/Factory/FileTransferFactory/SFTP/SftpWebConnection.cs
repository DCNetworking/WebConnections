using Renci.SshNet;
using Renci.SshNet.Async;
using Renci.SshNet.Common;

namespace WebConnections.Factory.FileTransferFactory.SFTP;

public sealed class SftpWebConnection : WebConnectionService
{
    private SftpClient _sftpClient;
    public override ConnectionCredentials ConnectionCredentials { get; }

    public SftpWebConnection(ConnectionCredentials connectionCredentials)
    {
        ConnectionCredentials = connectionCredentials;
        ConfigWebRequestConnection();
    }
    protected override void ConfigWebRequestConnection()
    {
        _sftpClient = new(
            host: ConnectionCredentials.Server,
            port: ConnectionCredentials.Port,
            username: ConnectionCredentials.Username,
            password: ConnectionCredentials.Password
            );
    }

    public override async Task<bool> TestConnection()
    {
        try
        {
            await Task.Run(() => _sftpClient.Connect());
            Console.WriteLine("Connected Succesfully");
            _sftpClient.Disconnect();
            return true;
        }
        catch (SshAuthenticationException authEx)
        {
            Console.WriteLine($"Authentication Error: {authEx.Message}");
            return false;
        }
        catch (SshConnectionException connEx)
        {
            Console.WriteLine($"Connection Error: {connEx.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    public override async Task<bool> Upload(string localPath)
    {
        using (_sftpClient)
        {
            _sftpClient.Connect();
            try
            {
                using (var fileStream = new FileStream(localPath, FileMode.Open))
                {
                    await _sftpClient.UploadAsync(fileStream, ConnectionCredentials.Server);
                }

                Console.WriteLine("File uploaded successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTimeOffset.Now}][{GetType().Name}][ERROR {ex.GetType().Name}] {ex.Message}");
                return false;
            }
        }
    }

    public override async Task<bool> Upload(string remotePath, string localPath)
    {
        using (_sftpClient)
        {
            _sftpClient.Connect();
            try
            {
                using (var fileStream = new FileStream(localPath, FileMode.Open))
                {
                    await _sftpClient.UploadAsync(fileStream, remotePath);
                }

                Console.WriteLine("File uploaded successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTimeOffset.Now}][{GetType().Name}][ERROR {ex.GetType().Name}] {ex.Message}");
                return false;
            }
        }
    }
    public override async Task<bool> Download(string localPath)
    {
        using (_sftpClient)
        {
            _sftpClient.Connect();
            try
            {
                using (var fileStream = File.Create(localPath))
                {
                    await _sftpClient.DownloadAsync(ConnectionCredentials.Server, fileStream);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTimeOffset.Now}][{GetType().Name}][ERROR {ex.GetType().Name}] {ex.Message}");
                return false;
            }
        }
    }
    public override async Task<bool> Download(string remotePath, string localPath)
    {
        using (_sftpClient)
        {
            _sftpClient.Connect();
            try
            {
                using (var fileStream = File.Create(localPath))
                {
                    await _sftpClient.DownloadAsync(remotePath, fileStream);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTimeOffset.Now}][{GetType().Name}][ERROR {ex.GetType().Name}] {ex.Message}");
                return false;
            }
        }
    }
    public override async Task ListRemoteFiles(string remoteDirectory)
    {
        using (_sftpClient)
        {
            _sftpClient.Connect();
            var files = await _sftpClient.ListDirectoryAsync(remoteDirectory);
            foreach (var file in files)
            {
                Console.WriteLine($"Name: {file.Name}, Size: {file.Length}");
            }
        }
    }
}