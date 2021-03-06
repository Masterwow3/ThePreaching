using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetworkCommsDotNet;
using DPSBase;

namespace ExamplesChat.iOS
{
    /// <summary>
    /// In an attempt to keep things as clear as possible all shared implementation across chat examples
    /// has been provided in this base class.
    /// </summary>
    public abstract class ChatAppBase
    {
        #region Private Fields
        /// <summary>
        /// A boolean used to track the very first initialisation
        /// </summary>
        protected bool FirstInitialisation { get; set; }

        /// <summary>
        /// Dictionary to keep track of which peer messages have already been written to the chat window
        /// </summary>
        protected Dictionary<ShortGuid, ChatMessage> lastPeerMessageDict = new Dictionary<ShortGuid, ChatMessage>();

        /// <summary>
        /// The maximum number of times a chat message will be relayed
        /// </summary>
        int relayMaximum = 3;

        /// <summary>
        /// A local counter used to track the number of messages sent from
        /// this instance.
        /// </summary>
        long messageSendIndex = 0;

        /// <summary>
        /// An optional encryption key to use should one be required.
        /// This can be changed freely but must obviously be the same
        /// for both sender and reciever.
        /// </summary>
        //string _encryptionKey = "ljlhjf8uyfln23490jf;m21-=scm20--iflmk;";
        #endregion

        #region Public Fields
        /// <summary>
        /// The type of connection currently used to send and recieve messages. Default is TCP.
        /// </summary>
        public ConnectionType ConnectionType { get; set; }

        /// <summary>
        /// The IP address of the server 
        /// </summary>
        public string ServerIPAddress { get; set; }

        /// <summary>
        /// The port of the server
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// The local name used when sending messages
        /// </summary>
        public string LocalName { get; set; }

        /// <summary>
        /// A boolean used to track if the local device is acting as a server
        /// </summary>
        public bool LocalServerEnabled { get; set; }

        /// <summary>
        /// A boolean used to track if encryption is currently being used
        /// </summary>
        public bool EncryptionEnabled { get; set; }
        #endregion

        /// <summary>
        /// Constructor for ChatAppBase
        /// </summary>
        public ChatAppBase(string name, ConnectionType connectionType)
        {
            LocalName = name;
            ConnectionType = connectionType;

            //Initialise the default values
            ServerIPAddress = "";
            ServerPort = 10000;
            LocalServerEnabled = false;
            EncryptionEnabled = false;
            FirstInitialisation = true;
        }

        #region NetworkComms.Net Methods
        /// <summary>
        /// Updates the configuration of this instance depending on set fields
        /// </summary>
        public void RefreshNetworkCommsConfiguration()
        {
            #region First Initialisation
            //On first initilisation we need to configure NetworkComms.Net to handle our incoming packet types
            //We only need to add the packet handlers once. If we call NetworkComms.Shutdown() at some future point these are not removed.
            if (FirstInitialisation)
            {
                FirstInitialisation = false;

                //Configure NetworkComms.Net to handle any incoming packet of type 'ChatMessage'
                //e.g. If we recieve a packet of type 'ChatMessage' execute the method 'HandleIncomingChatMessage'
                NetworkComms.AppendGlobalIncomingPacketHandler<ChatMessage>("ChatMessage", HandleIncomingChatMessage);

                //Configure NetworkComms.Net to perform some action when a connection is closed
                //e.g. When a connection is closed execute the method 'HandleConnectionClosed'
                NetworkComms.AppendGlobalConnectionCloseHandler(HandleConnectionClosed);
            }
            #endregion

            #region Optional Encryption
            //Configure encryption if requested
            //if (EncryptionEnabled && !NetworkComms.DefaultSendReceiveOptions.DataProcessors.Contains(DPSBase.DPSManager.GetDataProcessor<DPSBase.RijndaelPSKEncrypter>()))
            //{
            //    //Encryption is currently implemented using a pre-shared key (PSK) system
            //    //NetworkComms.Net supports multiple data processors which can be used with any level of granularity
            //    //To enable encryption globally (i.e. for all connections) we first add the encryption password as an option
            //    DPSBase.RijndaelPSKEncrypter.AddPasswordToOptions(NetworkComms.DefaultSendReceiveOptions.Options, _encryptionKey);
            //    //Finally we add the RijndaelPSKEncrypter data processor to the sendReceiveOptions
            //    NetworkComms.DefaultSendReceiveOptions.DataProcessors.Add(DPSBase.DPSManager.GetDataProcessor<DPSBase.RijndaelPSKEncrypter>());
            //}
            //else if (!EncryptionEnabled && NetworkComms.DefaultSendReceiveOptions.DataProcessors.Contains(DPSBase.DPSManager.GetDataProcessor<DPSBase.RijndaelPSKEncrypter>()))
            //{
            //    //If encryption has been disabled but is currently enabled
            //    //To disable encryption we just remove the RijndaelPSKEncrypter data processor from the sendReceiveOptions
            //    NetworkComms.DefaultSendReceiveOptions.DataProcessors.Remove(DPSBase.DPSManager.GetDataProcessor<DPSBase.RijndaelPSKEncrypter>());
            //}
            #endregion

            #region Local Server Mode and Connection Type Changes
            if (LocalServerEnabled && ConnectionType == ConnectionType.TCP && !TCPConnection.Listening())
            {
                //If we were previously listening for UDP we first shutdown comms.
                if (UDPConnection.Listening())
                {
                    AppendLineToChatHistory("Connection mode has been changed. Any existing connections will be closed.");
                    NetworkComms.Shutdown();
                }
                else
                {
                    AppendLineToChatHistory("Enabling local server mode. Any existing connections will be closed.");
                    NetworkComms.Shutdown();
                }

                //Start listening for new incoming TCP connections
                //Parameter is true so that we listen on a random port if the default is not available
                TCPConnection.StartListening(true);

                //Write the IP addresses and ports that we are listening on to the chatBox
                AppendLineToChatHistory("Listening for incoming TCP connections on:");
                foreach (var listenEndPoint in TCPConnection.ExistingLocalListenEndPoints())
                    AppendLineToChatHistory(listenEndPoint.Address + ":" + listenEndPoint.Port);

                //Add a blank line after the initialisation output
                AppendLineToChatHistory(System.Environment.NewLine);
            }
            else if (LocalServerEnabled && ConnectionType == ConnectionType.UDP && !UDPConnection.Listening())
            {
                //If we were previously listening for TCP we first shutdown comms.
                if (TCPConnection.Listening())
                {
                    AppendLineToChatHistory("Connection mode has been changed. Any existing connections will be closed.");
                    NetworkComms.Shutdown();
                }
                else
                {
                    AppendLineToChatHistory("Enabling local server mode. Any existing connections will be closed.");
                    NetworkComms.Shutdown();
                }

                //Start listening for new incoming UDP connections
                //Parameter is true so that we listen on a random port if the default is not available
                UDPConnection.StartListening(true);

                //Write the IP addresses and ports that we are listening on to the chatBox
                AppendLineToChatHistory("Listening for incoming UDP connections on:");
                foreach (var listenEndPoint in UDPConnection.ExistingLocalListenEndPoints())
                    AppendLineToChatHistory(listenEndPoint.Address + ":" + listenEndPoint.Port);

                //Add a blank line after the initialisation output
                AppendLineToChatHistory(System.Environment.NewLine);
            }
            else if (!LocalServerEnabled && (TCPConnection.Listening() || UDPConnection.Listening()))
            {
                //If the local server mode has been disabled but we are still listening we need to stop accepting incoming connections
                NetworkComms.Shutdown();
                AppendLineToChatHistory("Local server mode disabled. Any existing connections will be closed.");
                AppendLineToChatHistory(System.Environment.NewLine);
            }
            else if (!LocalServerEnabled &&
                ((ConnectionType == ConnectionType.UDP && NetworkComms.GetExistingConnection(ConnectionType.TCP).Count > 0) ||
                (ConnectionType == ConnectionType.TCP && NetworkComms.GetExistingConnection(ConnectionType.UDP).Count > 0)))
            {
                //If we are not running a local server but have changed the connection type after creating connections we need to close
                //existing connections.
                NetworkComms.Shutdown();
                AppendLineToChatHistory("Connection mode has been changed. Existing connections will be closed.");
                AppendLineToChatHistory(System.Environment.NewLine);
            }
            #endregion
        }

        /// <summary>
        /// Performs whatever functions we might so desire when we recieve an incoming ChatMessage
        /// </summary>
        /// <param name="header">The PacketHeader corresponding with the recieved object</param>
        /// <param name="connection">The Connection from which this object was recieved</param>
        /// <param name="incomingMessage">The incoming ChatMessage we are after</param>
        protected virtual void HandleIncomingChatMessage(PacketHeader header, Connection connection, ChatMessage incomingMessage)
        {
            //We only want to write a message once to the chat window
            //Because we support relaying and may recieve the same message twice from multiple sources
            //we use our history and message indexes to ensure we have a new message
            //We perform this action within a lock as HandleIncomingChatMessage could be called in parallel
            lock (lastPeerMessageDict)
            {
                if (lastPeerMessageDict.ContainsKey(incomingMessage.SourceIdentifier))
                {
                    if (lastPeerMessageDict[incomingMessage.SourceIdentifier].MessageIndex < incomingMessage.MessageIndex)
                    {
                        //If this message index is greater than the last seen from this source we can safely
                        //write the message to the ChatBox
                        AppendLineToChatHistory(incomingMessage.SourceName + " - " + incomingMessage.Message);

                        //We now replace the last recieved message with the current one
                        lastPeerMessageDict[incomingMessage.SourceIdentifier] = incomingMessage;
                    }
                }
                else
                {
                    //If we have never had a message from this source before then it has to be new
                    //by defintion
                    lastPeerMessageDict.Add(incomingMessage.SourceIdentifier, incomingMessage);
                    AppendLineToChatHistory(incomingMessage.SourceName + " - " + incomingMessage.Message);
                }
            }

            //This last section of the method is the relay feature
            //We start by checking to see if this message has already been relayed the maximum number of times
            if (incomingMessage.RelayCount < relayMaximum)
            {
                //If we are going to relay this message we need an array of 
                //all known connections, excluding the current one
                var allRelayConnections = (from current in NetworkComms.GetExistingConnection() where current != connection select current).ToArray();

                //We increment the relay count before we send
                incomingMessage.IncrementRelayCount();

                //We now send the message to every other connection
                foreach (var relayConnection in allRelayConnections)
                {
                    //We ensure we perform the send within a try catch
                    //To ensure a single failed send will not prevent the
                    //relay to all working connections.
                    try { relayConnection.SendObject("ChatMessage", incomingMessage); }
                    catch (CommsException) { /* Catch the comms exception, ignore and continue */ }
                }
            }
        }

        /// <summary>
        /// Performs whatever functions we might so desire when an existing connection is closed.
        /// </summary>
        /// <param name="connection">The closed connection</param>
        private void HandleConnectionClosed(Connection connection)
        {
            //We are going to write a message to the chat history when a connection disconnects
            //We perform the following within a lock incase mutliple connections disconnect simultaneously  
            lock (lastPeerMessageDict)
            {
                //Get the remoteIdentifier from the closed connection
                //This a unique GUID which can be used to identify peers
                ShortGuid remoteIdentifier = connection.ConnectionInfo.NetworkIdentifier;

                //If at any point we recieved a message with a matching identifier we can
                //include the peer name in the disconnection message.
                if (lastPeerMessageDict.ContainsKey(remoteIdentifier))
                    AppendLineToChatHistory("Connection with '" + lastPeerMessageDict[remoteIdentifier].SourceName + "' has been closed.");
                else
                    AppendLineToChatHistory("Connection with '" + connection.ToString() + "' has been closed.");

                //Last thing is to remove this peer from our message history
                lastPeerMessageDict.Remove(connection.ConnectionInfo.NetworkIdentifier);
            }
        }

        /// <summary>
        /// Send a message.
        /// </summary>
        public void SendMessage(string stringToSend)
        {
            //If we have tried to send a zero length string we just return
            if (stringToSend.Trim() == "") return;

            //We may or may not have entered some server connection information
            ConnectionInfo serverConnectionInfo = null;
            if (ServerIPAddress != "")
            {
                try { serverConnectionInfo = new ConnectionInfo(ServerIPAddress, ServerPort); }
                catch (Exception)
                {
                    ShowMessage("Failed to parse the server IP and port. Please ensure it is correct and try again");
                    return;
                }
            }

            //We wrap everything we want to send in the ChatMessage class we created
            ChatMessage chatMessage = new ChatMessage(NetworkComms.NetworkIdentifier, LocalName, stringToSend, messageSendIndex++);

            //We add our own message to the message history incase it gets relayed back to us
            lock (lastPeerMessageDict) lastPeerMessageDict[NetworkComms.NetworkIdentifier] = chatMessage;

            //We write our own message to the chatBox
            AppendLineToChatHistory(chatMessage.SourceName + " - " + chatMessage.Message);

            //Clear the input box text
            ClearInputLine();

            //If we provided server information we send to the server first
            if (serverConnectionInfo != null)
            {
                //We perform the send within a try catch to ensure the application continues to run if there is a problem.
                try
                {
                    if (ConnectionType == ConnectionType.TCP)
                        TCPConnection.GetConnection(serverConnectionInfo).SendObject("ChatMessage", chatMessage);
                    else if (ConnectionType == ConnectionType.UDP)
                        UDPConnection.GetConnection(serverConnectionInfo, UDPOptions.None).SendObject("ChatMessage", chatMessage);
                    else
                        throw new Exception("An invalid connectionType is set.");
                }
                catch (CommsException) { AppendLineToChatHistory("Error: A communication error occured while trying to send message to " + serverConnectionInfo + ". Please check settings and try again."); }
                catch (Exception) { AppendLineToChatHistory("Error: A general error occured while trying to send message to " + serverConnectionInfo + ". Please check settings and try again."); }
            }

            //If we have any other connections we now send the message to those as well
            //This ensures that if we are the server everyone who is connected to us gets our message
            //We want a list of all established connections not including the server if set
            List<ConnectionInfo> otherConnectionInfos;
            if (serverConnectionInfo != null)
                otherConnectionInfos = (from current in NetworkComms.AllConnectionInfo() where current.RemoteEndPoint != serverConnectionInfo.RemoteEndPoint select current).ToList();
            else
                otherConnectionInfos = NetworkComms.AllConnectionInfo();

            foreach (ConnectionInfo info in otherConnectionInfos)
            {
                //We perform the send within a try catch to ensure the application continues to run if there is a problem.
                try
                {
                    if (ConnectionType == ConnectionType.TCP)
                        TCPConnection.GetConnection(info).SendObject("ChatMessage", chatMessage);
                    else if (ConnectionType == ConnectionType.UDP)
                        UDPConnection.GetConnection(info, UDPOptions.None).SendObject("ChatMessage", chatMessage);
                    else
                        throw new Exception("An invalid connectionType is set.");
                }
                catch (CommsException) { AppendLineToChatHistory("Error: A communication error occured while trying to send message to " + info + ". Please check settings and try again."); }
                catch (Exception) { AppendLineToChatHistory("Error: A general error occured while trying to send message to " + info + ". Please check settings and try again."); }
            }

            return;
        }
        #endregion

        #region GUI Interface Methods
        /// <summary>
        /// Outputs the usage instructions to the chat window
        /// </summary>
        public void PrintUsageInstructions()
        {
            AppendLineToChatHistory("");
            AppendLineToChatHistory("Chat usage instructions:");
            AppendLineToChatHistory("");
            AppendLineToChatHistory("Step 1. Open atleast two chat applications. You can choose from Android, Windows Phone, iOS or native Windows versions.");
            AppendLineToChatHistory("Step 2. Enable local server mode in a single application, see settings.");
            AppendLineToChatHistory("Step 3. Provide remote server IP and port information in settings on remaining application.");
            AppendLineToChatHistory("Step 4. Start chatting.");
            AppendLineToChatHistory("");
            AppendLineToChatHistory("Note: Connections are established on the first message send.");
            AppendLineToChatHistory("");
        }

        /// <summary>
        /// Append the provided message to the chat history text box.
        /// </summary>
        /// <param name="message">Message to be appended</param>
        public abstract void AppendLineToChatHistory(string message);

        /// <summary>
        /// Clears the chat history
        /// </summary>
        public abstract void ClearChatHistory();

        /// <summary>
        /// Clears the input text box
        /// </summary>
        public abstract void ClearInputLine();

        /// <summary>
        /// Show a message box as an alternative to writing to the chat history
        /// </summary>
        /// <param name="message">Message to be output</param>
        public abstract void ShowMessage(string message);
        #endregion
    }
}