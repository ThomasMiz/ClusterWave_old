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
        public Player clientPlayer;
        public Player[] players;
        string name;

        public event OnPacketArrive OnPacket;

        public Client()
        {
            chat = new Chat();
            chat.OnLineEntered += OnChatLineEntered;

            config = new NetPeerConfiguration("Cluster Wave");
            client = new NetClient(config);
            client.Start();

            players = new Player[8];
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
                    case "/local":
                        connect("127.0.0.1", "debugTest");
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
                    switch (index)
                    {
                        case MsgIndex.chat:

                            break;
                        case MsgIndex.assignId:

                            break;
                        case MsgIndex.disconnect:
                            incomingMsg.ReadByte();
                            if (incomingMsg.ReadByte() == MsgIndex.subIndex.playerConnect)
                            {
                                int id = incomingMsg.ReadByte();
                                string name = incomingMsg.ReadString();
                                players[id] = new Player(name,id);
                            }
                            break;
                    }
                    if (index == MsgIndex.chat)
                    {
                        incomingMsg.ReadByte();
                        chat.Add(incomingMsg.ReadString());
                    }
                    else if (index == MsgIndex.assignId)
                    {
                        incomingMsg.ReadByte();
                        int id = incomingMsg.ReadByte();
                        players[id] = new Player(name,id);
                        clientPlayer = players[id];
                    }
                    else if (index == MsgIndex.disconnect)
                    {
                        
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
                        //Console.WriteLine("Connected to Server at IP :" + incomingMsg.SenderConnection.RemoteEndPoint.Address);
                        sendInfo();
                        chat.Add(String.Concat("&green;[Client] Connected to Server at IP :" + incomingMsg.SenderConnection.RemoteEndPoint.Address));
                    }
                    #endregion
                }

                //Console.WriteLine(incomingMsg.MessageType);
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
            msg.Write(MsgIndex.disconnect);
            msg.Write(MsgIndex.subIndex.doneLoading);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendRotation(float rot)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.playerMove);
            msg.Write(MsgIndex.subIndex.rot);
            msg.Write(rot);
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void MoveLeft()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.playerMove);
            msg.Write(MsgIndex.subIndex.left);
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void MoveRight()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.playerMove);
            msg.Write(MsgIndex.subIndex.right);
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void MoveUp()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.playerMove);
            msg.Write(MsgIndex.subIndex.up);
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void MoveDown()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.playerMove);
            msg.Write(MsgIndex.subIndex.down);
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void ShootSmg()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.playerAct);
            msg.Write(MsgIndex.subIndex.smgShot);
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void ShootShotgun()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.playerAct);
            msg.Write(MsgIndex.subIndex.shotyShot);
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void ShootSniper()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.playerAct);
            msg.Write(MsgIndex.subIndex.sniperShot);
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void Shield()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(MsgIndex.playerAct);
            msg.Write(MsgIndex.subIndex.shieldPlaced);
            client.SendMessage(msg, NetDeliveryMethod.ReliableUnordered);
        }

    }
}
