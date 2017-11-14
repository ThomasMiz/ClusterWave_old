using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterWave.Network
{
    static class MsgIndex
    {
        public const byte error = 9;
        public const byte chat = 1;
        public const byte statusUpdate = 2;
        public const byte disconnect = 3;
        public const byte playerMove = 4;
        public const byte playerAct = 5;
        public const byte scenarioRecieve = 6;
        public const byte assignId = 7;

        public class subIndex
        {
            public const byte worldUpdate = 20;
            public const byte playerUpdate = 21;
            public const byte BulletHit = 22;
            public const byte playerCreate = 23;

            public const byte playerExit = 31;
            public const byte connectionTimeout = 32;
            public const byte playerKicked = 33;
            public const byte playerConnect = 34;
            public const byte doneLoading = 35;

            public const byte dash = 40;
            public const byte left = 41;
            public const byte right = 42;
            public const byte up = 43;
            public const byte down = 44;
            public const byte rot = 45;

            public const byte smgShot = 51;
            public const byte shotyShot = 52;
            public const byte sniperShot = 53;
            public const byte shieldPlaced = 54;
            public const byte weaponSwap = 55;
        }
    }
}
