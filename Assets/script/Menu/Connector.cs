using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class Connector : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private TMP_Text buttonText;


    private void Start()
    {
        PhotonNetwork.Disconnect();

        buttonText.text = "Connect";
    }

    public void OnConnectButtonClicked()
    {
        if (userNameInput.text != "")
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.NickName = userNameInput.text;
                buttonText.text = "Connecting...";
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                SceneManager.LoadScene("Lobby");
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }

}
