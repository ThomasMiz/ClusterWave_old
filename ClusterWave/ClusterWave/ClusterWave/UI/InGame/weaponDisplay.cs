using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClusterWave.UI.Drawables;

namespace ClusterWave.UI.InGame
{
    class weaponDisplay
    {
        Vector2 pos, size;
        float time, switchTime, reloadTime;
        int fireRate, maxAmmo, currentAmmo;
        bool isEquipped, isFiring, isReloading;

        DrawChainText text;
        Texture2D tex;
        /// <summary>
        /// Base class for displaying weapons
        /// </summary>
        /// <param name="switchTime">The time(in seconds) the weapon takes to equip</param>
        /// <param name="reloadTime">The time(in seconds) the weapon takes to reload</param>
        /// <param name="fireRate">The amount of bullets (per second) that the weapon fires</param>
        /// <param name="ammo">The amount of bullets the weapon holds</param>
        /// <param name="tex">The texture to display</param>
        public weaponDisplay(float switchTime, float reloadTime, int fireRate, int ammo, Texture2D tex)
        {
            this.switchTime = switchTime;
            this.reloadTime = reloadTime;
            this.fireRate = fireRate;
            this.maxAmmo = ammo;
            this.tex = tex;

            text = new DrawChainText(new Color[] { Color.Black, Color.White }, new Vector2(1, 1));
        }
        public void Update() 
        {
            if (isFiring)
                currentAmmo -= (int)(fireRate * Game1.DeltaTime);

            if (currentAmmo <= 0)
                isReloading = true;

            if (isReloading)
                if (time >= reloadTime)
                {
                    currentAmmo = maxAmmo;
                    isReloading = false;
                    time = 0;
                }
                else
                    time += 1 * Game1.DeltaTime;

            
        }
        public void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            batch.Begin();

            batch.Draw(tex, new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y), Color.White);
            text.Draw(batch, device, String.Format("{0:0.0}", time), Vector2.Zero);

            batch.End();
        }
        public void Resize(Vector2 pos, Vector2 size)
        {
            this.pos = pos;
            this.size = size;
            text.Resize(pos + new Vector2(pos.X/16, pos.Y/16), 1);
        }
        public void Fire(bool b) 
        {
            isFiring = b;
        }
        public void equip()
        {

        }
    }
}
