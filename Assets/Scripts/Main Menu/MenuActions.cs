using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls all basic ations from the main menu's buttons.
/// </summary>
public class MenuActions : MonoBehaviour
{
    /// <summary>
    /// Starts the main game scenes.
    /// </summary>
    public void StartGame()
    {
        StartCoroutine("FadeAndLoad");
    }

    /// <summary>
    /// Waits for a screen fade and then loads the main game.
    /// </summary>
    private IEnumerator FadeAndLoad()
    {
        ScreenFade.FadeIn();
        yield return new WaitForSecondsRealtime(2f);
    }

    /// <summary>
    /// Quits the game :(
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
