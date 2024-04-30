//Attribution Declaration: The minion script is adapted from this tutorial: 
//    https://www.youtube.com/watch?v=vyW3z3v5gP4&list=PLuKMRhgr5rGkgABx8Sezws-ekSWIWRf4Q&index=12&t=254s

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
    

public class minionAIScript2222 : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent agent;
    public Transform currentTarget;
    public string characterTag = "Character";
    public float stopDistance = 2.0f;
    public float aggroRange = 5.0f;

    public float targetSwitchInterval = 2.0f;

    private float timeSinceLastTargetSwitch = 0.0f;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        agent = GetComponent<NavMeshAgent>();
        FindAndSetTarget();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        timeSinceLastTargetSwitch += Time.deltaTime;
        if(timeSinceLastTargetSwitch >= targetSwitchInterval)
        {
            CheckAndSwitchTargets();
            timeSinceLastTargetSwitch = 0.0f;
        }

        if (currentTarget != null)
        {
            Vector3 directionToTarget = currentTarget.position - transform.position;

            Vector3 stoppingPosition = currentTarget.position - directionToTarget.normalized * stopDistance;

            agent.SetDestination(stoppingPosition);
        }
    }

    private void CheckAndSwitchTargets()
    {
        GameObject[] enemyMinions = null;
        if (gameObject.CompareTag("RedMinion"))
        {
            enemyMinions = GameObject.FindGameObjectsWithTag("BlueMinion");
        }
        else if (gameObject.CompareTag("BlueMinion"))
        {
            enemyMinions = GameObject.FindGameObjectsWithTag("RedMinion");
        }
        
        Transform closestEnemyMinion = GetClosestObjectInRadius(enemyMinions, aggroRange);
        
        if(closestEnemyMinion != null)
        {
            currentTarget = closestEnemyMinion;
        }
        else
        {
            GameObject[] characters = GameObject.FindGameObjectsWithTag(characterTag);
            List<GameObject> enemyCharacters = new();

            foreach (GameObject character in characters)
            {
                if (gameObject.CompareTag("BlueMinion"))
                {
                    if (character.GetComponent<Info>().TeamNum == 1)
                    {
                        enemyCharacters.Add(character);
                    }
                }
                else if (gameObject.CompareTag("RedMinion"))
                {
                    if (character.GetComponent<Info>().TeamNum == 2)
                    {
                        enemyCharacters.Add(character);
                    }
                }
            }
            currentTarget = GetClosestObject(enemyCharacters);
        }
    }

    private Transform GetClosestObject(List<GameObject> objects)
    {
        float closestDistance = Mathf.Infinity;
        Transform closestObject = null;
        Vector3 currentPosition = transform.position;   

        foreach(GameObject obj in objects)
        {
            float distance = Vector3.Distance(currentPosition, obj.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj.transform;
            }
        }

        return closestObject;
    }

    private Transform GetClosestObjectInRadius(GameObject[] objects, float radius)
    {
        float closestDistance = Mathf.Infinity;
        Transform closestObject = null;
        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(currentPosition, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj.transform;
            }
        }

        return closestObject;
    }
    private void FindAndSetTarget()
    {
        GameObject[] enemyMinions = null;
        if (gameObject.CompareTag("RedMinion"))
        {
            enemyMinions = GameObject.FindGameObjectsWithTag("BlueMinion");
        }
        else if (gameObject.CompareTag("BlueMinion"))
        {
            enemyMinions = GameObject.FindGameObjectsWithTag("RedMinion");
        }

        Transform closestEnemyMinion = GetClosestObjectInRadius(enemyMinions, aggroRange);

        if(closestEnemyMinion != null)
        {
            currentTarget = closestEnemyMinion;
        }
        else
        {
            GameObject[] characters = GameObject.FindGameObjectsWithTag(characterTag);
            List<GameObject> enemyCharacters = new();

            foreach (GameObject character in characters)
            {
                if (gameObject.CompareTag("BlueMinion"))
                {
                    if (character.GetComponent<Info>().TeamNum == 1)
                    {
                        enemyCharacters.Add(character);
                    }
                }
                else if (gameObject.CompareTag("RedMinion"))
                {
                    if (character.GetComponent<Info>().TeamNum == 2)
                    {
                        enemyCharacters.Add(character);
                    }
                }
            }
            currentTarget = GetClosestObject(enemyCharacters);
        }
 
    }
}
