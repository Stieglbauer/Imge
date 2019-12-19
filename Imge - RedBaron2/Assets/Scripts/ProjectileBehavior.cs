using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float fallOffApproach;
    public GameObject particles;
    [SerializeField]
    private GameObject[] impact;

    // Update is called once per frame
    void Update()
    {
        //fallOff();
        move();
    }

    private void fallOff()
    {
        this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, new Quaternion(0, 0, 1, 0), fallOffApproach);
    }

    private void move()
    {
        this.transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Plane" || other.tag == "Player")
        {
            Vector3 pos = transform.position;
            for (int it = 0; it< 5*33; it++)
            {
                //Instantiate(cube, transform.position + it * (transform.rotation * Vector3.up), transform.rotation);
                //Instantiate(cube, transform.position + it * (transform.rotation * Vector3.up), transform.rotation);
                if (other.GetComponent<Collider>().bounds.Contains(transform.position + it * (transform.rotation * Vector3.up)))
                {
                    if (other.tag == "Plane")
                    {
                        GameObject hit = Instantiate(particles, transform.position + it * (transform.rotation * Vector3.up), transform.rotation);
                        hit.transform.Rotate(new Vector3(90, 0, 0), Space.Self);
                    }
                    int rand = Random.Range(0, impact.Length);
                    Instantiate(impact[rand], other.transform);
                    other.GetComponent<PlaneBehavior>().reduceHealth();
                    Debug.Log("Hit!");
                    it = 5 * 33;
                    Destroy(this.gameObject);
                }
            }
            //Destroy(this.gameObject);
        }
    }
}
