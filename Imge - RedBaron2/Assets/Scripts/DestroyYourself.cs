using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyYourself : MonoBehaviour
{
    [SerializeField]
    private float seconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(seconds > 0)
        {
            seconds -= 1 * Time.deltaTime;
        } else
        {
            Destroy(this.gameObject);
        }
    }

    public void setTime(float time)
    {
        if(this.seconds == 0)
        {
            this.seconds = time;
        }
    }
}
