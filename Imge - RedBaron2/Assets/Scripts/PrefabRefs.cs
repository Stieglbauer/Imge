using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabRefs : MonoBehaviour
{
    [SerializeField]
    private int speed;
    [SerializeField]
    private int generalMaxCooldown;
    [SerializeField]
    private int generalHealth;


    [SerializeField]
    private GameObject coneFlash;
    [SerializeField]
    private GameObject hull;
    [SerializeField]
    private GameObject mgShotSound;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject smoke;
    [SerializeField]
    private GameObject diveBegin;
    [SerializeField]
    private GameObject diveMid;
    [SerializeField]
    private GameObject ai;

    // Start is called before the first frame update
    public GameObject getConeFlash()
    {
        return this.coneFlash;
    }
    public GameObject getHull()
    {
        return this.hull;
    }
    public int getGMCooldown()
    {
        return this.generalMaxCooldown;
    }
    public GameObject getMGShotSound()
    {
        return this.mgShotSound;
    }

    public GameObject getProjectile()
    {
        return this.projectile;
    }

    public int getSpeed()
    {
        return this.speed;
    }

    public GameObject getSmoke()
    {
        return this.smoke;
    }

    public int getGeneralHealth()
    {
        return this.generalHealth;
    }

    public GameObject getDiveBegin()
    {
        return this.diveBegin;
    }

    public GameObject getDiveMid()
    {
        return this.diveMid;
    }

    public GameObject getAI()
    {
        return this.ai;
    }
}
