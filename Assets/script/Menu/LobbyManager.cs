using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("CreateOrJoinSelection Panel")]
    public GameObject CreateOrJoinSelectionPanel;

    [Header("CreateRoom Panel")]
    public GameObject CreateRoomPanel;
    public TMP_InputField roomNameInputfield;
    public TMP_Text createRoomButtonText;

    [Header("JoinRoom Panel")]
    public GameObject JoinRoomPanel;
    public GameObject roomButtonPrefab;
    public GameObject contentObject;

    private List<GameObject> roomButtonList = new();

    [Header("CharacterSelection Panel")]
    public GameObject CharacterSelectionPanel;
    public TMP_Text inRoomNameText;
    public TMP_Text playerCountText;
    public TMP_Text SelectAndReadyButtonText;

    [Header("Game Parameter")]
    public int requiredPlayerNum = 2;


    private void Start()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;

        CreateOrJoinSelectionPanel.SetActive(true);
        CreateRoomPanel.SetActive(false);
        JoinRoomPanel.SetActive(false);
        CharacterSelectionPanel.SetActive(false);
    }

    // CreateOrJoinSelection Panel
    public void OnSelectCreateRoute()
    {
        CreateOrJoinSelectionPanel.SetActive(false);
        CreateRoomPanel.SetActive(true);
    }

    public void OnSelectJoinRoute()
    {
        CreateOrJoinSelectionPanel.SetActive(false);
        JoinRoomPanel.SetActive(true);
    }


    // CreateRoom Panel
    public void OnCreateRoomButtonClicked()
    {
        if (roomNameInputfield.text != "")
        {
            createRoomButtonText.text = "creating...";

            RoomOptions roomOptions = new();
            roomOptions.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(roomNameInputfield.text, roomOptions);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("create room successfully");
        createRoomButtonText.text = "Create Room";
    }


    // JoinRoom Panel
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("start to update room list, room number: " + roomList.Count);

        foreach (GameObject roomButton in roomButtonList)
        {
            Destroy(roomButton);
        }
        roomButtonList.Clear();

        foreach (RoomInfo roomInfo in roomList)
        {
            GameObject newRoomButton = Instantiate(roomButtonPrefab, contentObject.transform);
            newRoomButton.SetActive(true);
            newRoomButton.GetComponent<RoomSetting>().SetRoomName(roomInfo.Name);
            roomButtonList.Add(newRoomButton);
        }
    }

    public void OnRoomButtonClicked(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnRandomlyJoinRoomButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnBackButtonClicked()
    {
        JoinRoomPanel.SetActive(false);
        CreateOrJoinSelectionPanel.SetActive(true);
    }


    // CharacterSelection Panel
    public override void OnJoinedRoom()
    {
        Debug.Log("join room successfully");
        CreateRoomPanel.SetActive(false);
        JoinRoomPanel.SetActive(false);
        CharacterSelectionPanel.SetActive(true);

        inRoomNameText.text = PhotonNetwork.CurrentRoom.Name;
        playerCountText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        SelectAndReadyButtonText.text = "Select and Ready";


        if (PhotonNetwork.CurrentRoom.PlayerCount % 2 == 1)
        {
            Hashtable teamProp = new() { { HashTableKey.TEAM_INDEX, 1 } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(teamProp);
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount % 2 == 0)
        {
            Hashtable teamProp = new() { { HashTableKey.TEAM_INDEX, 2 } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(teamProp);
        }
    }
    public void OnLeaveRoomButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left the room successfully");
        CharacterSelectionPanel.SetActive(false);
        CreateOrJoinSelectionPanel.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        playerCountText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        playerCountText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        int readyPlayerCount = 0;
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue(HashTableKey.READY_STATE, out object readyState))
                {
                    if ((bool)readyState)
                    {
                        readyPlayerCount++;
                    }
                }
            }

            if (readyPlayerCount >= requiredPlayerNum)
            {
                Invoke(nameof(LoadGameScene), 1);
            }
        }
    }


    private void LoadGameScene()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }

    public void LoadLeaderboardScene()
    {
        SceneManager.LoadScene("RankingSystem");
    }
}
