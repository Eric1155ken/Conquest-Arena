using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameEndController : MonoBehaviourPun, IPunObservable
{
    int RedTeamMemberNum;
    int BlueTeamMemberNum;
    // Start is called before the first frame update
    void Start()
    {
        RedTeamMemberNum = GameObject.Find("characterInstantiate").GetComponent<characterInstantiate>().RedTeamMemberNum;
        BlueTeamMemberNum = GameObject.Find("characterInstantiate").GetComponent<characterInstantiate>().BlueTeamMemberNum;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void updateTeamMember(string characterName, int teamNumber)
    {
        if (teamNumber == 1)
        {
            RedTeamMemberNum--;
        }
        else if (teamNumber == 2)
        {
            BlueTeamMemberNum--;
        }

        checkGameEnd();
    }

    [PunRPC]
    private void checkGameEnd()
    {
        if (RedTeamMemberNum == 0)
        {
            Hashtable winnerProp = new() { { "winTeam", 2 } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(winnerProp);
            Invoke(nameof(loadEndScene), 2);
        }
        else if (BlueTeamMemberNum == 0)
        {
            Hashtable winnerProp = new() { { "winTeam", 1 } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(winnerProp);
            Invoke(nameof(loadEndScene), 2);
        }
    }

    [PunRPC]
    public void loadEndScene()
    {
        SceneManager.LoadScene("End");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ;
    }
}
