using System.Collections;
using UnityEngine;

public class PaacjsSpawner : MonoBehaviour
{
    public AnimationCurve spawnRate;
    public float initialWait = 2.0f;
    public GameObject paacj;
    public GameObject veganEnemy;

    public static bool veganMode = false;

    const float randomSpawnRateRange = 10.0f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    void Spawn()
    {
        Vector2 spawnPosition;
        spawnPosition.x = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
        spawnPosition.y = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);

        GameObject newPaacj;

        if (!veganMode)
        {
            newPaacj = Instantiate(paacj, spawnPosition, Quaternion.identity);
        }
        else
        {
            newPaacj = Instantiate(veganEnemy, spawnPosition, Quaternion.identity);
        }

        newPaacj.name = "Paacj";
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(initialWait);
        while (Application.isPlaying)
        {
            Spawn();
            yield return new WaitForSeconds(spawnRate.Evaluate(Time.timeSinceLevelLoad + Random.Range(-randomSpawnRateRange, +randomSpawnRateRange)));
        }
    }
}
