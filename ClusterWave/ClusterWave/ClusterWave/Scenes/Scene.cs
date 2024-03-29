﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClusterWave.Scenes
{
    abstract class Scene
    {
        /// <summary>The GraphicsDevice from Game1</summary>
        protected GraphicsDevice GraphicsDevice { get { return game.GraphicsDevice; } }
        /// <summary>The SpriteBatch from Game1</summary>
        protected SpriteBatch batch { get { return game.batch; } }
        /// <summary>A reference to the Game1 instance of the program</summary>
        protected Game1 game;

        public Scene()
        {
            game = Game1.game;
        }

        public abstract void Update();
        public abstract void Draw();

        public abstract void OnResize();
        public abstract void OnExit();
    }
}
