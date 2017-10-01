using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave
{
    class Player
    {
        String name;
        Texture2D texture;

        public Texture2D Texture { get { return texture; } }
        public String Name { get { return name; } set { name = value; } }

        public int id { get { return id; } set { id = value; } }

        public Player(string name, int id)
        {
            this.id = id;
            this.name = name;
        }
    }
}
