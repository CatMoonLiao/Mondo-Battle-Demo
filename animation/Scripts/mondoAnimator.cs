using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mondoAnimator : MonoBehaviour
{
    public Animator animator;
    private CharacterController controller;
    public int anim_status;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        controller = transform.GetComponent<CharacterController>();
        anim_status = 0;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.RightArrow))
            animator.SetBool("death",true);
        if (Input.GetKeyDown(KeyCode.Space))
            animator.SetBool("win",true);

        //0=idle, 1=run, 2=attack, 3=win, 4= loss
    }
}
