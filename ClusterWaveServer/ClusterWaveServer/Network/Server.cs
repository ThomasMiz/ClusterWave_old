using System;
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

        public PlayerInfo[] players;

        string scenario;

        int connectedPlayers;
        int maximumCapacity = 8;
        int loadedPlayers;

        bool matchInProcess = false;

        public Dictionary<NetConnection, int> ConnectionToId;
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
            if (scene != null)
                scene.Update();
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
                        switch (incomingMsg.PeekByte())
                        {
                            case MsgIndex.chat:
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
                                break;
                            case MsgIndex.disconnect:
                                #region ConnectionHandling
                                incomingMsg.ReadByte();
                                int subIndex = incomingMsg.ReadByte();
                                if (subIndex == MsgIndex.subIndex.playerConnect)
                                {
                                    Console.WriteLine("Player connected from : " + incomingMsg.SenderConnection.RemoteEndPoint.Address);
                                    ConnectionMessageRecieve(incomingMsg);
                                }
                                if (subIndex == MsgIndex.subIndex.doneLoading)
                                {
                                    int id = ConnectionToId[incomingMsg.SenderConnection]; 
                                    players[id].DoneLoading();
                                    loadedPlayers++;
                                    Console.WriteLine("Player " + players[id].Name + " has finished loading");
                                    if (connectedPlayers == loadedPlayers) startMatch();
                                }
                                #endregion
                                break;
                            default:
                                scene.OnPacket(incomingMsg);
                                break;
                        }
                        #endregion
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        #region StatusChange
                        index = incomingMsg.ReadByte();
                        switch (incomingMsg.SenderConnection.Status)
                        {
                            case NetConnectionStatus.Connected:
                                Console.WriteLine("Connection started");
                                break;
                        }
                        #endregion
                        break;
                }

                server.Recycle(incomingMsg);
                incomingMsg = server.ReadMessage();
            }
        }

        public void SetScenario(string file)
        {
            scenario = file;
        }

        public void SetScene(Scene scene)
        {
            this.scene = scene;
        }

        public void startMatch() 
        {
            matchInProcess = true;
            createPlayers();
        }

        void createPlayers()
        {
            for (int id = 0; id < maximumCapacity; id++)
            {
                if (players[id] != null)
                {
                    NetOutgoingMessage newPlayerMsg = server.CreateMessage();
                    newPlayerMsg.Write(MsgIndex.statusUpdate);
                    newPlayerMsg.Write(MsgIndex.subIndex.playerCreate);
                    newPlayerMsg.Write((byte)id);
                    newPlayerMsg.Write(((InGameScene)scene).Scenario.PlayersPos[id].X);
                    newPlayerMsg.Write(((InGameScene)scene).Scenario.PlayersPos[id].Y);
                    //newPlayerMsg.Write()
                    scene.OnPacket(newPlayerMsg);
                    server.SendToAll(newPlayerMsg, NetDeliveryMethod.ReliableOrdered);

                }
            }
        }

        public void SendScenario()
        {
            signalScenarioSend();
            Scenario.ScenarioLoader load = new Scenario.ScenarioLoader(scenario);
            load.OnCreationPacket += Program.server.sendByteArray;
            Program.server.SetScene(new InGameScene(load.CreateScenario()));
            load.OnCreationPacket -= Program.server.sendByteArray;
        }

        void signalScenarioSend()
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(MsgIndex.statusUpdate);
            msg.Write(MsgIndex.subIndex.gameStarting);
            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void sendByteArray(byte[] byteArray)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(MsgIndex.scenarioRecieve);                                 
            msg.Write(byteArray);
            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        void ConnectionMessageRecieve(NetIncomingMessage msg)
        {
            string name = msg.ReadString();
            if (connectedPlayers <= maximumCapacity)
            {
                for (int i = 0; i < maximumCapacity; i++)
                {
                    if (players[i] == null)
                    {
                        ConnectionToId.Add(msg.SenderConnection, i);
                        players[i] = new PlayerInfo(name, i);
                        Console.WriteLine("Player : " + name + " connected with ID : " + i);
                        PlayerAssingIdMessage(i, msg.SenderConnection);
                        connectedPlayers += 1;
                        PlayerConnectedMessage(i, name, msg.SenderConnection);
                        break;
                    }
                }
            }
            Console.WriteLine("This brings the total connected player to : " + connectedPlayers + "/" + maximumCapacity);
        }

        void PlayerAssingIdMessage(int id, NetConnection connection)
        {
            NetOutgoingMessage idAssingMsg = server.CreateMessage();
            idAssingMsg.Write(MsgIndex.assignId);
            idAssingMsg.Write((byte)id);
            server.SendMessage(idAssingMsg, connection, NetDeliveryMethod.ReliableUnordered);
        }

        void PlayerConnectedMessage(int id, string name, NetConnection connection)
        {
            NetOutgoingMessage newPlayerMessage = server.CreateMessage();
            newPlayerMessage.Write(MsgIndex.disconnect);
            newPlayerMessage.Write(MsgIndex.subIndex.playerConnect);
            newPlayerMessage.Write((byte)id);
            newPlayerMessage.Write(name);
            server.SendToAll(newPlayerMessage, connection, NetDeliveryMethod.ReliableOrdered,0);
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

        public void MoveLeft(int id)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(MsgIndex.playerMove);
            msg.Write(MsgIndex.subIndex.left);
            msg.Write((byte)id);
            server.SendToAll(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void MoveRight(int id)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(MsgIndex.playerMove);
            msg.Write(MsgIndex.subIndex.right);
            msg.Write((byte)id);
            server.SendToAll(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void MoveUp(int id)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(MsgIndex.playerMove);
            msg.Write(MsgIndex.subIndex.up);
            msg.Write((byte)id);
            server.SendToAll(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void MoveDown(int id)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(MsgIndex.playerMove);
            msg.Write(MsgIndex.subIndex.down);
            msg.Write((byte)id);
            server.SendToAll(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void ShootSmg()
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(MsgIndex.playerAct);
            msg.Write(MsgIndex.subIndex.smgShot);
            server.SendToAll(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void ShootShotgun()
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(MsgIndex.playerAct);
            msg.Write(MsgIndex.subIndex.shotyShot);
            server.SendToAll(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void ShootSniper()
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(MsgIndex.playerAct);
            msg.Write(MsgIndex.subIndex.sniperShot);
            server.SendToAll(msg, NetDeliveryMethod.ReliableUnordered);
        }

        public void Shield()
        {
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write(MsgIndex.playerAct);
            msg.Write(MsgIndex.subIndex.shieldPlaced);
            server.SendToAll(msg, NetDeliveryMethod.ReliableUnordered);
        }

    }
}
