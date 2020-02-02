using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Color_setter : MonoBehaviour
{
    private Material m;
    // Start is called before the first frame update
    void Start()
    {

        m = this.gameObject.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void setGlowing(float f)
    {
        m.color = new Color(1, 1, 1, f);
    }


}
