using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    public Slider HP;
    public float PerReduceHP = 0.1f;
    public GameObject PlayerName = null;
    public Animator animator;
    HitDetect Player = null;

    private void Start()
    {
        animator = PlayerName.GetComponent<Animator>();
        if(PlayerName != null)
            Player = PlayerName.GetComponent<HitDetect>();
        HP.value = 1;
    }

    void Update()
    {
        if (Player.detectHit)
        {
            HP.value = HP.value - PerReduceHP;
            Player.detectHit = false;
        }
       
        if (HP.value <= 0.005)
        {
            PlayerName.GetComponent<RandomMove>().enabled = false;
            animator.SetBool("death",true);
            //Destroy(Player.gameObject);
            Destroy(this.gameObject);
        }
        HP.transform.LookAt(Camera.main.transform.position);
    }
}