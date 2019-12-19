using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallDown : MonoBehaviour {
    private Vector3 rot;
    private float fallSpeed = 0;
    [SerializeField]
    private float increase;
    private Vector3 sideSpeed;
    public float rand;

	// Use this for initialization
	void Start () {
        rot = new Vector3(Random.Range(-rand, rand), Random.Range(-rand, rand), Random.Range(-rand, rand));
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        fallSpeed += increase;
        sideSpeed *= 0.65f;
        transform.Rotate(rot);
        transform.Translate(Vector3.down*fallSpeed + sideSpeed, Space.World);
	}

    public void SetSideSpeed(Vector3 sideSpeed)
    {
        this.sideSpeed = sideSpeed;
    }
}
