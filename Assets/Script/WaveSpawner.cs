using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float timeBeforeStart;
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField] private int numberOfEnemies;
    public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine("SpawnWave");
            StartCoroutine("SpawnWave");
        }
    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(timeBeforeStart);

        while (numberOfEnemies > 0)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
            numberOfEnemies--;
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }
}
