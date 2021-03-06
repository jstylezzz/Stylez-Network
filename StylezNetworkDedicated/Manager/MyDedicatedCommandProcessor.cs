﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using StylezNetwork.Commands;
using StylezNetwork.Objects;
using StylezNetwork.MathEx;

namespace StylezNetworkDedicated.Manager
{
    public class MyDedicatedCommandProcessor
    {
        private MyServerWorldCache WorldCacheInstance
        {
            get
            {
                if (m_worldCache == null) m_worldCache = Program.Instance.WorldCacheInstance;
                return m_worldCache;
            }
        }

        private MyServerWorldCache m_worldCache;

        public void ProcessCommand(int cmdId, string cmdJson, int fromClient)
        {
            switch ((EMyNetworkCommand)Enum.Parse(typeof(EMyNetworkCommand), cmdId.ToString()))
            {
                case EMyNetworkCommand.COMMAND_OBJECT_CREATE:
                {
                    MyCreateObjectCommand cmd = JsonConvert.DeserializeObject<MyCreateObjectCommand>(cmdJson);
                    Program.Instance.WorldCacheInstance.AddObjectToWorld(new MySimpleWorldObject(cmd.AtPosition, cmd.InDimension, Program.Instance.WorldCacheInstance.GetFirstFreeObjectID(true), fromClient, cmd.ObjectType));
                    break;
                }
                case EMyNetworkCommand.COMMAND_WORLD_GETOBJECTS:
                {
                    MyRequestAllObjectsCommand requestCommand = JsonConvert.DeserializeObject<MyRequestAllObjectsCommand>(cmdJson);
                    IMyNetworkObject[] objectsToSend = Program.Instance.WorldCacheInstance.GetNetworkObjects(requestCommand.LoadingPoint, requestCommand.LoadingDimension, requestCommand.LoadingDistance);
                    MyRequestAllObjectsCommand outCommand = new MyRequestAllObjectsCommand(objectsToSend);
                    Program.Instance.ServerBaseInstance.ClientRegistry[fromClient].SendMessage(JsonConvert.SerializeObject(outCommand), (int)EMyNetworkCommand.COMMAND_WORLD_GETOBJECTS);
                    break;
                }
                case EMyNetworkCommand.COMMAND_MOVE_OBJECT:
                {
                    MyMoveObjectCommand cmd = JsonConvert.DeserializeObject<MyMoveObjectCommand>(cmdJson);
                    MyServerWorldCache c = Program.Instance.WorldCacheInstance;
                    c.UpdateMoveData(cmd.ObjectID, cmd.MovementData);
                    break;
                }
                case EMyNetworkCommand.COMMAND_REQUEST_AREAUPDATE:
                {
                    MyAreaUpdateCommand cmd = JsonConvert.DeserializeObject<MyAreaUpdateCommand>(cmdJson);
                    MyDedicatedClientData cd = Program.Instance.ServerBaseInstance.ClientRegistry[fromClient].DataInstance;
                    MyObjectInfoPackage[] o = Program.Instance.WorldCacheInstance.GetNetworkObjectsInfoPackages(cmd.StreamPoint, cmd.StreamDimension, cmd.StreamDistance);
                    int[] inRange = new int[o.Length];
                    Vector3Simple[] poses = new Vector3Simple[o.Length];
                    MyObjectMovementData[] mov = new MyObjectMovementData[o.Length];
                    EMyObjectType[] types = new EMyObjectType[o.Length];
                    int[] owners = new int[o.Length];

                    for (int i = 0; i < inRange.Length; i++)
                    {
                        inRange[i] = o[i].ObjectID;
                        poses[i] = o[i].ObjectPosition;
                        mov[i] = o[i].MovementData;
                        types[i] = o[i].ObjectType;
                        owners[i] = o[i].OwnerID;
                    }
                    MyAreaUpdateCommand outcmd = new MyAreaUpdateCommand(inRange, cd.UpdateObjectsInRange(inRange), poses, mov, types, owners);
                    SendCommandToClient(fromClient, JsonConvert.SerializeObject(outcmd), EMyNetworkCommand.COMMAND_REQUEST_AREAUPDATE);
                    break;
                }
                case EMyNetworkCommand.COMMAND_OBJECT_DELETE:
                {
                    MyDeleteObjectCommand cmd = JsonConvert.DeserializeObject<MyDeleteObjectCommand>(cmdJson);
                    WorldCacheInstance.RemoveObjectFromWorld(WorldCacheInstance.GetNetworkObject(cmd.ObjectID));
                    break;
                }
                case EMyNetworkCommand.COMMAND_PING:
                {
                    Program.Instance.ServerBaseInstance.ClientRegistry[fromClient].SendMessage(null, (int)EMyNetworkCommand.COMMAND_PING);
                    break;
                }
            }
        }

        private void SendCommandToClient(int client, string json, EMyNetworkCommand c)
        {
            Program.Instance.ServerBaseInstance.ClientRegistry[client].SendMessage(json, (int)c);
        }
    }
}
