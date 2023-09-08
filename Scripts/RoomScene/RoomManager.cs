using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text LocalPlayerName;
    [SerializeField] TMP_Text EnemyPlayerName;
    [SerializeField] Button buttonReady;
    [SerializeField] Button buttonReady_Enemy;
    [SerializeField] Button Skill1;
    [SerializeField] Button Skill2;
    [SerializeField] ScrollRect SkillList;

    public static RoomManager Rm;
    private SkillData SD;
    private int SkillState = 1;
    public int SkillD = 1;
    public int SkillF = 2;



    List<Sprite> icon_list = new List<Sprite>() { };

    //private void Awake(){
    //    if(Rm == null){
    //        Rm = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else{
    //        Destroy(gameObject);
    //    }
    //}

    private void changeIcon() {
        Skill1.GetComponent<Image>().sprite = icon_list[SkillD-1];
        Skill2.GetComponent<Image>().sprite = icon_list[SkillF-1];
    }

    void Start()
    {
        SD = GameObject.Find("SkillData").GetComponent<SkillData>();

        //check connection
        if(PhotonNetwork.IsConnected==false){
            SceneManager.LoadScene("Launcher");
            return;
        }
        else if(PhotonNetwork.CurrentLobby == null)
        {
                PhotonNetwork.JoinLobby();
        }
        if(PhotonNetwork.CurrentRoom==null){
            SceneManager.LoadScene("Lobby");
            return;
        }
        else{
            roomName.text = PhotonNetwork.CurrentRoom.Name;
        }
        PhotonNetwork.CurrentRoom.IsVisible =true;
        //set player's name
        LocalPlayerName.text = PhotonNetwork.LocalPlayer.NickName;
        foreach(var player in PhotonNetwork.CurrentRoom.Players){
            if(!player.Value.IsLocal){
                EnemyPlayerName.text = player.Value.NickName;
            }
        }

        //reset player properties
        PhotonNetwork.LocalPlayer.SetCustomProperties(null);
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash["Ready"] = false;   
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        buttonReady_Enemy.GetComponent<Image>().color = new Color32(0,20,32,255);
        buttonReady.GetComponent<Image>().color = new Color32(0,20,32,255);
        buttonReady_Enemy.GetComponentInChildren<TMP_Text>().color = new Color32(224, 172, 52, 255);
        buttonReady.GetComponentInChildren<TMP_Text>().color = new Color32(224, 172, 52, 255);

        //SkillList = Skill1.gameObject.GetChild(0).gameObject;
        SkillList.gameObject.SetActive(false);


        //make skill icon list
        Sprite img_heal = Resources.Load<Sprite>("Icon/heal");
        Sprite img_ghost = Resources.Load<Sprite>("Icon/ghost");
        Sprite img_barrier = Resources.Load<Sprite>("Icon/barrier");
        Sprite img_exhaust = Resources.Load<Sprite>("Icon/exhaust");
        Sprite img_flash = Resources.Load<Sprite>("Icon/flash");
        Sprite img_ignite = Resources.Load<Sprite>("Icon/ignite");
        icon_list.Add(img_heal);
        icon_list.Add(img_ghost);
        icon_list.Add(img_barrier);
        icon_list.Add(img_exhaust);
        icon_list.Add(img_flash);
        icon_list.Add(img_ignite);

        changeIcon();
    }

    

    public override void OnPlayerEnteredRoom(Player newPlayer){
        EnemyPlayerName.text = newPlayer.NickName;
    }

    public override void OnPlayerLeftRoom(Player newPlayer){
        EnemyPlayerName.text = "Waiting";
    }

    public void OnClickSkillD(){
        SkillState = 1;
        SkillList.gameObject.SetActive(true);
    }

    public void OnClickSkillF(){
        SkillState = 2;
        SkillList.gameObject.SetActive(true);
    }

    public void OnClickSkill1(){
        if(SkillState == 1){
            if(SkillF == 1) SkillF = SkillD;
            SkillD = 1;
        }
        else{
            if(SkillD == 1) SkillD = SkillF;
            SkillF = 1;
        }
        SkillList.gameObject.SetActive(false);
        changeIcon();
    }
    public void OnClickSkill2(){
        if(SkillState == 1){
            if(SkillF == 2) SkillF = SkillD;
            SkillD = 2;
        }
        else{
            if(SkillD == 2) SkillD = SkillF;
            SkillF = 2;
        }
        SkillList.gameObject.SetActive(false);
        changeIcon();
    }
    public void OnClickSkill3(){
        if(SkillState == 1){
            if(SkillF == 3) SkillF = SkillD;
            SkillD = 3;
        }
        else{
            if(SkillD == 3) SkillD = SkillF;
            SkillF = 3;
        }
        SkillList.gameObject.SetActive(false);
        changeIcon();
    }
    public void OnClickSkill4(){
        if(SkillState == 1){
            if(SkillF == 4) SkillF = SkillD;
            SkillD = 4;
        }
        else{
            if(SkillD == 4) SkillD = SkillF;
            SkillF = 4;
        }
        SkillList.gameObject.SetActive(false);
        changeIcon();
    }
    public void OnClickSkill5(){
        if(SkillState == 1){
            if(SkillF == 5) SkillF = SkillD;
            SkillD = 5;
        }
        else{
            if(SkillD == 5) SkillD = SkillF;
            SkillF = 5;
        }
        SkillList.gameObject.SetActive(false);
        changeIcon();
    }
    public void OnClickSkill6(){
        if(SkillState == 1){
            if(SkillF == 6) SkillF = SkillD;
            SkillD = 6;
        }
        else{
            if(SkillD == 6) SkillD = SkillF;
            SkillF = 6;
        }
        SkillList.gameObject.SetActive(false);
        changeIcon();
    }

    public void OnClickReady(){
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        if(hash.ContainsKey("Ready") && (bool)hash["Ready"]){
            hash["Ready"] = false;   
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            buttonReady.GetComponent<Image>().color = new Color32(0, 20, 32, 255);
            buttonReady.GetComponentInChildren<TMP_Text>().color = new Color32(94, 73, 22, 255);
        }
        else
        {
            hash["Ready"] = true;   
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            
            CheckAllPlayersReady();
        }
    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer,HashTable changedProps){
        if(targetPlayer != PhotonNetwork.LocalPlayer){
            if(changedProps.ContainsKey("Ready") && (bool)changedProps["Ready"]){
                buttonReady_Enemy.GetComponent<Image>().color = new Color32(0,7,10,255);
                buttonReady_Enemy.GetComponentInChildren<TMP_Text>().color = new Color32(94, 73, 22, 255);
            }
            else{
                buttonReady_Enemy.GetComponent<Image>().color = new Color32(0,20,32,255);
                buttonReady_Enemy.GetComponentInChildren<TMP_Text>().color = new Color32(224,172, 52, 255);
            }
        }
        CheckAllPlayersReady();
    }
    //check if all players are ready
    private void CheckAllPlayersReady ()
    { 
        foreach(var player in PhotonNetwork.CurrentRoom.Players){
            if(!player.Value.IsLocal){
                if(player.Value.CustomProperties.ContainsKey("Ready") && (bool)player.Value.CustomProperties["Ready"]){
                    buttonReady_Enemy.GetComponent<Image>().color = new Color32(0,7,10,255);
                    buttonReady_Enemy.GetComponentInChildren<TMP_Text>().color = new Color32(94, 73, 22, 255);
                }
                else
                {
                    buttonReady_Enemy.GetComponent<Image>().color = new Color32(0,20,32,255);
                    buttonReady_Enemy.GetComponentInChildren<TMP_Text>().color = new Color32(224,172, 52, 255);
                }
                SD.UpdateSkill(SkillD, SkillF);
            }
            else{
                if(player.Value.CustomProperties.ContainsKey("Ready") && (bool)player.Value.CustomProperties["Ready"]){
                    buttonReady.GetComponent<Image>().color = new Color32(0,7,10,255);
                    buttonReady.GetComponentInChildren<TMP_Text>().color = new Color32(94, 73, 22, 255);
                }
                else
                {
                    buttonReady.GetComponent<Image>().color = new Color32(0,20,32,255);
                    buttonReady.GetComponentInChildren<TMP_Text>().color = new Color32(224,172, 52, 255);
                }
                SD.UpdateSkill(SkillD, SkillF);
            }
        }
        
        if(!PhotonNetwork.IsMasterClient) return;

        if(buttonReady.GetComponent<Image>().color==new Color32(0,7,10,255)&&buttonReady_Enemy.GetComponent<Image>().color == new Color32(0,7,10,255)){
            PhotonNetwork.CurrentRoom.IsVisible =false;
            SceneManager.LoadScene("OnlineBattleScene");
        }
    }

    public void OnClickLeave(){
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom(){
        SceneManager.LoadScene("Lobby");
    }
}
