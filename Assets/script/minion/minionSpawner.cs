//Attribution Declaration: The minion script is adapted from this tutorial: 
//    https://www.youtube.com/watch?v=vyW3z3v5gP4&list=PLuKMRhgr5rGkgABx8Sezws-ekSWIWRf4Q&index=12&t=254s

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minionSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public float normalMinionSpeed;
    public float strongMinionSpeed;

    public GameObject normalMinionPrefab;
    public GameObject strongMinionPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 15.0f;
    public int minionsPerWave = 6;
    public int wavesUntilStrongMinion = 3;
    public float delayBetweenMinions;
    private int waveCount = 0;
    

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnMinions());
        }
    }

    private IEnumerator SpawnMinions()
    {
        while(true)
        {
            waveCount++;

            if(waveCount % wavesUntilStrongMinion == 0)
            {
                for(int i = 0; i < minionsPerWave - 1; i++)
                {
                    SpawnNormalMinion();
                    yield return new WaitForSeconds(delayBetweenMinions);
                }
                SpawnNormalMinion();
                yield return new WaitForSeconds(delayBetweenMinions);

                SpawnStrongMinion();
                yield return new WaitForSeconds(spawnInterval - delayBetweenMinions * (minionsPerWave - 1) - delayBetweenMinions);
            }
            else
            {
                for(int i = 0; i < minionsPerWave; i++)
                {
                    SpawnNormalMinion();
                    yield return new WaitForSeconds(delayBetweenMinions);
                }

                yield return new WaitForSeconds(spawnInterval - delayBetweenMinions * minionsPerWave);

            }
        }

    }
    

    private void SpawnNormalMinion()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject minion = PhotonNetwork.Instantiate(normalMinionPrefab.name, spawnPoint.position, spawnPoint.rotation);

        UnityEngine.AI.NavMeshAgent normalMinionAgent = minion.GetComponent<UnityEngine.AI.NavMeshAgent>();
        normalMinionAgent.speed = normalMinionSpeed;
    }

    private void SpawnStrongMinion()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject minion = PhotonNetwork.Instantiate(strongMinionPrefab.name, spawnPoint.position, spawnPoint.rotation);

        UnityEngine.AI.NavMeshAgent strongMinionAgent = minion.GetComponent<UnityEngine.AI.NavMeshAgent>();
        strongMinionAgent.speed = strongMinionSpeed;
    }
}
