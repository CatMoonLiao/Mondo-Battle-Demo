using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class OnlineHPController : MonoBehaviourPunCallbacks
{
    public float HpXOffset = 0.05f;
    public float HpYOffset = 3.5f;
    public float HpZOffset = 0.0f;

    //public GameObject HP;
    public RectTransform HP_RectTransform;
    public Slider HP_Slider;
    public Slider Shielded;

    public float PerReduceHP = 0.1f;
    private Animator animator;
    private OnlinePlayerMovement Player;
    private OnlineSkills Skill;
    //private OnlineSkills Skill;
    
    private HashTable hash;
    private PhotonView pv;

    OnlineBattleManager obm;

    private void Start()
    {
        obm = GameObject.Find("OnlineBattleManager").GetComponent<OnlineBattleManager>();
        
        animator = transform.GetComponent<Animator>();
        pv = transform.GetComponent<PhotonView>();
        Player = transform.GetComponent<OnlinePlayerMovement>();
        Skill = transform.GetComponent<OnlineSkills>();

        HP_Slider.value = 1.0f;
        Shielded.value = 0.0f;

        hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash["HP"] = HP_Slider.value;
        hash["Shi"] = Shielded.value;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    void Update()
    {
        if(pv.IsMine)
        {
            //got hit
            if (Player.detectHit)
            {
                if(!Skill.BarState)
                    HP_Slider.value = HP_Slider.value - PerReduceHP;
                Player.detectHit = false;
            }
            //dead
            if (HP_Slider.value <= 0.005)
            {
                Dead();
                Destroy(this);
            }
            var hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["HP"] = HP_Slider.value;
            hash["Shi"] = Shielded.value;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            HP_Slider.transform.LookAt(Camera.main.transform.position);

        }
        HP_RectTransform.position = transform.position + new Vector3(HpXOffset, HpYOffset, HpZOffset);
    }

    public void Dead()
    {
        Player.Dead();
        animator.SetBool("death",true);
        obm.CallRpcLocalPlayerDead();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps){
        if(!pv.IsMine){
            if(targetPlayer == pv.Owner){
                Shielded.value = (float)changedProps["Shi"];
                HP_Slider.value = (float)changedProps["HP"];
                HP_Slider.transform.LookAt(Camera.main.transform.position);
            }
        }
    }
}