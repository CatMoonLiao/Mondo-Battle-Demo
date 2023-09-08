using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using Photon.Pun;
using Photon.Realtime;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class OnlineSkills : MonoBehaviourPunCallbacks
{
    // 1."Heal", 2."Ghost", 3."Barrier", 4."Exhaust", 5."Flash", 6."Ignite" 
    // 1.治療 2.鬼步 3.光盾 4.虛弱 5.閃現 6.點燃

    OnlineHPController HPController;
    OnlinePlayerMovement PlayerController;
    CharacterController CharController;

    public int SkillD;
    public int SkillF;
    int skillD_enemy;
    int skillF_enemy;
    private bool SkillDState = true;
    private bool SkillFState = true;
    private PhotonView pv;
    //private PhotonView pv2;
    private HashTable hash;
    
    //Heal
    private float HealHP = 0.1f;
    private float HealSpeed = 1.0f;
    private float HealTime = 1.0f;
    private bool HealState = false;
    //Ghost
    private float GhostSpeed = 5.0f;
    private float GhostTime = 3.0f;
    private bool GhostState = false;    
    //Barrier
    private float BarHP = 0.2f;
    private float BarTime = 3.0f;
    private float BarHP_tmp;
    private int BarCount = 2;
    public bool BarState = false;
    //Flash
    private float FlashDistance = 5.0f;
    private bool FlashState = false;
    private Vector3 FlashDir;
    private Vector3 FlashTargetPos;
    //Exhaust
    //private OnlineSkills p2Skill;
    private float ExhaustSpeed = 3.0f;
    private float ExhaustTime = 3.0f;
    private bool ExhRayDetect = false;
    private bool ExhDetectState = false;
    private bool ExhState = false;
    //Ignite
    private int IgniteTime = 3;
    private float IgniteHP = 0.05f;
    private bool IgnRayDetect = false;
    private bool IgnDetectState = false;

    AudioManager aM;
    // Start is called before the first frame update
    void Start()
    {
        HPController = transform.GetComponent<OnlineHPController>();
        PlayerController = transform.GetComponent<OnlinePlayerMovement>();
        CharController = transform.GetComponent<CharacterController>();
        pv = transform.GetComponent<PhotonView>();
        aM = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        if(transform.name!="DrMundo_Online(Clone)"){
            SkillD = GameObject.Find("SkillData").GetComponent<SkillData>().d;
            SkillF = GameObject.Find("SkillData").GetComponent<SkillData>().f;
            aM.InitAudio(true, transform.GetComponent<AudioSource>(),SkillD,SkillF);
            pv.RPC("ReciveEnemySkill",RpcTarget.Others,SkillD,SkillF);
        }
        aM.InitAudio(false, GameObject.Find("DrMundo_Online(Clone)").GetComponent<AudioSource>(),skillD_enemy,skillF_enemy);

    }
    [PunRPC]
    void ReciveEnemySkill(int d,int f){
        Debug.Log("[ReciveEnemySkill]"+d+f);
        skillD_enemy = d;
        skillF_enemy = f;
    }


    // Update is called once per frame
    void Update()
    {
        if(HPController.HP_Slider.value > 0.0f){
            if (Input.GetKeyDown(KeyCode.D) && SkillDState && pv.IsMine){
                SkillSelect(SkillD);
                aM.triggerSkillD();
                pv.RPC("reciveSkillD",RpcTarget.Others);
                //SkillDState = false;
            }
            if (Input.GetKeyDown(KeyCode.F) && SkillFState && pv.IsMine){
                SkillSelect(SkillF);
                aM.triggerSkillF();
                pv.RPC("reciveSkillF",RpcTarget.Others);
                //SkillFState = false;
            }
            if (BarState && PlayerController.detectHit){
                if(BarHP_tmp + BarHP > 1.0f)
                    HPController.HP_Slider.value += HPController.PerReduceHP;
                else
                    HPController.Shielded.value -= HPController.PerReduceHP;
                //HPController.Shielded.value -= HPController.PerReduceHP;
            }
            if(IgnRayDetect){
                pv.RPC("RPC_Ignite", RpcTarget.All);
            }
            if(ExhRayDetect)
                pv.RPC("RPC_Exhaust", RpcTarget.All);
        }
    }

    [PunRPC]
    void reciveSkillD(){
        Debug.Log("[reciveSkillD]");
        aM.triggerSkillD_enemy();
    }
    [PunRPC]
    void reciveSkillF(){
        Debug.Log("[reciveSkillF]");
        aM.triggerSkillF_enemy();
    }
    void SkillSelect(int s){
        switch(s){
            case 1:
                if(pv.IsMine) Heal();
                break;
            case 2:
                if(pv.IsMine) Ghost();
                break;
            case 3:
                if(pv.IsMine) Barrier();
                break;
            case 4:
                if(pv.IsMine) Exhaust();
                break;
            case 5:
                if(pv.IsMine) Flash();
                break;
            case 6:
                if(pv.IsMine) Ignite();
                break;
            default:
                break;
        }
    }
    void Heal(){
        PlayerController.PlayerSpeed += HealSpeed;
        if(BarState){
            if(HPController.Shielded.value + HealHP > 1.0f){
                BarHP_tmp += HealHP;
                HPController.HP_Slider.value = 1.0f - (HPController.Shielded.value - HPController.HP_Slider.value);    
                HPController.Shielded.value = 1.0f;
            }
            else{
                BarHP_tmp += HealHP;
                HPController.Shielded.value += HealHP;
                HPController.HP_Slider.value += HealHP;
            }
        }
        else
            HPController.HP_Slider.value = Math.Min(1, HPController.HP_Slider.value + HealHP);
        HealState = true;
        Invoke("SkillEndTime", HealTime);
    }
    
    void Ghost(){
        PlayerController.PlayerSpeed += GhostSpeed;
        GhostState = true;
        Invoke("SkillEndTime", GhostTime);
    }
    
    void Barrier(){
        BarState = true;
        if(BarHP + HPController.HP_Slider.value > 1.0f){
            BarHP_tmp = HPController.HP_Slider.value;
            HPController.Shielded.value = 1.0f;
            HPController.HP_Slider.value = 1.0f - BarHP;            
        }
        else{
            BarHP_tmp = HPController.HP_Slider.value;
            HPController.Shielded.value = HPController.HP_Slider.value + BarHP; 
        }
        Invoke("SkillEndTime", BarTime);
        
    }
    void Flash(){
        FlashState = true;
        Ray();
        transform.LookAt(FlashTargetPos);
        if(FlashDir == transform.forward){
            PlayerController.targetPos = transform.position + transform.forward * FlashDistance;
            CharController.Move(transform.forward * FlashDistance);
            FlashState = false;
            return;
        }
    }
    void Exhaust(){
        ExhDetectState = true;
        Ray();
    }
    void Ignite(){
        IgnDetectState = true;
        Ray();
    }
    void IgnitePerReduceHP(){
        if(!BarState)
            HPController.HP_Slider.value -= IgniteHP;
        else
            HPController.Shielded.value -= IgniteHP;
    }
    void Ray(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(FlashState)
            if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Plane"))){
                FlashTargetPos = hit.point;
                FlashTargetPos.y = transform.position.y;
                FlashDir = (FlashTargetPos - transform.position).normalized; 
            }
            else
                FlashDir = transform.forward;
        if(ExhDetectState)
            if(Physics.Raycast(ray, out hit, 1000)){
                if(hit.transform.tag == "Mundo" && hit.transform.name != transform.name){
                    hit.transform.GetComponent<OnlineSkills>().ExhRayDetect = true;
                    ExhDetectState = false;
                }
            }
        if(IgnDetectState)
            if(Physics.Raycast(ray, out hit, 1000)){
                if(hit.transform.tag == "Mundo" && hit.transform.name != transform.name){
                    hit.transform.GetComponent<OnlineSkills>().IgnRayDetect = true;
                    IgnDetectState = false;
                }
            }
    }
    [PunRPC]
    void RPC_Ignite(PhotonMessageInfo info){
        IgnRayDetect = false;
        for(int i=0; i<IgniteTime; i++)
            Invoke("IgnitePerReduceHP", i);

    } 
    [PunRPC]
    void RPC_Exhaust(){
        ExhState = true;
        ExhRayDetect = false;
        PlayerController.PlayerSpeed -= ExhaustSpeed;
        Invoke("SkillEndTime", ExhaustTime);

    }
    void SkillEndTime(){
        if(HealState){
            PlayerController.PlayerSpeed -= HealSpeed;
            HealState = false;
        }
        if(GhostState){
            PlayerController.PlayerSpeed -= GhostSpeed;
            GhostState = false;
        }
        if(BarState){
            if(HPController.HP_Slider.value > HPController.Shielded.value){
                HPController.HP_Slider.value = HPController.Shielded.value;
            }
            else
                HPController.HP_Slider.value = BarHP_tmp;
            HPController.Shielded.value = 0.0f;
            BarState = false;
        }
        if(ExhState){
            PlayerController.PlayerSpeed += ExhaustSpeed;
            ExhState = false;
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps){
        if(!pv.IsMine){
            if(targetPlayer == pv.Owner){
                HPController.Shielded.value = (float)changedProps["Shi"];
                HPController.HP_Slider.value = (float)changedProps["HP"];
            }
        }
    }
}

