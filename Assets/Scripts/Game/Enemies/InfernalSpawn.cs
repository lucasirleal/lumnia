using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A little custom logic for some enemies.
/// This will spawn random explosions on a given radius.
/// </summary>
public class InfernalSpawn : MonoBehaviour
{
    [Header("Data")]
    public float damage;
    public float explosionRange;
    public float explosionKick;
    public float minDelayTimer;
    public float maxDelayTimer;
    [Header("References")]
    public Transform explosionParticles;
    public CircleCollider2D range;

    private PlayerStatus player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStatus>();
        StartCoroutine("DoExplosion");
    }

    public IEnumerator DoExplosion()
    {
        float radius = range.transform.lossyScale.x * range.radius;
        Vector2 position = new Vector2(Random.Range(-radius, radius) + transform.position.x, Random.Range(-radius, radius) + transform.position.y);

        Destroy(Instantiate(explosionParticles, position, Quaternion.identity).gameObject, 3f);

        if (Vector2.Distance(position, player.transform.position) <= explosionRange)
        {
            player.UpdateHealth(-damage);

            Vector2 direction = (position - (Vector2)player.transform.position).normalized;
            player.GetComponent<Rigidbody2D>().AddForce(direction * explosionKick, ForceMode2D.Impulse);
        }

        float timer = Random.Range(minDelayTimer, maxDelayTimer);
        yield return new WaitForSecondsRealtime(timer);
        StartCoroutine("DoExplosion");
    }
}
