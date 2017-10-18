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
        protected const float PlayerDrawSize = Constants.PlayerColliderSize / 0.33f;
        const int FrameWidth = 24, FrameHeight = 21;
        public static Texture2D[] PlayerTextures;
        public static void Load(ContentManager Content)
        {
            PlayerTextures = new Texture2D[2];
            //for (int i = 0; i < 2;)
            //    PlayerTextures[i] = Content.Load<Texture2D>("Player/" + ++i);
            PlayerTextures[0] = Content.Load<Texture2D>("Player/dusto");
        }

        /// <summary>
        /// The player's body in the physics simulation. This can be used to get and set the position and velocity.
        /// <para>For changing rotation, see <see cref="PlayerController.rotation"/></para>
        /// </summary>
        protected Body body;

        /// <summary>
        /// The rotation the player is facing. This value is used just for drawing
        /// </summary>
        protected float rotation;

        /// <summary>
        /// The Player this PlayerController is assigned to
        /// </summary>
        protected Player player;

        /// <summary>
        /// Gets the Player this PlayerController is assigned to
        /// </summary>
        public Player Player { get { return player; } }

        /// <summary>
        /// The <see cref="ClusterWave.Scenes.InGameScene"/> object controlling this shiet
        /// </summary>
        protected InGameScene scene;

        private Texture2D texture;
        private Vector2 origin;
        private Rectangle source;
        private float scale;

        /// <summary>
        /// Returns the position of this player's physics body
        /// </summary>
        public Vector2 Position { get { return body.Position; } }

        /// <summary>
        /// Creates a PlayerController, assigning it a given position, scene and it's assigned Player.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="scene"></param>
        /// <param name="player"></param>
        public PlayerController(Vector2 position, InGameScene scene, Player player)
        {
            this.scene = scene;
            rotation = 0;
            this.player = player;
            body = new Body(scene.Scenario.PhysicsWorld, position, 0f, null);
            body.CreateFixture(new FarseerPhysics.Collision.Shapes.CircleShape(Constants.PlayerColliderSize, Constants.PlayerDensity), this);
            body.BodyType = BodyType.Dynamic;
            body.FixedRotation = true;
            body.Friction = Constants.PlayerFriction;
            body.Restitution = Constants.PlayerRestitution;
            body.CollidesWith = Constants.PlayersCollideWith;
            body.CollisionCategories = Constants.PlayerCategory;

            texture = PlayerTextures[0/*player.ColorIndex*/];
            origin = new Vector2(texture.Width/2f, texture.Height/2f);
            scale = PlayerDrawSize / texture.Width;
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
        /// Call to start playing the walking animation. If another animation is in progress, that one will finish first
        /// <param name="time">The time at which the animation began, for calculating offset. Ignored if another animation is currently happening</param>
        /// </summary>
        protected void StartMovingAnimation(float time)
        {

        }

        /// <summary>
        /// Sets the gun the player is currently holding for drawing purposes
        /// </summary>
        /// <param name="gunType">The desired gun's id value from Constants</param>
        protected void SetAnimationGunType(int gunType)
        {

        }

        /// <summary>
        /// Call to instantly play the shooting animation and then idles (unless StartMovingAnimation is called before this ends)
        /// <param name="time">The time at which the animation began, for calculating offset</param>
        /// </summary>
        protected void StartShootingAnimation(float time)
        {

        }

        /// <summary>
        /// Call to instantly play the shield animation (and then idle)
        /// <param name="time">The time at which the animation began, for calculating offset</param>
        /// </summary>
        protected void StartShieldAnimation(float time)
        {

        }

        /// <summary>
        /// Plays the Dash animation and then idles (unless StartMovingAnimation is called before this ends)
        /// </summary>
        /// <param name="time">The time at which the animation began, for calculating offset</param>
        protected void StartDashAnimation(float time)
        {

        }
    }
}
