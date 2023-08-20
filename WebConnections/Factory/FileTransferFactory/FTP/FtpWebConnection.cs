using System.Net;

namespace WebConnections.Factory.FileTransferFactory.FTP;

public sealed class FtpWebConnection : WebConnectionService
{

    private readonly Uri _serverUri;
    public override ConnectionCredentials ConnectionCredentials { get; }
    public FtpWebConnection(ConnectionCredentials connectionCredentials)
    {
        ConnectionCredentials = connectionCredentials;
        _serverUri = new(ConnectionCredentials.Server);
    }
    protected override void ConfigWebRequestConnection()
    { }
    public override async Task<bool> TestConnection()
    {
        try
        {
            var ftpWebRequest = WebRequest.Create(_serverUri);
            ftpWebRequest.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;
            var ftpWebResponse = await ftpWebRequest.GetResponseAsync();
            Console.WriteLine("Connected to the FTP server successfully");
            ftpWebResponse.Close();
            return true;
        }
        catch (WebException ex)
        {
            FtpWebResponse ftpResponse = (FtpWebResponse)ex.Response;
            if (ftpResponse != null)
            {
                FtpStatusCode statusCode = ftpResponse.StatusCode;
                Console.WriteLine($"Status Code: {statusCode}");
                Console.WriteLine($"Server Response: {ftpResponse.StatusDescription}");
            }
            else
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
           
        }
        return false;
    }

    public override async Task<bool> Upload(string localPath)
    {
        Uri serverUri = new($"{ConnectionCredentials.Server}");
        return await UploadAction(serverUri, localPath);
    }
    public override async Task<bool> Upload(string remotePath, string localPath)
    {
        Uri serverUri = new($"{ConnectionCredentials.Server}{remotePath}");
        return await UploadAction(serverUri, localPath);
    }
    
    public override async Task<bool> Download(string localPath)
    {
        return await DownloadAction(new Uri(ConnectionCredentials.Server), localPath);
    }
    public override async Task<bool> Download(string remotePath, string localPath)
    {
        Uri serverUri = new($"{ConnectionCredentials.Server}{remotePath}");
        return await DownloadAction(serverUri, localPath);
    }
    public override async Task ListRemoteFiles(string remoteDirectory)
    {
        try
        {
            Uri serverUri = new Uri($"{ConnectionCredentials.Server}{remoteDirectory}");
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(serverUri);
            ftpRequest.Credentials = new NetworkCredential(ConnectionCredentials.Username, ConnectionCredentials.Password);
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            using (Stream responseStream = ftpResponse.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    Console.WriteLine(line);
                }
            }
            ftpResponse.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error listing directory: {ex.Message}");
        }
    }
    private async Task<bool> DownloadAction(Uri remotePath, string localPath)
    {
        var ftpRequest = WebRequest.Create(remotePath);
        ftpRequest.Credentials = new NetworkCredential(ConnectionCredentials.Username, ConnectionCredentials.Password);
        var ftpbResponse = await ftpRequest.GetResponseAsync();
        Stream responseStream = ftpbResponse.GetResponseStream();
        using (FileStream localFileStream = File.Create(localPath))
        {
            try
            {
                byte[] buffer = new byte[ConnectionCredentials.BufferSize];
                int bytesRead;
                while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await localFileStream.WriteAsync(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTimeOffset.Now}][{GetType().Name}][ERROR {ex.GetType().Name}] {ex.Message}");
                return false;
            }
        }
        Console.WriteLine("File downloaded successfully.");
        ftpRequest = null;
        return true;
    }

    private async Task<bool> UploadAction(Uri remotePath, string localPath)
    {
        FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(remotePath);
        ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
        ftpRequest.Credentials = new NetworkCredential(ConnectionCredentials.Username, ConnectionCredentials.Password);
        using (Stream localFileStream = File.OpenRead(localPath))
        using (Stream ftpStream = ftpRequest.GetRequestStream())
        {
            try
            {
                byte[] buffer = new byte[ConnectionCredentials.BufferSize];
                int bytesRead;
                while ((bytesRead = await localFileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await ftpStream.WriteAsync(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTimeOffset.Now}][{GetType().Name}][ERROR {ex.GetType().Name}] {ex.Message}");
                return false;
            }
        }
        Console.WriteLine("File uploaded successfully.");
        ftpRequest = null;
        return true;
        
    }
}