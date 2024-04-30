using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterInstantiate : MonoBehaviour
{
    public List<GameObject> characterPrefabs;

    public int RedTeamMemberNum;
    public int BlueTeamMemberNum;

    List<Vector3> instantiatePosition = new()
    {
        new Vector3(20, 10, 8),
        new Vector3(23, 10, 8),
        new Vector3(26, 10, 8),
        new Vector3(29, 10, 8),
    };

    private void Awake()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RedTeamMemberNum = 0;
            BlueTeamMemberNum = 0;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue(HashTableKey.CHARACTER_SELECTION, out object characSelectionIndex)
                && player.CustomProperties.TryGetValue(HashTableKey.TEAM_INDEX, out object teamIndex))
                {
                    if ((int)teamIndex == 1)
                    {
                        RedTeamMemberNum++;
                    }
                    else if ((int)teamIndex == 2)
                    {
                        BlueTeamMemberNum++;
                    }
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(HashTableKey.CHARACTER_SELECTION, out object characterSelectionIndex))
            {
                PhotonNetwork.Instantiate(characterPrefabs[(int)characterSelectionIndex].name, instantiatePosition[PhotonNetwork.LocalPlayer.ActorNumber - 1], Quaternion.identity);
            }
        }
    }

    
}
