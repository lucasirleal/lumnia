using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores data about a given layer setter's children.
/// Doesn't do much on its own.
/// </summary>
[System.Serializable]
public class LayerSetterChild
{
    public SpriteRenderer spriteChildren;
    public ParticleSystemRenderer particleChildren;
    public int offset;
}
