using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPC : MonoBehaviour
{
    private bool shooting = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    { 
        if(Input.GetKey(KeyCode.Space))
        {
            shooting = true;
        } else
        {
            shooting = false;
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
        



    }

    public void setShooting()
    {
        this.gameObject.GetComponent<PlaneBehavior>().setShooting(this.shooting);
    }
}
