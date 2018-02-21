using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StylezNetworkShared.Game.World.Objects
{
    [Serializable]
    public class MyMovementData
    {
        /// <summary>
        /// The time the movement started.
        /// </summary>
        [JsonProperty]
        public float TimeStarted { get; private set; }

        /// <summary>
        /// The time this transmission was sent.
        /// </summary>
        [JsonProperty]
        public float TimeUpdated { get; private set; }

        /// <summary>
        /// The speed of the movement;
        /// </summary>
        [JsonProperty]
        public float Speed { get; private set; }

        /// <summary>
        /// The X direction of the movement.
        /// </summary>
        [JsonProperty]
        public float XDirection { get; private set; }

        /// <summary>
        /// The Y direction of the movement.
        /// </summary>
        [JsonProperty]
        public float YDirection { get; private set; }

        /// <summary>
        /// The Z direction of the movement.
        /// </summary>
        [JsonProperty]
        public float ZDirection { get; private set; }

        /// <summary>
        /// Is the object moving?
        /// </summary>
        [JsonProperty]
        public bool IsMoving { get; private set; }

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

        public void UpdateMovement(float xDir, float yDir, float zDir, float speed, bool moving = true)
        {
            XDirection = xDir;
            YDirection = yDir;
            ZDirection = zDir;
            Speed = speed;
            IsMoving = moving;
        }

        public void StopAllMovement()
        {
            XDirection = 0f;
            YDirection = 0f;
            ZDirection = 0f;
            Speed = 0f;
            IsMoving = false;
        }

        public MyMovementData()
        {
            //Json constructor
        }
    }
}
