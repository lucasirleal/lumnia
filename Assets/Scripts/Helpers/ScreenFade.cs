using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlls the fading effect that takes place on transitions.
/// </summary>
public static class ScreenFade
{
    private static Animator fadeScreenAnimator;

    /// <summary>
    /// Fades Out the screen and allows clicks on the game.
    /// </summary>
    public static void FadeOut()
    {
        ValidateOrCreateFadeScreen();
        fadeScreenAnimator.SetBool("Faded", false);
        // Allows clicks on the game again.
        fadeScreenAnimator.transform.GetComponent<Image>().raycastTarget = false;
    }

    /// <summary>
    /// Fades In the screen and blocks clicks on the game.
    /// </summary>
    public static void FadeIn()
    {
        ValidateOrCreateFadeScreen();
        fadeScreenAnimator.SetBool("Faded", true);
        // Prevents unwanted clicks when the fade screen is present.
        fadeScreenAnimator.transform.GetComponent<Image>().raycastTarget = true;
    }

    /// <summary>
    /// Checks if there is a fade screen present, and then creates one if there is none.
    /// </summary>
    private static void ValidateOrCreateFadeScreen()
    {
        if (fadeScreenAnimator) { return; }
        fadeScreenAnimator = InstantiateFadeScreen();
    }

    /// <summary>
    /// Instantiates a copy of the fade screen.
    /// Does not check if there is already a fade screen.
    /// Should only be used when none is present.
    /// </summary>
    /// <returns>The animator that controls the fade screen.</returns>
    private static Animator InstantiateFadeScreen()
    {
        Transform prefab = Resources.Load<Transform>("Prefabs/UI/Fade Screen");
        Transform instance = GameObject.Instantiate(prefab);
        return instance.GetChild(0).GetComponent<Animator>();
    }
}
