using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will trigger the spawning of enemies.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Waves")]
    public List<EnemyWave> waves;
    public List<Transform> spawnedEnemies;

    private void Start()
    {
        spawnedEnemies = new List<Transform>();
    }

    /// <summary>
    /// Starts a wave with evenly spread spawn timers.
    /// </summary>
    /// <param name="waveIndex">The current wave index.</param>
    /// <param name="waveTime">The total time it takes for light to replenish.</param>
    public void StartWave(int waveIndex, float waveTime)
    {
        waveIndex = Mathf.Clamp(waveIndex, 0, waves.Count - 1);
        StartCoroutine(SpawnWave(waveIndex, waveTime));
    }

    private IEnumerator SpawnWave(int index, float timer)
    {
        timer -= 5f; // Adds a little wait time between the last enemy and the light replenish.

        int enemyCount = waves[index].enemies.Count;
        List<Transform> enemies = new List<Transform>(waves[index].enemies);

        float enemyDelay = timer / enemyCount;

        while (enemyCount != 0)
        {
            int randomEnemy = Random.Range(0, enemyCount);
            SpawnEnemy(enemies[randomEnemy]);
            enemies.RemoveAt(randomEnemy);
            enemyCount--;
            yield return new WaitForSecondsRealtime(enemyDelay);
        }
    }

    /// <summary>
    /// Spawns an enemy on the invisible border of the screen.
    /// The exact position is random.
    /// </summary>
    private void SpawnEnemy(Transform enemy)
    {
        Vector2 position = GetSpawnPosition();

        spawnedEnemies.Add(Instantiate(enemy, position, Quaternion.identity));
    }

    /// <summary>
    /// Calculates a random position outside the camera view.
    /// </summary>
    /// <returns>The final position</returns>
    private Vector2 GetSpawnPosition()
    {
        float cameraHeight = Mathf.Abs(Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).y - Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y);
        float cameraWidth = Mathf.Abs(Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).x - Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x);

        int spawningSector = Random.Range(0, 4); // 0 = left, 1 = top, 2 = right, 3 = bottom.

        Vector2 basePos;
        Vector2 offset;

        if (spawningSector == 0) { basePos = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0.5f)); }
        else if (spawningSector == 1) { basePos = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1f)); }
        else if (spawningSector == 2) { basePos = Camera.main.ViewportToWorldPoint(new Vector2(1f, 0.5f)); }
        else { basePos = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0f)); }

        if (spawningSector == 0) { offset = new Vector2(Random.Range(-10f, -1f), Random.Range(-(cameraHeight / 2f), (cameraHeight / 2f))); }
        else if (spawningSector == 1) { offset = new Vector2(Random.Range(-(cameraWidth / 2f), (cameraWidth / 2f)), Random.Range(1f, 10f)); }
        else if (spawningSector == 2) { offset = new Vector2(Random.Range(1, 10f), Random.Range(-(cameraHeight / 2f), (cameraHeight / 2f))); }
        else { offset = new Vector2(Random.Range(-(cameraWidth / 2f), (cameraWidth / 2f)), Random.Range(-10f, -1f)); }

        return basePos + offset;

    }
}
