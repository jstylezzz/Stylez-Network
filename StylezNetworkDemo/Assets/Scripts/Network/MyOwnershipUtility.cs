/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using StylezNetworkDemo.Network;
using StylezNetworkShared.Objects;

namespace StylezNetworkDemo.Utiliy
{
    /// <summary>
    ///
    /// </summary>
    public static class MyOwnershipUtility
    {
        public static EMyMultiplayerSide GetObjectOwner(MyDynamicObject wo)
        {
            if (MyNetworkManager.Instance.NetClient.ClientID == wo.OwnerClientID) return EMyMultiplayerSide.SIDE_SELF;
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