﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using ClusterWaveServer.Scenes;

namespace ClusterWaveServer.Network
{

    class Server
    {
        NetServer server;
        NetPeerConfiguration config;
        Scene scene;

        PlayerInfo[] players;

        int connectedPlayers;
        int maximumCapacity = 8;

        Dictionary<NetConnection,int> ConnectionToId;
        //26200 = Port


        public Server()
        {
            config = new NetPeerConfiguration("Cluster Wave");
            config.Port = 26200;
            server = new NetServer(config);
            server.Start();

            players = new PlayerInfo[maximumCapacity];

            ConnectionToId = new Dictionary<NetConnection, int>();
        }

        public void UpdateServer()
        {
            CheckMessage();
        }

        void CheckMessage()
        {
            NetIncomingMessage incomingMsg;
            incomingMsg = server.ReadMessage();

            while (incomingMsg != null)
            {
                byte index;

                switch (incomingMsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        #region Data
                        if (incomingMsg.PeekByte() != 3)
                        {
                            if (incomingMsg.PeekByte() == MsgIndex.chat)
                            {
                                #region ChatMessage
                                incomingMsg.ReadByte();
                                String str = incomingMsg.ReadString();
                                if (str.StartsWith("/"))
                                {
                                    String[] spl = str.Split(' ');
                                    switch (spl[0])
                                    {
                                        case "/help":
                                            SendChatMessage("There are no server commands other than help. Haha! &red;xddd", incomingMsg.SenderConnection);
                                            break;
                                        case "/colorhelp":
                                            SendChatMessage("You can use different colors and fonts using '&' followed by an identifier and a ';', typing \"&identifier;\". Possible identifiers:", incomingMsg.SenderConnection);
                                            SendChatMessage("   - &r;normal", incomingMsg.SenderConnection);
                                            SendChatMessage("   - &red;red", incomingMsg.SenderConnection);
                                            SendChatMessage("   - &blue;blue", incomingMsg.SenderConnection);
                                            SendChatMessage("   - &green;green", incomingMsg.SenderConnection);
                                            SendChatMessage("   - &macri;macri", incomingMsg.SenderConnection);
                                            SendChatMessage("   - &magenta;magenta", incomingMsg.SenderConnection);
                                            SendChatMessage("   - &i;italics", incomingMsg.SenderConnection);
                                            SendChatMessage("   - &b;bold", incomingMsg.SenderConnection);
                                            SendChatMessage("   - &w;webdings &r;(webdings)", incomingMsg.SenderConnection);
                                            SendChatMessage("   - &r;reset (takes out all formatting)", incomingMsg.SenderConnection);
                                            SendChatMessage("And lots of other colors!", incomingMsg.SenderConnection);
                                            break;
                                        default:
                                            SendChatMessage("&red;Command not found.", incomingMsg.SenderConnection);
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(str);
                                    SendChatMessage(str);
                                }
                                #endregion
                            }
                            else
                                if (scene != null) scene.OnPacket(incomingMsg);
                        }
                        else
                        {
                            Console.WriteLine("Player connected from : " + incomingMsg.SenderConnection.RemoteEndPoint.Address);
                            ConnectionMessageRecieve(incomingMsg);
                        }
                        #endregion
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        #region StatusChange
                        index = incomingMsg.ReadByte();
                        switch (index)
                        {
                            case 4:
                                Console.WriteLine("COSA");
                                break;
                        }
                        #endregion
                        break;
                }

                server.Recycle(incomingMsg);
                incomingMsg = server.ReadMessage();
            }
        }

        public void SendScenario(String file)
        {
            //para el juego terminado, agregar checkeo de que se haya cargado bien? (load.IsOk)
            Scenarios.ScenarioLoader load = new Scenarios.ScenarioLoader(file);
            load.OnCreationPacket += sendByteArray;
            scene = new InGameScene(this,load.CreateScenario());
            load.OnCreationPacket -= sendByteArray;
        }

        void sendByteArray(byte[] byteArray)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write((byte)6);                                 //cambia esto por una const
            msg.Write(byteArray);
            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        void ConnectionMessageRecieve(NetIncomingMessage msg)
        {
            msg.ReadByte();
            byte index = msg.ReadByte();
            string name = msg.ReadString();
            if (connectedPlayers <= maximumCapacity)
            {
                for (int i = 0; i <= maximumCapacity; i++)
                {
                    if (players[i] == null)
                    {
                        ConnectionToId.Add(msg.SenderConnection, i);
                        players[i] = new PlayerInfo(name,i);
                        Console.WriteLine("Player : " + name + " connected with ID : " + i);
                        break;
                    }
                }
            }

        }

        void SendChatMessage(String message)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(MsgIndex.chat);
            msg.Write(message);
            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        void SendChatMessage(String message, NetConnection to)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(MsgIndex.chat);
            msg.Write(message);
            server.SendMessage(msg, to, NetDeliveryMethod.ReliableOrdered);
        }
    }
}