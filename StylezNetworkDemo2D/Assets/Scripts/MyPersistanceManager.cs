/*
* Copyright (c) Jari Senhorst. All rights reserved.  
* Website: www.jarisenhorst.com
* Licensed under the MIT License. See LICENSE file in the project root for full license information.  
* 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPersistanceManager : MonoBehaviour
{
    public string PersistanceIdentifier;

    private void Awake()
    {
        Application.runInBackground = true;
        MyPersistanceManager[] m = FindObjectsOfType<MyPersistanceManager>();
        for (int i = 0; i < m.Length; i++)
        {
            if (m[i].PersistanceIdentifier.Equals(this.PersistanceIdentifier) && m[i].gameObject != gameObject) Destroy(m[i].gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

}
