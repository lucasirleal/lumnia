using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simply marks an object to not be destroyed on load.
/// </summary>
public class DDOL : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("Object [" + this.gameObject.name + "] marked to Don't Destroy on Load.");
    }
}
