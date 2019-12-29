using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA;

public class InputAndroid : MonoBehaviour
{
    private bool shooting;
    private static float AccelerometerUpdateInterval = 1.0f / 60.0f;
    private static float LowPassKernelWidthInSeconds = 0.5f;
    private float LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;// tweakable

    private Vector3 lowPassValue = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        lowPassValue = Input.acceleration;
        shooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //handle here wether player is shooting or not
        shooting = true;
        setShooting();


        Vector3 acc = LowPassFilterAccelerometer();
        this.gameObject.GetComponent<PlaneBehavior>().setSpeedY(acc.x);
        this.gameObject.GetComponent<PlaneBehavior>().setSpeedX(acc.y);
    }

    public void setShooting()
    {
        this.gameObject.GetComponent<PlaneBehavior>().setShooting(this.shooting);
    }

    private Vector3 LowPassFilterAccelerometer()
    {
        lowPassValue = Vector3.Lerp(lowPassValue, getAverage(), 0.1f);
        return lowPassValue;
    }

    private Vector3 getAverage()
    {
        Vector3 result = Vector3.zero;
        for (int i = 0; i < Input.accelerationEventCount; i++)
        {
            result += Input.accelerationEvents[i].acceleration * Input.accelerationEvents[i].deltaTime;
        }
        result /= Input.accelerationEventCount * Time.deltaTime;
        return result;
    }
}
