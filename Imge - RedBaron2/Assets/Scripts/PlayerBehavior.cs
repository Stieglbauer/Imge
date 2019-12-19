using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    private GameObject camera;
    private bool shooting = false;
    [SerializeField]
    private Animator prop;
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");

    }

    // Update is called once per frame
    void Update()
    {
        text.GetComponent<Text>().text = "" + this.gameObject.GetComponent<PlaneBehavior>().getHealth();
        shooting = false;
        if(Input.GetKey(KeyCode.Space))
        {
            shooting = true;
        }
        setShooting();
        this.gameObject.GetComponent<PlaneBehavior>().setSpeedX(Input.GetAxis("Horizontal"));
        //Debug.Log(" returned : " + Input.GetAxis("Horizontal"));
        this.gameObject.GetComponent<PlaneBehavior>().setSpeedY(Input.GetAxis("Vertical"));

        //changing animation speed of propeller
        //prop.SetFloat("speed", this.gameObject.GetComponent<PlaneBehavior>().getForwardV()/30);

        if(Input.GetKey(KeyCode.I))
        {
            this.gameObject.GetComponent<PlaneBehavior>().setForwardV(this.gameObject.GetComponent<PlaneBehavior>().getForwardV() + 1);
        }
        if (Input.GetKey(KeyCode.K))
        {
            this.gameObject.GetComponent<PlaneBehavior>().setForwardV(this.gameObject.GetComponent<PlaneBehavior>().getForwardV() - 1);
        }



        /*float shaking = 0.03f;
        float x = Random.Range(-shaking, shaking);
        float y = Random.Range(-shaking, shaking);
        float z = Random.Range(-shaking, shaking);
        camera.transform.position = new Vector3(0.8f + x, 2.13f + y, z);*/

        //camera.transform.position = Vector3.MoveTowards(camera.transform.position, new Vector3(0.8f, 2.13f, 0), 0.01f);

    }

    public void setShooting()
    {
        this.gameObject.GetComponent<PlaneBehavior>().setShooting(this.shooting);
    }
}
