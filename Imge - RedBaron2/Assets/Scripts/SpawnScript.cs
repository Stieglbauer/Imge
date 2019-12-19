using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private int amount;
    [SerializeField]
    private float maxSize;
    [SerializeField]
    private float minSize;
    [SerializeField]
    private float maxDistance;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<amount; i++)
        {
            GameObject instance = Instantiate(obj, this.transform.position + new Vector3(Random.Range(-maxDistance, maxDistance), Random.Range(0, maxDistance), Random.Range(0, maxDistance)), new Quaternion(0, 0, 0, 0));
            instance.transform.localScale = Random.Range(minSize, maxSize) * new Vector3(1, 1, 1);
            instance.transform.SetParent(this.transform);
        }
    }
}
