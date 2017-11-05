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
        public int[] WorldObjectIDs; //ID's of the objects
        public Vector3Simple[] WorldObjectLocations; //Objects to create clientside
        public int[] WorldObjectOwners; //Object owners
        public EMyObjectType[] ObjectTypes; //Types
        public Vector3Simple LoadingPoint; //Position to look for serverside
        public int LoadingDimension; //Dimension to look in serverside
        public double LoadingDistance; //Distance to load for serverside

        public MyRequestAllObjectsCommand()
        {

        }

        public MyRequestAllObjectsCommand(IMyNetworkObject[] objectsToSend)
        {
            int[] ids = new int[objectsToSend.Length];
            Vector3Simple[] locs = new Vector3Simple[objectsToSend.Length];
            int[] owners = new int[objectsToSend.Length];
            EMyObjectType[] types = new EMyObjectType[objectsToSend.Length];

            for (int i = 0; i < objectsToSend.Length; i++)
            {
                ids[i] = objectsToSend[i].ObjectNetworkID;
                locs[i] = objectsToSend[i].Position;
                owners[i] = objectsToSend[i].OwnerClientID;
                types[i] = objectsToSend[i].ObjectType;
            }
            WorldObjectLocations = locs;
            WorldObjectIDs = ids;
            ObjectTypes = types;
            WorldObjectOwners = owners;
        }

        public MyRequestAllObjectsCommand(Vector3Simple loadpoint, int loadDimension, double loadDistance)
        {
            LoadingPoint = loadpoint;
            LoadingDimension = loadDimension;
            LoadingDistance = loadDistance;
        }
    }
}
