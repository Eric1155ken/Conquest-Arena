using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviourPun, IPunObservable
{
    public int OwnerNum { get; set; }
    public int TeamNum { get; set; }

    public RawImage teamInfoUI;

    private void Awake()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(HashTableKey.TEAM_INDEX, out object teamNumber))
        {
            OwnerNum = PhotonNetwork.LocalPlayer.ActorNumber;
            TeamNum = (int)teamNumber;

            if (TeamNum == 1)
            {
                teamInfoUI.color = Color.red;
            }
            else if (TeamNum == 2)
            {
                teamInfoUI.color = Color.blue;
            }
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(OwnerNum);
            stream.SendNext(TeamNum);
        }
        else
        {
            OwnerNum = (int)stream.ReceiveNext();
            TeamNum = (int)stream.ReceiveNext();

            if (TeamNum == 1)
            {
                teamInfoUI.color = Color.red;
            }
            else if (TeamNum == 2)
            {
                teamInfoUI.color = Color.blue;
            }
        }
    }
}
