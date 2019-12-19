using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtRotation : MonoBehaviour
{
    public GameObject pl;
    public float angle;
    // Update is called once per frame
    void Update()
    {
        //this.transform.rotation = Quaternion.LookRotation(Vector3.down, pl.transform.rotation * Vector3.forward);
        //angle = Vector3.Angle(this.transform.rotation*Vector3.forward, Quaternion.LookRotation(pl.transform.rotation * Vector3.forward, Vector3.up) * Vector3.forward);
        /*pl.transform.rotation = Quaternion.LookRotation(this.transform.rotation * Vector3.forward,  Vector3.forward);
        angle = Quaternion.Dot(this.transform.rotation, pl.transform.rotation);
        */

        pl.transform.rotation = this.transform.rotation * Quaternion.AngleAxis(90, Vector3.forward);
    }
}
