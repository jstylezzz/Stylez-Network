/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
/// </summary>
public class MyUIManager : MonoBehaviour 
{
    [SerializeField]
    private Text m_pingTextFrame;

    [SerializeField]
    private Text m_pingTextPing;

	/// <summary>
	/// Script entry point.
	/// </summary>
	private void Start () 
	{
        MyDemoNetworkClient.Instance.OnFramePingUpdated += PingUpdate;
	}

    private void PingUpdate(int p, int ms)
    {
        m_pingTextFrame.text = "Frame Ping: " + p + " frames";
        m_pingTextPing.text = "Ping: " + ms + " ms";
    }
}
