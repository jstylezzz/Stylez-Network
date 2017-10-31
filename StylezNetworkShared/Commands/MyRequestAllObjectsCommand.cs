using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StylezNetwork.Objects;
using StylezNetwork.MathEx;

namespace StylezNetwork.Commands
{
    [Serializable]
    public class MyRequestAllObjectsCommand
    {
        public Vector3Simple[] WorldObjectLocations; //Objects to create clientside
        public Vector3Simple LoadingPoint; //Position to look for serverside
        public int LoadingDimension; //Dimension to look in serverside
        public double LoadingDistance; //Distance to load for serverside

        public MyRequestAllObjectsCommand()
        {

        }

        public MyRequestAllObjectsCommand(IMyNetworkObject[] objectsToSend)
        {
            List<Vector3Simple> locs = new List<Vector3Simple>();
            foreach(IMyNetworkObject o in objectsToSend)
            {
                locs.Add(o.Position);
            }
            WorldObjectLocations = locs.ToArray();
        }

        public MyRequestAllObjectsCommand(Vector3Simple loadpoint, int loadDimension, double loadDistance)
        {
            LoadingPoint = loadpoint;
            LoadingDimension = loadDimension;
            LoadingDistance = loadDistance;
        }
    }
}
