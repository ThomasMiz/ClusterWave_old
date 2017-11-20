using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ClusterWaveServer.Scenario;
using ClusterWaveServer.Scenario.Dynamic;
using ClusterWaveServer.Network;
using Lidgren.Network;

namespace ClusterWaveServer.Scenes
{
    class InGameScene : Scene
    {
        Scenario.Scenario scenario;

        BulletList bullets;

        public Scenario.Scenario Scenario { get { return scenario; } }
        DebugRenderer.DebugManager debugManager;

        NetPlayer[] netPlayers;

        float timeSinceLastUpdate;

        public InGameScene(Scenario.Scenario scenario) : base()
        {
            this.scenario = scenario;
            debugManager = new DebugRenderer.DebugManager(scenario).Start();
            netPlayers = new NetPlayer[8];
            bullets = new BulletList();
        }

        public override void Update()
        {
            for (int i = 0; i < 8; i++)
                if (netPlayers[i] != null) netPlayers[i].UpdatePrePhysics();
            scenario.PhysicsStep(Program.DeltaTime);
            for (int i = 0; i < 8; i++)
                if (netPlayers[i] != null) netPlayers[i].UpdatePostPhysics();

            if (timeSinceLastUpdate > 1) UpdatePositions();

            timeSinceLastUpdate += Program.DeltaTime;
        }

        public override void OnPacket(Lidgren.Network.NetBuffer msg)
        {
            byte index = msg.ReadByte();
            switch (index)
            {
                case MsgIndex.statusUpdate:
                    if (msg.ReadByte() == MsgIndex.subIndex.playerCreate)
                    {
                        byte id = msg.ReadByte();
                        netPlayers[id] = new NetPlayer(scenario.PlayersPos[id], this, Program.server.players[id]);
                    }
                    break;
                case MsgIndex.error:

                    break;
                case MsgIndex.playerAct:
                    PlayerAct((NetIncomingMessage)msg);
                    break;
                case MsgIndex.playerMove:
                    MovePlayer((NetIncomingMessage)msg);

                    break;
                case MsgIndex.playerRot:
                    PlayerRotate((NetIncomingMessage)msg);
                    break;

            }
        }

        public override void OnExit()
        {
            debugManager.Exit();
        }

        void UpdatePositions()
        {
            Console.WriteLine("BEEP BEEP UPDATING SHEEP");
            Program.server.UpdatePositions(netPlayers);
            timeSinceLastUpdate = 0;
        }

        void PlayerAct(NetIncomingMessage msg)
        {
            int id = Program.server.ConnectionToId[msg.SenderConnection];
            Bullet tempBullet;
            switch (msg.ReadByte())
            {
                case MsgIndex.subIndex.smgShot:
                    tempBullet = Bullet.CreateMachinegun(scenario.PhysicsWorld, netPlayers[id]);
                    bullets.Add(tempBullet);
                    Program.server.ShootSmg(id, tempBullet.id);
                    break;
                case MsgIndex.subIndex.shotyShot:
                    tempBullet = Bullet.CreateShotgun(scenario.PhysicsWorld, netPlayers[id]);
                    bullets.Add(tempBullet);
                    Program.server.ShootShotgun(id, tempBullet.id);
                    break;
                case MsgIndex.subIndex.sniperShot:
                    tempBullet = Bullet.CreateSniper(scenario.PhysicsWorld, netPlayers[id]);
                    bullets.Add(tempBullet);
                    Program.server.ShootSniper(id, tempBullet.id);
                    break;
                case MsgIndex.subIndex.shieldPlaced:
                    netPlayers[id].MoveRight();
                    break;
            }
        }

        void MovePlayer(NetIncomingMessage msg)
        {
            int id = Program.server.ConnectionToId[msg.SenderConnection];
            switch (msg.ReadByte())
            {
                case MsgIndex.subIndex.up:
                    netPlayers[id].MoveUp();
                    break;
                case MsgIndex.subIndex.down:
                    netPlayers[id].MoveDown();
                    break;
                case MsgIndex.subIndex.left:
                    netPlayers[id].MoveLeft();
                    break;
                case MsgIndex.subIndex.right:
                    netPlayers[id].MoveRight();
                    break;
                case MsgIndex.subIndex.rot:
                    netPlayers[id].Rotate(msg.ReadFloat());
                    Program.server.Rotate(id, msg.ReadFloat());
                    break;
            }
        }

        void PlayerRotate(NetIncomingMessage msg)
        {
            int id = Program.server.ConnectionToId[msg.SenderConnection];
            float rot = msg.ReadFloat();
            netPlayers[id].Rotate(rot);
            Program.server.Rotate(id, rot);
        }

    }
}
