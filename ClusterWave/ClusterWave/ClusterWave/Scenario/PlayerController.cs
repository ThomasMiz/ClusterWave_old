using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using ClusterWave.Scenario;
using ClusterWave.Scenes;

namespace ClusterWave.Scenario
{
    abstract class PlayerController
    {
        protected const float PlayerSize = 0.5f, PlayerColliderSize = PlayerSize * 0.33f;
        protected const float MovementSpeed = 0.75f;
        const int FrameWidth = 24, FrameHeight = 21;
        public static Texture2D[] PlayerTextures;
        public static void Load(ContentManager Content)
        {
            PlayerTextures = new Texture2D[2];
            for (int i = 0; i < 2;)
                PlayerTextures[i] = Content.Load<Texture2D>("Player/" + ++i);
            PlayerTextures[0] = Content.Load<Texture2D>("Player/dusto");
        }

        protected Body body;
        protected float rotation;

        protected Player player;
        protected InGameScene scene;

        private Texture2D texture;
        private Vector2 origin;
        private Rectangle source;
        private float scale;

        /// <summary>
        /// Returns the position of this player's physics body
        /// </summary>
        public Vector2 Position { get { return body.Position; } }

        public PlayerController(Vector2 position, InGameScene scene, Player player)
        {
            this.scene = scene;
            rotation = 0;
            this.player = player;
            body = new Body(scene.Scenario.PhysicsWorld, position, 0f, null);
            body.CreateFixture(new FarseerPhysics.Collision.Shapes.CircleShape(PlayerColliderSize, 1f), null);
            body.BodyType = BodyType.Dynamic;
            body.FixedRotation = true;
            body.Friction = 0.5f;
            body.Restitution = 0;
            body.CollidesWith = Constants.PlayersCollideWith;
            body.CollisionCategories = Constants.PlayerCategory;

            texture = PlayerTextures[0/*player.ColorIndex*/];
            origin = new Vector2(texture.Width/2f, texture.Height/2f);
            scale = PlayerSize / texture.Width;
            source = new Rectangle(0, 0, FrameWidth, FrameHeight);
        }

        public void Draw(SpriteBatch batch)
        {


            batch.Draw(texture, body.Position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Call to finish the current animation and stop moving (idle animation)
        /// </summary>
        protected void ResetAnimation()
        {

        }

        /// <summary>
        /// Call to start playing the walking animation. If another animation is in progress, that one will finish first.
        /// <param name="time">The time at which the animation began, for calculating offset. Ignored if another animation is currently happening</param>
        /// </summary>
        protected void StartMovingAnimation(float time)
        {

        }

        /// <summary>
        /// Call to instantly play the shooting animation (and then idle)
        /// <param name="time">The time at which the animation began, for calculating offset.</param>
        /// </summary>
        protected void StartShootingAnimation(float time)
        {

        }

        /// <summary>
        /// Call to instantly play the shield animation (and then idle)
        /// <param name="time">The time at which the animation began, for calculating offset.</param>
        /// </summary>
        protected void StartShieldAnimation(float time)
        {

        }
    }
}
