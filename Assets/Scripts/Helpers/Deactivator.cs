using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will basically deactivate every component of an object as it goes invisible.
/// </summary>
public class Deactivator : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        foreach (var item in GetComponentsInChildren<MonoBehaviour>())
        {
            if (item.GetType() == typeof(Deactivator)) continue;
            item.enabled = false;
        }
    }

    private void OnBecameVisible()
    {
        foreach (var item in GetComponentsInChildren<MonoBehaviour>())
        {
            if (item.GetType() == typeof(Deactivator)) continue;
            item.enabled = true;
        }
    }
}
