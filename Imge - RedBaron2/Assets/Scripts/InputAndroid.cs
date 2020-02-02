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

    private float scaleSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        lowPassValue = Input.acceleration;
        shooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        // handle here wether player is shooting or not
        setShooting();

        Vector3 acc = LowPassFilterAccelerometer();
        this.gameObject.GetComponent<PlaneBehavior>().setSpeedY(3*acc.y);
        this.gameObject.GetComponent<PlaneBehavior>().setSpeedX(3*acc.x);

        HandleTouches();
    }

    public void HandleTouches()
    {
        // acceleration handling for player-acceleration
        if (Input.touchCount >= 2)
        {
            if (Input.touchCount == 3)
            {
                // 3 touches
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                Touch touchTwo = Input.GetTouch(2);

                if (touchZero.position.x >= touchOne.position.x && touchZero.position.x >= touchTwo.position.x)
                {
                    HelperHandleTouches(touchOne, touchTwo);
                }else if(touchOne.position.x >= touchZero.position.x && touchOne.position.x >= touchTwo.position.x)
                {
                    HelperHandleTouches(touchZero, touchTwo);
                }
                else
                {
                    HelperHandleTouches(touchOne, touchZero);
                }
            }
            else
            {
                // 2 touches
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
                HelperHandleTouches(touchZero, touchOne);
            }
        }
    }

    private void HelperHandleTouches(Touch touchZero, Touch touchOne)
    {
        // Get the position in the previous frame of each touch.
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // Get the magnitude of the vector (the distance) between the touches in each frame.
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // Get the difference in the distances between each frame.
        float deltaMagnitude = touchDeltaMag - prevTouchDeltaMag;
        float result = Mathf.Round(this.gameObject.GetComponent<PlaneBehavior>().getForwardV() + deltaMagnitude * scaleSpeed);
        if (result < (this.gameObject.GetComponent<PlaneBehavior>().getMaxForwardV() / 2.0))
        {
            result = (float) (this.gameObject.GetComponent<PlaneBehavior>().getMaxForwardV() / 2.0);
        }
        else if (result > this.gameObject.GetComponent<PlaneBehavior>().getMaxForwardV())
        {
            result = (float) this.gameObject.GetComponent<PlaneBehavior>().getMaxForwardV();
        }

            this.gameObject.GetComponent<PlaneBehavior>().setForwardV(result);
    }

    public void StartShooting()
    {
        shooting = true;
    }

    public void StopShooting()
    {
        shooting = false;
    }

    public void setShooting()
    {
        this.gameObject.GetComponent<PlaneBehavior>().setShooting(this.shooting);
    }

    private Vector3 LowPassFilterAccelerometer()
    {
        lowPassValue = Vector3.Lerp(lowPassValue, getAverage(), 0.1f);
        // Y-0-Punkt auf Neigungswinkel 3% in Y-Richtung legen
        lowPassValue.y = (lowPassValue.y + 0.03f);
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
