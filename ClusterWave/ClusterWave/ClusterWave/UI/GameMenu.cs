using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ClusterWave.UI.Drawables;
using ClusterWave.UI.InGame;
using ClusterWave.UI.MenuManager;
using Microsoft.Xna.Framework.Input;

namespace ClusterWave.UI
{
    class GameMenu : Menu
    {
        public healthBar hp;
        public ability shieldAb, dashAb;
        //public weaponDisplay weapon;

        public static KeyboardState ks, oldks;

        public GameMenu()
        {
            hp = new healthBar(200, 25, inGameShield);
            dashAb = new ability(1,2,inGameDash);
            shieldAb = new ability(1, 0.25f, inGameShield);
            //weapon = new weaponDisplay(1f, 3f, 1, 50, inGameWeapons[0]);
        }
        public void Draw(SpriteBatch batch, GraphicsDevice device)
        {

            hp.Draw(batch, device);
            shieldAb.Draw(batch, device);
            dashAb.Draw(batch, device);
            //weapon.Draw(batch, device);
        }
        public override void Update()
        {
            base.Update();

            oldks = ks;
            ks = Keyboard.GetState();

            /*if (ks.IsKeyDown(Keys.Delete))
                hp.damage(1);
            if (ks.IsKeyDown(Keys.Enter))
                hp.heal(1);
            if (ks.IsKeyDown(Keys.Space))
                dashAb.Use();
            if (ks.IsKeyDown(Keys.LeftShift))
                shieldAb.Use();*/
            //hp.damage(1);


            dashAb.Update();
            shieldAb.Update();
            //weapon.Update();
        }
        public override void Resize(Vector2 pos, Vector2 size)
        {
            //weapon.Fire(true);


            //weapon.Resize(new Vector2(Game1.ScreenWidth - (Game1.ScreenWidth / 15 * 4 - 10) + Game1.ScreenWidth / 15, 9 / 10f * Game1.ScreenHeight - (Game1.ScreenWidth / 30)), new Vector2(2 * Game1.ScreenWidth / 15, Game1.ScreenWidth / 15));
            hp.Resize(new Vector2(Game1.ScreenWidth/50, 9/10f * Game1.ScreenHeight), new Vector2(Game1.ScreenWidth/40, Game1.ScreenHeight/20));
            shieldAb.Resize(new Vector2(Game1.ScreenWidth - (Game1.ScreenWidth/15 * 5), 9 / 10f * Game1.ScreenHeight - (Game1.ScreenWidth/30)), new Vector2(Game1.ScreenWidth / 15, Game1.ScreenWidth/ 15));
            dashAb.Resize(new Vector2(Game1.ScreenWidth - (Game1.ScreenWidth / 15 * 4 - 10), 9 / 10f * Game1.ScreenHeight - (Game1.ScreenWidth / 30)), new Vector2(Game1.ScreenWidth / 15, Game1.ScreenWidth / 15));
        }
    }
}
