using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA;

public class PlayerBehavior : MonoBehaviour
{

    private GameObject camera;
    private bool shooting = false;
    [SerializeField]
    private Animator prop;
    public GameObject health;
    public GameObject speed;

    //delete this
    public GameObject marker;

    private GameObject mg;

    private Vector3 lowPassValue = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        mg = GameObject.Find("PlayerMG");
    }

    // Update is called once per frame
    void Update()
    {
        //Instantiate(marker, this.gameObject.transform.position, new Quaternion(0, 0, 0, 0));
        health.GetComponent<Text>().text = "Health: " + this.gameObject.GetComponent<PlaneBehavior>().getHealth();
        speed.GetComponent<Text>().text = "Speed: " + this.gameObject.GetComponent<PlaneBehavior>().getForwardV();


        /*float shaking = 0.03f;
        float x = Random.Range(-shaking, shaking);
        float y = Random.Range(-shaking, shaking);
        float z = Random.Range(-shaking, shaking);
        camera.transform.position = new Vector3(0.8f + x, 2.13f + y, z);*/

        //camera.transform.position = Vector3.MoveTowards(camera.transform.position, new Vector3(0.8f, 2.13f, 0), 0.01f);

    }


    public void setGlowing(float f)
    {
        mg.GetComponent<Color_setter>().setGlowing(f);
    }

}
