using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// This script controls all the steps from the death event.
/// </summary>
public class Death : MonoBehaviour
{
    [Header("References")]
    public LightLevel waveCounter;

    private int finalCycleCount;
    private bool loading;

    private void Awake()
    {
        ScreenFade.FadeOut();
    }

    /// <summary>
    /// Triggers the death event.
    /// </summary>
    public void Trigger()
    {
        if (loading) return;

        loading = true;
        finalCycleCount = waveCounter.currentWave;
        StartCoroutine("DeathCycle");
    }

    public IEnumerator DeathCycle()
    {
        ScreenFade.FadeIn();
        yield return new WaitForSecondsRealtime(1f);

        yield return SceneManager.LoadSceneAsync("Death");

        GameObject.Find("Cycle Counter").GetComponent<TextMeshProUGUI>().text = "Your story ends here, after " + waveCounter.currentWave + " cycles.";

        ScreenFade.FadeOut();
    }
}
