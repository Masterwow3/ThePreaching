NetworkComms.Net is a fully featured [network library][4] with which high performance, flexible, network functionality can be effortlessly added to any application. Developing networked applications can become a very time consuming task, using NetworkComms.Net you can be up and running within minutes.

Features include:

+ TCP & UDP.
+ IPv4 & IPv6.
+ Integrated serialisation, compression & encryption.
+ Full [online documentation][1], [tutorials][2] and [support][3].
+ Platform independent implementation.
+ Data prioritisation.
+ RPC (Remote Procedure Call) capabilities.
+ Completely thread-safe.
+ Extremely flexible usage-cases.
+ Full logging and debugging capabilities.
+ Support for sending very large (>1TB) files.
+ Multiple network adapter support.

This component has been restricted in the following ways:

+ **Maximum 1 concurrent connection.**
+ **No UDP broadcast.**
+ **No encryption.**

These restrictions can be removed by purchasing licenses from <http://www.networkcomms.net>.

Implementation is platform independent. Custom objects can be sent either with a very concise global method:

```csharp
using NetworkCommsDotNet;

public void ObjectStaticSend()
{
    CustomObject myObj = new CustomObject();
    NetworkComms.SendObject("MyPacket", "127.0.0.1", 10000, myObj);
}
```
or connection specific methods, providing significantly greater flexibility:

```csharp
using NetworkCommsDotNet;
using SevenZipLZMACompressor;

public CustomObject ObjectSpecificSend()
{
    //Select custom serialisers and data processors
    SendReceiveOptions customOptions = new SendReceiveOptions<ProtobufSerializer, LZMACompressor>();

    //Ensure the server received each completed packet correctly
    customOptions.ReceiveConfirmationRequired = true;

    //Create a TCPConnection. Also see UDPConnection.
    ConnectionInfo serverInfo = new ConnectionInfo("::1", 10000);
    Connection conn = TCPConnection.GetConnection(serverInfo, customOptions);

    //Run the method 'ConnectionCloseMethod' if this connection closes
    conn.AppendShutdownHandler(ConnectionCloseMethod);

    //Send a 'MyPacket' packet and wait synchronously upto 1000ms for the reply
    //The reply will be automatically be converted from bytes into a <CustomObject>
    CustomObject mySendObj = new CustomObject();
    CustomObject myReceivedObj = conn.SendReceiveObject<CustomObject>("MyPacket", "MyPacketReply", 1000, mySendObj);

    return myReceivedObj;
}
```

Custom objects can be received as follows:

```csharp
using NetworkCommsDotNet;

public static void StartListening()
{
    //Configure NetworkComms.Net to handle incoming 'MyPacket' packets
    NetworkComms.AppendGlobalIncomingPacketHandler<CustomObject>("MyPacket", IncomingMyPacketHandler);

    //Start listening for TCP connections
    TCPConnection.StartListening();
    //Additional you can also start listening for UDP connections
    //UDPConnection.StartListening();
}

static void IncomingMyPacketHandler(PacketHeader header, Connection conn, CustomObject myObj)
{
    Console.WriteLine("Recieved custom object from {0}", conn.ConnectionInfo.ToString());
    
    //Perform further functions on myObj
    ....
}
```

[1]: http://www.networkcomms.net/api/
[2]: http://www.networkcomms.net/tutorials/
[3]: http://www.networkcomms.net/support/
[4]: http://www.networkcomms.net/features/