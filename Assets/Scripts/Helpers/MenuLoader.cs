using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is used in the death screen to simply call the menu loading.
/// </summary>
public class MenuLoader : MonoBehaviour
{
    /// <summary>
    /// Goes back to the main menu.
    /// </summary>
    public void BackToMenu()
    {
        StartCoroutine("Menu");
    }

    public IEnumerator Menu()
    {
        ScreenFade.FadeIn();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene("Main Menu");
    }
}
