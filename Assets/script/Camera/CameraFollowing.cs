using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public Transform character;
    public Vector3 cameraOffset;

    [Range(0.01f, 1.0f)]
    public float smoothness = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = new Vector3(0, 25, -25);

        GameObject[] Characters = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject Character in Characters)
        {
            if(Character.GetComponent<Info>().OwnerNum == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                character = Character.transform; 
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = character.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothness);
    }
}
