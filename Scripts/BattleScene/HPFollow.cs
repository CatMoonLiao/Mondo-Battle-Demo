using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPFollow : MonoBehaviour
{
    public float HpXOffset;
    public float HpYOffset;
    public float HpZOffset;
    public RectTransform HP = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(HP != null)
            HP.position = transform.position + new Vector3(HpXOffset, HpYOffset, HpZOffset);
    }
}
