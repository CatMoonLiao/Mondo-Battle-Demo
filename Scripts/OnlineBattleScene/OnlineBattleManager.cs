using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class OnlineBattleManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private GameObject player;
    PhotonView pv;

    public Dictionary<Player,bool>  alivePlayerMap = new Dictionary<Player,bool>();

    void Start()
    {
        pv = this.gameObject.GetComponent<PhotonView>();
        Debug.Log("OnlineBattleScene");
        if(PhotonNetwork.CurrentRoom == null){
            SceneManager.LoadScene("Lobby");
            return;
        }
        else{
            InitGame();
        }
    }

    public void InitGame(){
        if(PhotonNetwork.IsMasterClient){
            player = PhotonNetwork.Instantiate("DrMundo_Online",new Vector3((float)15.5,(float)-8.4,(float)-9.87),Quaternion.identity) as GameObject;
            player.name = "DrMundo_Master";
        }
        else{
            player = PhotonNetwork.Instantiate("DrMundo_Online",new Vector3((float)9.0,(float)-8.4,(float)-5.5),Quaternion.identity) as GameObject;
            player.name = "DrMundo_Client";
        }
        
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            alivePlayerMap[player.Value] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player==null)
            return;
        if(PhotonNetwork.CurrentRoom.PlayerCount <=1)
        {
            PhotonNetwork.LeaveRoom();
        }

    }
    //call when local player die
    public void CallRpcLocalPlayerDead()
    {
        pv.RPC("RpcPlayerDead", RpcTarget.All);
    }

    [PunRPC]
    void RpcPlayerDead(PhotonMessageInfo info)
    {
        if(alivePlayerMap.ContainsKey(info.Sender))
        {
            alivePlayerMap[info.Sender] = false;
        }

        
        if(info.Sender == pv.Owner)
        {
            //Defeat
            
        }
        else
        {
            //Victory
        }


        if(PhotonNetwork.IsMasterClient && CheckGameOver())
        {
            Invoke("GameOver",3.0f);
        }
    }

    bool CheckGameOver()
    {
        int aliveCount = 0;
        foreach(var player in alivePlayerMap)
        {
            if(player.Value)
                aliveCount++;
        }
        return aliveCount <=1 ;
    }

    void GameOver()
    {
        SceneManager.LoadScene("RoomScene");
    }


    public override void OnPlayerLeftRoom(Player newPlayer){
        Invoke("GameOver",3.0f);
    }

}
