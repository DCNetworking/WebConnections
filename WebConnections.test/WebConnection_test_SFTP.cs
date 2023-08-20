using WebConnections.Factory.FileTransferFactory;
using WebConnections.Factory.FileTransferFactory;

namespace WebConnections.test;

public class WebConnection_test_SFTP
{
    ConnectionCredentials connectionCredentials = 
        new(
            username: "create your own from", 
            password: "secured", 
            server: "environment",
            port: 22
        );
    [Fact]
    public async Task IsTestConnection_Succeded()
    {
        // Act
        bool expected = true;
        // Arrange
        WebConnectionFactory connectionFactory = new WebConnectionFactory(connectionCredentials, ConnectionType.Sftp);
        connectionFactory.CreateInstance();
        bool actual = await connectionFactory.Instance.TestConnection();
        // Assert
        Assert.Equal(expected,actual);
    }
    
    [Fact]
    public async Task IsTestConnection_UploadSucceded()
    {
        // Act
        bool expected = true;
        // Arrange
        WebConnectionFactory connectionFactory = new WebConnectionFactory(connectionCredentials, ConnectionType.Sftp);
        connectionFactory.CreateInstance();
        bool actual = await connectionFactory.Instance.Upload("/test.txt", "/test.txt");
        // Assert
        Assert.Equal(expected,actual);
    }
    
    [Fact]
    public async Task IsTestConnection_DownloadSucceded()
    {
        // Act
        bool expected = true;
        // Arrange
        WebConnectionFactory connectionFactory = new WebConnectionFactory(connectionCredentials, ConnectionType.Sftp);
        connectionFactory.CreateInstance();
        bool actual = await connectionFactory.Instance.Download("/test.txt", "/test.txt");
        // Assert
        Assert.Equal(expected,actual);
    }
}