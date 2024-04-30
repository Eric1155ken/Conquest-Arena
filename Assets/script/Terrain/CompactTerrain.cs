using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class CompactTerrain : MonoBehaviour
{
    private Terrain terrain;
    private Vector3 initialTerrainSize = new Vector3(100f, 100f, 100f);

    public List<GameObject> objectsOnTerrain;
    public GameObject myCharacter;

    private int currentIteration = 0;
    private int totalIteration = 6;
    private float compactDuration = 180f;
    private float iterationInterval;
    private Vector3 currentTerrainSize;
    private Vector3 compactAmount = new Vector3(5f, 5f, 5f);

    public GameObject NavMeshSurface;

    public TMP_Text countdownValueText;
    private float countdownValue;

    public List<GameObject> minions;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrain.terrainData.size = initialTerrainSize;
        currentTerrainSize = initialTerrainSize;

        GameObject[] Characters = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject Character in Characters)
        {
            if (Character.GetComponent<Info>().OwnerNum == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                myCharacter = Character;
                break;
            }
        }

        iterationInterval = compactDuration / totalIteration;
        InvokeRepeating(nameof(Compact), iterationInterval, iterationInterval);

        countdownValue = iterationInterval;
        countdownValueText.text = Mathf.RoundToInt(countdownValue).ToString();
        InvokeRepeating(nameof(UpdateCountdownValueText), 0f, 1f);
    }

    void Compact()
    {
        currentIteration++;


        Vector3 newTerrainSize = currentTerrainSize - compactAmount;
        terrain.terrainData.size = newTerrainSize;


        foreach (GameObject aGameObject in objectsOnTerrain)
        {
            Vector3 relativePosition = aGameObject.transform.position - transform.position;

            float newXCoordinate = relativePosition.x * newTerrainSize.x / currentTerrainSize.x + transform.position.x;
            float newZCoordinate = relativePosition.z * newTerrainSize.z / currentTerrainSize.z + transform.position.z;

            aGameObject.transform.position = new Vector3(newXCoordinate, aGameObject.transform.position.y, newZCoordinate);
        }


        NavMeshSurface.GetComponent<NavMeshSurface>().BuildNavMesh();


        Vector3 characterRelativePosition = myCharacter.transform.position - transform.position;
        float characterNewXCoordinate = characterRelativePosition.x * newTerrainSize.x / currentTerrainSize.x + transform.position.x;
        float characterNewZCoordinate = characterRelativePosition.z * newTerrainSize.z / currentTerrainSize.z + transform.position.z;

        Vector3 characterNewPosition = new Vector3(characterNewXCoordinate, myCharacter.transform.position.y, characterNewZCoordinate);
        if (NavMesh.SamplePosition(characterNewPosition, out NavMeshHit navMeshHit, 3f, NavMesh.AllAreas))
        {
            myCharacter.GetComponent<NavMeshAgent>().Warp(navMeshHit.position);
        }
        else
        {
            if (NavMesh.FindClosestEdge(characterNewPosition, out NavMeshHit navMeshHit2, NavMesh.AllAreas))
            {
                myCharacter.GetComponent<NavMeshAgent>().Warp(navMeshHit2.position);
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            minions.Clear();
            GameObject[] redMinions = GameObject.FindGameObjectsWithTag("RedMinion");
            GameObject[] blueMinions = GameObject.FindGameObjectsWithTag("BlueMinion");
            minions.AddRange(redMinions);
            minions.AddRange(blueMinions);
            foreach (GameObject minion in minions)
            {
                Vector3 minionRelativePosition = minion.transform.position - transform.position;
                float minionNewXCoordinate = minionRelativePosition.x * newTerrainSize.x / currentTerrainSize.x + transform.position.x;
                float minionNewZCoordinate = minionRelativePosition.z * newTerrainSize.z / currentTerrainSize.z + transform.position.z;

                Vector3 minionNewPosition = new Vector3(minionNewXCoordinate, minion.transform.position.y, minionNewZCoordinate);
                if (NavMesh.SamplePosition(minionNewPosition, out NavMeshHit minionNavMeshHit, 3f, NavMesh.AllAreas))
                {
                    minion.GetComponent<NavMeshAgent>().Warp(minionNavMeshHit.position);
                }
                else
                {
                    if (NavMesh.FindClosestEdge(minionNewPosition, out NavMeshHit minionNavMeshHit2, NavMesh.AllAreas))
                    {
                        minion.GetComponent<NavMeshAgent>().Warp(minionNavMeshHit2.position);
                    }
                }
            }
        }

        currentTerrainSize = newTerrainSize;


        countdownValue = iterationInterval;


        if (currentIteration >= totalIteration)
        {
            CancelInvoke(nameof(Compact));
        }
    }

    void UpdateCountdownValueText()
    {
        if (currentIteration >= totalIteration)
        {
            CancelInvoke(nameof(UpdateCountdownValueText));
        }

        countdownValue -= 1;

        if (countdownValue < 0)
        {
            countdownValue = 0;
        }

        countdownValueText.text = Mathf.RoundToInt(countdownValue).ToString();
    }
}
