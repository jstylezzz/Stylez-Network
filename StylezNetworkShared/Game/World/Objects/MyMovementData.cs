using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Game.World.Objects
{
    [Serializable]
    public struct MyMovementData
    {
        /// <summary>
        /// The time the movement started.
        /// </summary>
        public float TimeStarted { get; }

        /// <summary>
        /// The time this transmission was sent.
        /// </summary>
        public float TimeUpdated { get; }

        /// <summary>
        /// The speed of the movement;
        /// </summary>
        public float Speed { get; }

        /// <summary>
        /// The X direction of the movement.
        /// </summary>
        public float XDirection { get; }

        /// <summary>
        /// The Y direction of the movement.
        /// </summary>
        public float YDirection { get; }

        /// <summary>
        /// The Z direction of the movement.
        /// </summary>
        public float ZDirection { get; }

        /// <summary>
        /// Is the object moving?
        /// </summary>
        public bool IsMoving { get; }

        /// <summary>
        /// Create a new instance of movement data.
        /// </summary>
        /// <param name="xDir">X direction of the movement.</param>
        /// <param name="yDir">Y direction of the movement.</param>
        /// <param name="zDir">Z direction of the movement.</param>
        /// <param name="speed">Speed of the movement.</param>
        /// <param name="startTime">The time this movement has started.</param>
        public MyMovementData(float xDir, float yDir, float zDir, float speed, float startTime, bool isMoving)
        {
            TimeUpdated = startTime;
            TimeStarted = startTime;
            XDirection = xDir;
            YDirection = yDir;
            ZDirection = zDir;
            Speed = speed;
            IsMoving = isMoving;
        }

        /// <summary>
        /// Create a new instance of movement data.
        /// </summary>
        /// <param name="xDir">X direction of the movement.</param>
        /// <param name="yDir">Y direction of the movement.</param>
        /// <param name="zDir">Z direction of the movement.</param>
        /// <param name="speed">Speed of the movement.</param>
        /// <param name="startTime">The time this movement has started.</param>
        /// <param name="updateTime">The time this movement was updated.</param>
        public MyMovementData(float xDir, float yDir, float zDir, float speed, float startTime, float updateTime, bool isMoving)
        {
            TimeUpdated = updateTime;
            TimeStarted = startTime;
            XDirection = xDir;
            YDirection = yDir;
            ZDirection = zDir;
            Speed = speed;
            IsMoving = isMoving;
        }
    }
}
