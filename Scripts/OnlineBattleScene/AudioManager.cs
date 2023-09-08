using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AudioManager : MonoBehaviour
{
    
    AudioSource audiosource;
    public AudioSource audioMaster;
    public AudioSource audioClient;

    AudioClip bgm_knifeCollision;
    public AudioClip bgm_skillD;
    public AudioClip bgm_skillF;
    public AudioClip bgm_skillD_enemy;
    public AudioClip bgm_skillF_enemy;
    // Start is called before the first frame update

    List<string> skill_bgm_list = new List<string>(){"heal", "ghost", "barrier", "exhaust", "flash", "ignite"};
    void Start()
    {
        audiosource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        bgm_knifeCollision = Resources.Load<AudioClip>("BGM/Q-hit");
    }

    public void InitAudio(bool master, AudioSource aS, int d ,int f){
        if(master){
            audioMaster = aS;
            bgm_skillD = Resources.Load<AudioClip>("BGM/"+skill_bgm_list[d-1]);
            bgm_skillF = Resources.Load<AudioClip>("BGM/"+skill_bgm_list[f-1]);
        }else{
            audioClient = aS;
            bgm_skillD_enemy = Resources.Load<AudioClip>("BGM/"+skill_bgm_list[d-1]);
            bgm_skillF_enemy = Resources.Load<AudioClip>("BGM/"+skill_bgm_list[f-1]);
        }
    }
    public void triggerSkillD_enemy(){
        audioClient.PlayOneShot(bgm_skillD_enemy);
    }
    public void triggerSkillF_enemy(){
        audioClient.PlayOneShot(bgm_skillF_enemy);
    }
    public void triggerSkillD(){
        audioMaster.PlayOneShot(bgm_skillD);
    }
    public void triggerSkillF(){
        audioMaster.PlayOneShot(bgm_skillD);
    }
    public void KnifeCollision()
    {
        audiosource.PlayOneShot(bgm_knifeCollision);
    }

}
