using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadAlignment : MonoBehaviour
{

    private GameObject camera ;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        transform.LookAt(camera.transform.position);
        transform.Rotate(new Vector3(90, 0, 0), Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.transform.position);
        //transform.Rotate(new Vector3(90, 0, 0), Space.Self);
    }
}
