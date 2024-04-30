using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomSetting : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;

    private GameObject manager;


    private void Start()
    {
        manager = GameObject.Find("LobbyManager");
    }
    public void SetRoomName(string name)
    {
        roomName.text = name;
    }

    public void OnRoomButtonClicked()
    {
        manager.GetComponent<LobbyManager>().OnRoomButtonClicked(roomName.text);
    }
}
