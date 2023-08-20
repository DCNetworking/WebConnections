using WebConnections.Factory.FileTransferFactory;
using WebConnections.Factory.FileTransferFactory;

ConnectionCredentials connectionCredentials = 
    new(
        username: "create your own from", 
        password: "secured", 
        server: "environment"
    );
WebConnectionFactory webConnFactory = new WebConnectionFactory(connectionCredentials, ConnectionType.Sftp);
webConnFactory.CreateInstance();


