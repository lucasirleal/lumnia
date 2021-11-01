using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteUtils
{
    /// <summary>
    /// Returns a texture 2D converted from a sprite.
    /// </summary>
    /// <param name="sprite">The target sprite.</param>
    /// <returns>A Texture based on the sprite sent.</returns>
    public static Texture2D SpriteToTexture(Sprite sprite)
    {
        Texture2D returnTex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                              (int)sprite.textureRect.y,
                                              (int)sprite.textureRect.width,
                                              (int)sprite.textureRect.height);
        returnTex.SetPixels(pixels);
        returnTex.Apply();

        return returnTex;
    }
}
