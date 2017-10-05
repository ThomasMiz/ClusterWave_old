using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace ClusterWave.Network
{
    delegate void OnPacketArrive(NetIncomingMessage msg);

    class Client
    {
        public Chat chat;

        NetPeerConfiguration config;
        NetClient client;
        Player clientPlayer;
        string name;

        public event OnPacketArrive OnPacket;

        public Client()
        {
            chat = new Chat();
            chat.OnLineEntered += OnChatLineEntered;

            config = new NetPeerConfiguration("Cluster Wave");
            client = new NetClient(config);
            client.Start();
        }

        void OnChatLineEntered(ref string line)
        {
            if (line.StartsWith("/"))
            {
                #region LocalCommands
                String[] thing = line.Split(' ');
                switch (thing[0])
                {
                    case "/connect":
                        if (thing.Length > 2)
                            connect(thing[1], thing[2]); //para "/connect <ip> <name>"
                        break;
                    default:
                        SendAsChat(line);
                        break;
                }
                #endregion
            }
            else if (client.ConnectionStatus == NetConnectionStatus.Connected)
            {
                SendAsChat(line);
            }
            else
                chat.Add(line);
        }

        public void connect(string ip, string name)
        {
            this.name = name;
            client.Connect(ip, 26200);
            chat.Add(String.Concat("&green;[Client] Attempting connection to ", ip, "..."));
        }

        public void Update()
        {
            chat.Update();

            NetIncomingMessage incomingMsg = client.ReadMessage();

            while (incomingMsg != null)
            {
                if (incomingMsg.MessageType == NetIncomingMessageType.Data)
                {
                    #region Data
                    byte index = incomingMsg.PeekByte();
                    if (index == MsgIndex.chat)
                    {
                        incomingMsg.ReadByte();
                        chat.Add(incomingMsg.ReadString());
                    }
                    else if (index == MsgIndex.assignId)
                    {
                        incomingMsg.ReadByte();
                        clientPlayer = new Player(name, incomingMsg.ReadByte());
                    }
                    else if (OnPacket != null)
                        OnPacket(incomingMsg);
                    #endregion
                }
                else if (incomingMsg.MessageType == NetIncomingMessageType.StatusChanged)
                {
                    #region StatusChanged
                    if (incomingMsg.SenderConnection.Status == NetConnectionStatus.Connected)
                    {
                        Console.WriteLine("Connected to Server at IP :" + incomingMsg.SenderConnection.RemoteEndPoint.Address);
                        sendInfo();
                    }
                    #endregion
                }

                Console.WriteLine(incomingMsg.MessageType);
                incomingMsg = client.ReadMessage();
            }

            client.Recycle(incomingMsg);
        }

        void sendInfo()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.disconnect);
            msg.Write(MsgIndex.subIndex.playerConnect);
            msg.Write(name);
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        void SendAsChat(String text)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.chat);
            msg.Write(text);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void signalLoadingIsFinished()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.scenarioRecieve);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void sendMove(byte dir) //wtf es esto?
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.playerMove);
            msg.Write(dir);
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }
    }
}
