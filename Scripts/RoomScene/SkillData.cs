using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : MonoBehaviour
{
    public static SkillData SD;
    public int d;
    public int f;
    RoomManager Rm;

    private void Awake(){
        if(SD == null){
            SD = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void UpdateSkill(int SkillD, int SkillF){
        d = SkillD;
        f = SkillF;
    }
}
