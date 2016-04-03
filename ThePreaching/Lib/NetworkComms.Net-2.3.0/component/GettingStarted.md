You can use NetworkComms.Net to easily communicate across your Xamarin.iOS, Xamarin.Android, Windows Phone, Linux Desktop and Windows Desktop applications. To get started simply download the component and reference the DLLs from your platform.

## Sending Data

You can send any custom object in a single line:

```csharp
using NetworkCommsDotNet;

public void ObjectStaticSend()
{
    try
    {
        CustomObject myObj = new CustomObject();
        NetworkComms.SendObject("MyPacket", "127.0.0.1", 10000, myObj);
    }
    catch(CommsException ex)
    {
        //Exceptions may occur if the target was not listening etc
        //You can decide what to do if something goes wrong here
    }
}
```

or you can use a connection specific method which provides significantly greater flexibility:

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

## Receiving Data

Custom objects can be received from anywhere in your application as follows:

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

## Custom Objects

NetworkComms.Net allows you to send any custom object, avoiding the need to ever handle raw byte streams. To ensure your custom object can be correctly serialised ensure it has the following:

+ Blank private constructor
+ Class is marked with [ProtoContract] attribute.
+ All fields that are to be serialised marked with [ProtoMember(n)], where n is a unique index.

Example:

```csharp
using ProtoBuf;

[ProtoContract]
class CustomObject
{
    [ProtoMember(1)]
    string _objectName = "Unspecified";;

    [ProtoMember(2)]
    int _objectIndex = 0;

    /// <summary>
    /// Parameterless constructor required to deserialise object
    /// </summary>
    private CustomObject() { }

    public CustomObject(string objectName, int objectIndex)
    {
        this._objectName = objectName;
        this._objectIndex = objectIndex;
    }
}
```

## Further Uses

NetworkComms.Net offers a plethora of features which give you the developer massive freedom. To discover some of the other features offered by NetworkComms.Net please see our further resources below. We always appreciate your feedback. If you feel there is an important feature missing please let us know via the website.

## Further Resources

* [API Documentation](http://www.networkcomms.net/api/)
* [Other Tutorials](http://www.networkcomms.net/tutorials/)
* [Support](http://www.networkcomms.net/support/)
