using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlinePlayerMovement : MonoBehaviourPunCallbacks
{
    public float PlayerSpeed = 5;
    public Vector3 mousePos = Vector3.zero;
    public GameObject BulletPrefab;
    public Transform BulletSpawn;
    public Animator animator;
    public float BulletSpeed = 20;
    public float cdTime = 4;
    public float BulletDestroyTime = 1; //子彈消失時間
    public bool detectHit = false; //判斷是否擊中目標
    public Vector3 targetPos = Vector3.zero;

    AudioManager aM; 

    //private GameObject bullet;
    private PhotonView pv;
    private GameObject bullet;
    private CharacterController controller;
    private float nextShow = 0f;
    private bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        aM = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        targetPos = transform.position;
        controller = transform.GetComponent<CharacterController>();
        transform.hasChanged = false;

        alive = true;

        pv = this.gameObject.GetComponent<PhotonView>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine && alive){
            Control();
        }
    }

    private void Control(){
        //anime: Idle or Run
        if (transform.hasChanged)
        {
            animator.SetBool("run",true);
            transform.hasChanged = false;
        }
        else
            animator.SetBool("run",false);

        //fire
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > nextShow)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //判斷mouse指向的位置
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Plane")))
                mousePos = hit.point;
            //attack anime
            animator.SetTrigger("attack");
            //create bullet
            bullet = PhotonNetwork.Instantiate("Knife_Online", BulletSpawn.position, BulletSpawn.rotation) as GameObject;
            bullet.name = this.name + "Bullet";
            bullet.transform.LookAt(new Vector3(mousePos.x, bullet.transform.position.y, mousePos.z));
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * BulletSpeed;

            nextShow = Time.time + cdTime; //cal. cd time
        }
        //move
        if (Input.GetMouseButtonDown(1)) //char. move
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
    
            if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Plane")))
            {
                targetPos = hit.point;
                transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
            }
        }
        float distance = Vector3.Distance(targetPos, transform.position);
        if (distance > 0.1f)
            controller.Move(transform.forward * PlayerSpeed * Time.deltaTime);
    }

    ///got hit
    private void OnTriggerEnter(Collider collider)
    {
        OnlineBullet bullet = collider.gameObject.GetComponent<OnlineBullet>();
        if(pv.IsMine)
        {
            if(!bullet.pv.IsMine)
            {
                aM.KnifeCollision();
                detectHit = true;
            }
        }
        else
        {
            if(bullet.pv.IsMine)
            {
                aM.KnifeCollision();
            }
        }
    }

    public void Dead()
    {
        alive=false;
    }
}