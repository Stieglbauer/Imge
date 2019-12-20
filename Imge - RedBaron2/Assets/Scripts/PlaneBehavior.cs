using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehavior : MonoBehaviour
{
    private float x = 0, y = 0;
    [SerializeField]
    private float agilityX, agilityY;
    [SerializeField]
    private float speed;
    private float maxSpeed;
    private bool shooting = false;
    [SerializeField]
    private float maxCooldown;
    public int health;
    private int maxHealth;
    private bool crash = false;
    private bool lockOnTarget = false;
    public void setLock(bool val)
    {
        this.lockOnTarget = val;
    }
    public bool getLock()
    {
        return this.lockOnTarget;
    }
    public float getMaxCooldown()
    {
        return this.maxCooldown;
    }
    private float cooldown = 0;
    public float getCooldown()
    {
        return this.cooldown;
    }
    private float diveSound = 7;
    private float crashRot = 0;
    private int mg = 1;
    private GameObject coneFlash;
    private GameObject hull;
    private GameObject sound;
    private GameObject projectile;
    private GameObject referencez;

    private GameObject diveMid;
    private GameObject diveBegin;

    // Start is called before the first frame update
    void Start()
    {
        referencez = GameObject.Find("PrefabReferences");
        if (maxCooldown == 0)
        {
            this.maxCooldown = referencez.GetComponent<PrefabRefs>().getGMCooldown();
        }
        if (speed == 0)
        {
            //this.speed = referencez.GetComponent<PrefabRefs>().getSpeed();
        }
        if(maxSpeed == 0)
        {
            this.maxSpeed = this.speed;
        }
        if (health == 0)
        {
            this.health = referencez.GetComponent<PrefabRefs>().getGeneralHealth();
        }
        if (agilityX == 0) { 
            this.agilityX = 0.5f;
        }
        if(agilityY == 0)
        {
            this.agilityY = 0.5f;
        }
        maxHealth = health;
        coneFlash = referencez.GetComponent<PrefabRefs>().getConeFlash();
        hull = referencez.GetComponent<PrefabRefs>().getHull();
        sound = referencez.GetComponent<PrefabRefs>().getMGShotSound();
        projectile = referencez.GetComponent<PrefabRefs>().getProjectile();
        diveBegin = referencez.GetComponent<PrefabRefs>().getDiveBegin();
        diveMid = referencez.GetComponent<PrefabRefs>().getDiveMid();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.tag.Equals("Player"))
        {
            //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        }
        //handle steering
        if (crash)
        {
            if (diveSound != 0.0f) {
                diveSound -= Time.deltaTime;
                if(diveSound < 0.0f)
                {
                    Instantiate(diveMid, this.gameObject.transform);
                    diveSound = 0.0f;
                }
            }
            /*if(this.gameObject.transform.rotation.z > -this.gameObject.transform.rotation.w)
            {
                y = 1;
            } else
            {
                y = -1;
            }*/
            //Quaternion rot = Quaternion.LookRotation(Vector3.down, this.gameObject.transform.rotation * Vector3.up /*Vector3.left*/ /*Vector3.forward*/); //this.gameObject.transform.rotation, new Quaternion(0, 0, -0.7f, 0.7f), 0.005f);
            //this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, new Quaternion(rot.y, rot.z, rot.x, rot.w), 0.1f);
            this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, Quaternion.LookRotation(Vector3.down, this.gameObject.transform.rotation * Vector3.forward), 0.005f);
                
                y = 0;
            if(crashRot < 4 / agilityX)
                crashRot += 0.01f;
            x = crashRot;
        }
        this.gameObject.transform.Rotate(new Vector3(y*agilityY*Time.deltaTime, 0, -x*agilityX*Time.deltaTime), Space.Self);

        x = 0;
        y = 0;




        //move Plane forward
        this.gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);


        //handle shooting
        //shooting = this.gameObject.GetComponent<PlayerBehavior>().getShooting();
        cooldown+=Random.Range(0.9f,1.1f) * Time.deltaTime * 80;
        if (shooting)
        {
            if(cooldown >= maxCooldown )
            {
                cooldown %= maxCooldown;
                mg *= -1;
                GameObject coneFlashInstance = Instantiate(coneFlash, this.gameObject.transform.position + this.gameObject.transform.rotation * new Vector3(0, coneFlash.transform.position.y, coneFlash.transform.position.z), this.gameObject.transform.rotation,  this.gameObject.transform);
                coneFlashInstance.transform.Translate(coneFlash.transform.position.x * mg, 0, 0, Space.Self);
                //coneFlashInstance.transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                //coneFlashInstance.transform.rotation = this.gameObject.transform.rotation;
                //GameObject hullInstance = Instantiate(hull, this.gameObject.transform.position + new Vector3(hull.transform.position.x, hull.transform.position.y, hull.transform.position.z * mg), this.gameObject.transform.rotation);
                GameObject hullInstance = Instantiate(hull, this.gameObject.transform.position + this.gameObject.transform.rotation * new Vector3(0, hull.transform.position.y, hull.transform.position.z), this.gameObject.transform.rotation);
                hullInstance.transform.Translate(new Vector3(hull.transform.position.x * mg, 0, 0));
                //hullInstance.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
                hullInstance.GetComponent<fallDown>().SetSideSpeed(this.gameObject.transform.rotation * Vector3.right * mg * 0.5f);
                Instantiate(sound, this.gameObject.transform);
                GameObject projectileInstance = Instantiate(projectile, this.gameObject.transform.position + this.gameObject.transform.rotation * new Vector3(0, projectile.transform.position.y, projectile.transform.position.z), this.gameObject.transform.rotation);
                projectileInstance.transform.Translate(projectile.transform.position.x * mg, 0, 0, Space.Self);
                if (this.gameObject.tag == "Player")
                {
                    projectileInstance.transform.Rotate(new Vector3(90, 0, 0), Space.Self);
                }
                else
                {
                    float randa = Random.Range(-2, 2), randb = Random.Range(-2, 2), randc = Random.Range(-2, 2);
                    projectileInstance.transform.Rotate(new Vector3(90 + randa, randb, randc), Space.Self);
                }
                //projectileInstance.transform.Translate(0, 0, projectile.transform.position.z * mg, Space.Self);
            }
        }
    }

    public void setSpeedX(float val)
    {
        if (!crash)
        {
            if (val > 1) val = 1;
            if (val < -1) val = -1;
            x = val;
        }
    }

    public void setSpeedY(float val)
    {
        if (!crash)
        {
            if (val > 1) val = 1;
            if (val < -1) val = -1;
            y = val;
        }
    }

    public void setShooting(bool val)
    {
        this.shooting = val;
    }

    public void reduceHealth()
    {
        health--;
        if(health == maxHealth/2)
        {
            Instantiate(referencez.GetComponent<PrefabRefs>().getSmoke(), this.gameObject.transform);
        }
        if(health <= 0 && !crash)
        {
            if (this.gameObject.tag == "Plane")
            {
                this.gameObject.AddComponent<DestroyYourself>();
                this.gameObject.GetComponent<DestroyYourself>().setTime(20);
            }
            Instantiate(diveBegin, this.gameObject.transform);
            crash = true;
        }
    }

    public void setForwardV(float v)
    {
        this.speed = v;
    }

    public float getForwardV()
    {
        return this.speed;
    }

    public int getHealth()
    {
        return this.health;
    }

    public float getMaxForwardV()
    {
        return this.maxSpeed;
    }

    public float getAgilityX()
    {
        return this.agilityX;
    }

    public float getAgilityY()
    {
        return this.agilityY;
    }
}
