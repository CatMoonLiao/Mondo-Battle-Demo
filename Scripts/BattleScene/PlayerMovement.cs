using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerSpeed = 5;
    public Vector3 mousePos = Vector3.zero;
    public GameObject BulletPrefab;
    public Transform BulletSpawn;
    public Animator animator;
    public float BulletSpeed = 20;
    public float cdTime = 0;
    public float BulletDestroyTime = 1; //子彈消失時間
    public bool detectHit = false; //判斷是否擊中目標

    //private GameObject bullet;
    private GameObject bullet;
    private float nextShow = 0f;
    private CharacterController controller;
    private Vector3 targetPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        controller = transform.GetComponent<CharacterController>();
        transform.hasChanged = false;
    }

    // Update is called once per frame
    void Update()
    {
        //anime: Idle or Run
        if (transform.hasChanged)
        {
            animator.SetBool("run",true);
            transform.hasChanged = false;
        }
        else
            animator.SetBool("run",false);

        if (Input.GetKeyDown(KeyCode.Q) && Time.time > nextShow)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //判斷mouse指向的位置
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Plane")))
                mousePos = hit.point;
            //attack anime
            animator.SetTrigger("attack");
            //create bullet
            bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation) as GameObject;
            bullet.name = this.name + "Bullet";
            bullet.transform.LookAt(new Vector3(mousePos.x, bullet.transform.position.y, mousePos.z));
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * BulletSpeed;

            nextShow = Time.time + cdTime; //cal. cd time
            Invoke("DestroyGameObject", BulletDestroyTime);
        }
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

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(collider.name);
        if(collider.name != this.name + "Bullet")
        {
            Destroy(collider.gameObject);
            detectHit = true;
        }
    }
    private void DestroyGameObject()
    {
        Destroy(bullet);
    }
}