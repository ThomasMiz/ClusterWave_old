using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave
{
    class Player
    {
        String name;
        private int id;
        private int colorIndex;

        public String Name { get { return name; } set { name = value; } }

        public int Id { get { return id; } set { id = value; } } //set? not very object oriented...
        public int ColorIndex { get { return colorIndex; } }

        public Player()
        { //este constructor es un cancer y espero que sea temporal

        }

        public Player(string name, int id)
        {
            this.id = id;
            this.name = name;
        }
    }
}
