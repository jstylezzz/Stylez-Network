/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections;
using System.Collections.Generic;
using StylezNetworkShared.Game.World.Objects;
using StylezNetworkDemo.Network;

namespace StylezNetworkDemo.Utiliy
{
    /// <summary>
    ///
    /// </summary>
    public static class MyOwnershipUtility
    {
        public static EMyMultiplayerSide GetObjectOwner(MyWorldObject wo)
        {
            if (MyNetworkManager.Instance.NetClient.ClientID == wo.OwnerID) return EMyMultiplayerSide.SIDE_SELF;
            else return EMyMultiplayerSide.SIDE_OTHER;
        }

    }

    public enum EMyMultiplayerSide
    {
        SIDE_SELF,
        SIDE_OTHER,
        SIDE_SERVER
    }
}