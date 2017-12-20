﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylezNetworkShared.Game.World.Objects
{
    [Serializable]
    public class MyWorldObject
    {
        /// <summary>
        /// The object's position data.
        /// </summary>
        public MyWorldPosition ObjectPosition { get; set; } = new MyWorldPosition();

        /// <summary>
        /// The object's movement data.
        /// </summary>
        public MyMovementData MovementData { get; set; } = new MyMovementData();
        
        /// <summary>
        /// The object ID as assigned by the server.
        /// </summary>
        public int ObjectID { get; set; }

        /// <summary>
        /// The clientID of the object's owner.
        /// </summary>
        public int OwnerID { get; set; }

        /// <summary>
        /// Create an instance of a world object.
        /// This constructor is best used clientside.
        /// </summary>
        /// <param name="x">X position of the object.</param>
        /// <param name="y">Y position of the object.</param>
        /// <param name="z">Z position of the object.</param>
        /// <param name="objectID">The local ID of the object.</param>
        /// <param name="ownerID">The owner ID of the object.</param>
        public MyWorldObject(float x, float y, float z, int objectID, int ownerID)
        {
            ObjectID = objectID;
            OwnerID = ownerID;
            ObjectPosition = new MyWorldPosition(x, y, z);
        }

        /// <summary>
        /// Create an instance of a world object.
        /// This constructor is best used clientside.
        /// </summary>
        /// <param name="x">X position of the object.</param>
        /// <param name="y">Y position of the object.</param>
        /// <param name="z">Z position of the object.</param>
        /// <param name="dimension">The dimension to create this object in.</param>
        /// <param name="objectID">The local ID of the object.</param>
        /// <param name="ownerID">The owner ID of the object.</param>
        public MyWorldObject(float x, float y, float z, int dimension, int objectID, int ownerID)
        {
            OwnerID = ownerID;
            ObjectID = objectID;
            ObjectPosition = new MyWorldPosition(x, y, z, dimension);
        }

        /// <summary>
        /// Create an instance of a world object.
        /// </summary>
        /// <param name="objectID">The local ID of the object.</param>
        /// <param name="ownerID"></param>
        public MyWorldObject(int objectID, int ownerID)
        {
            OwnerID = ownerID;
            ObjectID = objectID;
        }
    }
}