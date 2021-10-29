using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides basic utility calls related to camera behaviour and properties.
/// </summary>
public static class CameraUtils
{
    /// <summary>
    /// Calculates the minimum bounds for the active camera. (With some room to spare)
    /// </summary>
    /// <returns>A Vector2 containing the world position for the bound.</returns>
    public static Vector2 ActiveCameraMinBounds()
    {
        return Camera.main.ViewportToWorldPoint(new Vector2(-0.5f, -0.5f));
    }

    /// <summary>
    /// Calculates the maximum bounds for the active camera. (With some room to spare)
    /// </summary>
    /// <returns>A Vector2 containing the world position for the bound.</returns>
    public static Vector2 ActiveCameraMaxBounds()
    {
        return Camera.main.ViewportToWorldPoint(new Vector2(1.5f, 1.5f));
    }
}
