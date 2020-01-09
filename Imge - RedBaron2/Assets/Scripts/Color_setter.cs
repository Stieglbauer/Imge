using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Color_setter : MonoBehaviour
{
    float val = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Material c = this.gameObject.GetComponent<MeshRenderer>().sharedMaterial;
        c.color = new Color(1, 1, 1, val);
        if (val < 2) 
        val += 0.01f;
    }
}
