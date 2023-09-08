using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomItem : MonoBehaviour
{
    public TMP_Text roomName;
    public TMP_Text playerCount;

    LobbyManager lobbyManager;

    private void Start(){
        lobbyManager = FindObjectOfType<LobbyManager>();
    }
    public void SetRoomName(string _roomName){
        roomName.text = _roomName;
    }

    public void SetPlayerCount(int _playerCount){
        playerCount.text = "PlayerCount: " + _playerCount+"/2";
    }

    public void OnClickRoomItem(){
        lobbyManager.inputRoomName.text = roomName.text;
    }
}
