using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMove : MonoBehaviour
{
    public float PlayerSpeed = 2000;
    public float RandomMinX = -5000;
    public float RandomMaxX = 5000;
    public float RandomMinZ = -5000;
    public float RandomMaxZ = 5000;
    public Animator animator;

    private float targetX;
    private float targetZ;
    private Vector3 targetPosition = Vector3.zero;
    private float time = 0;
    public float ChangeTime = 3;

    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        targetX = Random.Range(RandomMinX, RandomMaxX) + transform.position.x;
        targetZ = Random.Range(RandomMinZ, RandomMaxZ) + transform.position.z;
        targetPosition = new Vector3(targetX, transform.position.y, targetZ);   
        
        controller = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged)
        {
            animator.SetBool("run",true);
            transform.hasChanged = false;
        }
        else
            animator.SetBool("run",false);

        time += Time.deltaTime;
        if(time > ChangeTime)
        {
            targetX = Random.Range(RandomMinX, RandomMaxX) + transform.position.x;
            targetZ = Random.Range(RandomMinZ, RandomMaxZ) + transform.position.z;
            //targetPosition = new Vector3(targetX, transform.position.y, targetZ);
            transform.LookAt(new Vector3(targetX, transform.position.y, targetZ));

            time = 0;
        }
        controller.SimpleMove(transform.forward * PlayerSpeed);
        //controller.Move(transform.forward * PlayerSpeed * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, PlayerSpeed * Time.deltaTime);
    }
}
