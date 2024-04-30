using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Minimap : MonoBehaviourPun
{
    private GameObject character;

    private void Start()
    {
        FindCharacter();
    }

    private void Update()
    {
        if (!character)
        {
            FindCharacter();
        }
    }

    private void LateUpdate()
    {
        if (character)
        {
            Vector3 newPosition = character.transform.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }
    }

    private void FindCharacter()
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject characterObj in characters)
        {
            Info characterInfo = characterObj.GetComponent<Info>();
            if (characterInfo != null && characterInfo.OwnerNum == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                character = characterObj;
                break;
            }
        }
    }
}
