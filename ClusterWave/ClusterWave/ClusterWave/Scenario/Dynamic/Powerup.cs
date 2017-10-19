using System;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

namespace ClusterWave.Scenario.Dynamic
{
    class Powerup
    {
        static Texture2D texture;
        static float drawScale;
        public static Effect DistortEffect;
        public static EffectParameter DistortTimeParam, DistortTexParam, RedOffParam, GreenOffParam, BlueOffParam, DistortMultParam;
        public static void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Scenario/Powerup/texture");
            drawScale = Constants.PowerupRadius * 2.0f / texture.Width;

            DistortEffect = Content.Load<Effect>("Scenario/Powerup/distort");
            DistortTimeParam = DistortEffect.Parameters["time"];
            DistortTexParam = DistortEffect.Parameters["tex"];
            RedOffParam = DistortEffect.Parameters["redOff"];
            GreenOffParam = DistortEffect.Parameters["greenOff"];
            BlueOffParam = DistortEffect.Parameters["blueOff"];
            DistortMultParam = DistortEffect.Parameters["multiply"];
            DistortMultParam.SetValue(1f);
        }


        private Vector2 pos, origin;
        private bool active;
        private Body body;

        //float m = 1;

        public Vector2 Position { get { return pos; } }
        public bool IsActive { get { return active; } }

        public Powerup(Vector2 pos, World physicsWorld)
        {
            this.pos = pos;
            active = true;
            body = new Body(physicsWorld, pos, 0f, this);
            body.BodyType = BodyType.Static;
            body.IsSensor = true;
            body.CollisionCategories = Constants.PowerupCategory;
            body.CollidesWith = Constants.PowerupCollideWith;
            Fixture f = body.CreateFixture(new CircleShape(Constants.PowerupRadius, 1f));
            f.CollisionCategories = Constants.PowerupCategory;
            f.CollidesWith = Constants.PowerupCollideWith;
            body.OnCollision += body_OnCollision;
            origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            active = false;
            return false;
        }

        public void Update()
        {
            RedOffParam.SetValue(new Vector2(Stuff.CheapSineWave(Game1.Time * 0.5126f + 1.952f), Stuff.CheapSineWave(Game1.Time * 0.3631f + 0.6312f)));
            GreenOffParam.SetValue(new Vector2(Stuff.CheapSineWave(Game1.Time * 0.9517f + 0.8831f), Stuff.CheapSineWave(Game1.Time * 0.9515f + 0.3693f)));
            BlueOffParam.SetValue(new Vector2(Stuff.CheapSineWave(Game1.Time * 0.6732f + 0.1248f), Stuff.CheapSineWave(Game1.Time * 0.6552f + 0.8535f)));

            /*if (Game1.ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                m += Game1.DeltaTime * 0.5f;
            if (Game1.ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                m -= Game1.DeltaTime * 0.5f;

            Game1.game.Window.Title = m.ToString();
            DistortMultParam.SetValue(m);*/
        }

        public void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            batch.Draw(texture, pos, null, Color.White, Stuff.CheapSineWave(Game1.Time * 1.25f) * 0.8f, origin, drawScale, SpriteEffects.None, 0f);
        }

        public void SetActive()
        {
            active = true;
        }

        public void OnPacket(NetIncomingMessage msg)
        {

        }
    }
}
