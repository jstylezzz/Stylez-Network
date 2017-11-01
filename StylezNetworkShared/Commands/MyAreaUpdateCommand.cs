using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StylezNetwork.Objects;
using StylezNetwork.MathEx;

namespace StylezNetwork.Commands
{
    [Serializable]
    public class MyAreaUpdateCommand
    {
        public int[] ObjectIDsInRange;
        public int[] ObjectsNoLongerInRange;
        public Vector3Simple[] ObjectPositions;
        public MyObjectMovementData[] ObjectMovement;
        public double StreamDistance;
        public Vector3Simple StreamPoint;
        public int StreamDimension;

        public MyAreaUpdateCommand()
        {
            //Json construct empty
        }

        public MyAreaUpdateCommand(double streamDistance, int dim, Vector3Simple streamPoint)
        {
            StreamDistance = streamDistance;
            StreamPoint = streamPoint;
            StreamDimension = dim;
        }

        public MyAreaUpdateCommand(int[] objsInRange, int[] objsNoLongerInRange, Vector3Simple[] objPoses, MyObjectMovementData[] objsMovs)
        {
            ObjectIDsInRange = objsInRange;
            ObjectsNoLongerInRange = objsNoLongerInRange;
            ObjectPositions = objPoses;
            ObjectMovement = objsMovs;
        }
    }
}
