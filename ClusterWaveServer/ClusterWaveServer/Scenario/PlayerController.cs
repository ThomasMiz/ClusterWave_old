using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using ClusterWaveServer.Scenes;
using ClusterWaveServer.Network;

namespace ClusterWaveServer.Scenario
{
    abstract class PlayerController
    {
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
        protected PlayerInfo player;

        /// <summary>
        /// Gets the Player this PlayerController is assigned to
        /// </summary>
        public PlayerInfo Player { get { return player; } }

        /// <summary>
        /// The <see cref="ClusterWave.Scenes.InGameScene"/> object controlling this shiet
        /// </summary>
        protected InGameScene scene;

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
        public PlayerController(Vector2 position, InGameScene scene, PlayerInfo player)
        {
            rotation = 0;
            this.scene = scene;
            this.player = player;
            body = new Body(scene.Scenario.PhysicsWorld, position, 0f, null);
            body.CreateFixture(new FarseerPhysics.Collision.Shapes.CircleShape(Constants.PlayerColliderSize, Constants.PlayerDensity), this);
            body.BodyType = BodyType.Dynamic;
            body.FixedRotation = true;
            body.Friction = Constants.PlayerFriction;
            body.Restitution = Constants.PlayerRestitution;
            body.CollidesWith = Constants.PlayersCollideWith;
            body.CollisionCategories = Constants.PlayerCategory;
        }
    }
}
