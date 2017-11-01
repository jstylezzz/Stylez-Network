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
        public EMyObjectMovementState MovementState = EMyObjectMovementState.MOVEMENT_STOP;
    }
}
