using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using ClusterWave.Scenario;
using ClusterWave.Scenes;
using ClusterWave.Scenario.PlayerAnimation;

namespace ClusterWave.Scenario
{
    abstract class PlayerController
    {
        public static void Load(ContentManager Content)
        {
            PlayerAnimator.Load(Content);
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

        private PlayerAnimator animator;

        /// <summary>
        /// Returns the position of this player's physics body
        /// </summary>
        public Vector2 Position { get { return body.Position; } }

        public float Rotation { get { return rotation; } set { rotation = value; } }

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
            body = new Body(scene.Scenario.PhysicsWorld, position, 0f, this);
            body.CreateFixture(new FarseerPhysics.Collision.Shapes.CircleShape(Constants.PlayerColliderSize, Constants.PlayerDensity), this);
            body.BodyType = BodyType.Dynamic;
            body.FixedRotation = true;
            body.Friction = Constants.PlayerFriction;
            body.Restitution = Constants.PlayerRestitution;
            body.CollidesWith = Constants.PlayersCollideWith;
            body.CollisionCategories = Constants.PlayerCategory;

            animator = new PlayerAnimator(this);
        }

        public void Draw(SpriteBatch batch)
        {
            animator.Draw(batch, body.Position, rotation);
        }

        /// <summary>
        /// Call to finish the current animation and stop moving (idle animation)
        /// </summary>
        protected void ResetAnimation()
        {
            animator.ResetAnimation();
        }

        /// <summary>
        /// Call to start playing the walking animation. If another animation is in progress, that one will finish first
        /// <param name="time">The time at which the animation began, for calculating offset. Ignored if another animation is currently happening</param>
        /// </summary>
        protected void StartMovingAnimation(float time)
        {
            animator.StartMovingAnimation(time);
        }

        /// <summary>
        /// Sets the gun the player is currently holding for drawing purposes
        /// </summary>
        /// <param name="gunType">The desired gun's id value from Constants</param>
        protected void SetAnimationGunType(int gunType)
        {
            animator.SetGunType(gunType);
        }

        /// <summary>
        /// Call to instantly play the shooting animation and then idles (unless StartMovingAnimation is called before this ends)
        /// <param name="time">The time at which the animation began, for calculating offset</param>
        /// </summary>
        protected void StartShootingAnimation(float time)
        {
            animator.StartShootingAnimation(time);
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

        public void UpdatePos(Vector2 pos)
        {
            body.Position = pos;
        }
    }
}
