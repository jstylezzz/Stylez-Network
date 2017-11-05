using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StylezNetwork.MathEx;

namespace StylezNetwork.Objects
{
    [Serializable]
    public class MyObjectMovementData
    {
        public Vector3Simple MovementDirection = Vector3Simple.Zero;
        public double MovementSpeed = 0d;
        public EMyObjectMovementState MovementState = EMyObjectMovementState.MOVEMENT_NONE;
        public Vector3Simple CurrentLocation;

        public MyObjectMovementData()
        {
            //Json constructor, empty
        }

        public MyObjectMovementData(Vector3Simple location, Vector3Simple direction, double speed, EMyObjectMovementState movestate)
        {
            MovementDirection = direction;
            CurrentLocation = location;
            MovementSpeed = speed;
            MovementState = movestate;
        }

        public MyObjectMovementData(Vector3Simple currentLocation, EMyObjectMovementState state = EMyObjectMovementState.MOVEMENT_POSITION_UPDATE)
        {
            CurrentLocation = currentLocation;
            MovementState = state;
        }
    }
}
