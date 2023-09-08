using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlineBullet : MonoBehaviour
{
    public float destroyTime = 1;
    public PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = this.gameObject.GetComponent<PhotonView>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine)
        {
            destroyTime -= Time.deltaTime;
            if(destroyTime<=0)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    //hit
    private void OnTriggerEnter(Collider collider)
    {
        if(pv.IsMine)
        {
            if(collider.name == "DrMundo_Online(Clone)")
            {
                PhotonView colliderPV = collider.gameObject.GetComponent<PhotonView>();
                if(!colliderPV.IsMine)
                {   
                    destroyTime = (float)0.2;
                }
            }    
        }
            
    }
}
