using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls movement blocking, and following AI of the camera.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    public Transform currentTarget;

    private void Update()
    {
        Vector3 positionToFollow = new Vector3(currentTarget.position.x, currentTarget.position.y, -10f);
        this.transform.position = positionToFollow;
    }
}
