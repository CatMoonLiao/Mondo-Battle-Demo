using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using HashTable = ExitGames.Client.Photon.Hashtable;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputRoomName;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;
    

    void Start()
    {
        if(PhotonNetwork.IsConnected==false){
            SceneManager.LoadScene("Launcher");
        }
        else{
            if(PhotonNetwork.CurrentLobby == null){
                PhotonNetwork.JoinLobby();
            }
        }
    }
    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby(){
        print("LobbyScene");
    }

    public string GetRoomName(){
        string roomName = inputRoomName.text;
        return roomName.Trim();
    }


    public void OnClickCreateRoom(){
        string roomName = GetRoomName();
        if(roomName.Length > 0){
            PhotonNetwork.CreateRoom(roomName,new RoomOptions(){MaxPlayers = 2});
        }
        else{
            print("Invalid RoomName!");
        }
    }

    public void OnClickJoinRoom(){
        string roomName = GetRoomName();
        if(roomName.Length > 0){
            PhotonNetwork.JoinRoom(roomName);
        }
        else{
            print("Invalid RoomName!");
        }
    }

    public override void OnJoinedRoom(){
        SceneManager.LoadScene("RoomScene");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList){
        UpdateRoomList(roomList);
    }

    void UpdateRoomList(List<RoomInfo> roomList){

        foreach(RoomItem item in roomItemsList){
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();
        
        foreach(RoomInfo room in roomList){
            if(room.PlayerCount==1)
            {
                RoomItem newRoom = Instantiate(roomItemPrefab,contentObject);
                newRoom.SetRoomName(room.Name);
                newRoom.SetPlayerCount(room.PlayerCount);
                roomItemsList.Add(newRoom);
            }
        }
    }
}
