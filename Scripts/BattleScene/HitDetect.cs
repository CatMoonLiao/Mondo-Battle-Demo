using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetect : MonoBehaviour
{
    public bool detectHit = false; //�P�_�O�_�����ؼ�

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(collider.name);
        if (collider.name != this.name + "Bullet")
        {
            Destroy(collider.gameObject);
            detectHit = true;
        }
    }
}
