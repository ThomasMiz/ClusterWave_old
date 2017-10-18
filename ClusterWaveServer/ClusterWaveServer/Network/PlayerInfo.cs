using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterWaveServer.Network
{
    class PlayerInfo
    {
        String name;
        #pragma warning disable //disable warning para que no moleste hasta que hagas algo con esto
        int roundsWon, roundsLost;
        bool doneLoading;
        int id;

        public String Name { get { return name; } set { name = value; } }

        public PlayerInfo(string name, int id)
        {
            this.name = name;
            this.id = id;
        }

        public PlayerInfo(int id)
        {
            this.id = id;
        }

        public int GetId() { return id; }

        public void LostRound() { roundsLost--; }

        public void WonRound() { roundsWon++; }

        public void DoneLoading() { doneLoading = true; }

        public bool IsLoaded() { return doneLoading; }

    }
}
