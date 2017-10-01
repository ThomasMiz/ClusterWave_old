using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;

namespace ClusterWave.Scenario
{
    abstract class PlayerController
    {
        const float PlayerSize = 0.2f, PlayerColliderSize = PlayerSize * 0.75f;

        protected Body body;
        protected float rotation;

        Player player;
        Texture2D texture;
        Vector2 origin;
        float scale;


        public PlayerController(Vector2 position, World world, Player player)
        {
            rotation = 0;
            this.player = player;
            body = new Body(world, position, 0f, null);
            body.CreateFixture(new FarseerPhysics.Collision.Shapes.CircleShape(PlayerColliderSize, 1f), null);
            body.BodyType = BodyType.Dynamic;
            body.FixedRotation = true;
            body.Friction = 0.5f;
            body.Restitution = 0;
            body.CollidesWith = Constants.PlayersCollideWith;
            body.CollisionCategories = Constants.PlayerCategory;

            origin = new Vector2(texture.Width/2f, texture.Height/2f);
            scale = PlayerSize / texture.Width;
        }

        public abstract void UpdatePrePhysics();
        public abstract void UpdatePostPhysics();

        public void Draw(SpriteBatch batch, GraphicsDevice device)
        {
            batch.Draw(texture, body.Position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
